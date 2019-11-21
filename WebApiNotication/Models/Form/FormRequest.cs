
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiProxy.Models.Form
{
	/// <summary>
	/// Contexto de entrada formularios RPM
	/// </summary>
	[Serializable]
	public class FormRequest
	{
		[JsonProperty(PropertyName = "ms_entry_id")]
		public int EntryID { set; get; }
		[JsonProperty(PropertyName = "ms_cup")]
		public string CUP { set; get; }
		[JsonProperty(PropertyName = "ms_token_date")]
		public DateTime DateToken { set; get; }

		public int simple_id { set; get; }
		public string procedure_id { set; get; }

		public Company company { get; set; }
		public LegalRepresentative legal_representative { get; set; }
		public Proyect proyect { get; set; }
		public Petitioner petitioner { get; set; }

		[Serializable]
		public class Contact
		{
			public string rut { get; set; }
			public string name { get; set; }
			public string paternal_last_name { get; set; }
			public string maternal_last_name { get; set; }
			public string email { get; set; }
			public string address { get; set; }
			public string phone { get; set; }
		}

		[Serializable]
		public class Company
		{
			public bool valid { get; set; }
			public string rut { get; set; }
			public string name { get; set; }
			//public List<Document> documents { get; set; }
			public Contact contact { get; set; }
		}

		[Serializable]
		public class LegalRepresentative
		{
			public string rut { get; set; }
			public string name { get; set; }
			public string first_name { get; set; }
			public string last_name { get; set; }
			public string email { get; set; }
			public string address { get; set; }
			public string phone { get; set; }
		}

		[Serializable]
		public class Proyect
		{
			public bool valid { get; set; }
			public string code { get; set; }
			public string name { get; set; }
			public List<float> map_coordinates { get; set; }
		}

		[Serializable]
		public class Petitioner
		{
			public string rut { get; set; }
			public string first_name { get; set; }
			public string last_name { get; set; }
			public string email { get; set; }
		}
	}
}