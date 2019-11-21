using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace WebInvocadorDummy.Util
{
    public static class RestClient
    {
        /// <summary>
        /// Ejecuta la llamada al servicio REST.
        /// </summary>
        /// <typeparam name="T">Tipo del objeto de respuesta.</typeparam>
        /// <param name="uri">URL Endpoint.</param>
        /// <param name="httpMethod">Método HTTP.</param>
        /// <param name="jsonParams">Parámetros opcionales Json.</param>
        /// <returns>Retorna el tipo especificado.</returns>
        /// <exception cref="ApplicationException"></exception>
        public static T GetResponse<T>(string uri, HttpMethod httpMethod, string jsonParams = null, string authorizationHeader = null) where T : new()
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Method = httpMethod.ToString();
            httpWebRequest.ContentType = "application/json";
            if (!string.IsNullOrEmpty(authorizationHeader))
            {
                httpWebRequest.Headers.Add(HttpRequestHeader.Authorization, authorizationHeader);
            }

            if (httpMethod != HttpMethod.GET && !string.IsNullOrEmpty(jsonParams))
            {
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWriter.Write(jsonParams);
                }
            }

            string textResponse = string.Empty;
            try
            {
                var httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                if (httpWebResponse.StatusCode != HttpStatusCode.OK)
                {
                    throw new ApplicationException(httpWebResponse.StatusCode.ToString());
                }

                StreamReader reader = new StreamReader(httpWebResponse.GetResponseStream());
                textResponse = reader.ReadToEnd();
                httpWebResponse.Close();
            }
            catch (Exception ex)
            {
                string message = String.Format("{0} fallido. Respuesta HTTP {1}", httpMethod, ex.Message);
                throw new ApplicationException(message);
            }

            try
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(textResponse);
                return result;
            }
            catch (Exception ex)
            {
                string message = String.Format("{0} fallido. Error procesamiento Json {1}", httpMethod, ex.Message);
                throw new ApplicationException(message);
            }
        }

		public static RestSharp.IRestResponse<T> Execute<T>(string uri, RestSharp.Method method, object objJsonBody = null) where T : new()
		{
			var client = new RestSharp.RestClient(uri);
			var request = new RestSharp.RestRequest(method);

			request.AddHeader("cache-control", "no-cache");
			request.AddHeader("Content-Type", "application/json");

			if (objJsonBody != null)
				request.AddJsonBody(objJsonBody);

			RestSharp.IRestResponse<T> response = client.Execute<T>(request);

			return response;
		}

		public enum HttpMethod
        {
            GET,
            POST,
            PUT,
            DELETE
        }
    }
}