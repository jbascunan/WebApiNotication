using Newtonsoft.Json;
using System;

namespace WebApiProxy.Models
{
    public class LoginRequest
    {
		[JsonProperty(PropertyName = "api_key")]
		public string Api_key { get; set; }
		[JsonProperty(PropertyName = "api_secret")]
		public string Api_Secret { get; set; }
    }
}