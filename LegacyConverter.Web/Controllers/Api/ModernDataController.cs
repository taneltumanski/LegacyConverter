using LegacyConverter.Core;
using LegacyConverter.Core.Dto;
using LegacyConverter.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace LegacyConverter.Web.Controllers.Api
{
    public class ModernDataController : ApiController
    {
		public IDataItemBufferService DataItemBufferService { get; set; }
		public ILegacyRequestService RequestService { get; set; }
		public IOldFormatParserService ParserService { get; set; }

		public IConfig Config { get; set; }

		public async Task<DataItemDto> Get()
		{
			var data = await RequestService.Request(Config.ApiData.ApiEndpoint, "data_1.txt");
			var item = ParserService.ParseOldFormat(data);

			return item;
		}
	}
}
