using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiProxy.Models
{
    public class Licitaciones
    {
        public int Cantidad { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Version { get; set; }
        public Listado[] Listado { get; set; }
    }
}