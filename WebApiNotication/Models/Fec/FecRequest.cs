using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiProxy.Models.Fec
{
	public class FecRequest
	{
		public int simple_id { get; set; }
		public string procedure_id { get; set; }
		public MsData ms_data { get; set; }
		public object procedure_data { get; set; }
	}

	public class MsData
	{
		public int ms_entry_id { get; set; }
        private string _cup;
		public string cup {
            get
            {
                return _cup;
            }
            set {
                this._cup = value == "null" ? null : value;
                
            }
        }
		public string beneficiary_type { get; set; }
		public Petitioner petitioner { get; set; }
		public Company company { get; set; }
		public Project project { get; set; }
	}

	public class Project
	{
		public string name { get; set; }
		public List<float> coords { get; set; }
		public List<Document> documents { get; set; }
		public string sector { get; set; }
		public string estimated_start_date { get; set; }
		public string estimated_end_date { get; set; }
		public string description { get; set; }
        private string _cup;
        public string cup
        {
            get
            {
                return _cup;
            }
            set
            {
                this._cup = value == "null" ? null : value;

            }
        }
    }

	public class Emails
	{
		public string email_clave_unica { get; set; }
		public string email_ms { get; set; }
	}

	public class Petitioner
	{
		public bool ms_entity { get; set; }
		public string rut { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public Emails emails { get; set; }
	}

	public class PublicDeedOfConstitution
	{
		public string filename { get; set; }
		public string url { get; set; }
		public string base_64 { get; set; }
	}

	public class CompanyBylaws
	{
		public string filename { get; set; }
		public string url { get; set; }
		public string base_64 { get; set; }
	}


	public class Company
	{
		public bool ms_entity { get; set; }
		public string rut { get; set; }
		public string name { get; set; }
		public Documents documents { get; set; }
		public Contact contact { get; set; }
		public LegalRepresentative legal_representative { get; set; }
	}

	public class Documents
	{
		public PublicDeedOfConstitution public_deed_of_constitution { get; set; }
		public CompanyBylaws company_bylaws { get; set; }
	}

	public class Contact
	{
		public string rut { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public string email { get; set; }
		public string address { get; set; }
		public string phone { get; set; }
	}

	public class LegalRepresentative
	{
		public string rut { get; set; }
		public string first_name { get; set; }
		public string last_name { get; set; }
		public string email { get; set; }
		public string address { get; set; }
		public string phone { get; set; }
	}

	public class Document
	{
		public string type { get; set; }
		public string filename { get; set; }
		public string url { get; set; }
		public string base_64 { get; set; }
	}


}