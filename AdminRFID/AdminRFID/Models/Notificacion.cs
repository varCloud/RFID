using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AdminRFID.Models
{
    public class Notificacion<T> where T : class
    {
        public int Estatus { get; set; }
        public string Mensaje { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public T Modelo { get; set; }
    }
}