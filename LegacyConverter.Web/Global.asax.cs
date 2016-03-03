using Castle.Windsor;
using Castle.Windsor.Installer;
using LegacyConverter.Services.Services;
using LegacyConverter.Web.CastleWindsor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace LegacyConverter.Web
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		private IWindsorContainer Container { get; set; }

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			this.Container = new WindsorContainer()
				.Install(FromAssembly.This());

			GlobalConfiguration.Configuration.Services.Replace(typeof(IHttpControllerActivator), new WindsorCompositionRoot(this.Container));
		}

		public override void Dispose()
		{
			base.Dispose();
		}
	}
}
