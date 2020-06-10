using ImageMagick;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using ZXing;

namespace AdminRFID.Utilerias
{
    public static class Utils
    {
        public static byte[] GenerarCodigoBarras(string cadena)
        {
            System.Drawing.Image img = null;
            using (var ms = new MemoryStream())
            {
                var writer = new ZXing.BarcodeWriter() { Format = BarcodeFormat.CODE_128 };
                writer.Options.Height = 80;
                writer.Options.Width = 280;
                writer.Options.PureBarcode = false;
                img = writer.Write(cadena);
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //img.Save(ObtnerFolderCodigos() + "barras_" + cadena + "_.jpg");
                return ms.ToArray();
            }
        }

        public static string SaveCodigoBarras(string cadena)
        {
            System.Drawing.Image img = null;
            string nameFile = ObtnerFolderCodigos() + "barras_" + cadena + "_.png";
            DeleteFile(nameFile);
            using (var ms = new MemoryStream())
            {
                var writer = new ZXing.BarcodeWriter() { Format = BarcodeFormat.CODE_128 };
                writer.Options.Height = 80;
                writer.Options.Width = 280;
                writer.Options.PureBarcode = false;
                img = writer.Write(cadena);
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                img.Save(nameFile);
                return nameFile;
            }
        }


        public static byte[] GenerarQR(string cadena)
        {
            System.Drawing.Image img = null;
            using (var ms = new MemoryStream())
            {
                var writer = new BarcodeWriter() { Format = BarcodeFormat.QR_CODE };
                writer.Options.Height = 200;
                writer.Options.Width = 200;
                img = writer.Write(cadena);
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                //img.Save(ObtnerFolderCodigos() + "QR_" + cadena + "_.jpg");
                return ms.ToArray();
            }
        }

        public static string SaveCodigoQR(string cadena)
        {
            System.Drawing.Image img = null;
            string NameFile = ObtnerFolderCodigos() + "QR_" + cadena + "_.png";
            DeleteFile(NameFile);
            using (var ms = new MemoryStream())
            {
                var writer = new BarcodeWriter() { Format = BarcodeFormat.QR_CODE };
                writer.Options.Height = 200;
                writer.Options.Width = 200;
                img = writer.Write(cadena);
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                img.Save(NameFile);
                return NameFile;
            }
        }

        public static string ObtnerFolderCodigos()
        {
            string ruta = string.Empty;
            try
            {
                ruta = Path.Combine("/Codigos/");
                ruta = HttpContext.Current.Server.MapPath("~" + ruta);
                DateTime fecha = System.DateTime.Now;

                if (!Directory.Exists(ruta))
                    Directory.CreateDirectory(ruta);
            }
            catch (Exception ex)
            {
                throw new Exception("AL OBTENER LA RUTA DEL PDF", ex);
            }
            return ruta;
        }

        public static byte[] GenerarImprimibleCodigos(string codigo,string producto)
        {
            byte[] content = null;
            string TamañoLetra = "10px";
            string cssTabla = @"style='text-align:center;font-size:" + TamañoLetra + ";font-family:Arial; color:#3E3E3E'";
            string cabeceraTablas = "bgcolor='#404040' style='font-weight:bold; text-align:center; color:white'";
            Document document = new Document(PageSize.A4, 30, 30, 15, 15);
            MemoryStream memStream = new MemoryStream();
            MemoryStream memStreamReader = new MemoryStream();
            PdfWriter PDFWriter = PdfWriter.GetInstance(document, memStream);
            ItextEvents eventos = new ItextEvents();
            eventos.TituloCabecera = "Códigos del Producto: ";
            string UrlcodigoQR= Utilerias.Utils.SaveCodigoQR(codigo);
            string UrlcodigoBarras= Utilerias.Utils.SaveCodigoBarras(codigo);
            int renglonesQR = 5;
            int renglonesBarras = 10;
            //Uri baseUri = new Uri("data:image/png;base64," + codigoQR);
            //Uri baseUriBarras = new Uri("data:image/png;base64," + codigoBarras);         

            //PDFWriter.PageEvent = eventos;
            try
            {
                DateTime fechaActual = System.DateTime.Now;
                DateTimeFormatInfo formatoFecha = new CultureInfo("es-ES", false).DateTimeFormat;
                string nombreMes = formatoFecha.GetMonthName(fechaActual.Month).ToUpper();
                string html = "<br/>";

                // codigos QR
                html += @"<table width='100%' " + cssTabla + @"  CELLPADDING='1' >
                        <tr " + cabeceraTablas + @">
                            <td colspan='4' >Producto: " + producto + @" </td>    
                        </tr>";

                for (int i = 0; i < renglonesQR; i++)
                {
                    html += @"<tr>";
                    html += @"   <td><img src='" + UrlcodigoQR + @"' width = '150' height = '150' align='right' /></td>";
                    html += @"   <td><img src='" + UrlcodigoQR + @"' width = '150' height = '150' align='right' /></td>";
                    html += @"   <td><img src='" + UrlcodigoQR + @"' width = '150' height = '150' align='right' /></td>";
                    html += @"   <td><img src='" + UrlcodigoQR + @"' width = '150' height = '150' align='right' /></td>";
                    html += @"</tr>";

                }

                html += "</table> <br><br> ";


                // codigos de barras
                html += @"<table width='100%' " + cssTabla + @"  CELLPADDING='6' >
                        <tr " + cabeceraTablas + @">
                            <td colspan='3' >Producto: " + producto + @" </td>    
                        </tr>";

                for (int i = 0; i < renglonesBarras; i++)
                {
                    html += @"<tr>";
                    html += @"   <td><img src='" + UrlcodigoBarras + @"' width = '196' height = '56' align='left' /></td>";
                    html += @"   <td><img src='" + UrlcodigoBarras + @"' width = '196' height = '56' align='left' /></td>";
                    html += @"   <td><img src='" + UrlcodigoBarras + @"' width = '196' height = '56' align='left' /></td>";                    
                    html += @"</tr>";

                }

                html += "</table>";

                document.Open();
                foreach (IElement E in HTMLWorker.ParseToList(new StringReader(html.ToString()), new StyleSheet()))
                {
                    document.Add(E);
                }
                document.AddAuthor("RFID");
                document.AddTitle("Codigos: " + producto);
                document.AddCreator("Victor Adrian Reyes");
                document.AddSubject("Codigos de Productos");
                document.CloseDocument();
                document.Close();

                content = memStream.ToArray();

               
                    DeleteFile(UrlcodigoQR);
                    DeleteFile(UrlcodigoBarras);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return content;
        }

        private static void DeleteFile(string nameFile )
        {
            try
            {
                if (File.Exists(nameFile))
                    File.Delete(nameFile);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


    }
}