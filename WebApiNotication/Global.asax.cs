using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApiProxy.Service.Quartz;
using WebApiSegura.Service.Notificacion;

namespace WebApiProxy
{
	public class WebApiApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			GlobalConfiguration.Configure(WebApiConfig.Register);
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);
			var activeJob = bool.Parse(ConfigurationManager.AppSettings["activar_notificacion_estado"]);
			if (activeJob) JobScheduler.Start();
		}

		private void Init_Job()
		{			
			try
			{
				var intervalo = int.Parse(ConfigurationManager.AppSettings["intervalo_job"]);
				// Creamos el demonio
				IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

				scheduler.Clear();
				// Iniciamos el demonio
				scheduler.Start();

				// Definimos un trabajo y lo asociamos a la clase
				IJobDetail job = JobBuilder.Create<CambioEstadoJobs>()
					.WithIdentity("cadaXm", "app")
					.Build();

				// Lanzamos el job cada x minutos
				ITrigger trigger = TriggerBuilder.Create()
					.WithIdentity("cadaXm", "app")
					.StartNow()
					.WithSimpleSchedule(s => s
						.WithIntervalInMinutes(intervalo) 
						//.WithIntervalInSeconds(30) // SOLO PARA PRUEBAS LUEGO ELIMINAR
						.RepeatForever())
					.Build();

				// Asociamos el job y el trigger al demonio
				scheduler.ScheduleJob(job, trigger);
			}
			catch (SchedulerException se)
			{
				Console.WriteLine(se);
			}
		}
	}
}