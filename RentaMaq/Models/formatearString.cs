using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RentaMaq.Models
{
    public class formatearString
    {
        public static string fechaSinHoraDiaPrimero(DateTime fecha) 
        {
            string dia = fecha.Day.ToString();
            string mes = fecha.Month.ToString();
            if (dia.Length == 1) dia = "0" + dia;
            if (mes.Length == 1) mes = "0" + mes;

            return dia + "/" + mes + "/" + fecha.Year;
        }

        public static string mesAñoAPalabras(string mes, string año)
        {
            string descripcionMes = "";

            if (mes == "1")
            {
                descripcionMes = "Enero de ";
            }
            else if (mes == "2")
            {
                descripcionMes = "Febrero de ";
            }
            else if (mes == "3")
            {
                descripcionMes = "Marzo de ";
            }
            else if (mes == "4")
            {
                descripcionMes = "Abril de ";
            }
            else if (mes == "5")
            {
                descripcionMes = "Mayo de ";
            }
            else if (mes == "6")
            {
                descripcionMes = "Junio de ";
            }
            else if (mes == "7")
            {
                descripcionMes = "Julio de ";
            }
            else if (mes == "8")
            {
                descripcionMes = "Agosto de ";
            }
            else if (mes == "9")
            {
                descripcionMes = "Septiembre de ";
            }
            else if (mes == "10")
            {
                descripcionMes = "Octubre de ";
            }
            else if (mes == "11")
            {
                descripcionMes = "Noviembre de ";
            }
            else if (mes == "12")
            {
                descripcionMes = "Diciembre de ";
            }
            descripcionMes += año + "";

            return descripcionMes;
        }

        public static DateTime stringtoDateTime(string fecha)
        {
            string[] fecha_1 = fecha.Split('/');
            int dia = Convert.ToInt32(fecha_1[0]);
            int mes = Convert.ToInt32(fecha_1[1]);
            int ano = Convert.ToInt32(fecha_1[2]);

            DateTime fechaFiniquito = new DateTime(ano, mes, dia);
            return fechaFiniquito;

        }
        public static string mesAñoAPalabras(int mes, int año)
        {
            return mesAñoAPalabras(mes.ToString(), año.ToString());
        }
        public static string mesPalabra(string mes)
        {
            string descripcionMes = "";
            if (mes == "1")
            {
                descripcionMes = "Enero de ";
            }
            else if (mes == "2")
            {
                descripcionMes = "Febrero de ";
            }
            else if (mes == "3")
            {
                descripcionMes = "Marzo de ";
            }
            else if (mes == "4")
            {
                descripcionMes = "Abril de ";
            }
            else if (mes == "5")
            {
                descripcionMes = "Mayo de ";
            }
            else if (mes == "6")
            {
                descripcionMes = "Junio de ";
            }
            else if (mes == "7")
            {
                descripcionMes = "Julio de ";
            }
            else if (mes == "8")
            {
                descripcionMes = "Agosto de ";
            }
            else if (mes == "9")
            {
                descripcionMes = "Septiembre de ";
            }
            else if (mes == "10")
            {
                descripcionMes = "Octubre de ";
            }
            else if (mes == "11")
            {
                descripcionMes = "Noviembre de ";
            }
            else if (mes == "12")
            {
                descripcionMes = "Diciembre de ";
            }

            return descripcionMes;
        }

        public static string valores_Pesos(string valor)
        {

            string retorno = "";

            string valorIngresado = valor.Replace(".", ",");

            char[] caracteres = valorIngresado.ToCharArray();
            int posicionComa = caracteres.Length;

            if (valorIngresado.Contains(','))
            {
                for (int i = 0; i < caracteres.Length; i++)
                {
                    if (caracteres[i] == ',') posicionComa = i;
                }
            }

            for (int i = posicionComa; i < caracteres.Length; i++)
            {
                retorno += caracteres[i];
            }

            for (int i = posicionComa - 1; i >= 0; i--)
            {
                if (i == posicionComa - 3)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == posicionComa - 6)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == posicionComa - 9)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == posicionComa - 12)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == posicionComa - 15)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == posicionComa - 18)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == posicionComa - 21)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else
                {
                    retorno = caracteres[i] + retorno;
                }
            }

            if (retorno.StartsWith(".")) retorno = retorno.TrimStart('.');
            else if (retorno.StartsWith("-."))
            {
                retorno = retorno.Replace("-.", "-");
            }

            return retorno;
        }

        public static string valores_Pesos(int valor)
        {
            return valores_Pesos(valor.ToString());
        }

        public static string valores_Pesos(double valor)
        {
            return valores_Pesos(valor.ToString());
        }

        public static string valores_Pesos(float valor)
        {
            return valores_Pesos(valor.ToString());
        }

        public static string rut_(string rut)
        {
            string retorno = "";
            char[] caracteres = rut.ToCharArray();

            for (int i = caracteres.Length - 1; i >= 0; i--)
            {
                if (i == caracteres.Length - 1) retorno = "-" + caracteres[i] + retorno;
                else if (i == caracteres.Length - 4) retorno = "." + caracteres[i] + retorno;
                else if (i == caracteres.Length - 7) retorno = "." + caracteres[i] + retorno;
                else retorno = caracteres[i] + retorno;
            }
            return retorno;
        }

        public string valoresPesos(string valor)
        {

            string retorno = "";

            char[] caracteres = valor.ToCharArray();

            for (int i = caracteres.Length - 1; i >= 0; i--)
            {
                if (i == caracteres.Length - 3)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 6)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 9)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 12)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 15)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 18)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else if (i == caracteres.Length - 21)
                {
                    retorno = "." + caracteres[i] + retorno;
                }
                else
                {
                    retorno = caracteres[i] + retorno;
                }
            }

            if (retorno.StartsWith(".")) retorno = retorno.TrimStart('.');
            else if (retorno.StartsWith("-."))
            {
                retorno = retorno.Replace("-.", "-");
            }


            return retorno;
        }

        public string valoresPesos(int valor)
        {
            return valoresPesos(valor.ToString());
        }

        public string valoresPesos(double valor)
        {
            return valoresPesos(valor.ToString());
        }

        public string valoresPesos(float valor)
        {
            return valoresPesos(valor.ToString());
        }

        public string rut(string rut)
        {
            string retorno = "";
            char[] caracteres = rut.ToCharArray();

            for (int i = caracteres.Length - 1; i >= 0; i--)
            {
                if (i == caracteres.Length - 1) retorno = "-" + caracteres[i] + retorno;
                else if (i == caracteres.Length - 4) retorno = "." + caracteres[i] + retorno;
                else if (i == caracteres.Length - 7) retorno = "." + caracteres[i] + retorno;
                else retorno = caracteres[i] + retorno;
            }
            return retorno;
        }

        public static string formatoRut(string rut)
        {
            if (string.IsNullOrEmpty(rut)) return "";

            string retorno = "";
            char[] caracteres = soloNumeros(rut).ToCharArray();

            for (int i = caracteres.Length - 1; i >= 0; i--)
            {
                if (i == caracteres.Length - 1) retorno = "-" + caracteres[i] + retorno;
                else if (i == caracteres.Length - 4) retorno = "." + caracteres[i] + retorno;
                else if (i == caracteres.Length - 7) retorno = "." + caracteres[i] + retorno;
                else retorno = caracteres[i] + retorno;
            }
            return retorno;
        }

        private static string soloNumeros(string entrada)
        {
            string retorno = "";
            char[] caracteres = entrada.ToCharArray();

            for (int i = caracteres.Length - 1; i >= 0; i--)
            {
                if (char.IsNumber(caracteres[i])) retorno = caracteres[i] + retorno;
            }
            return retorno;
        }

        public static int limpiarEntero(string numero)
        {
            string retorno = "";
            char[] caracteres = numero.ToString().ToCharArray();

            for (int i = caracteres.Length - 1; i >= 0; i--)
            {
                if (!caracteres[i].Equals("$") && !caracteres[i].Equals("."))
                {
                    retorno += caracteres[i];
                }
            }
            return Convert.ToInt32(retorno);
        }
        public static string limpiarString(string numero)
        {
            string retorno = "";
            char[] caracteres = numero.ToString().ToCharArray();

            for (int i = 0; i <= caracteres.Length - 1; i++)
            {
                if (caracteres[i].Equals('$') || caracteres[i].Equals('.'))
                {

                }
                else
                {
                    retorno += caracteres[i];
                }
            }
            return retorno;
        }
        public static double limpiarDouble(string numero)
        {
            string retorno = "";
            char[] caracteres = numero.ToString().ToCharArray();

            for (int i = caracteres.Length - 1; i >= 0; i--)
            {
                if (!caracteres[i].Equals("$") && !caracteres[i].Equals("$"))
                {
                    retorno += caracteres[i];
                }
            }
            return Convert.ToDouble(retorno);
        }


        internal static string numeroAPalabras(int numero)
        {
            string resultado = "";
            bool negativo = false;
            if (numero < 0) negativo = true;

            char[] numeroChar = numero.ToString().Replace("-", "").ToCharArray();

            int division3 = numeroChar.Length / 3;

            if (division3 * 3 == numeroChar.Length) division3--;

            while (division3 >= 0)
            {
                int comienzo = numeroChar.Length - 3 * (division3 + 1);
                int final = numeroChar.Length - 3 * division3;
                if (comienzo < 0) comienzo = 0;

                string num = "";
                for (int i = comienzo; i < final; i++)
                {
                    num = num + numeroChar[i];
                }

                string lectura = retornarValorMenorAMilComoString(num);

                if (division3 == 4)
                {
                    if (lectura == "uno ") lectura = "un billón";
                    else
                    {
                        lectura += "billones ";
                    }
                }
                else if (division3 == 3)
                {
                    if (lectura == "uno ") lectura = "un mil millones ";
                    else
                    {
                        lectura += "miles de millones ";
                    }
                }
                else if (division3 == 2)
                {
                    if (lectura == "uno ") lectura = "un millón ";
                    else
                    {
                        lectura += "millones ";
                    }
                }
                else if (division3 == 1)
                {
                    if (lectura == "uno ") lectura = "un mil ";
                    else
                    {
                        lectura += "mil ";
                    }
                }
                resultado += lectura;

                division3--;
            }
            if (negativo) resultado = "-" + resultado;

            return resultado;
        }

        static string retornarValorMenorAMilComoString(string numero)
        {
            string resultado = "";
            char[] numeros = new char[3];

            if (numero.Length == 3)
                numeros = numero.ToCharArray();
            else if (numero.Length == 2)
            {
                numeros[0] = '0';
                numeros[1] = numero.ToCharArray()[0];
                numeros[2] = numero.ToCharArray()[1];
            }
            else if (numero.Length == 1)
            {
                numeros[0] = '0';
                numeros[1] = '0';
                numeros[2] = numero.ToCharArray()[0];
            }
            else return resultado;

            if (numeros[0] == '1')
            {
                if (numero[1] != '0' || numero[2] != '0') resultado += "ciento ";
                else resultado += "cien ";
            }
            else if (numeros[0] == '2')
            {
                resultado += "doscientos ";
            }
            else if (numeros[0] == '3')
            {
                resultado += "trescientos ";
            }
            else if (numeros[0] == '4')
            {
                resultado += "cuatrocientos ";
            }
            else if (numeros[0] == '5')
            {
                resultado += "quinientos ";
            }
            else if (numeros[0] == '6')
            {
                resultado += "seiscientos ";
            }
            else if (numeros[0] == '7')
            {
                resultado += "setecientos ";
            }
            else if (numeros[0] == '8')
            {
                resultado += "ochocientos ";
            }
            else if (numeros[0] == '9')
            {
                resultado += "novecientos ";
            }

            string ultimosDos = numeros[1] + "" + numeros[2];
            if (int.Parse(ultimosDos) > 9 && int.Parse(ultimosDos) < 30)
            {
                if (ultimosDos == "10")
                {
                    resultado += "diez ";
                }
                else if (ultimosDos == "11")
                {
                    resultado += "once ";
                }
                else if (ultimosDos == "12")
                {
                    resultado += "doce ";
                }
                else if (ultimosDos == "13")
                {
                    resultado += "trece ";
                }
                else if (ultimosDos == "14")
                {
                    resultado += "catorce ";
                }
                else if (ultimosDos == "15")
                {
                    resultado += "quince ";
                }
                else if (ultimosDos == "16")
                {
                    resultado += "dieciséis ";
                }
                else if (ultimosDos == "17")
                {
                    resultado += "diecisiete ";
                }
                else if (ultimosDos == "18")
                {
                    resultado += "dieciocho ";
                }
                else if (ultimosDos == "19")
                {
                    resultado += "diecinueve ";
                }
                else if (ultimosDos == "20")
                {
                    resultado += "veinte ";
                }
                else if (ultimosDos == "21")
                {
                    resultado += "veintiuno ";
                }
                else if (ultimosDos == "22")
                {
                    resultado += "veintidos ";
                }
                else if (ultimosDos == "23")
                {
                    resultado += "veintitres ";
                }
                else if (ultimosDos == "24")
                {
                    resultado += "veinticuatro ";
                }
                else if (ultimosDos == "25")
                {
                    resultado += "veinticinco ";
                }
                else if (ultimosDos == "26")
                {
                    resultado += "veintiséis ";
                }
                else if (ultimosDos == "27")
                {
                    resultado += "veintisiete ";
                }
                else if (ultimosDos == "28")
                {
                    resultado += "veintiocho ";
                }
                else if (ultimosDos == "29")
                {
                    resultado += "veintinueve ";
                }
            }
            else
            {
                if (numeros[1] == '3')
                {
                    resultado += "treinta ";
                    if (numeros[2] != '0') resultado += "y ";
                }
                else if (numeros[1] == '4')
                {
                    resultado += "cuarenta ";
                    if (numeros[2] != '0') resultado += "y ";
                }
                else if (numeros[1] == '5')
                {
                    resultado += "cincuenta ";
                    if (numeros[2] != '0') resultado += "y ";
                }
                else if (numeros[1] == '6')
                {
                    resultado += "sesenta ";
                    if (numeros[2] != '0') resultado += "y ";
                }
                else if (numeros[1] == '7')
                {
                    resultado += "setenta ";
                    if (numeros[2] != '0') resultado += "y ";
                }
                else if (numeros[1] == '8')
                {
                    resultado += "ochenta ";
                    if (numeros[2] != '0') resultado += "y ";
                }
                else if (numeros[1] == '9')
                {
                    resultado += "noventa ";
                    if (numeros[2] != '0') resultado += "y ";
                }

                if (numeros[2] == '1')
                {
                    resultado += "uno ";
                }
                if (numeros[2] == '2')
                {
                    resultado += "dos ";
                }
                if (numeros[2] == '3')
                {
                    resultado += "tres ";
                }
                if (numeros[2] == '4')
                {
                    resultado += "cuatro ";
                }
                if (numeros[2] == '5')
                {
                    resultado += "cinco ";
                }
                if (numeros[2] == '6')
                {
                    resultado += "seis ";
                }
                if (numeros[2] == '7')
                {
                    resultado += "siete ";
                }
                if (numeros[2] == '8')
                {
                    resultado += "ocho ";
                }
                if (numeros[2] == '9')
                {
                    resultado += "nueve ";
                }
            }

            return resultado;
        }
        public static string obtenerDVRUT(string rut)
        {
            string retorno = "";
            char[] caracteres = rut.ToString().ToCharArray();
            retorno += caracteres[caracteres.Length - 1];
            return retorno;
        }
        public static string rutSinDV(string rut)
        {
            string rutsindv = rut.Substring(0, rut.Length - 1);
            return rutsindv;
        }

        public static string apellidoMat(string apellidos)
        {
            string[] palabras = apellidos.Split(' ');
            string apellido_mat = palabras[1];
            return apellido_mat;
        }
        public static string apellidoPat(string apellidos)
        {
            string[] palabras = apellidos.Split(' ');
            string apellido_pat = palabras[0];
            return apellido_pat;
        }

        internal static string mesPalabraUnicaMinuscula(string mes)
        {
            string descripcionMes = "";
            if (mes == "1")
            {
                descripcionMes = "enero";
            }
            else if (mes == "2")
            {
                descripcionMes = "febrero";
            }
            else if (mes == "3")
            {
                descripcionMes = "marzo";
            }
            else if (mes == "4")
            {
                descripcionMes = "abril";
            }
            else if (mes == "5")
            {
                descripcionMes = "mayo";
            }
            else if (mes == "6")
            {
                descripcionMes = "junio";
            }
            else if (mes == "7")
            {
                descripcionMes = "julio";
            }
            else if (mes == "8")
            {
                descripcionMes = "agosto";
            }
            else if (mes == "9")
            {
                descripcionMes = "septiembre";
            }
            else if (mes == "10")
            {
                descripcionMes = "octubre";
            }
            else if (mes == "11")
            {
                descripcionMes = "noviembre";
            }
            else if (mes == "12")
            {
                descripcionMes = "diciembre";
            }

            return descripcionMes;
        }

        internal static string mesPalabraUnicaMinuscula(int mes)
        {
            return mesPalabraUnicaMinuscula(mes.ToString());
        }

        internal static int mesPalabraANumero(string palabra)
        {
            int mes = 0;
            if (palabra == "Enero" || palabra == "ENERO")
            {
                mes = 1;
            }
            else if (palabra == "Febrero" || palabra == "FEBRERO")
            {
                mes = 2;
            }
            else if (palabra == "Marzo" || palabra == "MARZO")
            {
                mes = 3;
            }
            else if (palabra == "Abril" || palabra == "ABRIL")
            {
                mes = 4;
            }
            else if (palabra == "Mayo" || palabra == "MAYO")
            {
                mes = 5;
            }
            else if (palabra == "Junio" || palabra == "JUNIO")
            {
                mes = 6;
            }
            else if (palabra == "Julio" || palabra == "JULIO")
            {
                mes = 7;
            }
            else if (palabra == "Agosto" || palabra == "AGOSTO")
            {
                mes = 8;
            }
            else if (palabra == "Septiembre" || palabra == "SEPTIEMBRE")
            {
                mes = 9;
            }
            else if (palabra == "Octubre" || palabra == "OCTUBRE")
            {
                mes = 10;
            }
            else if (palabra == "Noviembre" || palabra == "NOVIEMBRE")
            {
                mes = 11;
            }
            else if (palabra == "Diciembre" || palabra == "DICIEMBRE")
            {
                mes = 12;
            }

            return mes;
        }

        internal static string mesPalabraSoloMes(string mes)
        {
            string descripcionMes = "";
            if (mes == "1")
            {
                descripcionMes = "Enero";
            }
            else if (mes == "2")
            {
                descripcionMes = "Febrero";
            }
            else if (mes == "3")
            {
                descripcionMes = "Marzo";
            }
            else if (mes == "4")
            {
                descripcionMes = "Abril";
            }
            else if (mes == "5")
            {
                descripcionMes = "Mayo";
            }
            else if (mes == "6")
            {
                descripcionMes = "Junio";
            }
            else if (mes == "7")
            {
                descripcionMes = "Julio";
            }
            else if (mes == "8")
            {
                descripcionMes = "Agosto";
            }
            else if (mes == "9")
            {
                descripcionMes = "Septiembre";
            }
            else if (mes == "10")
            {
                descripcionMes = "Octubre";
            }
            else if (mes == "11")
            {
                descripcionMes = "Noviembre";
            }
            else if (mes == "12")
            {
                descripcionMes = "Diciembre";
            }

            return descripcionMes;
        }

        internal static string fechaPalabras(DateTime dateTime)
        {
            return dateTime.Day + " de " + mesAñoAPalabras(dateTime.Month, dateTime.Year);
        }
    }
}