using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace WebApiProxy.Security.Jwt
{
	public class TokenAuthorizeAttribute : AuthorizeAttribute
	{
		public override void OnAuthorization(HttpActionContext actionContext)
		{
			//Skip authorization on [AllowAnonymous]
			if (SkipAuthorization(actionContext))
			{
				base.OnAuthorization(actionContext);
			}
			else
			{
				var authorization = actionContext.Request.Headers.Authorization; //Get the AuthorizationHeader

				if (authorization == null) //If auth header is not set - return 401.
				{
					actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
					return;
				}
				if (authorization.Scheme == "Bearer") //Check the Scheme
				{
					JsonWebAuthorizationTokenProvider _tokenProvider = new JsonWebAuthorizationTokenProvider();
					var tokenValue = authorization.Parameter; //Get the token value
					var principal = _tokenProvider.ValidateToken(tokenValue); //Validate the token through our provider
					if (principal != null) //If null, token is invalid
					{
						//Set the thread & HttpContext Principal on Valid Token.
						Thread.CurrentPrincipal = principal;
						if (HttpContext.Current != null)
						{
							HttpContext.Current.User = principal;
						}
					}
					else
					{
						//Set response to be 401 on invalid token.
						actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
						return;
					}
				}
				else
				{
					//Set response to be 401 - Unauthorized if scheme is wrong
					actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
					return;
				}
			}
		}

		//Allows anonymous access through the [AllowAnonymous] annotation
		private static bool SkipAuthorization(HttpActionContext actionContext)
		{
			Contract.Assert(actionContext != null);

			return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
				   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
		}
	}
}
