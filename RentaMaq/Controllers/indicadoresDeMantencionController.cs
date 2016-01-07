using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using RentaMaq.DAL;
using RentaMaq.Models;

namespace RentaMaq.Controllers
{
    public class indicadoresDeMantencionController : Controller
    {
        private Context db = new Context();

        public ActionResult Indicadores(string fechaInicio, string fechaFinal)
        {

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
            System.Collections.Hashtable Disponibilidad = new System.Collections.Hashtable();
            System.Collections.Hashtable Confiabilidad = new System.Collections.Hashtable();
            System.Collections.Hashtable TiempoMedioEntreFallas = new System.Collections.Hashtable();
            System.Collections.Hashtable TiempoMedioParaReparar = new System.Collections.Hashtable();
            System.Collections.Hashtable TiempoMedioOperacionAntesDeFalla = new System.Collections.Hashtable();
            System.Collections.Hashtable Utilizacion = new System.Collections.Hashtable();


            foreach (equipos Equip in Equipos)
            {

                disponibilidad disponibilidad = new disponibilidad(Inicio, Termino, Equip);
                double disp = Math.Round(disponibilidad.tiempoDisponible, 2);
                Disponibilidad.Add(Equip.ID, disp);


                tiempoMedioEntreFallas tMedioEntreFallas = new Models.tiempoMedioEntreFallas(Equip, Inicio, Termino);
                double tMedioFallas = Math.Round(tMedioEntreFallas.MTBF, 2);
                TiempoMedioEntreFallas.Add(Equip.ID, tMedioFallas);

                confiabilidad confiabilidad = new confiabilidad(Equip, Inicio, Termino, tMedioEntreFallas);
                double conf = Math.Round(confiabilidad.RazonDeFallas, 2);
                Confiabilidad.Add(Equip.ID, conf);

                tiempoMedioParaReparar MTTR = new tiempoMedioParaReparar(Inicio, Termino, Equip);
                double mttr = Math.Round(MTTR.MTTR, 2);
                TiempoMedioParaReparar.Add(Equip.ID, mttr);

                tiempoMedioDeOperacionAntesDeFalla tMedioOperacion =
                    new tiempoMedioDeOperacionAntesDeFalla(Equip, Inicio, Termino, MTTR.cantidadTotalDetenciones, confiabilidad.tMedioEntreFallas.totalHorasOperacionales);
                double tMedioOp = Math.Round(tMedioOperacion.MTBS, 2);
                TiempoMedioOperacionAntesDeFalla.Add(Equip.ID, tMedioOp);

                utilizacion Utiliz = new utilizacion(Equip, Inicio, Termino, disponibilidad.tiempoTranscurrido, disponibilidad.tiemposDeBaja, tMedioEntreFallas.totalHorasOperacionales);
                double utiliz = Math.Round(Utiliz.Utilizacion, 2);
                Utilizacion.Add(Equip.ID, utiliz);
            }
            ViewBag.Inicio = Inicio;
            ViewBag.Termino = Termino;
            ViewData["Disponibilidad"] = Disponibilidad;
            ViewData["Confiabilidad"] = Confiabilidad;
            ViewData["TiempoMedioEntreFallas"] = TiempoMedioEntreFallas;
            ViewData["tiempoMedioParaReparar"] = TiempoMedioParaReparar;
            ViewData["TiempoMedioOperacionAntesDeFalla"] = TiempoMedioOperacionAntesDeFalla;
            ViewData["Utilizacion"] = Utilizacion;

            return View(Equipos);
        }

        // GET: indicadoresDeMantencion/Detalle/5
        public ActionResult Detalle(int id,string fechaInicio, string fechaFinal, string tipoAgrupacion) {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DateTime Inicio = DateTime.Today.AddMonths(-1);
            DateTime Termino = DateTime.Today;
            string TipoAgrupacion = "semanal";
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

            List<double> ListaDisponibilidad = new List<double>();
            List<double> ListaTMedioFallas = new List<double>();
            List<double> ListaConfiabilidad = new List<double>();
            List<double> ListaTiempoMedioReparar = new List<double>();
            List<double> ListaTiempoMedioOperacion = new List<double>();
            List<double> ListaUtilizacion = new List<double>();
            List<DateTime> ListaInicio = new List<DateTime>();
            List<DateTime> ListaTermino = new List<DateTime>();

            equipos Equip = equipos.ObtenerConTipo(id);
            
            //Se realiza el cálculo según la agrupación:

            DateTime inicioTemp = Inicio;
            DateTime terminoTemp = Inicio;

            if(TipoAgrupacion.Equals("semanal"))
            {
                terminoTemp=Inicio.AddDays(7);
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

            while (terminoTemp < Termino)
            {
                ListaInicio.Add(inicioTemp);
                ListaTermino.Add(terminoTemp);

                disponibilidad disponibilidad = new disponibilidad(inicioTemp, terminoTemp, Equip);
                double disp = Math.Round(disponibilidad.tiempoDisponible, 2);
                ListaDisponibilidad.Add(disp);

                tiempoMedioEntreFallas tMedioEntreFallas = new Models.tiempoMedioEntreFallas(Equip, inicioTemp, terminoTemp);
                double tMedioFallas = Math.Round(tMedioEntreFallas.MTBF, 2);
                ListaTMedioFallas.Add(tMedioFallas);

                confiabilidad confiabilidad = new confiabilidad(Equip, inicioTemp, terminoTemp, tMedioEntreFallas);
                double conf = Math.Round(confiabilidad.RazonDeFallas, 2);
                ListaConfiabilidad.Add(conf);

                tiempoMedioParaReparar MTTR = new tiempoMedioParaReparar(inicioTemp, terminoTemp, Equip);
                double mttr = Math.Round(MTTR.MTTR, 2);
                ListaTiempoMedioReparar.Add(mttr);

                tiempoMedioDeOperacionAntesDeFalla tMedioOperacion =
                    new tiempoMedioDeOperacionAntesDeFalla(Equip, inicioTemp, terminoTemp, MTTR.cantidadTotalDetenciones, confiabilidad.tMedioEntreFallas.totalHorasOperacionales);
                double tMedioOp = Math.Round(tMedioOperacion.MTBS, 2);
                ListaTiempoMedioOperacion.Add(tMedioOp);

                utilizacion Utiliz = new utilizacion(Equip, inicioTemp, terminoTemp, disponibilidad.tiempoTranscurrido, disponibilidad.tiemposDeBaja, tMedioEntreFallas.totalHorasOperacionales);
                double utiliz = Math.Round(Utiliz.Utilizacion, 2);
                ListaUtilizacion.Add(utiliz);

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

                ListaInicio.Add(inicioTemp);
                ListaTermino.Add(terminoTemp);

                disponibilidad disponibilidadUlt = new disponibilidad(inicioTemp, terminoTemp, Equip);
                double dispUlt = Math.Round(disponibilidadUlt.tiempoDisponible, 2);
                ListaDisponibilidad.Add(dispUlt);

                tiempoMedioEntreFallas tMedioEntreFallasUlt = new Models.tiempoMedioEntreFallas(Equip, inicioTemp, terminoTemp);
                double tMedioFallasUlt = Math.Round(tMedioEntreFallasUlt.MTBF, 2);
                ListaTMedioFallas.Add(tMedioFallasUlt);

                confiabilidad confiabilidadUlt = new confiabilidad(Equip, inicioTemp, terminoTemp, tMedioEntreFallasUlt);
                double confUlt = Math.Round(confiabilidadUlt.RazonDeFallas, 2);
                ListaConfiabilidad.Add(confUlt);

                tiempoMedioParaReparar MTTRUlt = new tiempoMedioParaReparar(inicioTemp, terminoTemp, Equip);
                double mttrUlt = Math.Round(MTTRUlt.MTTR, 2);
                ListaTiempoMedioReparar.Add(mttrUlt);

                tiempoMedioDeOperacionAntesDeFalla tMedioOperacionUlt =
                    new tiempoMedioDeOperacionAntesDeFalla(Equip, inicioTemp, terminoTemp, MTTRUlt.cantidadTotalDetenciones, confiabilidadUlt.tMedioEntreFallas.totalHorasOperacionales);
                double tMedioOpUlt = Math.Round(tMedioOperacionUlt.MTBS, 2);
                ListaTiempoMedioOperacion.Add(tMedioOpUlt);

                utilizacion UtilizUlt = new utilizacion(Equip, inicioTemp, terminoTemp, disponibilidadUlt.tiempoTranscurrido, disponibilidadUlt.tiemposDeBaja, tMedioEntreFallasUlt.totalHorasOperacionales);
                double utilizUlt = Math.Round(UtilizUlt.Utilizacion, 2);
                ListaUtilizacion.Add(utilizUlt);
            }

            
            ViewBag.Inicio = Inicio;
            ViewBag.Termino = Termino;
            ViewBag.TipoAgrupacion = TipoAgrupacion;

            ViewBag.Disponibilidad = ListaDisponibilidad;
            ViewBag.TiempoMedioEntreFallas = ListaTMedioFallas;
            ViewBag.Confiabilidad = ListaConfiabilidad;
            ViewBag.TiempoMedioParaReparar = ListaTiempoMedioReparar;
            ViewBag.TiempoMedioOperacionAntesDeFalla = ListaTiempoMedioOperacion;
            ViewBag.Utilizacion = ListaUtilizacion;
            ViewBag.ListaInicio = ListaInicio;
            ViewBag.ListaTermino = ListaTermino;

            return View(Equip);
        }

        // GET: indicadoresDeMantencion/DetalleDisponibilidad/5
        public ActionResult DetalleDisponibilidad(int id, string fechaInicio, string fechaFinal, string tipoAgrupacion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DateTime Inicio = DateTime.Now.AddMonths(-1);
            DateTime Termino = DateTime.Now;
            string TipoAgrupacion = "semanal";
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

            List<double> ListaDisponibilidad = new List<double>();                       
            List<DateTime> ListaInicio = new List<DateTime>();
            List<DateTime> ListaTermino = new List<DateTime>();

            equipos Equip = equipos.ObtenerConTipo(id);         

            DateTime inicioTemp = Inicio;
            DateTime terminoTemp = Inicio;

            if (TipoAgrupacion.Equals("semanal"))
            {
                terminoTemp = Inicio.AddDays(7);
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

            while (terminoTemp < Termino)
            {
                ListaInicio.Add(inicioTemp);
                ListaTermino.Add(terminoTemp);

                disponibilidad disponibilidad = new disponibilidad(inicioTemp, terminoTemp, Equip);
                double disp = Math.Round(disponibilidad.tiempoDisponible*100, 2);
                ListaDisponibilidad.Add(disp);                

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

                ListaInicio.Add(inicioTemp);
                ListaTermino.Add(terminoTemp);

                disponibilidad disponibilidadUlt = new disponibilidad(inicioTemp, terminoTemp, Equip);
                double dispUlt = Math.Round(disponibilidadUlt.tiempoDisponible*100, 2);
                ListaDisponibilidad.Add(dispUlt);
                
            }


            ViewBag.Inicio = Inicio;
            ViewBag.Termino = Termino;
            ViewBag.TipoAgrupacion = TipoAgrupacion;

            ViewBag.Disponibilidad = ListaDisponibilidad;       
            ViewBag.ListaInicio = ListaInicio;
            ViewBag.ListaTermino = ListaTermino;

            return View(Equip);
        }

        // GET: indicadoresDeMantencion/DetalleDisponibilidad/5
        public ActionResult DetalleConfiabilidad(int id, string fechaInicio, string fechaFinal, string tipoAgrupacion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DateTime Inicio = DateTime.Now.AddMonths(-1);
            DateTime Termino = DateTime.Now;
            string TipoAgrupacion = "semanal";
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

            List<double> ListaConfiabilidad = new List<double>();
            List<DateTime> ListaInicio = new List<DateTime>();
            List<DateTime> ListaTermino = new List<DateTime>();

            equipos Equip = equipos.ObtenerConTipo(id);

            DateTime inicioTemp = Inicio;
            DateTime terminoTemp = Inicio;

            if (TipoAgrupacion.Equals("semanal"))
            {
                terminoTemp = Inicio.AddDays(7);
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

            while (terminoTemp < Termino)
            {
                ListaInicio.Add(inicioTemp);
                ListaTermino.Add(terminoTemp);

                tiempoMedioEntreFallas tMedioEntreFallasUlt = new Models.tiempoMedioEntreFallas(Equip, inicioTemp, terminoTemp);              

                confiabilidad confiabilidadUlt = new confiabilidad(Equip, inicioTemp, terminoTemp, tMedioEntreFallasUlt);
                double confUlt = Math.Round(confiabilidadUlt.RazonDeFallas, 2);
                ListaConfiabilidad.Add(confUlt);


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

                ListaInicio.Add(inicioTemp);
                ListaTermino.Add(terminoTemp);

                tiempoMedioEntreFallas tMedioEntreFallasUlt = new Models.tiempoMedioEntreFallas(Equip, inicioTemp, terminoTemp);                

                confiabilidad confiabilidadUlt = new confiabilidad(Equip, inicioTemp, terminoTemp, tMedioEntreFallasUlt);
                double confUlt = Math.Round(confiabilidadUlt.RazonDeFallas, 2);
                ListaConfiabilidad.Add(confUlt);


            }


            ViewBag.Inicio = Inicio;
            ViewBag.Termino = Termino;
            ViewBag.TipoAgrupacion = TipoAgrupacion;

            ViewBag.Confiabilidad = ListaConfiabilidad;
            ViewBag.ListaInicio = ListaInicio;
            ViewBag.ListaTermino = ListaTermino;

            return View(Equip);
        }

        public ActionResult DetalleUtilizacion(int id, string fechaInicio, string fechaFinal, string tipoAgrupacion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DateTime Inicio = DateTime.Today.AddMonths(-1);
            DateTime Termino = DateTime.Today;
            string TipoAgrupacion = "semanal";
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

            List<utilizacion> ListaUtilizacion = new List<utilizacion>();
            List<DateTime> ListaInicio = new List<DateTime>();
            List<DateTime> ListaTermino = new List<DateTime>();

            equipos Equip = equipos.ObtenerConTipo(id);

            //Se realiza el cálculo según la agrupación:

            DateTime inicioTemp = Inicio;
            DateTime terminoTemp = Inicio;

            if (TipoAgrupacion.Equals("semanal"))
            {
                terminoTemp = Inicio.AddDays(7);
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

            while (terminoTemp < Termino)
            {
                ListaInicio.Add(inicioTemp);
                ListaTermino.Add(terminoTemp);

                utilizacion Utiliz = new utilizacion(Equip, inicioTemp, terminoTemp);
                ListaUtilizacion.Add(Utiliz);

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

                ListaInicio.Add(inicioTemp);
                ListaTermino.Add(terminoTemp);

                utilizacion UtilizUlt = new utilizacion(Equip, inicioTemp, terminoTemp);
                ListaUtilizacion.Add(UtilizUlt);
            }


            ViewBag.Inicio = Inicio;
            ViewBag.Termino = Termino;
            ViewBag.TipoAgrupacion = TipoAgrupacion;

            ViewBag.Utilizacion = ListaUtilizacion;
            ViewBag.ListaInicio = ListaInicio;
            ViewBag.ListaTermino = ListaTermino;

            return View(Equip);
        }

        public ActionResult DetalleTiempoEntreFallas(int id, string fechaInicio, string fechaFinal, string tipoAgrupacion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DateTime Inicio = DateTime.Today.AddMonths(-1);
            DateTime Termino = DateTime.Today;
            string TipoAgrupacion = "semanal";
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

            List<tiempoMedioEntreFallas> ListaTMedioFallas = new List<tiempoMedioEntreFallas>();
            List<DateTime> ListaInicio = new List<DateTime>();
            List<DateTime> ListaTermino = new List<DateTime>();

            equipos Equip = equipos.ObtenerConTipo(id);

            //Se realiza el cálculo según la agrupación:

            DateTime inicioTemp = Inicio;
            DateTime terminoTemp = Inicio;

            if (TipoAgrupacion.Equals("semanal"))
            {
                terminoTemp = Inicio.AddDays(7);
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

            while (terminoTemp < Termino)
            {
                ListaInicio.Add(inicioTemp);
                ListaTermino.Add(terminoTemp);

                tiempoMedioEntreFallas tMedioEntreFallas = new Models.tiempoMedioEntreFallas(Equip, inicioTemp, terminoTemp);
                ListaTMedioFallas.Add(tMedioEntreFallas);

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

                ListaInicio.Add(inicioTemp);
                ListaTermino.Add(terminoTemp);

                tiempoMedioEntreFallas tMedioEntreFallasUlt = new Models.tiempoMedioEntreFallas(Equip, inicioTemp, terminoTemp);
                ListaTMedioFallas.Add(tMedioEntreFallasUlt);
            }

            ViewBag.Inicio = Inicio;
            ViewBag.Termino = Termino;
            ViewBag.TipoAgrupacion = TipoAgrupacion;

            ViewBag.TiempoMedioEntreFallas = ListaTMedioFallas;
            ViewBag.ListaInicio = ListaInicio;
            ViewBag.ListaTermino = ListaTermino;

            return View(Equip);
        }

        public ActionResult DetalleTiempoMedioOperacionAntesFalla(int id, string fechaInicio, string fechaFinal, string tipoAgrupacion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DateTime Inicio = DateTime.Today.AddMonths(-1);
            DateTime Termino = DateTime.Today;
            string TipoAgrupacion = "semanal";
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
            List<tiempoMedioDeOperacionAntesDeFalla> ListaTiempoMedioOperacion = new List<tiempoMedioDeOperacionAntesDeFalla>();
            List<DateTime> ListaInicio = new List<DateTime>();
            List<DateTime> ListaTermino = new List<DateTime>();

            equipos Equip = equipos.ObtenerConTipo(id);

            //Se realiza el cálculo según la agrupación:

            DateTime inicioTemp = Inicio;
            DateTime terminoTemp = Inicio;

            if (TipoAgrupacion.Equals("semanal"))
            {
                terminoTemp = Inicio.AddDays(7);
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

            while (terminoTemp < Termino)
            {
                ListaInicio.Add(inicioTemp);
                ListaTermino.Add(terminoTemp);

                

                tiempoMedioDeOperacionAntesDeFalla tMedioOperacion =
                    new tiempoMedioDeOperacionAntesDeFalla(Equip, inicioTemp, terminoTemp);
                ListaTiempoMedioOperacion.Add(tMedioOperacion);

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

                ListaInicio.Add(inicioTemp);
                ListaTermino.Add(terminoTemp);

                

                tiempoMedioDeOperacionAntesDeFalla tMedioOperacionUlt =
                    new tiempoMedioDeOperacionAntesDeFalla(Equip, inicioTemp, terminoTemp);
                ListaTiempoMedioOperacion.Add(tMedioOperacionUlt);
            }


            ViewBag.Inicio = Inicio;
            ViewBag.Termino = Termino;
            ViewBag.TipoAgrupacion = TipoAgrupacion;

            ViewBag.TiempoMedioOperacionAntesDeFalla = ListaTiempoMedioOperacion;
            ViewBag.ListaInicio = ListaInicio;
            ViewBag.ListaTermino = ListaTermino;

            return View(Equip);
        }

        public ActionResult DetalleTiempoMedioParaReparar(int id, string fechaInicio, string fechaFinal, string tipoAgrupacion)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            DateTime Inicio = DateTime.Today.AddMonths(-1);
            DateTime Termino = DateTime.Today;
            string TipoAgrupacion = "semanal";
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

            List<tiempoMedioParaReparar> ListaTiempoMedioReparar = new List<tiempoMedioParaReparar>();
            List<DateTime> ListaInicio = new List<DateTime>();
            List<DateTime> ListaTermino = new List<DateTime>();

            equipos Equip = equipos.ObtenerConTipo(id);

            //Se realiza el cálculo según la agrupación:

            DateTime inicioTemp = Inicio;
            DateTime terminoTemp = Inicio;

            if (TipoAgrupacion.Equals("semanal"))
            {
                terminoTemp = Inicio.AddDays(7);
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

            while (terminoTemp < Termino)
            {
                ListaInicio.Add(inicioTemp);
                ListaTermino.Add(terminoTemp);
                
                tiempoMedioParaReparar MTTR = new tiempoMedioParaReparar(inicioTemp, terminoTemp, Equip);
                ListaTiempoMedioReparar.Add(MTTR);

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

                ListaInicio.Add(inicioTemp);
                ListaTermino.Add(terminoTemp);

                tiempoMedioParaReparar MTTR = new tiempoMedioParaReparar(inicioTemp, terminoTemp, Equip);
                ListaTiempoMedioReparar.Add(MTTR);
            }


            ViewBag.Inicio = Inicio;
            ViewBag.Termino = Termino;
            ViewBag.TipoAgrupacion = TipoAgrupacion;

            ViewBag.TiempoMedioParaReparar = ListaTiempoMedioReparar;
            ViewBag.ListaInicio = ListaInicio;
            ViewBag.ListaTermino = ListaTermino;

            return View(Equip);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
