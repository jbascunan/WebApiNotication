using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using WebInvocadorDummy.Models;
using WebInvocadorDummy.Util;

namespace WebInvocadorDummy.Controllers
{
	public class DefaultController : Controller
	{
		// GET: Default
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public void GoForm(string rut, string formCode)
		{

			try
			{
				string jsonAut = new JavaScriptSerializer().Serialize(new
				{
					api_key = "super",
					api_secret = "Cnuafy42OVFJnfV"
				});

				var uriAut = "http://localhost/webapiproxy/api/authenticate";
				//se invoka autenticacion en webapiproxy de sernageomin
				var autResponse = RestClient.GetResponse<AutResponse>(uriAut
					 , RestClient.HttpMethod.POST
					 , jsonAut);

				//emulacion JSON enviado desde FEC
				//string json = "{\"simple_id\":123,\"procedure_id\":234,\"ms_data\":{\"ms_entry_id\":234,\"cup\":\"\",\"beneficiary_type\":\"person\",\"petitioner\":{\"ms_entity\":true,\"rut\":\"11111111-K\",\"first_name\":\"\",\"last_name\":\"\",\"emails\":{\"email_clave_unica\":\"\",\"email_ms\":\"\"}},\"company\":{\"ms_entity\":true,\"rut\":\"78186160-K\",\"name\":\"\",\"documents\":{\"public_deed_of_constitution\":{\"filename\":\"\",\"url\":\"\",\"base_64\":\"\"},\"company_bylaws\":{\"filename\":\"\",\"url\":\"\",\"base_64\":\"\"}},\"contact\":{\"rut\":\"22222-3\",\"first_name\":\"contacto1\",\"last_name\":\"contacto name\",\"email\":\"mail@contacto.cl\",\"address\":\"\",\"phone\":\"\"},\"legal_representative\":{\"rut\":\"5555-3\",\"first_name\":\"claudio\",\"last_name\":\"guerra\",\"email\":\"mail@mail.cl\",\"address\":\"pasaje 1\",\"phone\":\"34234234\"}},\"project\":{\"cup\":\"UU9898\",\"name\":\"test prueba super\",\"coords\":[2.5,5.6,7.8],\"documents\":[{\"type\":\"\",\"filename\":\"\",\"url\":\"\",\"base_64\":\"\"}],\"sector\":\"ase\",\"estimated_start_date\":\"\",\"estimated_end_date\":\"\",\"description\":\"dddd\"}},\"procedure_data\":null}"; ;
				string json = @"{  
				   'simple_id':123,
				   'procedure_id':'55987',
				   'ms_data':{
						'ms_entry_id':5,
						  'cup':'GZLMYP83',
						  'beneficiary_type':'person',
						  'petitioner':{
							'ms_entity':true,
							 'rut':'11111111-K',
							 'first_name':'',
							 'last_name':'',
							 'emails':{
								'email_clave_unica':'',
								'email_ms':''
							 }
										},
						  'company':{
							'ms_entity':true,
							 'rut':'78186160-K',
							 'name':'emrpesa',
							 'documents':{
								'public_deed_of_constitution':{
									'filename':'',
								   'url':'',
								   'base_64':''				
								},
								'company_bylaws':{
									'filename':'',
								   'url':'',
								   'base_64':''

								}
											},
							 'contact':{
								'rut':'44444-8',
								'first_name':'juan',
								'last_name':'rivas',
								'email':'rivas@mail.com',
								'address':'pasaje 34',
								'phone':'24243'

							 },
							 'legal_representative':{
								'rut':'555555555-9',
								'first_name':'carlos',
								'last_name':'cerda',
								'email':'mail@mail.com',
								'address':'casa 344',
								'phone':'23434'

						}
					},
				  'project':{
					'cup':'GZLMYP83',
					 'name':'test super',
					 'coords':[
						2.5,
						5.6,
						7.8
					 ],
					 'documents':[
						{  
						   'type':'txt',
						   'filename':'prueba.txt',
						   'url':'',
						   'base_64':'cHJ1ZWJhZmRzYQ0KYXNkDQpmDQphc2QNCmYNCmFzZmQ='

						}
					 ],
					 'sector':'ase',
					 'estimated_start_date':'',
					 'estimated_end_date':'',
					 'description':'dddd'
				  }
			   },
			   'procedure_data':null
			}";

				string json1 = new JavaScriptSerializer().Serialize(new
				{
					simple_id = 123,
					procedure_id = 234,
					ms_data = new
					{
						ms_entry_id = 234,
						cup = "",
						beneficiary_type = "person",
						petitioner = new
						{
							ms_entity = true,
							rut = "11111111-K",
							first_name = "",
							last_name = "",
							emails = new
							{
								email_clave_unica = "",
								email_ms = ""

							}
						},
						company = new
						{
							ms_entity = true,
							rut = rut,
							name = "",
							documents = new
							{
								public_deed_of_constitution = new
								{
									filename = "",
									url = "",
									base_64 = ""

								},
								company_bylaws = new
								{
									filename = "",
									url = "",
									base_64 = ""

								}
							},
							contact = new
							{
								rut = "11111111-K",
								name = "Luis",
								first_name = "Miguel",
								last_name = "Vidal",
								email = "pepitopagadoble@gmail.com",
								address = "la casa vecina",
								phone = "13132321"
							},
							legal_representative = new
							{
								rut = "22222-3",
								first_name = "representante 1",
								last_name = "apellido",
								email = "representantes@gmail.com",
								address = "La casa del lado #123",
								phone = "569089898987"
							}
						},
						proyect = new
						{
							cup = "TR2002",
							name = "Proyecto Test Integración MS",
							coords = new List<float> { 10.90F, 20.40F, 2.40F },
							documents = new List<WebInvocadorDummy.Models.Document>
							{								
								new WebInvocadorDummy.Models.Document{
								type = "xls",
								filename = "nada",
								url = "",
								base_64 = ""
								}
							},
							sector = "SECTOR AGRICOLA",
							estimated_start_date = "",
							estimated_end_date = "",
							description = "FRUTAS"
						}
					},
					procedure_data = new { }
				});

				//http://localhost/webapiproxy/v1/form/rpm-mayor
				var uri = "http://localhost/webapiproxy/v1/form/" + formCode.ToUpper();
				var headerAuthorization = "Bearer " + autResponse.token;
				//se invoka webapiproxy de sernageomin
				var formResponse = RestClient.GetResponse<FormResponse>(uri
				 , RestClient.HttpMethod.POST
				 , json
				 , headerAuthorization);


				Response.Redirect(formResponse.Url, true);
			}
			catch (Exception)
			{

				throw;
			}



		}

		public ActionResult PruebaRestSharp()
		{
			try
			{
				//var client = new RestSharp.RestClient("http://localhost/WebApiSegura/api/admin/v1/application/1");


				//RestSharp.IRestResponse response = client.Execute(request);
				//request.AddHeader("cache-control", "no-cache");
				//request.AddHeader("Authorization", "Basic c25nbTo0TmFqdGYzbFdlMTRyVUw=");
				//RestSharp.IRestResponse response = client.Execute(request);
				//RestSharp.IRestResponse<WebApiResponseBase> response = client.Execute<WebApiResponseBase>(request);
				//var result = Execute<WebApiResponseBase>(request);

				string uri = "http://localhost/WebApiSegura/api/admin/v1/application/1";
				var objJsonBody = new
				{
					api_key = "superocho",
					api_secret = "Cnuafy42OVFJnfV"
				};

				var result = RestClient.Execute<WebApiResponseBase>(uri, RestSharp.Method.POST, objJsonBody);
				var response = JsonConvert.DeserializeObject<WebApiResponseBase>(result.Content);
				var x = "";
			}
			catch (Exception ex)
			{
				throw;
			}

			return null;
		}


		public async Task<JObject> PosdtAsync(string uri)
		{
			var httpClient = new HttpClient();
			//new StringContent(data)


			var response = httpClient.PostAsync("http://localhost/WebApiSegura/api/admin/prueba/1", null);
			//response.EnsureSuccessStatusCode();
			var x = "nada";

			var content = await response;

			var str = content.Content.ReadAsStringAsync().Result;



			return await Task.Run(() => JObject.Parse(str));
		}

		public JObject PostSync()
		{
			using (var client = new HttpClient())
			{
				var url = "http://localhost/WebApiSegura/api/admin/prueba/1";

				var response = client.PostAsync(url, null).Result;

				if (response.IsSuccessStatusCode)
				{
					// by calling .Result you are performing a synchronous call
					var responseContent = response.Content;

					// by calling .Result you are synchronously reading the result
					string responseString = responseContent.ReadAsStringAsync().Result;

					return JObject.Parse(responseString);
				}

				return JObject.Parse("NOOK");
			}
		}

		public JObject PruebaHttp()
		{

			var uri = "http://localhost/WebApiSegura/api/admin/prueba/1";

			var formResponse = RestClient.GetResponse<FormResponse>(uri
			 , RestClient.HttpMethod.POST
			 , null
			 , null);

			return JObject.Parse("NOOK");

		}

		public class FormResponse
		{
			//[JsonProperty(PropertyName = "statusCode")]
			//public int StatusCode { set; get; }
			[JsonProperty(PropertyName = "status")]
			public string Status { set; get; }
			[JsonProperty(PropertyName = "message")]
			public string Message { set; get; }
			[JsonProperty(PropertyName = "url")]
			public string Url { set; get; }
		}

		public class AutResponse
		{
			public string token { set; get; }
			public DateTime expiration_date { set; get; }
		}
	}
}