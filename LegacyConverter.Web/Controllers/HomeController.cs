using LegacyConverter.Core.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LegacyConverter.Web.Controllers
{
	public partial class HomeController : Controller
	{
		public IDataItemService DataItemService { get; set; }

		public virtual ActionResult Index()
		{
			ViewBag.Title = "Home Page";

			return View();
		}

		/// <summary>
		/// Create the partial view for the currently buffered legacy object
		/// </summary>
		/// <returns>View for current legacy object</returns>
		public virtual PartialViewResult ModernDataView()
		{
			var item = DataItemService.GetCurrentDataItem();

			return PartialView("_ModernDataView", item);
		}
	}
}
