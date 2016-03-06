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
		public IDataItemService DataItemService { get; set; }

		public DataItemDto Get()
		{
			var item = DataItemService.GetCurrentDataItem();

			return item;
		}
	}
}
