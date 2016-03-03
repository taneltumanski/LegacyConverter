using LegacyConverter.Core.Dto;
using LegacyConverter.Core.Exceptions;
using LegacyConverter.Services.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LegacyConverter.Tests
{
	[TestFixture]
	public class OldFormatParserServiceTest
	{
		[Test]
		public void Service_ParsesCorrectly()
		{
			var service = new OldFormatParserService();

			{
				var file = GetData("OldFormatParserService", "data_1.txt");
				var data = service.ParseOldFormat(file);

				Assert.AreEqual(data.IsActive, true);
				Assert.AreEqual(data.PhoneNumber, "0551234567");
				Assert.AreEqual(data.HasXLAdditionalService, true);
				Assert.AreEqual(data.Language, Language.Estonian);
				Assert.AreEqual(data.XLAdditionalServiceLanguage, Language.Undefined);
				Assert.AreEqual(data.ServiceEndDateTime, new DateTime(2011, 10, 23, 16, 0, 0));
				Assert.AreEqual(data.XLServiceActivationTime, new TimeSpan(8, 0, 0));
				Assert.AreEqual(data.XLServiceEndTime, new TimeSpan(12, 0, 0));
				Assert.AreEqual(data.IsOverrideListInUse, false);
			}

			{
				var file = GetData("OldFormatParserService", "data_3.txt");
				var data = service.ParseOldFormat(file);

				Assert.AreEqual(data.IsActive, true);
				Assert.AreEqual(data.PhoneNumber, "0551234555");
				Assert.AreEqual(data.HasXLAdditionalService, true);
				Assert.AreEqual(data.Language, Language.English);
				Assert.AreEqual(data.XLAdditionalServiceLanguage, Language.English);
				Assert.AreEqual(data.ServiceEndDateTime, new DateTime(2011, 11, 11, 21, 59, 0));
				Assert.AreEqual(data.XLServiceActivationTime, new TimeSpan(0, 0, 0));
				Assert.AreEqual(data.XLServiceEndTime, new TimeSpan(12, 0, 0));
				Assert.AreEqual(data.IsOverrideListInUse, true);

				var clientList = data.Clients.ToList();

				clientList[0].Name = "Rein Ratas";
				clientList[0].PhoneNumber = "0552212211";
			}

			{
				var file = GetData("OldFormatParserService", "data_4.txt");
				var data = service.ParseOldFormat(file);

				Assert.AreEqual(data.IsActive, false);
				Assert.AreEqual(data.PhoneNumber, "0502234569");
				Assert.AreEqual(data.HasXLAdditionalService, false);
				Assert.AreEqual(data.Language, Language.Estonian);
				Assert.AreEqual(data.XLAdditionalServiceLanguage, Language.Undefined);
				Assert.AreEqual(data.ServiceEndDateTime, new DateTime(2012, 1, 1, 23, 59, 0));
				Assert.AreEqual(data.XLServiceActivationTime, null);
				Assert.AreEqual(data.XLServiceEndTime, null);
				Assert.AreEqual(data.IsOverrideListInUse, false);
			}

			{
				var file = GetData("OldFormatParserService", "data_5.txt");
				var data = service.ParseOldFormat(file);

				Assert.AreEqual(data.IsActive, true);
				Assert.AreEqual(data.PhoneNumber, "0551234569");
				Assert.AreEqual(data.HasXLAdditionalService, true);
				Assert.AreEqual(data.Language, Language.English);
				Assert.AreEqual(data.XLAdditionalServiceLanguage, Language.English);
				Assert.AreEqual(data.ServiceEndDateTime, new DateTime(2011, 10, 23, 23, 59, 0));
				Assert.AreEqual(data.XLServiceActivationTime, new TimeSpan(8, 0, 0));
				Assert.AreEqual(data.XLServiceEndTime, new TimeSpan(12, 0, 0));
				Assert.AreEqual(data.IsOverrideListInUse, true);

				var clientList = data.Clients.ToList();

				clientList[0].Name = "Jaan Juurikas";
				clientList[0].PhoneNumber = "0551111111";

				clientList[0].Name = "Peeter";
				clientList[0].PhoneNumber = "0509999999";
			}
		}

		[Test]
		public void Service_InvalidLength()
		{
			var service = new OldFormatParserService();

			Assert.Throws<FormatParserException>(() => service.ParseOldFormat(new string(' ', 500)));
		}

		[Test]
		public void Service_InvalidFormat()
		{
			var service = new OldFormatParserService();

			Assert.Throws<FormatParserException>(() => service.ParseOldFormat(new string('a', 50)));
		}

		private static string GetData(string folder, string file)
		{
			var fileName = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceNames()
				.Where(x => x.Contains("Data") && x.Contains(folder) && x.Contains(file))
				.FirstOrDefault();

			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName))
			using (var reader = new StreamReader(s)) {
				return reader.ReadToEnd();
			}
		}
	}
}
