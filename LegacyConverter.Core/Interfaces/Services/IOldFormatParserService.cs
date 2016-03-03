using LegacyConverter.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyConverter.Core.Interfaces.Services
{
	public interface IOldFormatParserService : IApplicationService
	{
		DataItemDto ParseOldFormat(string format);
	}
}
