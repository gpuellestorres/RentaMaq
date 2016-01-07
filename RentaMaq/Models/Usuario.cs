using RentaMaq.DAL;
using sarey_erp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class Usuario
    {
        public int usuarioID { get; set; }
        [Display(Name="Nombre de Usuario")]
        public string nombreUsuario { get; set; }
        [Display(Name = "Nombre Completo")]
        public string nombreCompleto { get; set; }
        [Display(Name = "Correo Electrónico")]
        public string correoElectronico { get; set; }
        public string password { get; set; }


        internal void actualizar()
        {
            SqlConnection con = conexion.crearConexion();
            con.Open();

            using (SqlCommand command = new SqlCommand("UPDATE Usuario SET "
                + "nombreUsuario=@nombreUsuario, nombreCompleto=@nombreCompleto,"
                + " correoElectronico=@correoElectronico, password=@password"
                + " WHERE usuarioID=@usuarioID", con))
            {
                command.Parameters.Add("@nombreUsuario", SqlDbType.NVarChar).Value = this.nombreUsuario;
                command.Parameters.Add("@nombreCompleto", SqlDbType.NVarChar).Value = this.nombreCompleto;
                command.Parameters.Add("@correoElectronico", SqlDbType.NVarChar).Value = this.correoElectronico;
                command.Parameters.Add("@password", SqlDbType.NVarChar).Value = this.password;
                command.Parameters.Add("@usuarioID", SqlDbType.Int).Value = this.usuarioID;

                command.ExecuteNonQuery();
            }
            con.Close();
        }

        internal static int obtenerIDPorNombre(string p)
        {
            Context db = new Context();
            List<Usuario> usuario = db.Usuarios.Where(s => s.nombreUsuario == p).ToList();

            if (usuario.Count > 0) return usuario[0].usuarioID;
            else return -1;
        }

        internal static bool revisarUsuarioPassword(string p1, string p2)
        {
            Context db = new Context();
            List<Usuario> usuario = db.Usuarios.Where(s => s.nombreUsuario == p1 && s.password==p2).ToList();

            if (usuario.Count > 0) return true;
            else return false;
        }
    }
}