using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using LegacyConverter.Core.Interfaces.Services;
using LegacyConverter.Services.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LegacyConverter.Web.CastleWindsor.Installers
{
	public class ServiceInstaller : IWindsorInstaller
	{
		public void Install(IWindsorContainer container, IConfigurationStore store)
		{
			container.Register(
				Classes
					.FromAssemblyContaining<OldFormatParserService>()
					.BasedOn<IApplicationService>()
					.WithService.FromInterface()
					.LifestylePerWebRequest());
		}
	}
}