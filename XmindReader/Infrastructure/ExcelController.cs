using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ClosedXML.Excel;

namespace XmindReader.Infrastructure
{
    static class ExcelController
    {
        public static void ConvertFromBookToCsvs(
            string excelfile, 
            string outdir)
        {
            using var workbook = new XLWorkbook(excelfile);

            foreach (var worksheet in workbook.Worksheets)
            {
                WindowsCsv.Write(
                    GetRows(worksheet),
                    Path.Combine(outdir, worksheet.Name));
            }
        }

        public static void ConvertFromSheetToCsv(
            string excelfile, 
            string sheetname, 
            string outdir)
        {
            using var workbook = new XLWorkbook(excelfile);
            var worksheet = workbook.Worksheet(sheetname);
            WindowsCsv.Write(
                GetRows(worksheet),
                Path.Combine(outdir, sheetname));
        }

        private static IEnumerable<string[]> GetRows(IXLWorksheet worksheet)
        {
            // 見た目のまま返すのをデフォルトにする
            return GetRows(worksheet, x => x.GetFormattedString());
        }

        private static IEnumerable<string[]> GetRows(
            IXLWorksheet worksheet,
            Func<IXLCell, string> getCellValue)
        {
            foreach (var row in worksheet.Rows())
                yield return row.Cells().Select(x => getCellValue.Invoke(x)).ToArray();
        }
    }
}
