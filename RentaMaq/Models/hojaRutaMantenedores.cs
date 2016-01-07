using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class hojaRutaMantenedores
    {
        public int hojaRutaMantenedoresID { get; set; }
        [Display(Name="Fecha")]
        public DateTime fecha { get; set; }
        [Display(Name = "Nombre mantenedor (es)")]
        public string nombreMantenedor { get; set; }
        [Display(Name = "Equipo")]
        public int equipoID { get; set; }
        [Display(Name = "Lugar")]
        public string lugar { get; set; }
        [Display(Name = "Horómetro")]
        public int horometro { get; set; }
        [Display(Name = "Trabajo realizado")]
        public string trabajoRealizado { get; set; }
        [Display(Name = "Número")]
        public int numero { get; set; }

        internal static int obtenerUltimoHorometro(int equipoID)
        {
            Context db = new Context();
            List<hojaRutaMantenedores> datos = db.hojaRutaMantenedores.Where(s => s.equipoID == equipoID).OrderByDescending(s=>s.fecha).Take(1).ToList();
            if (datos.Count > 0) return datos[0].horometro;
            else return 0;
        }

        public static object obtenerHorometroAnteriorLubricacion(int equipoID)
        {
            return obtenerUltimoHorometro(equipoID);
        }

        internal static int obtenerNuevoNumero()
        {
            Context db = new Context();
            List<hojaRutaMantenedores> lista = db.hojaRutaMantenedores.OrderByDescending(s => s.numero).Take(1).ToList();

            if (lista.Count == 0) return 1;
            return lista[0].numero + 1;
        }

        public static int obtenerHorometroAnteriorLubricacion(int equipoID, DateTime fecha)
        {
            Context db = new Context();
            List<hojaRutaMantenedores> datos = 
                db.hojaRutaMantenedores.Where(s => s.equipoID == equipoID && s.fecha<fecha).OrderByDescending(s => s.fecha).Take(1).ToList();
            if (datos.Count > 0) return datos[0].horometro;
            else return 0;
        }

        internal static void eliminar(int numero)
        {
            Context db = new Context();

            foreach (hojaRutaMantenedores eliminar in db.hojaRutaMantenedores.Where(s => s.numero == numero).ToList()) 
            {
                db.hojaRutaMantenedores.Remove(eliminar);
            }

            db.SaveChanges();
        }
    }

    public class hojaRutaMantenedoresReporte 
    {
        public string fecha { get; set; }
        public string equipo { get; set; }
        public string horometroAnterior { get; set; }
        public string numero { get; set; }
        public string mantenedores { get; set; }
        public string lugar { get; set; }
        public string horometro { get; set; }
        public string trabajoRealizado { get; set; }


        public static List<hojaRutaMantenedoresReporte> convertirDatosAntesDeRegistro(List<hojaRutaMantenedores> entrada)
        {
            List<hojaRutaMantenedoresReporte> retorno = new List<hojaRutaMantenedoresReporte>();

            foreach (hojaRutaMantenedores HR in entrada)
            {
                hojaRutaMantenedoresReporte nueva = new hojaRutaMantenedoresReporte();

                equipos Equipo = equipos.Obtener(HR.equipoID);

                nueva.equipo = Equipo.numeroAFI + "\n" + Equipo.patenteEquipo;
                nueva.horometroAnterior = hojaRutaMantenedores.obtenerHorometroAnteriorLubricacion(Equipo.ID).ToString();

                retorno.Add(nueva);
            }

            return retorno;
        }

        internal static List<hojaRutaMantenedoresReporte> convertirDatos(List<hojaRutaMantenedores> entrada)
        {
            List<hojaRutaMantenedoresReporte> retorno = new List<hojaRutaMantenedoresReporte>();

            foreach (hojaRutaMantenedores HR in entrada)
            {
                hojaRutaMantenedoresReporte nueva = new hojaRutaMantenedoresReporte();

                equipos Equipo = equipos.Obtener(HR.equipoID);

                nueva.equipo = Equipo.numeroAFI + "\n" + Equipo.patenteEquipo;

                nueva.horometroAnterior = hojaRutaMantenedores.obtenerHorometroAnteriorLubricacion(HR.equipoID, HR.fecha).ToString();
                nueva.horometro = HR.horometro.ToString();
                nueva.lugar = HR.lugar;
                nueva.mantenedores = HR.nombreMantenedor;
                nueva.trabajoRealizado = HR.trabajoRealizado;
                nueva.numero = HR.numero.ToString();
                nueva.fecha = formatearString.fechaPalabras(HR.fecha);

                retorno.Add(nueva);
            }

            return retorno;
        }
    }
}