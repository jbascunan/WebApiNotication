using NLog;
using Quartz;
using System;
using WebApiProxy.Repository;
using WebApiProxy.Service;

namespace WebApiSegura.Service.Notificacion
{
	/// <summary>
	/// Job para notificar a mi super por cambios de estados de proyectos
	/// </summary>
	public class CambioEstadoJobs : IJob
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();
		public void Execute(IJobExecutionContext context)
		{

			// Según el job que se vaya a ejecutar
			switch (context.JobDetail.Key.ToString())
			{
				case "app.cadaXm":
                    ApiSuper.NotificacionCambioEstado();

                    //var estadoProyectos = NotificacionRepository.GetEstadoProyectos();

					//foreach (var item in estadoProyectos)
					//{
					//	logger.Warn("[{0}]:: Estado proyecto: {1}", DateTime.Now, item.Status); 
					//}
					
					break;
			}
		}
	}
}