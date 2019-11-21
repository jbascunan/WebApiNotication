using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiProxy.Models
{
	public class Token
	{
		public string token { get; set; }
		public DateTime expiration_date { get; set; }
	}
}