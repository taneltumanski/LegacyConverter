using LegacyConverter.Core;
using LegacyConverter.Core.Dto;
using LegacyConverter.Core.Interfaces.Services;
using LegacyConverter.Services.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LegacyConverter.Tests
{
	[TestFixture]
	public class DataItemServiceTest
	{
		[Test]
		public void DataItemService_GetsCorrectDataItems()
		{
			var service = GetService();

			var dataTests = new[] {
				new { Index = 1, Action = new Action<DataItemDto>(OldFormatParserServiceTest.Matches_Data1) },
				new { Index = 3, Action = new Action<DataItemDto>(OldFormatParserServiceTest.Matches_Data3) },
				new { Index = 4, Action = new Action<DataItemDto>(OldFormatParserServiceTest.Matches_Data4) },
				new { Index = 5, Action = new Action<DataItemDto>(OldFormatParserServiceTest.Matches_Data5) },
			};

			foreach (var test in dataTests) {
				var dataItem = service.RequestDataItem(test.Index);

				test.Action(dataItem);
			}
		}

		private DataItemService GetService()
		{
			var configMock = new Mock<IConfig>()
				.SetupProperty(x => x.ApiData, new Mock<IApiData>().SetupProperty(y => y.ApiEndpoint, string.Empty).Object)
				.SetupProperty(x => x.RequestConfig, new Mock<IRequestConfig>()
																.SetupProperty(y => y.DataBufferSeconds, 1)
																.SetupProperty(y => y.FileFormat, "data_{0}.txt")
																.SetupProperty(y => y.MinFileIndex, 1)
																.SetupProperty(y => y.MaxFileIndex, 9)
																.Object);

			var requestServiceMock = new Mock<ILegacyRequestService>();

			requestServiceMock.Setup(x => x.Request(It.IsAny<string>())).Returns<string>(x => OldFormatParserServiceTest.GetData("OldFormatParserService", x));

			var service = new DataItemService();

			service.Config = configMock.Object;
			service.ParserService = new OldFormatParserService();
			service.RequestService = requestServiceMock.Object;

			return service;
		}
	}
}
