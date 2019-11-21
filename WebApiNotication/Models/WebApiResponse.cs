using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiProxy.Models
{
    public class WebApiResponseBase
    {
        [JsonProperty(PropertyName = "status")]
        public string Status { set; get; }
        [JsonProperty(PropertyName = "message")]
        public string Message { set; get; }
    }

    public class RegistroExpedienteResponse : WebApiResponseBase
    {
        [JsonProperty(PropertyName = "CUP")]
        public string CUP { set; get; }
        [JsonProperty(PropertyName = "ms_application_id")]
        public int ApplicationId { set; get; }
    }

	public static class Status
	{
		public const string OK = "OK";
		public const string ERROR = "ERROR";
	}
}