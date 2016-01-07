using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class Proveedor
    {
        public int ProveedorID { set; get; }
        public string nombreProveedor { set; get; }
        public string requerimiento { set; get; }
        public string personaContacto1 { set; get; }
        public string personaContacto2 { set; get; }
        public string telefonos { set; get; }
        public string correo { set; get; }

        public string rut { set; get; }
        public string domicilio { set; get; }

    }
}