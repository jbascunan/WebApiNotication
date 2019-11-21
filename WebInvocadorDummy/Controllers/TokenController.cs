using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net;
using WebApiProxy.Utils;

namespace WebInvocadorDummy.Controllers
{
	public class TokenController : Controller
	{
		public ActionResult Index()
		{		
			return View();
		}

		public ActionResult SimulacionToken()
		{
			ViewBag.Message = "Your application description page.";

			const string userName = "sngm";
			const string password = "4Najtf3lWe14rUL";
			const string apiBaseUri = "http://localhost/WebApiProxy/api";

			ViewBag.Token = GetAPITokenBasic(userName, password, apiBaseUri);
			//ViewBag.Token = GetToken();
			return View();
		}

		private static string GetAPITokenBasic(string userName, string password, string apiBaseUri)
		{

			//For Basic Authentication
			string authInfo = userName + ":" + password;
			authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiBaseUri + "/proxy/get-token-super");
			request.Method = "GET";
			request.Accept = "application/json; charset=utf-8";
			//request.Proxy = proxy;

			request.Headers["Authorization"] = "Basic " + authInfo;

			var response = (HttpWebResponse)request.GetResponse();

			string strResponse = "";
			using (var sr = new System.IO.StreamReader(response.GetResponseStream()))
			{
				strResponse = sr.ReadToEnd();

			}

			return strResponse;
		
		}		

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
	}
}