using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyConverter.Core.Exceptions
{
	public class FormatParserException : Exception
	{
		public FormatParserException(string msg) : base(msg) { }
		public FormatParserException(string msg, Exception ex) : base(msg, ex) { }
	}
}
