using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiProxy.Service.Quartz
{
    public class JobEstado: IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            ApiSuper.NotificacionCambioEstado();
        }
    }
}