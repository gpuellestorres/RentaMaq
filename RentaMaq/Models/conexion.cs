using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class conexion
    {
        public static SqlConnection crearConexion()
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.
                ConnectionStrings["Context"].ConnectionString);
            return con;
        }

    }
}