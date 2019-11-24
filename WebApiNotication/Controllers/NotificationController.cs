using FcmSharp;
using FcmSharp.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Web.Http;
using WebApiProxy.Models;
using WebApiProxy.Repository;
using WebApiProxy.Utils;

namespace WebApiProxy.Controllers
{
    [AllowAnonymous]
    public class NotificationController : ApiController
    {
        [HttpPost]
        [Route("send")]
        public IHttpActionResult Send(dynamic data)
        {
            /*
            var client = new RestClient("https://fcm.googleapis.com/fcm/send");
            var request = new RestRequest(Method.POST);
            request.AddHeader("Postman-Token", "4caff8c1-b538-4ada-857b-bfb1ea1a5d37");
            request.AddHeader("cache-control", "no-cache");
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("Authorization", "key=AAAAyGO93io:APA91bFN8E6ZEsM7BN02Oof6mSIIyO6wWrdpgOAz-maDpM6MBOK1oZrT3aXldz_-KGfH2F7pmwn9KdkI1Nuq_5O43aNhlNvAvth1QME0gBRe9vLL44V-ZE01YI5AmBUUicyhjTPE_d7P");
            request.AddParameter("undefined", "{  \r\n   \"to\":\"cVkieKkkV9M:APA91bETHwW2K9LbK9vUe3faL6TfgUmSZ0Vzm7GXAjxtC24zpmTdr00-4WqnIj5rkvP4ppzF7chEqqh0ZReOSYvqPXFRbwnB2RDpthrCjhxi11u9c9zs4lXL5EoVkeZF9W5dLR4E_iHw\",\r\n   \"data\":{  \r\n      \"ShortDesc\":\"Some short desc\",\r\n      \"IncidentNo\":\"any number\",\r\n      \"Description\":\"detail desc\"\r\n   },\r\n   \"notification\":{  \r\n      \"title\":\"ServiceNow: Incident No. number\",\r\n      \"text\":\"This is Notification\",\r\n      \"sound\":\"default\",\r\n      \"color\": \"#rrggbb\"\r\n   }\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            */

            // read file into a string and deserialize JSON to a type
            Licitaciones licitaciones = new Licitaciones();
            string pathFile = @"D:\Proyectos_Ionic\WebApiNotication\WebApiNotication\Files\licitaciones" + data.fecha + ".json";
            using (StreamReader r = new StreamReader(pathFile, Encoding.Default))
            {
                string json = r.ReadToEnd();
                licitaciones = JsonConvert.DeserializeObject<Licitaciones>(json);
            }
           

            var filtro = licitaciones.Listado.Where(p => p.Nombre.ToLower().Contains("construcción") || p.Nombre.ToLower().Contains("construccion")).ToList();
            
           NotificacionRepository.Update();
           foreach (var licitacion in filtro)
           {
               NotificacionRepository.Insert(licitacion);
           }
            /*

            string uri = "https://fcm.googleapis.com/fcm/send";
            string autorization = "key=AAAAyGO93io:APA91bFN8E6ZEsM7BN02Oof6mSIIyO6wWrdpgOAz-maDpM6MBOK1oZrT3aXldz_-KGfH2F7pmwn9KdkI1Nuq_5O43aNhlNvAvth1QME0gBRe9vLL44V-ZE01YI5AmBUUicyhjTPE_d7P";

            var objJsonBody = new
            {
                to = "cVkieKkkV9M:APA91bETHwW2K9LbK9vUe3faL6TfgUmSZ0Vzm7GXAjxtC24zpmTdr00-4WqnIj5rkvP4ppzF7chEqqh0ZReOSYvqPXFRbwnB2RDpthrCjhxi11u9c9zs4lXL5EoVkeZF9W5dLR4E_iHw",
                data = new
                {
                    ShortDesc = "Some short desc",
                    IncidentNo = "any number",
                    Description = "detail desc"
                },
                notification = new
                {
                    title = "App Licitaciones Sugeridas",
                    text = "Ud. tiene " + filtro.Count + " nuevas oportunidades de negocio en mercado público",
                    sound = "default",
                    color = "#rrggbb"
                }
            };


            var result = Utils.RestClient.Execute<dynamic>(uri, RestSharp.Method.POST, JsonConvert.SerializeObject(objJsonBody), autorization);
            var response = JsonConvert.DeserializeObject<dynamic>(result.Content);
            */

            return Ok();
        }

        [HttpGet]
        [Route("getLicitaciones")]
        public IHttpActionResult getLicitaciones(string rut)
        {
            return Ok(NotificacionRepository.GetLicitaciones());
        }
    }
}
