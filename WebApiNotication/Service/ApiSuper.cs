using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiProxy.Utils;
using WebApiProxy.Models;
using System.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.Text;
using NLog;
using System.Net;
using WebApiProxy.Repository;

namespace WebApiProxy.Service
{
	public static class ApiSuper
	{
		private static string apiBaseUri = ConfigurationManager.AppSettings["api_Super_BaseUri"];
		private static Logger logger = LogManager.GetCurrentClassLogger();
		/// <summary>
		/// Método de paso para obtener el token API SUPER
		/// </summary>
		/// <returns></returns>
		public static string GetToken()
		{
			var token = string.Empty;
			//verifica existe token en cache
			token = (string)MemoryCacher.GetValue("token");

			if (token == null)	
			{
				LoginSuperRequest credenciales = new LoginSuperRequest
				{
					Api_key = ConfigurationManager.AppSettings["api_key_super"],
					ChileAtiendeId = ConfigurationManager.AppSettings["chile_atiende_id"]
				};				

				//Get the token
				var jObject = RequestToken(credenciales);
				if (jObject != null)
				{
					token = "Token " + jObject.GetValue("token").ToString();
					var tokenExpiracion = jObject.GetValue("expiration_date").ToString();

					//almacena token en cache
					MemoryCacher.Add("token", token, DateTimeOffset.Parse(tokenExpiracion));
				}
				else
					token = string.Empty;
			}			

			return token;
		}

		/// <summary>
		/// Solicitud token api super
		/// </summary>
		/// <param name="userName"></param>
		/// <param name="password"></param>
		/// <param name="apiBaseUri"></param>
		/// <returns></returns>
		private static JObject RequestToken(LoginSuperRequest credenciales)
		{
			using (var client = new HttpClient())
			{
				JObject jObject = null;
				//setup client				
				client.DefaultRequestHeaders.Accept.Clear();

				//setup login data
				string myJson = JsonConvert.SerializeObject(credenciales);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

                Certificates.Instance.GetCertificatesAutomatically();
                //send request
                HttpResponseMessage response = client.PostAsync(apiBaseUri + "/api-token-auth/", new StringContent(myJson, Encoding.UTF8, "application/json")).Result;
				
				if (response.IsSuccessStatusCode)
				{
					var responseJson = response.Content.ReadAsStringAsync().Result;
					jObject = JObject.Parse(responseJson);
				}
				else
				{
					throw new Exception(response.StatusCode.ToString());
				}

				return jObject;
			}
		}

		/// <summary>
		/// Notificar a MI SUPER y SIMPLE cambios de estados de proyectos
		/// </summary>
		public static void NotificacionCambioEstado()
		{
            string uriSuper = string.Empty;
            string uriSimple = string.Empty;

            var authorizationHeader = GetToken();
			if (string.IsNullOrEmpty(authorizationHeader)) return;
			
			var estadoProyectos = NotificacionRepository.GetEstadoProyectos();

			foreach (var item in estadoProyectos)
			{				

                 //NOTIFICAR MI SUPER
				if (item.MsApplicationId != null && item.MsApplicationId > 0 && item.ProcesadoSuper != 1) {
					uriSuper = string.Format("{0}/applications/{1}/", apiBaseUri, item.MsApplicationId);
                    System.Threading.Tasks.Task.Run(() =>
                    {
                        notificar(uriSuper, authorizationHeader, item, NotityType.SUPER);
                    });
                }

                //NOTIFICAR A SIMPLE  
                if (item.SimpleId != null && item.SimpleId > 0 && item.ProcesadoSimple != 1)
                {
                    uriSimple = ConfigurationManager.AppSettings["url_simple_notificar"] + item.SimpleId;

                    System.Threading.Tasks.Task.Run(() =>
                    {
                        notificar(uriSimple, string.Empty, item, NotityType.SIMPLE);

                    });
                }
            }
		}

        /// <summary>
        /// Notificar cambios de estados según url, a api super o simple
        /// </summary>
        /// <param name="uri">url del servicio a notificar, super o simple</param>
        /// <param name="authorizationHeader">Cabecera autorización api</param>
        /// <param name="notify">Objeto Notificación Estado</param>
        /// <param name="notityType">Identifica el tipo de notificación</param>
        private static void notificar(string uri, string authorizationHeader, NotificacionEstadoExpedienteRequest notify, NotityType notityType)
        {
            System.Threading.Tasks.Task.Run(() =>
            {
                var json = JsonConvert.SerializeObject(notify);
                var result = RestClient.Execute<WebApiResponseBase>(uri, RestSharp.Method.POST, json, authorizationHeader);
                var response = new WebApiResponseBase();
                var estadoProceso = 0;
                
                if (result != null && !string.IsNullOrEmpty(result.Content))
                    response = JsonConvert.DeserializeObject<RegistroExpedienteResponse>(result.Content);

                if ((result.StatusCode == HttpStatusCode.OK || result.StatusCode == HttpStatusCode.Created) && response.Status == "OK")
                {
                    estadoProceso = 1;
                }
                else
                {
                    logger.Error("'{0}' | '{1}' | \n Data: '{2}'", uri, result.Content, json);
                    estadoProceso = 2;
                }

                //NotificacionRepository.Update(notify, estadoProceso, notityType);
            });
        }
    }
}