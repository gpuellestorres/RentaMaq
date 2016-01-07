using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentaMaq.DAL;

namespace RentaMaq.Models
{
    public class detalleEquipo
    {
        public int detalleEquipoID { get; set; }
        public int EquipoID { set; get; }
        public DateTime inicioCertificacion { get; set; }
        public DateTime terminoCertificacion { set; get; }
        public DateTime revisionTecnica { set; get; }
        public DateTime permisoCirculacion { set; get; }
        public DateTime seguro { set; get; }
        public string proveedor { set; get; }


        //CERTIFICACIONES VENCIDAS
        public static int cantidadEquiposCertificacionVencida()
        {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(10);
            DateTime fechaIgnorar = new DateTime(2000, 1, 1);
            var L2EQueryCant = db.detalleEquipos.Where(s => s.terminoCertificacion <= fechaActual & s.terminoCertificacion !=fechaIgnorar).Count();
            int retorno = Convert.ToInt32(L2EQueryCant);            
            return retorno;
        }

        public static int cantidadEquiposCertificacionVencidaUrgente()
        {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(5);
            DateTime fechaIgnorar = new DateTime(2000, 1, 1);
            var L2EQueryCant = db.detalleEquipos.Where(s => s.terminoCertificacion <= fechaActual & s.terminoCertificacion != fechaIgnorar).Count();
            int retorno = Convert.ToInt32(L2EQueryCant);
            return retorno;
        }

        public static List<equipos> equiposCertificacionVencida()
        {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(10);
            DateTime fechaIgnorar = new DateTime(2000, 1, 1);
            var L2EQuery = db.detalleEquipos.Where(s => s.terminoCertificacion <= fechaActual & s.terminoCertificacion!=fechaIgnorar);
            List<detalleEquipo> detalleEquipo = L2EQuery.ToList();
            List<equipos> listaEquipos = new List<equipos>();
            foreach (var detalle in detalleEquipo)
            {
                var queryEquipo = db.Equipos.SingleOrDefault(s => s.ID == detalle.EquipoID);
                listaEquipos.Add((equipos)queryEquipo);

            }
            return listaEquipos;
        }

        public static List<detalleEquipo> detalleEquiposCertificacionVencida()
        {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(10);
            DateTime fechaIgnorar = new DateTime(2000, 1, 1);
            var L2EQuery = db.detalleEquipos.Where(s => s.terminoCertificacion<= fechaActual & s.terminoCertificacion!=fechaIgnorar);
            List<detalleEquipo> detalleEquipo = L2EQuery.ToList();
            return detalleEquipo;           
        }

        //REVISION TECNICA
        public static int cantidadEquiposRevisionTecnicaVencida()
        {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(10);
            DateTime fechaIgnorar = new DateTime(2000, 1, 1);
            var L2EQueryCant = db.detalleEquipos.Where(s => s.revisionTecnica <= fechaActual & s.revisionTecnica != fechaIgnorar).Count();
            int retorno = Convert.ToInt32(L2EQueryCant);
            return retorno;
        }

        public static int cantidadEquiposRevisionTecnicaVencidaUrgente()
        {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(5);
            DateTime fechaIgnorar = new DateTime(2000, 1, 1);
            var L2EQueryCant = db.detalleEquipos.Where(s => s.revisionTecnica <= fechaActual & s.revisionTecnica != fechaIgnorar).Count();
            int retorno = Convert.ToInt32(L2EQueryCant);
            return retorno;
        }

        public static List<equipos> equiposRevisionTecnicaVencida()
        {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(10);
            DateTime fechaIgnorar = new DateTime(2000, 1, 1);
            var L2EQuery = db.detalleEquipos.Where(s => s.revisionTecnica <= fechaActual & s.revisionTecnica != fechaIgnorar);
            List<detalleEquipo> detalleEquipo = L2EQuery.ToList();
            List<equipos> listaEquipos = new List<equipos>();
            foreach (var detalle in detalleEquipo)
            {
                var queryEquipo = db.Equipos.SingleOrDefault(s => s.ID == detalle.EquipoID);
                listaEquipos.Add((equipos)queryEquipo);

            }
            return listaEquipos;
        }

        public static List<detalleEquipo> detalleEquiposRevisionTecnicaVencida()
        {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(10);
            DateTime fechaIgnorar = new DateTime(2000, 1, 1);
            var L2EQuery = db.detalleEquipos.Where(s => s.revisionTecnica <= fechaActual & s.revisionTecnica != fechaIgnorar);
            List<detalleEquipo> detalleEquipo = L2EQuery.ToList();
            return detalleEquipo;

        }

        //PERMISO CIRCULACION
        public static int cantidadEquiposPermisoCirculacionVencida()
        {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(10);
            DateTime fechaIgnorar = new DateTime(2000, 1, 1);
            var L2EQueryCant = db.detalleEquipos.Where(s => s.permisoCirculacion <= fechaActual & s.permisoCirculacion != fechaIgnorar).Count();
            int retorno = Convert.ToInt32(L2EQueryCant);
            return retorno;
        }

        public static int cantidadEquiposPermisoCirculacionVencidaUrgente()
        {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(5);
            DateTime fechaIgnorar = new DateTime(2000, 1, 1);
            var L2EQueryCant = db.detalleEquipos.Where(s => s.permisoCirculacion <= fechaActual & s.permisoCirculacion != fechaIgnorar).Count();
            int retorno = Convert.ToInt32(L2EQueryCant);
            return retorno;
        }

        public static List<equipos> equiposPermisoCirculacionVencida()
        {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(10);
            DateTime fechaIgnorar = new DateTime(2000, 1, 1);
            var L2EQuery = db.detalleEquipos.Where(s => s.permisoCirculacion <= fechaActual & s.permisoCirculacion != fechaIgnorar);
            List<detalleEquipo> detalleEquipo = L2EQuery.ToList();
            List<equipos> listaEquipos = new List<equipos>();
            foreach (var detalle in detalleEquipo)
            {
                var queryEquipo = db.Equipos.SingleOrDefault(s => s.ID == detalle.EquipoID);
                listaEquipos.Add((equipos)queryEquipo);

            }
            return listaEquipos;
        }

        public static List<detalleEquipo> detalleEquiposPermisoCirculacionVencida()
        {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(10);
            DateTime fechaIgnorar = new DateTime(2000, 1, 1);
            var L2EQuery = db.detalleEquipos.Where(s => s.permisoCirculacion <= fechaActual & s.permisoCirculacion != fechaIgnorar);
            List<detalleEquipo> detalleEquipo = L2EQuery.ToList();
            return detalleEquipo;

        }

        //SEGURO
        public static int cantidadSeguroVencido()
        {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(10);
            DateTime fechaIgnorar = new DateTime(2000, 1, 1);
            var L2EQueryCant = db.detalleEquipos.Where(s => s.seguro <= fechaActual & s.seguro != fechaIgnorar).Count();
            int retorno = Convert.ToInt32(L2EQueryCant);
            return retorno;
        }

        public static int cantidadSeguroVencidoUrgente()
        {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(5);
            DateTime fechaIgnorar = new DateTime(2000, 1, 1);
            var L2EQueryCant = db.detalleEquipos.Where(s => s.seguro <= fechaActual & s.seguro != fechaIgnorar).Count();
            int retorno = Convert.ToInt32(L2EQueryCant);
            return retorno;
        }

        public static List<equipos> equiposSeguroVencido()
        {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(10);
            DateTime fechaIgnorar = new DateTime(2000, 1, 1);
            var L2EQuery = db.detalleEquipos.Where(s => s.seguro <= fechaActual & s.seguro != fechaIgnorar);
            List<detalleEquipo> detalleEquipo = L2EQuery.ToList();
            List<equipos> listaEquipos = new List<equipos>();
            foreach (var detalle in detalleEquipo)
            {
                var queryEquipo = db.Equipos.SingleOrDefault(s => s.ID == detalle.EquipoID);
                listaEquipos.Add((equipos)queryEquipo);

            }
            return listaEquipos;
        }

        public static List<detalleEquipo> detalleEquiposSeguroVencido()
        {
            Context db = new Context();
            DateTime fechaActual = DateTime.Now.AddDays(10);
            DateTime fechaIgnorar = new DateTime(2000,1,1);
            var L2EQuery = db.detalleEquipos.Where(s => s.seguro <= fechaActual & s.seguro != fechaIgnorar);
            List<detalleEquipo> detalleEquipo = L2EQuery.ToList();
            return detalleEquipo;

        }
    }   
}