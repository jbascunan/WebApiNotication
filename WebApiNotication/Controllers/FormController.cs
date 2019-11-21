using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Http;
using System.Configuration;
using Mvc = System.Web.Mvc;
using WebApiProxy.Utils;
using System.Web;
using WebApiProxy.Security.Jwt;
using NLog;
using WebApiProxy.Models.Fec;
using WebApiProxy.Models.Form;

namespace WebApiProxy.Controllers
{
    public class FormController : ApiController
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        // GET api/v1/form/5
        public string Get(int id)
        {
            return id.ToString();
        }

        // POST api/v1/form
        [TokenAuthorize]
        [Route("v1/form/{id}")]
        [HttpPost]
        public IHttpActionResult Post(string id, [FromBody]FecRequest fecRequest, bool isReturnHtml = false)
        {
            //validaciones request y formulario solicitado
            if (string.IsNullOrEmpty(id))
            {
                return GetResult(HttpStatusCode.BadRequest, "ID de formulario no válido. " + id);
            }
            var uri = ConfigurationManager.AppSettings["frmID_" + id.ToUpper()];
            if (string.IsNullOrEmpty(uri))
            {
                return GetResult(HttpStatusCode.BadRequest, "ID de formulario no válido. " + id);
            }
            else if (!IsValidUrl(uri))
            {
                logger.Error("URL '{0}' NO RESPONDE", uri);
                return GetResult(HttpStatusCode.NotFound, string.Format("Url '{0}' no responde", uri));
            }

            string message;
            if (!IsValidRequest(ref fecRequest, out message))
            {
                return GetResult(HttpStatusCode.BadRequest, message);
            }

            var formRequest = MapToFormRequest(ref fecRequest);
            //expiracion token
            formRequest.DateToken = DateTime.Now.AddMinutes(Convert.ToDouble(ConfigurationManager.AppSettings["vigenciaTokenRequest"]));

            var qs = string.Empty;
            try
            {
                var jsonText = Newtonsoft.Json.JsonConvert.SerializeObject(formRequest);
                var xHash = new xHash(ConfigurationManager.AppSettings["xHash.semilla"]);
                var dataEncrypt = xHash.EncryptData(jsonText);
                qs = HttpUtility.UrlEncode(dataEncrypt);

                if (isReturnHtml)
                {
                    string currentUri = HttpContext.Current.Request.Url.AbsoluteUri.Split('?')[0];
                    var uriView = currentUri.Replace("api/v1/form/" + id, "FormView/Index");

                    //view desde mismo controller en iframe
                    return Redirect(uriView + "?qs=" + qs + "&uri=" + uri);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Error en proceso de Serialización");
                return GetResult(HttpStatusCode.InternalServerError, string.Empty);
            }

            return GetResult(HttpStatusCode.OK, (uri + "?qx=" + qs));
        }

        [NonAction]
        private bool IsValidRequest(ref FecRequest fecRequest, out string message)
        {
            message = string.Empty;

            if ((fecRequest == null) || (fecRequest.ms_data.company == null) || 
                (fecRequest.ms_data.ms_entry_id == 0 && string.IsNullOrEmpty(fecRequest.ms_data.company.rut) 
                && (fecRequest.ms_data.project == null || string.IsNullOrEmpty(fecRequest.ms_data.project.cup))) )
            {
                message = "JSON de entrada no válido, no cumple con los datos mínimos. Debe especificar al menos el rut empresa";
                return false;
            }
            if (fecRequest.ms_data.ms_entry_id < 0)
            {
                message = "Entry_ID no válido. " + fecRequest.ms_data.ms_entry_id;
                return false;
            }
            

            var rut = Util.ToRutFormat(fecRequest.ms_data.company.rut);

            if (string.IsNullOrEmpty(rut))
            {
                message = "Rut de entrada no válido. " + fecRequest.ms_data.company.rut;
                return false;
            }

            fecRequest.ms_data.company.rut = rut;
            return true;
        }

        [NonAction]
        private IHttpActionResult GetResult(HttpStatusCode statusCode, string value)
        {
            if (statusCode == HttpStatusCode.OK)
            {
                return Content(statusCode, new { status = "OK", url = value });
            }
            if (statusCode == HttpStatusCode.InternalServerError)
            {
                return Content(statusCode, new { status = "ERROR", url = "Ha ocurrido un error interno en la aplicación. Intente más tarde." });
            }

            logger.Warn(value);

            return Content(statusCode, new { status = "ERROR", message = value });

        }

        [NonAction]
        private FormRequest MapToFormRequest(ref FecRequest fecRequest)
        {
            return new FormRequest
            {
                CUP = fecRequest.ms_data.cup,
                EntryID = fecRequest.ms_data.ms_entry_id,
				procedure_id = fecRequest.procedure_id,
                simple_id = fecRequest.simple_id,
                company = new FormRequest.Company
                {
                    name = fecRequest.ms_data.company.name,
                    rut = fecRequest.ms_data.company.rut,
                    
                    contact = fecRequest.ms_data.company.contact == null ? new FormRequest.Contact() : new FormRequest.Contact
                    {
                        address = fecRequest.ms_data.company.contact.address,
                        email = fecRequest.ms_data.company.contact.email,
                        maternal_last_name = fecRequest.ms_data.company.contact.first_name,
                        paternal_last_name = fecRequest.ms_data.company.contact.last_name,
                        name = fecRequest.ms_data.company.contact.first_name + fecRequest.ms_data.company.contact.last_name,
                        phone = fecRequest.ms_data.company.contact.phone,
                        rut = fecRequest.ms_data.company.contact.rut
                    }
                },
                proyect = fecRequest.ms_data.project == null ? new FormRequest.Proyect() : new FormRequest.Proyect
                {
                    code = fecRequest.ms_data.project.cup,
					map_coordinates = fecRequest.ms_data.project.coords ?? new List<float>(),
					name = fecRequest.ms_data.project.name
				},
                legal_representative = fecRequest.ms_data.company.legal_representative == null ? new FormRequest.LegalRepresentative() : new FormRequest.LegalRepresentative
                {
                    address = fecRequest.ms_data.company.legal_representative.address,
                    email = fecRequest.ms_data.company.legal_representative.email,
                    name = fecRequest.ms_data.company.legal_representative.first_name,
					first_name = fecRequest.ms_data.company.legal_representative.first_name,
                    last_name = fecRequest.ms_data.company.legal_representative.last_name,
                    phone = fecRequest.ms_data.company.legal_representative.phone,
                    rut = fecRequest.ms_data.company.legal_representative.rut
                },
				petitioner = fecRequest.ms_data.petitioner == null ? new FormRequest.Petitioner() : new FormRequest.Petitioner
				{
					rut = fecRequest.ms_data.petitioner.rut,
					first_name = fecRequest.ms_data.petitioner.first_name,
					last_name = fecRequest.ms_data.petitioner.last_name,
					email = fecRequest.ms_data.petitioner.emails.email_ms
				}

			};
        }

        [NonAction]
        private bool IsValidUrl(string url)
        {
            try
            {
                var request = WebRequest.Create(url);
                request.Timeout = int.Parse(ConfigurationManager.AppSettings["timeout_validar_Url"]);
                request.Method = "HEAD";

                using (var response = (HttpWebResponse)request.GetResponse())
                {
                    response.Close();
                    return response.StatusCode == HttpStatusCode.OK;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}