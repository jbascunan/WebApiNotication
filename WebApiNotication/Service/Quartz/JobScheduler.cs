using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebApiProxy.Service.Quartz
{
    public class JobScheduler
    {
        public static void Start()
        {
            var intervalo = int.Parse(ConfigurationManager.AppSettings["intervalo_job"]);
            IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();
            scheduler.Start();

            IJobDetail job = JobBuilder.Create<JobEstado>().Build();

            ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("jobEstado", "jobs")
            .StartNow()
            .WithSimpleSchedule(x => x
            .WithIntervalInMinutes(intervalo)
            .RepeatForever())
            .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}