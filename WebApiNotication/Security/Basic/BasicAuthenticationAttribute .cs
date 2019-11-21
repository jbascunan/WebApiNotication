using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Configuration;

namespace WebApiProxy.Security
{
	public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
	{
		//public override void OnAuthorization(HttpActionContext actionContext)
		//{
		//	var authHeader = actionContext.Request.Headers.Authorization;

		//	if (authHeader != null)
		//	{
		//		var authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
		//		var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
		//		var usernamePasswordArray = decodedAuthenticationToken.Split(':');
		//		var userName = usernamePasswordArray[0];
		//		var password = usernamePasswordArray[1];
		//		var api_key = ConfigurationManager.AppSettings["basic_api_key"];
		//		var api_secret = ConfigurationManager.AppSettings["basic_api_secret"];

		//		// validacion de credenciales
		//		var isValid = userName == api_key && password == api_secret;

		//		if (isValid)
		//		{
		//			var principal = new GenericPrincipal(new GenericIdentity(userName), null);
		//			Thread.CurrentPrincipal = principal;				

		//			return;
		//		}
		//	}

		//	HandleUnathorized(actionContext);
		//}

		public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
		{
			if (actionContext.Request.Headers.Authorization == null)
			{
				HandleUnathorized(actionContext);
			}
			else
			{
				var basic_api_key = ConfigurationManager.AppSettings["basic_api_key"];
				var basic_api_secret = ConfigurationManager.AppSettings["basic_api_secret"];

				// Gets header parameters  
				string authenticationString = actionContext.Request.Headers.Authorization.Parameter;
				string originalString = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationString));

				// Gets credenciales  
				string api_key = originalString.Split(':')[0];
				string api_secret = originalString.Split(':')[1];

				// validacion de credenciales  
				var isValid = api_key == basic_api_key && api_secret == basic_api_secret;
				if (!isValid)
				{
					// returns unauthorized error  
					HandleUnathorized(actionContext);
				}
			}

			base.OnAuthorization(actionContext);
		}

		private void HandleUnathorized(HttpActionContext actionContext)
		{
			actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);			
		}
	}

	

}