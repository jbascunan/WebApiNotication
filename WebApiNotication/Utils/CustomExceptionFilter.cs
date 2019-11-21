using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Web;
using System.Web.Http.Filters;
using WebApiProxy.Models;

namespace WebApiProxy.Utils
{  
    public class CustomExceptionFilter : ExceptionFilterAttribute
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		public override void OnException(HttpActionExecutedContext actionExecutedContext)
		{
			string exceptionMessage = string.Empty;
			if (actionExecutedContext.Exception.InnerException == null)
			{
				exceptionMessage = actionExecutedContext.Exception.Message;
			}
			else
			{
				exceptionMessage = actionExecutedContext.Exception.InnerException.Message;
			}

			exceptionMessage = (string.IsNullOrEmpty(exceptionMessage) ? "Internal Server Error." : exceptionMessage);

			//Genera código identificador de error para seguimiento
			string IDTiempo = DateTime.Now.ToString("yyyyMMddHHmmss") + DateTime.Now.Millisecond;
			var strCode = Guid.NewGuid().ToString("n").Substring(0, 8) + IDTiempo;
			var xHash = new xHash(ConfigurationManager.AppSettings["xHash.semilla"]);
			var codigo = xHash.EncryptData(strCode);
			codigo = codigo.Substring(0, 20);

			//Crea objeto de respuesta. 
			var webApiResponse = new WebApiResponseBase()
			{
				Status = "ERROR",
				Message = string.Format("Código Error: {0}", codigo)
			};

			logger.Fatal(actionExecutedContext.Exception, "ERROR INTERNO|CODIGO| {0} | MENSAJE::{1}", codigo, exceptionMessage);

			//return objeto formato json
			actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(
				HttpStatusCode.InternalServerError,
				JObject.Parse(JsonConvert.SerializeObject(webApiResponse)),
				JsonMediaTypeFormatter.DefaultMediaType
			);
			
		}
	}
}