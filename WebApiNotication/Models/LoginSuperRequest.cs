using Newtonsoft.Json;
using System;

namespace WebApiProxy.Models
{
    public class LoginSuperRequest
	{
		[JsonProperty(PropertyName = "api_key")]
		public string Api_key { get; set; }
		[JsonProperty(PropertyName = "chile_atiende_id")]
		public string ChileAtiendeId { get; set; }
    }
}