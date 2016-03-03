using Cfg.ValidationAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyConverter.Core
{
	public interface IConfig
	{
		[Required]
		IApiData ApiData { get; set; }
	}

	public interface IApiData
	{
		[Required]
		string ApiEndpoint { get; set; }
	}
}
