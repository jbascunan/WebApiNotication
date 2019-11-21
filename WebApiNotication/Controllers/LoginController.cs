using System;
using System.Net;
using System.Threading;
using System.Web.Http;
using WebApiProxy.Models;
using WebApiProxy.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using WebApiProxy.Security.Jwt;
using System.Configuration;
using WebApiProxy.Service;

namespace WebApiProxy.Controllers
{
    /// <summary>
    /// login controller class for authenticate users
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api")]
    public class LoginController : ApiController
    {
        [HttpGet]
        [Route("echoping")]
        public IHttpActionResult EchoPing()
        {			
			ApiSuper.NotificacionCambioEstado();

			return Ok(true);
        }
	
		/// <summary>
		/// Autenticación para obtener token público
		/// </summary>
		/// <param name="login">Json de entrada con credenciales de acceso</param>
		/// <returns></returns>
		[Route("authenticate")]
		public IHttpActionResult Authenticate(LoginRequest login)
		{
			var api_key = ConfigurationManager.AppSettings["bearer_api_key"];
			var api_secret = ConfigurationManager.AppSettings["bearer_api_secret"];
			
			if (login != null && login.Api_key == api_key && login.Api_Secret == api_secret)
			{
				JsonWebAuthorizationTokenProvider _tokenProvider = new JsonWebAuthorizationTokenProvider();
				var user = new User();

				user.Username = login.Api_key;
				user.Roles = new List<string> { "Super" };
		
				var token = _tokenProvider.CreateToken(user);
				return Ok(token);
			}

			return Unauthorized();
		}
	}
}
