using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegacyConverter.Core.Dto
{
	public class DataItemDto
	{
		public bool IsValid { get; set; }

		public int SequenceId { get; set; }
		public bool IsActive { get; set; }
		public string PhoneNumber { get; set; }
		public bool HasXLAdditionalService { get; set; }
		public Language Language { get; set; }
		public Language XLAdditionalServiceLanguage { get; set; }
		public DateTime? ServiceEndDateTime { get; set; }
		public TimeSpan? XLServiceActivationTime { get; set; }
		public TimeSpan? XLServiceEndTime { get; set; }
		public bool IsOverrideListInUse { get; set; }

		public IEnumerable<ClientDto> Clients { get; set; }
	}

	public enum Language
	{
		Undefined,
		Estonian,
		English
	}

	public class ClientDto
	{
		public string Name { get; set; }
		public string PhoneNumber { get; set; }
	}
}
