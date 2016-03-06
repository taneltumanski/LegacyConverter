using LegacyConverter.Core.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyConverter.Services.Models
{
	public class DataItemBuffer
	{
		public DataItemDto Item { get; set; }
		public DateTime RequestTime { get; set; }
		public int FileIndex { get; set; }
	}
}
