using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiProxy.Models
{
    public class Listado
    {
        public string CodigoExterno { get; set; }
        public string Nombre { get; set; }
        public int CodigoEstado { get; set; }
        public DateTime FechaCierre { get; set; }
        public string Rubro { get; set; }
        public int Estado { get; set; }
        public int DiasPorVencer { get; set; }
        public DateTime FechaPublicacion { get; set; }
    }
}