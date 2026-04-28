using System.IO;
using ClosedXML.Excel;

namespace OsakidetzaListas.Services;

public class ExportService
{
    public byte[] ExportarRankingExcel(
        List<(string Dni, string Nombre, decimal Zcalexa, decimal Zcalexp, decimal Zptoeus, decimal Puntuacion)> ranking,
        string categoria,
        string? dniCorte = null)
    {
        using var wb = new XLWorkbook();
        var ws = wb.Worksheets.Add(categoria.Length > 31 ? categoria[..31] : categoria);

        // Headers
        ws.Cell("A1").Value = "Posición";
        ws.Cell("B1").Value = "Nombre";
        ws.Cell("C1").Value = "DNI";
        ws.Cell("D1").Value = "Experiencia";
        ws.Cell("E1").Value = "Euskera";
        ws.Cell("F1").Value = "Oposición";
        ws.Cell("G1").Value = "Puntuación Total";

        // Estilos header
        var headerRange = ws.Range("A1:G1");
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.DarkBlue;
        headerRange.Style.Font.FontColor = XLColor.White;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        // Datos
        var fila = 2;
        var posicion = 1;
        var cortado = false;

        foreach (var item in ranking)
        {
            if (!string.IsNullOrEmpty(dniCorte) && item.Dni == dniCorte)
            {
                cortado = true;
            }

            ws.Cell($"A{fila}").Value = posicion;
            ws.Cell($"B{fila}").Value = item.Nombre;
            ws.Cell($"C{fila}").Value = item.Dni;
            ws.Cell($"D{fila}").Value = item.Zcalexp;
            ws.Cell($"E{fila}").Value = item.Zptoeus;
            ws.Cell($"F{fila}").Value = item.Zcalexa;
            ws.Cell($"G{fila}").Value = item.Puntuacion;

            // Estilos datos
            ws.Cell($"A{fila}").Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
            ws.Cell($"D{fila}").Style.NumberFormat.Format = "0.00";
            ws.Cell($"E{fila}").Style.NumberFormat.Format = "0.00";
            ws.Cell($"F{fila}").Style.NumberFormat.Format = "0.00";
            ws.Cell($"G{fila}").Style.NumberFormat.Format = "0.00";

            if (cortado)
            {
                var rowRange = ws.Range($"A{fila}:G{fila}");
                rowRange.Style.Fill.BackgroundColor = XLColor.LightGray;
            }

            fila++;
            posicion++;

            if (!string.IsNullOrEmpty(dniCorte) && cortado) break;
        }

        // Ajustar ancho columnas
        ws.Columns("A", "G").AdjustToContents();

        // Guardar a MemoryStream
        using var ms = new MemoryStream();
        wb.SaveAs(ms);
        return ms.ToArray();
    }
}
