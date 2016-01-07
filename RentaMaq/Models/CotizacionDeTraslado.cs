using RentaMaq.DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class CotizacionDeTraslado
    {
        public int CotizacionDeTrasladoID { get; set; }
        [Display(Name = "Número de Cotización")]
        public int NumeroDeCotizacion { get; set; }
        [Display(Name = "Año")]
        public int año { get; set; }
        [Display(Name = "Fecha")]
        public DateTime fecha { get; set; }
        [Display(Name = "Cliente")]
        public string cliente { get; set; }
        [Display(Name = "Rut")]
        public int rut { get; set; }
        [Display(Name = "Dirección")]
        public string direccion { get; set; }
        [Display(Name = "Teléfono")]
        public string telefono { get; set; }
        [Display(Name = "Atención a")]
        public string atencionA { get; set; }
        [Display(Name = "Referencia")]
        public string referencia { get; set; }
        [Display(Name = "Moneda")]
        public string moneda { get; set; }
        [Display(Name = "Tipo de Cambio")]
        public string tipoCambio { get; set; }
        [Display(Name = "Encabezado")]
        public string encabezado { get; set; }
        [Display(Name = "Nota")]
        public string nota { get; set; }
        [Display(Name = "Disponibilidad")]
        public string disponibilidad { get; set; }
        [Display(Name = "Valor incluye")]
        public string valorIncluye { get; set; }
        [Display(Name = "Valor no incluye")]
        public string valorNoIncluye { get; set; }
        [Display(Name = "Condiciones Generales")]
        public string condicionesGenerales { get; set; }
        [Display(Name = "Formas de Pago")]
        public string formasDePago { get; set; }
        [Display(Name="Texto sobre documentos adjuntos")]
        public string textoAdjuntarDocumentos { get; set; }

        internal void quitarNulos()
        {
            if (fecha == null) fecha = DateTime.Now;
            if (cliente == null) cliente = "";
            if (rut == null) rut = 0;
            if (direccion == null) direccion = "";
            if (telefono == null) telefono = "";
            if (atencionA == null) atencionA = "";
            if (referencia == null) referencia = "";
            if (moneda == null) moneda = "";
            if (tipoCambio == null) tipoCambio = "";
            if (encabezado == null) encabezado = "";
            if (nota == null) nota = "";
            if (disponibilidad == null) disponibilidad = "";
            if (valorIncluye == null) valorIncluye = "";
            if (valorNoIncluye == null) valorNoIncluye = "";
            if (condicionesGenerales == null) condicionesGenerales = "";
            if (formasDePago == null) formasDePago = "";
            if (textoAdjuntarDocumentos == null) textoAdjuntarDocumentos = "";
        }

        internal void eliminarDetalle()
        {
            Context db = new Context();

            List<detalleCotizacionDeTraslado> detalle =
                db.detalleCotizacionTraslado.Where(s => s.IDCotizacionTraslado == this.CotizacionDeTrasladoID).ToList();

            foreach (detalleCotizacionDeTraslado det in detalle) 
            {
                db.detalleCotizacionTraslado.Remove(det);
            }

            db.SaveChanges();
        }
    }

    public class cotizacionDeTrasladoReporte 
    {
        public int NumeroDeCotizacion { get; set; }
        public int año { get; set; }
        public string fecha { get; set; }
        public string cliente { get; set; }
        public string rut { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }
        public string atencionA { get; set; }
        public string referencia { get; set; }
        public string moneda { get; set; }
        public string tipoCambio { get; set; }
        public string encabezado { get; set; }
        public string nota { get; set; }
        public string disponibilidad { get; set; }
        public string valorIncluye { get; set; }
        public string valorNoIncluye { get; set; }
        public string condicionesGenerales { get; set; }
        public string formasDePago { get; set; }
        public string textoAdjuntarDocumentos { get; set; }
        public string codigo { get; set; }
        public string descripcion { get; set; }
        public int cantidad { get; set; }
        public string precioUnitario { get; set; }
        public string total { get; set; }

        public static List<cotizacionDeTrasladoReporte> convertirDatos(CotizacionDeTraslado CotizacionTraslado) 
        {
            List<cotizacionDeTrasladoReporte> lista = new List<cotizacionDeTrasladoReporte>();

            foreach (detalleCotizacionDeTraslado detalle in new Context().detalleCotizacionTraslado.Where(s => s.IDCotizacionTraslado == CotizacionTraslado.CotizacionDeTrasladoID).ToList())
            {
                cotizacionDeTrasladoReporte nuevo = new cotizacionDeTrasladoReporte();
                nuevo.NumeroDeCotizacion = CotizacionTraslado.NumeroDeCotizacion;
                nuevo.año = CotizacionTraslado.año;
                nuevo.fecha = CotizacionTraslado.fecha.Day + "/" + CotizacionTraslado.fecha.Month + "/" + CotizacionTraslado.fecha.Year;
                nuevo.cliente = CotizacionTraslado.cliente;
                nuevo.rut = formatearString.formatoRut(CotizacionTraslado.rut.ToString());
                nuevo.direccion = CotizacionTraslado.direccion;
                nuevo.telefono = CotizacionTraslado.telefono;
                nuevo.atencionA = CotizacionTraslado.atencionA;
                nuevo.referencia = CotizacionTraslado.referencia;
                nuevo.moneda = CotizacionTraslado.moneda;
                nuevo.tipoCambio = CotizacionTraslado.tipoCambio;
                nuevo.encabezado = CotizacionTraslado.encabezado;
                nuevo.nota = CotizacionTraslado.nota;
                nuevo.disponibilidad = CotizacionTraslado.disponibilidad;
                nuevo.valorIncluye = CotizacionTraslado.valorIncluye;
                nuevo.valorNoIncluye = CotizacionTraslado.valorNoIncluye;
                nuevo.condicionesGenerales = CotizacionTraslado.condicionesGenerales;
                nuevo.formasDePago = CotizacionTraslado.formasDePago;
                nuevo.textoAdjuntarDocumentos = CotizacionTraslado.textoAdjuntarDocumentos;
                nuevo.codigo = detalle.codigo;
                nuevo.descripcion = detalle.descripcion;
                nuevo.cantidad = detalle.cantidad;
                nuevo.precioUnitario = formatearString.valores_Pesos(detalle.precioUnitario);
                nuevo.total = formatearString.valores_Pesos(detalle.total);

                lista.Add(nuevo);
            }

            return lista;
        }
    }
}