using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiProxy.Models;
using WebApiProxy.Security.Jwt;

namespace WebApiProxy.Controllers
{
    public class ExpedienteController : ApiController
    {
        public string Get(int id)
        {
            return id.ToString();
        }

        [TokenAuthorize]
        public IHttpActionResult Post([FromBody]RegistroExpedienteRequest value)
        {
            var appId = Convert.ToInt32(DateTime.Now.ToString("MMddHHmm"));
            return Content(HttpStatusCode.OK, new RegistroExpedienteResponse { Status = value.Status, Message = "Test " + DateTime.Now, CUP = "1223", ApplicationId = appId });
        }

        public IHttpActionResult Put([FromBody]NotificacionEstadoExpedienteRequest value)
        {
            return Content(HttpStatusCode.OK, new WebApiResponseBase { });
        }
    }
}
