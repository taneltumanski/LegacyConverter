﻿using LegacyConverter.Core.Interfaces.Services;
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

		public virtual PartialViewResult ModernDataView()
		{
			var item = DataItemService.GetCurrentDataItem();

			return PartialView("_ModernDataView", item);
		}
	}
}
