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

			var dataTests = new[] {
				new { FileName = "data_1.txt", Action = new Action<DataItemDto>(Matches_Data1) },
				new { FileName = "data_3.txt", Action = new Action<DataItemDto>(Matches_Data3) },
				new { FileName = "data_4.txt", Action = new Action<DataItemDto>(Matches_Data4) },
				new { FileName = "data_5.txt", Action = new Action<DataItemDto>(Matches_Data5) }
			};

			foreach (var dataTest in dataTests) {
				var file = GetData("OldFormatParserService", dataTest.FileName);
				var data = service.ParseOldFormat(file);

				dataTest.Action(data);
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

		public static void Matches_Data1(DataItemDto data)
		{
			Assert.AreEqual(true, data.IsActive);
			Assert.AreEqual("0551234567", data.PhoneNumber);
			Assert.AreEqual(true, data.HasXLAdditionalService);
			Assert.AreEqual(Language.Estonian, data.Language);
			Assert.AreEqual(Language.Undefined, data.XLAdditionalServiceLanguage);
			Assert.AreEqual(new DateTime(2011, 10, 23, 16, 0, 0), data.ServiceEndDateTime);
			Assert.AreEqual(new TimeSpan(8, 0, 0), data.XLServiceActivationTime);
			Assert.AreEqual(new TimeSpan(12, 0, 0), data.XLServiceEndTime);
			Assert.AreEqual(false, data.IsOverrideListInUse);
		}

		public static void Matches_Data3(DataItemDto data)
		{
			Assert.AreEqual(true, data.IsActive);
			Assert.AreEqual("0551234555", data.PhoneNumber);
			Assert.AreEqual(true, data.HasXLAdditionalService);
			Assert.AreEqual(Language.English, data.Language);
			Assert.AreEqual(Language.English, data.XLAdditionalServiceLanguage);
			Assert.AreEqual(new DateTime(2011, 11, 11, 21, 59, 0), data.ServiceEndDateTime);
			Assert.AreEqual(new TimeSpan(0, 0, 0), data.XLServiceActivationTime);
			Assert.AreEqual(new TimeSpan(12, 0, 0), data.XLServiceEndTime);
			Assert.AreEqual(true, data.IsOverrideListInUse);

			var clientList = data.Clients.ToList();

			Assert.AreEqual(1, clientList.Count);

			Assert.AreEqual(clientList[0].Name, "Rein Ratas");
			Assert.AreEqual(clientList[0].PhoneNumber, "0552212211");
		}

		public static void Matches_Data4(DataItemDto data)
		{
			Assert.AreEqual(false, data.IsActive);
			Assert.AreEqual("0502234569", data.PhoneNumber);
			Assert.AreEqual(false, data.HasXLAdditionalService);
			Assert.AreEqual(Language.Estonian, data.Language);
			Assert.AreEqual(Language.Undefined, data.XLAdditionalServiceLanguage);
			Assert.AreEqual(new DateTime(2012, 1, 1, 23, 59, 0), data.ServiceEndDateTime);
			Assert.AreEqual(null, data.XLServiceActivationTime);
			Assert.AreEqual(null, data.XLServiceEndTime);
			Assert.AreEqual(false, data.IsOverrideListInUse);
		}

		public static void Matches_Data5(DataItemDto data)
		{
			Assert.AreEqual(true, data.IsActive);
			Assert.AreEqual("0551234569", data.PhoneNumber);
			Assert.AreEqual(true, data.HasXLAdditionalService);
			Assert.AreEqual(Language.English, data.Language);
			Assert.AreEqual(Language.English, data.XLAdditionalServiceLanguage);
			Assert.AreEqual(new DateTime(2011, 10, 23, 23, 59, 0), data.ServiceEndDateTime);
			Assert.AreEqual(new TimeSpan(8, 0, 0), data.XLServiceActivationTime);
			Assert.AreEqual(new TimeSpan(12, 0, 0), data.XLServiceEndTime);
			Assert.AreEqual(true, data.IsOverrideListInUse);

			var clientList = data.Clients.ToList();

			Assert.AreEqual(2, clientList.Count);

			Assert.AreEqual("Jaan Juurikas", clientList[0].Name);
			Assert.AreEqual("0551111111", clientList[0].PhoneNumber);

			Assert.AreEqual("Peeter", clientList[1].Name);
			Assert.AreEqual("0509999999", clientList[1].PhoneNumber);
		}

		public static string GetData(string folder, string file)
		{
			var fileName = Assembly
				.GetExecutingAssembly()
				.GetManifestResourceNames()
				.Where(x => x.Contains("Data") && x.Contains(folder) && x.Contains(file))
				.FirstOrDefault();

			if (fileName == null) {
				return null;
			}

			using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(fileName))
			using (var reader = new StreamReader(s)) {
				return reader.ReadToEnd();
			}
		}
	}
}
