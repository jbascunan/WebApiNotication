using System.Web.Http;
using WebApiProxy.Security;
using WebApiProxy.Service;
using System.Net.Http;
using System.Net;
using System;
using WebApiProxy.Security.Jwt;
using WebApiProxy.Models;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using NLog;

namespace WebApiProxy.Controllers
{
	/// <summary>
	/// Proxy para obtener token desde SUPER
	/// </summary>
	//[RoutePrefix("api")]
	public class ProxyController : ApiController
	{
        private static Logger logger = LogManager.GetCurrentClassLogger();

        [BasicAuthentication]
		[HttpGet]
		[Route("get-token-super")]
		public IHttpActionResult GetTokenSuper()
		{
			var token = ApiSuper.GetToken();
			
			return Ok(token);
		}

		[TokenAuthorize]
		[HttpGet]
		[Route("atlas/get-empresa/{rut}")]
        public IHttpActionResult getEmpresaFromRut(string rut)
        {

            WSAtlas.AtlasClient ws = new WSAtlas.AtlasClient();
            WSAtlas.empresa EmpresaEncontrada = ws.getEmpresaFromRut(rut.Replace(".", "").ToUpper());
            WebApiResponseBase response = new WebApiResponseBase();

            EmpresaEncontrada = EmpresaEncontrada ?? new WSAtlas.empresa();

            return Ok(JObject.Parse(JsonConvert.SerializeObject(EmpresaEncontrada)));

        }
    }
}