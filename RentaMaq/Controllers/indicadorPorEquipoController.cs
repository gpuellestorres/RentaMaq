using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RentaMaq.Models;

namespace RentaMaq.Controllers
{
    public class indicadorPorEquipoController : Controller
    {
        // GET: indicadorPorEquipo
        public ActionResult Index()
        {
            //new indicadoresPorEquipo("C1");              
            ViewData["equipos"] = equipos.todosDeMaestros();

            return View(indicadoresPorEquipo.obtenerTodosConsumos());
        }

        public ActionResult Detalle(string nombreEquipo) {
            //indicadoresPorEquipo.obtenerProductosPorEquipoSinIntervalorFecha(nombreEquipo);
            ViewBag.nombreEquipo = nombreEquipo;
            ViewBag.fechaInicial = "";
            return View(indicadoresPorEquipo.obtenerProductosPorEquipoSinIntervalorFecha(nombreEquipo));
        }
        public ActionResult DetalleFecha(string nombreEquipo, string fechaInicial, string fechaFinal) {

            DateTime fechaDesde = Formateador.fechaStringToDateTime(fechaInicial);
            DateTime fechaA = Formateador.fechaStringToDateTime(fechaFinal);
            ViewBag.nombreEquipo = nombreEquipo;
            ViewBag.fechaInicial = fechaInicial;
            ViewBag.fechaFinal = fechaFinal;
            return View("Detalle", indicadoresPorEquipo.obtenerProductosPorEquipoConIntervalorFecha(nombreEquipo,fechaDesde,fechaA));
        }

        public JsonResult Filtrar(string fechaInicial, string fechaFinal, string nombreEquipo)
        {
            DateTime fecha1 = Formateador.formatearFechaCompleta(fechaInicial);
            DateTime fecha2 = Formateador.formatearFechaCompleta(fechaFinal);

            if (nombreEquipo.Equals("todos"))
            {
                var retornoJson = indicadoresPorEquipo.obtenerProductosTodosEquiposConIntervalorFecha(fecha1, fecha2);
                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            else {
                var retornoJson = indicadoresPorEquipo.obtenerProductosPorEquipo(nombreEquipo, fecha1, fecha2);
                return Json(retornoJson, JsonRequestBehavior.AllowGet);
            }
            return null;
            //return Json(retornoJson, JsonRequestBehavior.AllowGet);
        }

    }
}