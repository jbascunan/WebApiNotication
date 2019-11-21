using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebInvocadorDummy.Models
{
	public class Document
	{
		public string type { get; set; }
		public string filename { get; set; }
		public string url { get; set; }
		public string base_64 { get; set; }
	}
}