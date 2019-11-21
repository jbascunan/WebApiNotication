using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Net;
using System.Globalization;
using System.Configuration;
using System.Threading;

namespace WebApiProxy.Utils
{
    public class xHash
    {
        private TripleDESCryptoServiceProvider tripleDes = new TripleDESCryptoServiceProvider();

        public xHash(string secretKey)
        {
            this.tripleDes.Key = this.TruncateHash(secretKey, this.tripleDes.KeySize / 8);
            this.tripleDes.IV = this.TruncateHash("", this.tripleDes.BlockSize / 8);
        }

        private byte[] TruncateHash(string key, int length)
        {
            byte[] hash = new SHA1CryptoServiceProvider().ComputeHash(Encoding.Unicode.GetBytes(key));
            Array.Resize<byte>(ref hash, length);
            return hash;
        }

        public string EncryptData(string plaintext)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(plaintext);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, this.tripleDes.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();
            return Convert.ToBase64String(memoryStream.ToArray());
        }

        public string DecryptData(string encryptedtext)
        {
            byte[] buffer = Convert.FromBase64String(encryptedtext);
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, this.tripleDes.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(buffer, 0, buffer.Length);
            cryptoStream.FlushFinalBlock();
            return Encoding.Unicode.GetString(memoryStream.ToArray());
        }
    }

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

		/// <summary>
		/// Ejecuta la llamada al servicio REST con RestSharp
		/// </summary>
		/// <typeparam name="T">Tipo de dato del objeto de retorno</typeparam>
		/// <param name="uri">url del endpoint del servicio rest</param>
		/// <param name="httpMethod">Método HTTP</param>
		/// <param name="objJsonBody">Objeto request body</param>
		/// <returns>Interface IRestResponse<T></returns>
		public static RestSharp.IRestResponse<T> Execute<T>(string uri, RestSharp.Method httpMethod, object objJsonBody = null, string authorizationHeader = null) where T : new()
		{
			var client = new RestSharp.RestClient(uri);
			var request = new RestSharp.RestRequest(httpMethod);

			request.AddHeader("cache-control", "no-cache");
			request.AddHeader("Content-Type", "application/json");

			if (!string.IsNullOrEmpty(authorizationHeader))
			{
				request.AddHeader("Authorization", authorizationHeader);
			}

			if (objJsonBody != null)
				request.AddParameter("application/json", objJsonBody, RestSharp.ParameterType.RequestBody);
            //request.AddJsonBody(objJsonBody);
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            RestSharp.IRestResponse<T> response = null;
			for (int i = 0; i < 2; i++)
			{
                response = client.Execute<T>(request);
                if (response.StatusCode == HttpStatusCode.OK || response.StatusCode == HttpStatusCode.Created)
                    break;
                else if (response.StatusCode == HttpStatusCode.RequestTimeout || (int)response.StatusCode > 500)
                    Thread.Sleep(int.Parse(ConfigurationManager.AppSettings["timer_web_api"]));
            }

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
	public static class Util
	{
		public static string ToRutFormat(string value)
		{
			if (string.IsNullOrEmpty(value))
			{
				return string.Empty;
			}

			var ruts = value.Split('-');
			if (ruts.Length < 2)
			{
				return string.Empty;
			}
			var dv = ruts[1].ToUpper();
			if (dv.Length > 1)
			{
				return string.Empty;
			}
			try
			{
				var irut = Convert.ToInt32(ruts[0]);
				return irut.ToString("N0", CultureInfo.CreateSpecificCulture("es-CL")) + "-" + dv;
			}
			catch (Exception)
			{
				return string.Empty;
			}
		}
	}

}