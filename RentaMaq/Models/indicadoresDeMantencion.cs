using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RentaMaq.DAL;

namespace RentaMaq.Models
{
    public class indicadoresDeMantencion
    {
        public int indicadoresDeMantencionID { set; get; }
    }

    public class disponibilidad {
        public double tiempoTranscurrido {set;get;}
        public double tiemposDeBaja { set; get; }
        public double tiempoDisponible { set; get; }

        public disponibilidad(DateTime fechaInicio, DateTime fechaFinal, equipos Equipo)
        {        
            tiempoTranscurrido =0;
            tiemposDeBaja =0;
            tiempoDisponible = 0;           
            TimeSpan ts = fechaFinal - fechaInicio;
            
            tiempoTranscurrido = ts.Days*24;            
            tiemposDeBaja = disponibilidad.sumatoriaTiemposDeBaja(fechaInicio,fechaFinal,Equipo.ID.ToString());

            tiempoDisponible=(tiempoTranscurrido-tiemposDeBaja)/tiempoTranscurrido;
            
        }

        public static double sumatoriaTiemposDeBaja(DateTime fechaInicio, DateTime fechaFinal, string idEquipo) {
            double sumatoria = 0;
            Context db = new Context();
            List<ordenDeTrabajoGeneral> ordenesDeTrabajo = db.ordenDeTrabajoGenerals.Where(s => s.horasMantenimientoFecha >= fechaInicio && s.horasMantenimientoFecha <= fechaFinal && s.idEquipo==idEquipo).ToList();
            foreach (ordenDeTrabajoGeneral orden in ordenesDeTrabajo) {

                sumatoria += orden.horasMantenimientoHRSDetenido;          
            }
            return sumatoria; 
       
        }
       
    }

    public class confiabilidad {
        public double e=Math.E;
        public double RazonDeFallas { set; get; }
        public double tiempoTranscurrido {set;get;}
        public double MTBF { get; set; }
        public tiempoMedioEntreFallas tMedioEntreFallas;

        public confiabilidad(equipos equipo, DateTime inicio, DateTime fin)
        {
            TimeSpan ts = fin - inicio;

            tMedioEntreFallas = new tiempoMedioEntreFallas(equipo, inicio, fin);

            // Difference in days.
            tiempoTranscurrido = ts.Days * 24;
            MTBF = tMedioEntreFallas.MTBF;

            double potencia = -tiempoTranscurrido / MTBF;
            RazonDeFallas = Math.Pow(e, potencia);
        }

        public confiabilidad(equipos equipo, DateTime inicio, DateTime fin, tiempoMedioEntreFallas TiempoMedioEntreFallas)
        {
            TimeSpan ts = fin - inicio;

            tMedioEntreFallas = TiempoMedioEntreFallas;

            // Difference in days.
            tiempoTranscurrido = ts.Days * 24;
            MTBF = tMedioEntreFallas.MTBF;

            double potencia = -tiempoTranscurrido / MTBF;
            RazonDeFallas = Math.Pow(e, potencia);
        }      
    }

    public class tiempoMedioEntreFallas {
        public double totalHorasOperacionales { set; get; }
        public double cantidadTotalFallas { set; get; }
        public double MTBF { get; set; }

        public static double cantidadFallas(equipos Equipo, DateTime inicio, DateTime fin)
        {
            Context db = new Context();
            string idEquipoString = Equipo.ID.ToString();
            return db.ordenDeTrabajoGenerals.Where(s => s.idEquipo == idEquipoString && s.horasMantenimientoFecha >= inicio && s.horasMantenimientoFecha <= fin && s.tipoMantenimientoARealizar == "Correctivo").ToList().Count;
        }

        public static int TotalHorasOperacionales(equipos equipo, DateTime inicio, DateTime fin)
        {
            int horometroFin = registrokmhm.obtenerEstimado(equipo.ID, fin).horometro;
            int horometroInicio = registrokmhm.obtenerEstimado(equipo.ID, inicio).horometro;

            if (horometroInicio > horometroFin) 
            {
                Console.WriteLine("aqui");
            }

            return horometroFin - horometroInicio;
        }

        public tiempoMedioEntreFallas(equipos equipo, DateTime inicio, DateTime fin)
        {
            totalHorasOperacionales = TotalHorasOperacionales(equipo, inicio, fin);
            cantidadTotalFallas = cantidadFallas(equipo, inicio, fin);
            
            MTBF = 0;
            if (cantidadTotalFallas > 0)
            {
                MTBF = totalHorasOperacionales / cantidadTotalFallas;
            }
            else MTBF = totalHorasOperacionales;
        }
    }

    public class tiempoMedioParaReparar {
        public double totalHorasDetencion { set; get; }
        public double cantidadTotalDetenciones { set; get; }
        public double MTTR { set; get; }

        public tiempoMedioParaReparar(DateTime fechaInicio, DateTime fechaFinal, equipos Equipo)
        {
            MTTR = 0;
            totalHorasDetencion = 0;
            cantidadTotalDetenciones = 0;
            Context db = new Context();
            string idEquipo = Equipo.ID.ToString();
            List<ordenDeTrabajoGeneral> ordenesDeTrabajo = db.ordenDeTrabajoGenerals.Where(s => s.horasMantenimientoFecha >= fechaInicio && s.horasMantenimientoFecha <= fechaFinal && s.idEquipo == idEquipo).ToList();
            foreach (ordenDeTrabajoGeneral orden in ordenesDeTrabajo)
            {

                totalHorasDetencion += orden.horasMantenimientoHRSDetenido;
            }
            if (ordenesDeTrabajo.Count != 0)
            {
                cantidadTotalDetenciones = ordenesDeTrabajo.Count;
                MTTR = totalHorasDetencion / cantidadTotalDetenciones;   
            }
                                            
        }
        
    }

    public class tiempoMedioDeOperacionAntesDeFalla{
        public double totalHorasOperacionales { set; get; }
        public double cantidadTotalDetenciones { set; get; }
        public double MTBS { get; set; }

        public tiempoMedioDeOperacionAntesDeFalla(equipos equipo, DateTime inicio, DateTime fin,
            double cantidadDetenciones, double TotalHorasOperacionales)
        {
            Context db = new Context();
            string idEquipoString = equipo.ID.ToString();

            totalHorasOperacionales = TotalHorasOperacionales;
            cantidadTotalDetenciones = cantidadDetenciones;
            MTBS = 0;
            if (cantidadTotalDetenciones > 0)
            {
                MTBS = totalHorasOperacionales / cantidadTotalDetenciones;
            }
            else MTBS = totalHorasOperacionales;
        }

        public tiempoMedioDeOperacionAntesDeFalla(equipos equipo, DateTime inicio, DateTime fin)
        {
            Context db = new Context();
            string idEquipoString = equipo.ID.ToString();

            totalHorasOperacionales = tiempoMedioEntreFallas.TotalHorasOperacionales(equipo, inicio, fin);
            cantidadTotalDetenciones = db.ordenDeTrabajoGenerals.Where(s => s.idEquipo == idEquipoString && s.horasMantenimientoFecha >= inicio && s.horasMantenimientoFecha <= fin).ToList().Count;
            MTBS = 0;
            if (cantidadTotalDetenciones > 0)
            {
                MTBS = totalHorasOperacionales / cantidadTotalDetenciones;
            }
            else MTBS = totalHorasOperacionales;
        }
    }

    public class utilizacion {
        public double tiemposDeOperacion { set; get; }
        public double tiempoTranscurrido { set; get; }
        public double tiemposDeBaja { set; get; }
        public double Utilizacion { get; set; }

        public utilizacion(equipos equipo, DateTime inicio, DateTime fin)
        {
            TimeSpan ts = fin - inicio;

            // Difference in days.
            tiempoTranscurrido = ts.Days * 24;

            tiemposDeOperacion = tiempoMedioEntreFallas.TotalHorasOperacionales(equipo, inicio, fin);
            tiemposDeBaja = disponibilidad.sumatoriaTiemposDeBaja(inicio, fin, equipo.ID.ToString());

            Utilizacion = tiemposDeOperacion / (tiempoTranscurrido - tiemposDeBaja);

            if (Utilizacion < 0) {
                Console.WriteLine("aqui");
            }
        }

        public utilizacion(equipos equipo, DateTime inicio, DateTime fin, 
            double TiempoTranscurrido, double TiemposDeBaja, double TiemposDeOperacion)
        {
            // Difference in days.
            tiempoTranscurrido = TiempoTranscurrido;

            tiemposDeOperacion = TiemposDeOperacion;
            tiemposDeBaja = TiemposDeBaja;

            Utilizacion = tiemposDeOperacion / (tiempoTranscurrido - tiemposDeBaja);

            if (Utilizacion > 1)
            {
                Console.WriteLine("aqui");
            }
        } 
    }

    public class indicadoresReporte 
    {
        public string fechaInicio { get; set; }
        public string fechaTermino { get; set; }
        public string equipo { get; set; }
        public double disponibilidad { get; set; }
        public double confiabilidad { get; set; }
        public double utilizacion { get; set; }
        public double tiempoMedioEntreFallas { get; set; }
        public double tiempoMedioParaReparar { get; set; }
        public double tiempoMedioOperacionAntesDeFalla { get; set; }
        public string periodo { get; set; }


        public static List<indicadoresReporte> obtenerDatosEquipos(string fechaInicio, string fechaFinal)
        {
            List<indicadoresReporte> retorno = new List<indicadoresReporte>();
            Context db = new Context();

            DateTime Inicio = DateTime.Today.AddMonths(-1);
            DateTime Termino = DateTime.Today;
            if (fechaInicio != null || fechaFinal != null)
            {
                string[] inicioSeparado = fechaInicio.Split('-');
                string[] terminoSeparado = fechaFinal.Split('-');

                Inicio = new DateTime(int.Parse(inicioSeparado[2]), int.Parse(inicioSeparado[1]), int.Parse(inicioSeparado[0]));
                Termino = new DateTime(int.Parse(terminoSeparado[2]), int.Parse(terminoSeparado[1]), int.Parse(terminoSeparado[0]));
            }

            List<equipos> Equipos = db.Equipos.ToList();

            foreach (equipos Equip in Equipos)
            {

                disponibilidad disponibilidad = new disponibilidad(Inicio, Termino, Equip);
                double disp = Math.Round(disponibilidad.tiempoDisponible, 2);
                
                tiempoMedioEntreFallas tMedioEntreFallas = new Models.tiempoMedioEntreFallas(Equip, Inicio, Termino);
                double tMedioFallas = Math.Round(tMedioEntreFallas.MTBF, 2);

                confiabilidad confiabilidad = new confiabilidad(Equip, Inicio, Termino, tMedioEntreFallas);
                double conf = Math.Round(confiabilidad.RazonDeFallas, 2);

                tiempoMedioParaReparar MTTR = new tiempoMedioParaReparar(Inicio, Termino, Equip);
                double mttr = Math.Round(MTTR.MTTR, 2);

                tiempoMedioDeOperacionAntesDeFalla tMedioOperacion =
                    new tiempoMedioDeOperacionAntesDeFalla(Equip, Inicio, Termino, MTTR.cantidadTotalDetenciones, confiabilidad.tMedioEntreFallas.totalHorasOperacionales);
                double tMedioOp = Math.Round(tMedioOperacion.MTBS, 2);

                utilizacion Utiliz = new utilizacion(Equip, Inicio, Termino, disponibilidad.tiempoTranscurrido, disponibilidad.tiemposDeBaja, tMedioEntreFallas.totalHorasOperacionales);
                double utiliz = Math.Round(Utiliz.Utilizacion, 2);

                indicadoresReporte nuevo = new indicadoresReporte();
                nuevo.fechaInicio = fechaInicio;
                nuevo.fechaTermino = fechaFinal;
                nuevo.equipo = Equip.numeroAFI;
                nuevo.disponibilidad = disp * 100;
                nuevo.confiabilidad = conf * 100;
                nuevo.utilizacion = utiliz * 100;
                nuevo.tiempoMedioEntreFallas = tMedioFallas;
                nuevo.tiempoMedioOperacionAntesDeFalla = tMedioOp;
                nuevo.tiempoMedioParaReparar = mttr;

                retorno.Add(nuevo);

            }

            return retorno;
        }

        public static List<indicadoresReporte> obtenerDatosEquipo(int id, string fechaInicio, string fechaFinal, string tipoAgrupacion) 
        {
            List<indicadoresReporte> retorno = new List<indicadoresReporte>();

            DateTime Inicio = DateTime.Today.AddMonths(-1);
            DateTime Termino = DateTime.Today;
            string TipoAgrupacion = "semanal";
            string agrupacionEscrita = TipoAgrupacion;
            if (fechaInicio != null || fechaFinal != null)
            {
                string[] inicioSeparado = fechaInicio.Split('-');
                string[] terminoSeparado = fechaFinal.Split('-');

                Inicio = new DateTime(int.Parse(inicioSeparado[2]), int.Parse(inicioSeparado[1]), int.Parse(inicioSeparado[0]));
                Termino = new DateTime(int.Parse(terminoSeparado[2]), int.Parse(terminoSeparado[1]), int.Parse(terminoSeparado[0]));
            }

            if (tipoAgrupacion != null)
            {
                TipoAgrupacion = tipoAgrupacion;
            }

            equipos Equip = equipos.ObtenerConTipo(id);

            //Se realiza el cálculo según la agrupación:

            DateTime inicioTemp = Inicio;
            DateTime terminoTemp = Inicio;

            if (TipoAgrupacion.Equals("semanal"))
            {
                agrupacionEscrita = "Semanal";
                terminoTemp = Inicio.AddDays(7);
            }
            else if (TipoAgrupacion.Equals("dosSemanas"))
            {
                agrupacionEscrita = "Dos Semanas";
                terminoTemp = terminoTemp.AddDays(14);
            }
            else if (TipoAgrupacion.Equals("mensual"))
            {
                agrupacionEscrita = "Mensual";
                terminoTemp = terminoTemp.AddMonths(1);
            }
            else if (TipoAgrupacion.Equals("bimensual"))
            {
                agrupacionEscrita = "Bimensual";
                terminoTemp = terminoTemp.AddMonths(2);
            }
            else if (TipoAgrupacion.Equals("trimestral"))
            {
                agrupacionEscrita = "Trimensual";
                terminoTemp = terminoTemp.AddMonths(3);
            }
            else if (TipoAgrupacion.Equals("semestral"))
            {
                agrupacionEscrita = "Semestral";
                terminoTemp = terminoTemp.AddMonths(6);
            }
            else if (TipoAgrupacion.Equals("anual"))
            {
                agrupacionEscrita = "Anual";
                terminoTemp = terminoTemp.AddYears(1);
            }

            while (terminoTemp < Termino)
            {
                disponibilidad disponibilidad = new disponibilidad(inicioTemp, terminoTemp, Equip);
                double disp = Math.Round(disponibilidad.tiempoDisponible, 2);

                tiempoMedioEntreFallas tMedioEntreFallas = new Models.tiempoMedioEntreFallas(Equip, inicioTemp, terminoTemp);
                double tMedioFallas = Math.Round(tMedioEntreFallas.MTBF, 2);

                confiabilidad confiabilidad = new confiabilidad(Equip, inicioTemp, terminoTemp, tMedioEntreFallas);
                double conf = Math.Round(confiabilidad.RazonDeFallas, 2);

                tiempoMedioParaReparar MTTR = new tiempoMedioParaReparar(inicioTemp, terminoTemp, Equip);
                double mttr = Math.Round(MTTR.MTTR, 2);

                tiempoMedioDeOperacionAntesDeFalla tMedioOperacion =
                    new tiempoMedioDeOperacionAntesDeFalla(Equip, inicioTemp, terminoTemp, MTTR.cantidadTotalDetenciones, confiabilidad.tMedioEntreFallas.totalHorasOperacionales);
                double tMedioOp = Math.Round(tMedioOperacion.MTBS, 2);

                utilizacion Utiliz = new utilizacion(Equip, inicioTemp, terminoTemp, disponibilidad.tiempoTranscurrido, disponibilidad.tiemposDeBaja, tMedioEntreFallas.totalHorasOperacionales);
                double utiliz = Math.Round(Utiliz.Utilizacion, 2);

                indicadoresReporte nuevo = new indicadoresReporte();

                nuevo.fechaInicio = Formateador.fechaCompletaToString(inicioTemp);
                nuevo.fechaTermino = Formateador.fechaCompletaToString(terminoTemp);
                nuevo.equipo = Equip.numeroAFI;
                nuevo.disponibilidad = disp * 100;
                nuevo.confiabilidad = conf * 100;
                nuevo.utilizacion = utiliz * 100;
                nuevo.tiempoMedioEntreFallas = tMedioFallas;
                nuevo.tiempoMedioOperacionAntesDeFalla = tMedioOp;
                nuevo.tiempoMedioParaReparar = mttr;
                nuevo.periodo = agrupacionEscrita;

                retorno.Add(nuevo);
                
                inicioTemp = terminoTemp.AddDays(1);
                if (TipoAgrupacion.Equals("semanal"))
                {
                    terminoTemp = terminoTemp.AddDays(7);
                }
                else if (TipoAgrupacion.Equals("dosSemanas"))
                {
                    terminoTemp = terminoTemp.AddDays(14);
                }
                else if (TipoAgrupacion.Equals("mensual"))
                {
                    terminoTemp = terminoTemp.AddMonths(1);
                }
                else if (TipoAgrupacion.Equals("bimensual"))
                {
                    terminoTemp = terminoTemp.AddMonths(2);
                }
                else if (TipoAgrupacion.Equals("trimestral"))
                {
                    terminoTemp = terminoTemp.AddMonths(3);
                }
                else if (TipoAgrupacion.Equals("semestral"))
                {
                    terminoTemp = terminoTemp.AddMonths(6);
                }
                else if (TipoAgrupacion.Equals("anual"))
                {
                    terminoTemp = terminoTemp.AddYears(1);
                }
            }

            if (inicioTemp < Termino)
            {
                terminoTemp = Termino;

                disponibilidad disponibilidadUlt = new disponibilidad(inicioTemp, terminoTemp, Equip);
                double dispUlt = Math.Round(disponibilidadUlt.tiempoDisponible, 2);

                tiempoMedioEntreFallas tMedioEntreFallasUlt = new Models.tiempoMedioEntreFallas(Equip, inicioTemp, terminoTemp);
                double tMedioFallasUlt = Math.Round(tMedioEntreFallasUlt.MTBF, 2);

                confiabilidad confiabilidadUlt = new confiabilidad(Equip, inicioTemp, terminoTemp, tMedioEntreFallasUlt);
                double confUlt = Math.Round(confiabilidadUlt.RazonDeFallas, 2);

                tiempoMedioParaReparar MTTRUlt = new tiempoMedioParaReparar(inicioTemp, terminoTemp, Equip);
                double mttrUlt = Math.Round(MTTRUlt.MTTR, 2);

                tiempoMedioDeOperacionAntesDeFalla tMedioOperacionUlt =
                    new tiempoMedioDeOperacionAntesDeFalla(Equip, inicioTemp, terminoTemp, MTTRUlt.cantidadTotalDetenciones, confiabilidadUlt.tMedioEntreFallas.totalHorasOperacionales);
                double tMedioOpUlt = Math.Round(tMedioOperacionUlt.MTBS, 2);

                utilizacion UtilizUlt = new utilizacion(Equip, inicioTemp, terminoTemp, disponibilidadUlt.tiempoTranscurrido, disponibilidadUlt.tiemposDeBaja, tMedioEntreFallasUlt.totalHorasOperacionales);
                double utilizUlt = Math.Round(UtilizUlt.Utilizacion, 2);

                indicadoresReporte nuevo = new indicadoresReporte();

                nuevo.fechaInicio = Formateador.fechaCompletaToString(inicioTemp);
                nuevo.fechaTermino = Formateador.fechaCompletaToString(terminoTemp);
                nuevo.equipo = Equip.numeroAFI;
                nuevo.disponibilidad = dispUlt * 100;
                nuevo.confiabilidad = confUlt * 100;
                nuevo.utilizacion = utilizUlt * 100;
                nuevo.tiempoMedioEntreFallas = tMedioFallasUlt;
                nuevo.tiempoMedioOperacionAntesDeFalla = tMedioOpUlt;
                nuevo.tiempoMedioParaReparar = mttrUlt;
                nuevo.periodo = agrupacionEscrita;

                retorno.Add(nuevo);
            }

            return retorno;
        }
    }
}