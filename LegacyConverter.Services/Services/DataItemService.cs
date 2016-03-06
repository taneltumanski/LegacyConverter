using LegacyConverter.Core;
using LegacyConverter.Core.Dto;
using LegacyConverter.Core.Interfaces.Services;
using LegacyConverter.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace LegacyConverter.Services.Services
{
	public class DataItemService : IDataItemService
	{
		private static readonly string DATA_ITEM_BUFFER_KEY = "DataItemBuffer_CacheKey";
		private static readonly object CACHE_LOCK = new object();

		private MemoryCache Cache = MemoryCache.Default;

		public ILegacyRequestService RequestService { get; set; }
		public IOldFormatParserService ParserService { get; set; }

		public IConfig Config { get; set; }

		public DataItemDto GetCurrentDataItem()
		{
			var now = DateTime.UtcNow;
			var item = Cache[DATA_ITEM_BUFFER_KEY] as DataItemBuffer;

			if (item == null) {
				item = new DataItemBuffer() {
					RequestTime = DateTime.MinValue,
					FileIndex = Config.RequestConfig.MinFileIndex
				};

				Cache[DATA_ITEM_BUFFER_KEY] = item;
			}

			if (item.RequestTime.AddSeconds(Config.RequestConfig.DataBufferSeconds) < now) {
				lock (CACHE_LOCK) {
					item = Cache[DATA_ITEM_BUFFER_KEY] as DataItemBuffer;

					if (item.RequestTime.AddSeconds(Config.RequestConfig.DataBufferSeconds) < now) {
						var currentFileIndex = CalculateFileIndex(item.FileIndex, item.RequestTime);

						item = GetDataItemBuffer(currentFileIndex);

						Cache[DATA_ITEM_BUFFER_KEY] = item;
					}
				}
			}

			return item.Item;
		}

		public DataItemDto RequestDataItem(int fileIndex)
		{
			var fileName = string.Format(Config.RequestConfig.FileFormat, fileIndex);

			return RequestDataItem(fileName, fileIndex);
		}

		private DataItemDto RequestDataItem(string fileName, int fileIndex)
		{
			var dataItemData = RequestService.Request(fileName);

			DataItemDto dataItem;

			if (dataItemData == null) {
				dataItem = new DataItemDto();
			} else {
				dataItem = ParserService.ParseOldFormat(dataItemData);
			}

			dataItem.SequenceId = fileIndex;

			return dataItem;
		}

		private DataItemBuffer GetDataItemBuffer(int fileIndex)
		{
			var dataItem = RequestDataItem(fileIndex);
			var isValid = dataItem != null;

			dataItem = dataItem ?? new DataItemDto();

			dataItem.SequenceId = fileIndex;
			dataItem.IsValid = isValid;

			var buffer = new DataItemBuffer() {
				Item = dataItem,
				RequestTime = DateTime.UtcNow,
				FileIndex = fileIndex
			};

			return buffer;
		}

		private int CalculateFileIndex(int fileIndex, DateTime requestTime)
		{
			TimeSpan passedTime;

			if (requestTime == DateTime.MinValue) {
				passedTime = TimeSpan.Zero;
			} else {
				passedTime = DateTime.UtcNow.Subtract(requestTime);
			}

			// Calculate the current file index based on the last file index and passed time
			var wouldBeFileIndex = passedTime.TotalSeconds / Config.RequestConfig.DataBufferSeconds;
			var currentFileIndex = fileIndex + (int)(wouldBeFileIndex % Config.RequestConfig.MaxFileIndex);

			// If we overflow, fall back to the minimum value
			if (currentFileIndex > Config.RequestConfig.MaxFileIndex) {
				currentFileIndex = Config.RequestConfig.MinFileIndex;
			}

			return currentFileIndex;
		}
	}
}
