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
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                //img.Save(ObtnerFolderCodigos() + "barras_" + cadena + "_.jpg");
                return ms.ToArray();
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
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                //img.Save(ObtnerFolderCodigos() + "QR_" + cadena + "_.jpg");
                return ms.ToArray();
            }
        }

        public static string ObtnerFolderCodigos()
        {
            string ruta = string.Empty;
            try
            {
                ruta = HttpContext.Current.Server.MapPath("~" + WebConfigurationManager.AppSettings["pathPdfCodigos"].ToString());
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

        public static string GenerarImprimibleCodigos(string path, string articulo, string producto)
        {
            string TamañoLetra = "10px";
            string cssTabla = @"style='text-align:center;font-size:" + TamañoLetra + ";font-family:Arial; color:#3E3E3E'";
            string cabeceraTablas = "bgcolor='#404040' style='font-weight:bold; text-align:center; color:white'";
            Document document = new Document(PageSize.A4, 30, 30, 15, 15);
            MemoryStream memStream = new MemoryStream();
            MemoryStream memStreamReader = new MemoryStream();
            PdfWriter PDFWriter = PdfWriter.GetInstance(document, memStream);
            ItextEvents eventos = new ItextEvents();
            eventos.TituloCabecera = "Códigos del Producto: ";
            Utilerias.Utils.GenerarQR(articulo);
            Utilerias.Utils.GenerarCodigoBarras(articulo);
            int renglonesQR = 5;
            int renglonesBarras = 10;

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
                    html += @"   <td><img src='" + Path.Combine(path, "QR_" + articulo + "_.jpg") + @"' width = '150' height = '150' align='right' /></td>";
                    html += @"   <td><img src='" + Path.Combine(path, "QR_" + articulo + "_.jpg") + @"' width = '150' height = '150' align='right' /></td>";
                    html += @"   <td><img src='" + Path.Combine(path, "QR_" + articulo + "_.jpg") + @"' width = '150' height = '150' align='right' /></td>";
                    html += @"   <td><img src='" + Path.Combine(path, "QR_" + articulo + "_.jpg") + @"' width = '150' height = '150' align='right' /></td>";
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
                    html += @"   <td><img src='" + Path.Combine(path, "barras_" + articulo + "_.jpg") + @"' width = '196' height = '56' align='left' /></td>";
                    html += @"   <td><img src='" + Path.Combine(path, "barras_" + articulo + "_.jpg") + @"' width = '196' height = '56' align='left' /></td>";
                    html += @"   <td><img src='" + Path.Combine(path, "barras_" + articulo + "_.jpg") + @"' width = '196' height = '56' align='left' /></td>";
                    //html += @"   <td><img src='" + Path.Combine(path, "barras_" + articulo + "_.jpg") + @"' width = '210' height = '60' align='right' /></td>";
                    html += @"</tr>";

                }

                html += "</table>";

                document.Open();
                foreach (IElement E in HTMLWorker.ParseToList(new StringReader(html.ToString()), new StyleSheet()))
                {
                    document.Add(E);
                }
                document.AddAuthor("LLUVIA");
                document.AddTitle("Codigos: " + producto);
                document.AddCreator("Victor Adrian Reyes");
                document.AddSubject("Codigos de Productos");
                document.CloseDocument();
                document.Close();

                byte[] content = memStream.ToArray();
                using (FileStream fs = File.Create(Path.Combine(path, "Codigos_" + articulo + ".pdf")))
                {
                    fs.Write(content, 0, (int)content.Length);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "Codigos_" + articulo + ".pdf";
        }


    }
}