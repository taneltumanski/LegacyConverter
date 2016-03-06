using LegacyConverter.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegacyConverter.Core.Dto;
using System.Linq.Expressions;
using System.Globalization;
using LegacyConverter.Core.Exceptions;

namespace LegacyConverter.Services.Services
{
	public class OldFormatParserService : IOldFormatParserService
	{
		public DataItemDto ParseOldFormat(string format)
		{
			if (string.IsNullOrWhiteSpace(format)) {
				throw new FormatParserException($"Argument cannot be empty - {nameof(format)}");
            }

			// Since we have a legacy service, these values are VERY unlikely to change
			// If we want to fail on invalid values then we should make a separate method for each property
			var formatPartActions = new [] {
				Tuple.Create(1, new Action<DataItemDto, string>((item, val) => item.IsActive = val == "A")),
				Tuple.Create(20, new Action<DataItemDto, string>((item, val) => item.PhoneNumber = val.Trim())),
				Tuple.Create(1, new Action<DataItemDto, string>((item, val) => item.HasXLAdditionalService = val == "J")),
				Tuple.Create(1, new Action<DataItemDto, string>((item, val) => item.Language = val == "E" ? Language.Estonian : (val == "I" ? Language.English : Language.Undefined))),
				Tuple.Create(1, new Action<DataItemDto, string>((item, val) => item.XLAdditionalServiceLanguage = val == "E" ? Language.Estonian : (val == "I" ? Language.English : Language.Undefined))),
				Tuple.Create(8, new Action<DataItemDto, string>((item, val) => item.ServiceEndDateTime = string.IsNullOrWhiteSpace(val) ? (DateTime?)null : DateTime.ParseExact(val, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None))),
				Tuple.Create(4, new Action<DataItemDto, string>((item, val) => item.ServiceEndDateTime = item.ServiceEndDateTime == null || string.IsNullOrWhiteSpace(val) ? item.ServiceEndDateTime : item.ServiceEndDateTime.Value.Add(TimeSpan.ParseExact(val, "hhmm", CultureInfo.InvariantCulture, TimeSpanStyles.None)))),
				Tuple.Create(4, new Action<DataItemDto, string>((item, val) => item.XLServiceActivationTime = string.IsNullOrWhiteSpace(val) ? (TimeSpan?)null : TimeSpan.ParseExact(val, "hhmm", CultureInfo.InvariantCulture, TimeSpanStyles.None))),
				Tuple.Create(4, new Action<DataItemDto, string>((item, val) => item.XLServiceEndTime = string.IsNullOrWhiteSpace(val) ? (TimeSpan?)null : TimeSpan.ParseExact(val, "hhmm", CultureInfo.InvariantCulture, TimeSpanStyles.None))),
				Tuple.Create(1, new Action<DataItemDto, string>((item, val) => item.IsOverrideListInUse = val == "K")),
				Tuple.Create(120, new Action<DataItemDto, string>((item, val) => InsertClientNumbers(item, val, 15))),
				Tuple.Create(160, new Action<DataItemDto, string>((item, val) => InsertClientNames(item, val, 20)))
			};

			var totalWidth = formatPartActions.Sum(x => x.Item1);

			// Pad the item so we always have the correct length
			format = format.Trim().PadRight(totalWidth);

			if (format.Length != totalWidth) {
				throw new FormatParserException($"Invalid length data - was {format.Length} - should be {totalWidth}");
			}

			try {
				var dataItem = GetDataItem(format, formatPartActions);

				return dataItem;
			} catch (Exception e) {
				if (e is FormatParserException) {
					throw;
				}

				throw new FormatParserException("An error has occured", e);
			}
		}

		private DataItemDto GetDataItem(string format, IEnumerable<Tuple<int, Action<DataItemDto, string>>> formatPartActions)
		{
			var dataItem = new DataItemDto() {
				Clients = new ClientDto[8]
			};

			int currentStartIndex = 0;

			foreach (var part in formatPartActions) {
				var formatPart = format.Substring(currentStartIndex, part.Item1);

				currentStartIndex += part.Item1;

				part.Item2(dataItem, formatPart);
			}

			// Filter out empty clients
			dataItem.Clients = dataItem.Clients
				.Where(x => !string.IsNullOrWhiteSpace(x.Name) && !string.IsNullOrWhiteSpace(x.PhoneNumber))
				.ToList()
				.AsReadOnly();

			return dataItem;
		}

		/// <summary>
		/// Cut the data into equal parts
		/// </summary>
		/// <param name="format">Data</param>
		/// <param name="partSize">Part size</param>
		/// <returns>Data cut into parts</returns>
		private IEnumerable<string> GetFormatParts(string format, int partSize)
		{
			var partCount = format.Length / partSize;
			var partLengths = Enumerable.Range(0, partCount).Select(x => partSize);

			return GetFormatParts(format, partLengths);
		}

		/// <summary>
		/// Cut the data into different sized parts
		/// </summary>
		/// <param name="format">Data</param>
		/// <param name="formatPartsLengths">Part sizes</param>
		/// <returns>Data cut into parts</returns>
		private IEnumerable<string> GetFormatParts(string format, IEnumerable<int> formatPartsLengths)
		{
			int currentStartIndex = 0;

			foreach (var partLength in formatPartsLengths) {
				yield return format.Substring(currentStartIndex, partLength);

				currentStartIndex += partLength;
			}
		}

		private void InsertClientNames(DataItemDto item, string format, int partSize)
		{
			InsertClientProperty(item, format, partSize, (part, c) => new ClientDto() { Name = part.Trim(), PhoneNumber = c.PhoneNumber });
		}

		private void InsertClientNumbers(DataItemDto item, string format, int partSize)
		{
			InsertClientProperty(item, format, partSize, (part, c) => new ClientDto() { Name = c.Name, PhoneNumber = part.Trim() });
		}

		private void InsertClientProperty(DataItemDto item, string format, int partSize, Func<string, ClientDto, ClientDto> selectorFunc)
		{
			var parts = GetFormatParts(format, partSize);

			item.Clients = parts.Zip(item.Clients ?? new List<ClientDto>(), (part, c) => selectorFunc(part, c ?? new ClientDto()));
		}
	}
}
