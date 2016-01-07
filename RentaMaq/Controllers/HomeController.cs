using RentaMaq.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


using System.Data.OleDb;
using System.Data;
using System.Data.Entity;
using RentaMaq.DAL;

namespace RentaMaq.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //importarProductos();
            //importarMaestros();
            //verificarDatosKMHM();
            //arreglarDatosHMKM("hm", 90);
            //arreglarMaestros();
            return View();
        }

        private void arreglarMaestros()
        {
            Context db = new Context();            
            List<Maestro> maestros = db.Maestros.ToList();

            foreach(Maestro maestro in maestros)
            {
                if (!string.IsNullOrEmpty(maestro.proveedor))                 
                {
                    int idProveedor = 0;
                    if (int.TryParse(maestro.proveedor, out idProveedor)) 
                    {
                        Proveedor proveedor = db.Proveedores.Find(idProveedor);
                        if (proveedor != null) 
                        {
                            maestro.proveedor = proveedor.nombreProveedor;
                            db.Entry(proveedor).State = EntityState.Modified;
                            db.SaveChanges();
                        }
                    }
                }
            }
        }

        private void verificarDatosKMHM()
        {
            Context db =  new Context();

            eliminarCeros();

            List<equipos> Equipos= db.Equipos.ToList();

            foreach (equipos Equipo in Equipos) 
            {
                int total=0, km=0, hm=0;
                foreach (registrokmhm registro in db.registrokmhms.Where(s => s.equipoID == Equipo.ID)) 
                {
                    total++;
                    if (registro.horometro != 0 && registro.kilometraje == 0) 
                    {
                        hm++;
                    }
                    else if(registro.kilometraje!=0 && registro.horometro==0)
                    {
                        km++;
                    }
                }
                if (hm > km)
                {
                    arreglarDatosHMKM("hm", Equipo.ID);
                }
                else 
                {
                    arreglarDatosHMKM("km", Equipo.ID);
                }
            }
        }

        private void eliminarCeros()
        {
            Context db = new Context();
            db.registrokmhms.RemoveRange(db.registrokmhms.Where(s => s.horometro == 0 && s.kilometraje == 0));
            db.SaveChanges();
        }

        private void arreglarDatosHMKM(string tipoDato, int equipoID)
        {
            Context db = new Context();
            List<registrokmhm> datos = db.registrokmhms.Where(s=>s.equipoID==equipoID).OrderByDescending(s=>s.fecha).ToList();

            registrokmhm anterior = new registrokmhm();

            foreach (registrokmhm registro in datos) 
            {
                if (tipoDato.Equals("hm") && registro.horometro == 0 && registro.kilometraje > 0
                    && registro.kilometraje<anterior.horometro) 
                {
                    registro.horometro = registro.kilometraje;
                    registro.kilometraje = 0;
                    db.Entry(registro).State = EntityState.Modified;
                    db.SaveChanges();
                }
                else if (tipoDato.Equals("km") && registro.horometro > 0 && registro.kilometraje == 0 
                    && registro.horometro<anterior.kilometraje) 
                {
                    registro.kilometraje = registro.horometro;
                    registro.horometro = 0;
                    db.Entry(registro).State = EntityState.Modified;
                    db.SaveChanges();
                }
                if(tipoDato.Equals("hm"))
                {
                    if (anterior.horometro > 0 && registro.horometro > anterior.horometro + 1000)
                    {
                        registro.kilometraje = registro.horometro;
                        registro.horometro = 0;
                        db.Entry(registro).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
                /*else if(tipoDato.Equals("km"))
                {
                    if (anterior.kilometraje > 0 && anterior.kilometraje - 1000 > registro.kilometraje)
                    {
                        registro.horometro = registro.kilometraje;
                        registro.kilometraje = 0;
                    }
                }//*/

                anterior = registro;
            }
        }

        public ActionResult About()
        { 
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        private void  importarProductos(){            
            string fileLocation = Server.MapPath("~/Content/stock.xls");            
          

            string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
            fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
           
                excelConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
                fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";
           
            OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);


            string nombreHoja = "materiales$";
            string strSQL = "SELECT * FROM [" + nombreHoja + "]";

            OleDbDataAdapter da = new OleDbDataAdapter(strSQL, excelConnection);
            DataSet ds = new DataSet();

            da.Fill(ds);
            int i = 0;
         
            RentaMaq.DAL.Context db = new DAL.Context();
            string total = "";
            while (i<858) 
            {               
                
                string numpart= Convert.ToString(ds.Tables[0].Rows[i].ItemArray[0]);
                string desc = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[1]);
                string unidad = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[2]);
                string precio = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[3]);
                double stock_min=0;
                if(!Convert.ToString(ds.Tables[0].Rows[i].ItemArray[4]).Equals("")){
                    stock_min= Convert.ToDouble(Convert.ToString(ds.Tables[0].Rows[i].ItemArray[4]));
                }
                 
                double stock_act = Convert.ToDouble(Convert.ToString(ds.Tables[0].Rows[i].ItemArray[5]));                
               
                try
                {
                    Producto productoBuscar = db.Productos.SingleOrDefault(s => s.numeroDeParte == numpart && s.descripcion==desc);
                    if (Object.ReferenceEquals(null, productoBuscar))
                    {
                        //INGRESAR
                        productoBuscar = db.Productos.SingleOrDefault(s => s.numeroDeParte == numpart);
                        if (Object.ReferenceEquals(null, productoBuscar))
                        {
                            Producto producto = new Producto();
                            producto.numeroDeParte = numpart;
                            producto.descripcion = desc;
                            producto.unidadDeMedida = unidad;
                            producto.stockMinimo = stock_min;
                            producto.stockActual = stock_act;
                            db.Productos.Add(producto);
                            db.SaveChanges();  
                            
                        }
                        else {
                            productoBuscar.descripcion = desc;
                            productoBuscar.stockActual = stock_act;
                            productoBuscar.stockMinimo = stock_min;
                            db.Entry(productoBuscar).State = EntityState.Modified;
                            db.SaveChanges();  
                             
                        }
                            
                    }
                    else { 
                        //actualizar 
                        productoBuscar.descripcion = desc;
                        productoBuscar.stockActual = stock_act;
                        productoBuscar.stockMinimo = stock_min;
                        db.Entry(productoBuscar).State = EntityState.Modified;
                          db.SaveChanges();  
                         
                    }
                   
                }
                catch (Exception e) {
                    total = total+"/n" +numpart;
                }             
                i++;                 
            }                       
        }



        private void importarMaestros()
        {
            string fileLocation = Server.MapPath("~/Content/stock.xls");


            string excelConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" +
            fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";

            excelConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" +
            fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=1\"";

            OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);


            string nombreHoja = "registro$";
            string strSQL = "SELECT * FROM [" + nombreHoja + "]";

            OleDbDataAdapter da = new OleDbDataAdapter(strSQL, excelConnection);
            DataSet ds = new DataSet();

            da.Fill(ds);
            int i = 992;
            
            RentaMaq.DAL.Context db = new DAL.Context();
            while (i<=1116)
            {
                Maestro maestro = new Maestro();
                DateTime fecha = Formateador.fechaStringToDateTime(Convert.ToString(ds.Tables[0].Rows[i].ItemArray[0]));
                string numeroParte = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[1]);
                string descripcion = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[2]);
                string unidad = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[3]);
                string entrada = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[4]);
                string salida = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[5]);
                string proveedor = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[6]);
                string preciounitario = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[7]);
                string preciototal = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[8]);
                string entregado = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[9]);
                string afi = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[10]);
                string observaciones = Convert.ToString(ds.Tables[0].Rows[i].ItemArray[11]);


                double cantidadEntrante = 0;
                double cantidadSaliente = 0;

                if (entrada.Equals(""))
                {
                    cantidadSaliente = Convert.ToDouble(salida);
                }
                else {

                    cantidadEntrante = Convert.ToDouble(entrada);
                }
                int precioUnitario = 0;
                int precioTotal = 0;
                if (!preciounitario.Equals("")) {
                    precioUnitario = Convert.ToInt32(preciounitario.Replace(",",""));

                }
                if (!preciototal.Equals("")) {
                    precioTotal = Convert.ToInt32(Math.Floor(double.Parse(preciototal.Replace(",", ""))));
                }

                if (proveedor.Equals(""))
                {
                    proveedor = "0";
                }
                else {
                    if (proveedor.Trim().Equals("SALFA"))
                    {
                        proveedor = "18";
                    }
                    else
                    {
                        Proveedor proveedorBuscar = db.Proveedores.Where(s => s.nombreProveedor.Contains(proveedor.Trim())).ToList()[0];
                        proveedor = proveedorBuscar.ProveedorID.ToString();
                    }
                }

                Producto producto = db.Productos.SingleOrDefault(s => s.numeroDeParte == numeroParte && s.descripcion == descripcion);
                maestro.fecha = fecha;
                if (producto != null)
                {
                    maestro.ProductoID = Convert.ToString(producto.ProductoID);
                }
                else {
                    producto = db.Productos.SingleOrDefault(s => s.numeroDeParte == numeroParte);
                    if (producto != null)
                    {
                        maestro.ProductoID = Convert.ToString(producto.ProductoID);
                    }
                    else
                    {
                        //INGRESAR PRODUCTO
                        Producto nuevoProducto = new Producto();
                        nuevoProducto.numeroDeParte = numeroParte;
                        nuevoProducto.idBodega = 1;
                        nuevoProducto.descripcion = descripcion;
                        nuevoProducto.unidadDeMedida = unidad;

                        db.Productos.Add(nuevoProducto);
                        db.SaveChanges();

                        maestro.ProductoID = nuevoProducto.ProductoID.ToString();

                    }   
                }                
                maestro.descripcionProducto = descripcion;
                maestro.cantidadEntrante = cantidadEntrante;
                maestro.cantidadSaliente = cantidadSaliente;
                maestro.proveedor = proveedor;
                maestro.valorUnitario = precioUnitario;
                maestro.valorTotal = precioTotal;
                maestro.entragadoA = entregado;
                maestro.afiEquipo = afi;
                maestro.observaciones = observaciones;


                 i++;
                db.Maestros.Add(maestro);
            }
            db.SaveChanges();
        }
    }
}