using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebApiProxy.Models
{
    public class WebApiRequestBase
    {
        [JsonProperty(PropertyName = "version")]
        public string Version { set; get; }
        [JsonProperty(PropertyName = "status")]
        public string Status { set; get; }
        [JsonProperty(PropertyName = "status_detail")]
        public string StatusDetail { set; get; }
    }

    public class RegistroExpedienteRequest : WebApiRequestBase
    {
        [JsonProperty(PropertyName = "ms_entry_id")]
        public int EntryId { set; get; }
        [JsonProperty(PropertyName = "service_application_id")]
        public string ServiceApplicationId { set; get; }
    }

    public class NotificacionEstadoExpedienteRequest
    {
        [JsonIgnore]
        [JsonProperty(PropertyName = "id")]
        public int Id { set; get; }

        [JsonIgnore]
		[JsonProperty(PropertyName = "ms_application_id")]
        public int? MsApplicationId { set; get; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "simple_id")]
        public int? SimpleId { set; get; }

        [JsonProperty(PropertyName = "service_application_id")]
        public string ServiceApplicationId { set; get; }

        [JsonProperty(PropertyName = "documents")]
        public string Documents { set; get; }

        [JsonProperty(PropertyName = "description")]
		public string Description { set; get; }

        [JsonProperty(PropertyName = "message")]
		public string Message { set; get; }

        [JsonProperty(PropertyName = "stage")]
		public string Stage { set; get; }

        [JsonProperty(PropertyName = "stage_code")]
		public string StageCode { set; get; }

        [JsonProperty(PropertyName = "status")]
		public string Status { set; get; }

        [JsonProperty(PropertyName = "status_code")]
		public string StatusCode { set; get; }

        [JsonProperty(PropertyName = "token")]
        public string Token { get { return ConfigurationManager.AppSettings["token_simple"]; } }

        [JsonProperty(PropertyName = "responsible")]
        public string Responsible { set; get; }

        [JsonProperty(PropertyName = "extra_data")]
        public string ExtraData { set; get; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "procesado_super")]
        public int? ProcesadoSuper { set; get; }

        [JsonIgnore]
        [JsonProperty(PropertyName = "procesado_simple")]
        public int? ProcesadoSimple { set; get; }
    }

    public enum NotityType
    {
        SUPER,
        SIMPLE
    }
}