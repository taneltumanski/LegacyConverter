using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LegacyConverter.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LegacyConverter.Web.CastleWindsor.Installers
{
	public class ConfigInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			var config = Cfg.AppSettings.Get<IConfig>();

			container.Register(Component.For<IConfig>().Instance(config).LifestyleSingleton());
		}
	}
}