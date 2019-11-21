using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebApiProxy.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiProxy.Models;

namespace WebApiProxy.Repository.Tests
{
    [TestClass()]
    public class NotificacionRepositoryTests
    {
        [TestMethod()]
        public void GetEstadoProyectosTest()
        {
            List<NotificacionEstadoExpedienteRequest> lista  = NotificacionRepository.GetEstadoProyectos();
            Assert.IsTrue(true);
        }
    }
}