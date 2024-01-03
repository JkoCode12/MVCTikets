using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Net.Security;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace ApplicationCore.Utils
{
    public class Utilidades
    {
        #region VALIDACIONES
        public static string ToProperCase(string s)
        {
            if (s == null) return s;

            string[] words = s.Split(' ');
            for (Int32 i = 0; i < words.Length; i++)
            {
                if (words[i].Length == 0) continue;

                Char firstChar = Char.ToUpper(words[i][0]);
                string rest = "";
                if (words[i].Length > 1)
                {
                    rest = words[i].Substring(1).ToLower();
                }
                words[i] = firstChar + rest;
            }
            return string.Join(" ", words);
        }

        public static string ToTitleCase(string str)
        {
            if (!ReferenceEquals(null, str))
            {
                var cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                return cultureInfo.TextInfo.ToTitleCase(str.ToLower());
            }
            else
            {
                return string.Empty;
            }
        }
        // Numero: parametro string
        public static bool EsNumero(string miTextBox)
        {
            Regex regex = new Regex(@"^[0-9]+$");
            return regex.IsMatch(miTextBox);
        }
        // Cadena: parametro string
        public static bool EsCadena(string pValor)
        {
            Regex regex = new Regex(@"^[^ ][a-zA-Z ]+[^ ]$");
            return regex.IsMatch(pValor);
        }
        //Es mayuscula
        public static bool EsMayuscula(string pValor)
        {
            Regex regex = new Regex(@"^[A-Z]+$");
            return regex.IsMatch(pValor);
        }
        //Es minuscula
        public static bool esMinuscula(string pValor)
        {
            Regex regex = new Regex(@"^[a-z]+$");
            return regex.IsMatch(pValor);
        }
        // Telefono
        public static bool EsTelefono(string miTextBox)
        {
            if (miTextBox.Trim().Equals("-"))
            {
                return true;
            }
            else
            {
                Regex regex = new Regex(@"^([0-9]{4})[\-]([0-9]{4})?$");
                return regex.IsMatch(miTextBox);
            }
        }

        /// <summary>
        /// veirifica que el telefono no sea solo maskara 
        /// </summary>
        /// <param name="telefono"></param>
        /// <returns></returns>
        public static string VerificaTelefono(string telefono)
        {
            if (telefono.Trim().Equals("-"))
            {
                return "";
            }
            else
            {
                return telefono;
            }
        }



        // Decimal: parametro string
        public static bool EsDecimal(string pValor)
        {
            Regex regex = new Regex(@"^[0-9]{1,9}([\.][0-9]{1,3})?$");
            return regex.IsMatch(pValor);
        }
        // URL                                       
        public static bool EsUrl(string miTextBox)
        {
            Regex regex = new Regex(@"^[a-zA-Z0-9\-\.]+\.(com|org|net|mil|edu|es|COM|ORG|NET|MIL|EDU|ES)$");
            return regex.IsMatch(miTextBox);
        }
        // Email
        public static bool EsEmail(string miTextBox)
        {                              // \w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*
            Regex regex = new Regex(@"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
                @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$");

            // Resultado: 
            //       Valid: david.jones@proseware.com 
            //       Valid: d.j@server1.proseware.com 
            //       Valid: jones@ms1.proseware.com 
            //       Invalid: j.@server1.proseware.com 
            //       Invalid: j@proseware.com9 
            //       Valid: js#internal@proseware.com 
            //       Valid: j_9@[129.126.118.1] 
            //       Invalid: j..s@proseware.com 
            //       Invalid: js*@proseware.com 
            //       Invalid: js@proseware..com 
            //       Invalid: js@proseware.com9 
            //       Valid: j.s@server1.proseware.com

            return regex.IsMatch(miTextBox);
        }


        public static string ValidarExepcionSQL(Exception exception)
        {

            string message = "";

            if (exception.InnerException != null)
            {
                message += exception.InnerException.Message;
                if (exception.InnerException.InnerException != null)
                {
                    message += exception.InnerException.InnerException.Message;
                }
            }
            else
            {
                message += exception.Message;
            }

            if (message.ToUpper().Contains("DUPLICATE ENTRY") || message.ToUpper().Contains("SAME PRIMARY KEY"))
            {
                return "El registro a insertar ya se encuentra en el sistema. " +
                        "Cambiar el o los códigos que desea guardar por otros que no existan en el sistema.";
            }
            else
            {
                if (message.ToUpper().Contains("CANNOT DELETE OR UPDATE A PARENT ROW"))
                {
                    return "El registro está siendo utilizado por otras funcionalidades del sistema. " +
                            "Para eliminar el registro primero se deben eliminar los registros asociados en los respectivos mantenimientos o modificar estos para que no se relacionen con este registro.";
                }
                else
                {
                    if (message.ToUpper().Contains("FOREIGN KEY"))
                    {
                        if (message.ToUpper().Contains("ADD"))
                        {
                            return "El registro a insertar contiene datos asociados que aún no están almacenados en el sistema. " + "\n" +
                                                "Para insertar el registro primero debe verificar los registros asociados, esto porque algún registro asociado no existe en el sistema.";
                        }
                        else
                        {
                            return "El registro está siendo utilizado por otras funcionalidades del sistema. " +
                                                "Para eliminar el registro primero se deben eliminar los registros asociados en los respectivos mantenimientos o modificar estos para que no se relacionen con este registro.";
                        }
                    }
                    else
                    {
                        return "Error: " + message;
                    }
                }
            }
        }

        #endregion

        #region CULTURE

        public static CultureInfo GetCulture()
        {
            CultureInfo Micultura = new CultureInfo("es-CR", false);
            Micultura.NumberFormat.CurrencySymbol = "₵";
            Micultura.NumberFormat.CurrencyDecimalDigits = 2;
            Micultura.NumberFormat.CurrencyDecimalSeparator = ".";
            Micultura.NumberFormat.CurrencyGroupSeparator = ",";
            int[] grupo = new int[] { 3, 3, 3 };
            Micultura.NumberFormat.CurrencyGroupSizes = grupo;
            Micultura.NumberFormat.NumberDecimalDigits = 2;
            Micultura.NumberFormat.NumberGroupSeparator = ",";
            Micultura.NumberFormat.NumberDecimalSeparator = ".";
            Micultura.NumberFormat.NumberGroupSizes = grupo;
            return Micultura;
        }

        public static CultureInfo GetCultureCaja()
        {
            CultureInfo Micultura = new CultureInfo("es-CR", false);
            Micultura.NumberFormat.CurrencySymbol = "₵";
            Micultura.NumberFormat.CurrencyDecimalDigits = 2;
            Micultura.NumberFormat.CurrencyDecimalSeparator = ".";
            Micultura.NumberFormat.CurrencyGroupSeparator = ",";
            int[] grupo = new int[] { 3, 3, 3 };
            Micultura.NumberFormat.CurrencyGroupSizes = grupo;
            Micultura.NumberFormat.NumberDecimalDigits = 2;
            //Micultura.NumberFormat.NumberGroupSeparator = ",";
            //Micultura.NumberFormat.NumberDecimalSeparator = ".";
            Micultura.NumberFormat.NumberGroupSizes = grupo;
            Micultura.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
            Micultura.DateTimeFormat.ShortDatePattern = "yyyy-MM-dd";
            Micultura.DateTimeFormat.FullDateTimePattern = "yyyy-MM-dd HH:mm:ss";
            Micultura.DateTimeFormat.TimeSeparator = ":";
            Micultura.DateTimeFormat.DateSeparator = "-";

            return Micultura;

        }
        #endregion

        #region Varios
        public static Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        /// <summary>
        /// genera un Stream a partir de un MemoryStream
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static Stream GenerateStreamFromMemoryStream(MemoryStream stream)
        {
            StreamWriter writer = new StreamWriter(stream);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static string GetCorreoPrincipal(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
            {
                return null;
            }
            else
            {
                string[] lista = correo.Split(';');
                if (lista.Length == 0)
                    return correo;
                else
                    return lista[0];

            }
        }

        /// <summary>
        /// Genera una contraseña de valores de a-z A-Z  0-0 !@#$%*_+-/
        /// </summary>
        /// <param name="longitud">Cantidad de digitos que va a tener la contraseña</param>
        /// <returns></returns>
        public static string GenerarContrasena(int longitud)
        {
            const string valido = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%*_+-/";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < longitud--)
            {
                res.Append(valido[rnd.Next(valido.Length)]);
            }
            return res.ToString();
        }

        /// <summary>
        /// verifica que existan los datos de la conexionn con hacienda
        /// </summary>
        /// <param name="dato"></param>
        /// <returns></returns>


        public static string GenerarCodigoVerificador(string pIdCliente, string pConsecutivo, string pIdTipoDocumento, string pImporte)
        {
            string valor = "";
            string cadenaSeguridad = "";
            byte[] HashValue;
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] MessageBytes = UE.GetBytes(pIdCliente + pConsecutivo + pIdTipoDocumento + pImporte);
            SHA256Managed SHhash = new SHA256Managed();
            HashValue = SHhash.ComputeHash(MessageBytes);
            foreach (byte b in HashValue)
            {
                cadenaSeguridad = cadenaSeguridad + b;
            }
            if (cadenaSeguridad.Length >= 8)
            {
                valor = cadenaSeguridad.Substring(0, 8).ToString();
            }
            else
            {
                valor = "";
            }
            return valor;
        }

        public static bool AccesoInternet()
        {
            try
            {
                System.Net.IPHostEntry host = System.Net.Dns.GetHostEntry("fe.msasoft.net");
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        #endregion

    }
}
