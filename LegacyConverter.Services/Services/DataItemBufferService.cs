using LegacyConverter.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegacyConverter.Core.Dto;
using System.Runtime.Caching;

namespace LegacyConverter.Services.Services
{
	public class DataItemBufferService : IDataItemBufferService
	{
		public ICacheService CacheService { get; set; }

		private static readonly string DataItemKey = "DataItem_Buffer_Key";

		public void AddDataItem(DataItemDto dataItem)
		{
			CacheService.Add(dataItem, DataItemKey);
		}

		public DataItemDto GetDataItem()
		{
			return CacheService.Get<DataItemDto>(DataItemKey);
		}
	}
}
