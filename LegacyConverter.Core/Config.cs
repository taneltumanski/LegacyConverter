using Cfg.Attributes;
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
		IApiData ApiData { get; set; }
		IRequestConfig RequestConfig { get; set; }
	}

	public interface IApiData
	{
		[Required]
		string ApiEndpoint { get; set; }
	}

	public interface IRequestConfig
	{
		[Min(0), Default(5)]
		int DataBufferSeconds { get; set; }

		[Default("data_{0}.txt")]
		string FileFormat { get; set; }

		[Min(1), Default(9)]
		int MaxFileIndex { get; set; }

		[Min(0), Default(1)]
		int MinFileIndex { get; set; }
	}
}
