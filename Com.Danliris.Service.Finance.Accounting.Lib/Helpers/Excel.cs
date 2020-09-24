using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;

namespace Com.Danliris.Service.Finance.Accounting.Lib.Helpers
{
    public static class Excel
    {
        /// <summary>
        /// Create an excel file using MemoryStream.
        /// File name is assigned later in Response.AddHeader() when you want to download.
        /// Each DataTable will be rendered in its own sheet, so you need to supply its sheet name as well.
        /// </summary>
        /// <param name="dtSourceList">A List of KeyValuePair of DataTable and its sheet name</param>
        /// <param name="styling">Default style is set to False</param>
        /// <returns>MemoryStream object to be written into Response.OutputStream</returns>
        public static MemoryStream CreateExcel(List<KeyValuePair<DataTable, string>> dtSourceList, bool styling = false)
        {
            ExcelPackage package = new ExcelPackage();
            foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(item.Value);
                sheet.Cells["A1"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        public static MemoryStream CreateExcelVBStatusReport(KeyValuePair<DataTable, string> dataSource, KeyValuePair<DataTable, string> currencySource, DateTimeOffset requestDateFrom, DateTimeOffset requestDateTo, bool styling = false, double requestTotal = 0, double realizationTotal = 0)
        {
            ExcelPackage package = new ExcelPackage();
            //foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            //{
            var sheet = package.Workbook.Worksheets.Add(dataSource.Value);

            int totalRow = dataSource.Key.Rows.Count + 7;
            int period = 3;
            int from = dataSource.Key.Columns.IndexOf("No VB") + 2;
            int to = dataSource.Key.Columns.IndexOf("Aging (Hari)") + 2;
            sheet.Cells[totalRow, from, totalRow, to].Merge = true;
            sheet.Cells[period, from, period, to].Merge = true;

            sheet.Cells["L2"].Value = DateTimeOffset.Now.ToString("dd MMMM yyyy", new CultureInfo("id-ID"));
            sheet.Cells["B2"].Value = "LAPORAN STATUS VB";
            sheet.Cells["B6"].LoadFromDataTable(dataSource.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);

            //sheet.Cells[period, 2].Value = "PERIODE : " + item.Key.Compute("Min([Tanggal VB])", string.Empty) + " SAMPAI DENGAN " + item.Key.Compute("Max([Tanggal VB])", string.Empty);
            sheet.Cells[period, 2].Value = $"PERIODE : { requestDateFrom.Date.ToString("dd MMMM yyyy", new CultureInfo("id-ID"))} sampai dengan { requestDateTo.Date.ToString("dd MMMM yyyy", new CultureInfo("id-ID"))}";
            sheet.Cells[totalRow, 2].Value = "TOTAL";

            int jumlahVb = dataSource.Key.Columns.IndexOf("Jumlah VB") + 2;
            sheet.Cells[totalRow, jumlahVb].Value = requestTotal.ToString("#,##0.###0");

            int realisasi = dataSource.Key.Columns.IndexOf("Realisasi") + 2;
            sheet.Cells[totalRow, realisasi].Value = realizationTotal.ToString("#,##0.###0");

            int sisa = dataSource.Key.Columns.IndexOf("Sisa (Kurang/Lebih)") + 2;
            sheet.Cells[totalRow, sisa].Value = (requestTotal - realizationTotal).ToString("#,##0.###0");

            sheet.Cells[sheet.Dimension.Address].AutoFitColumns();

            var sheet2 = package.Workbook.Worksheets.Add(currencySource.Value);
            sheet2.Cells["A1"].LoadFromDataTable(currencySource.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
            sheet2.Cells[sheet2.Dimension.Address].AutoFitColumns();

            //}

            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        public static MemoryStream CreateExcelJournalTransaction(List<KeyValuePair<DataTable, string>> dtSourceList, DateTimeOffset dateFrom, DateTimeOffset dateTo, bool styling = false)
        {
            ExcelPackage package = new ExcelPackage();

            var sheet = package.Workbook.Worksheets.Add("Sheet 1");

            sheet.Cells["A1"].Value = "LAPORAN JURNAL TRANSAKSI";
            sheet.Cells["A1:C1"].Merge = true;

            sheet.Cells["A2"].Value = $"{dateFrom.Date} - {dateTo.Date}";
            sheet.Cells["B2:C2"].Merge = true;

            foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            {
                sheet.Cells["A4"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        public static MemoryStream CreateExcelNoFilters(List<KeyValuePair<DataTable, string>> dtSourceList, bool styling = false)
        {
            ExcelPackage package = new ExcelPackage();
            int index = 1;
            foreach (KeyValuePair<DataTable, string> item in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(item.Value);
                sheet.Cells["A1"].LoadFromDataTable(item.Key, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                sheet.Tables[string.Format("Table{0}", index++)].ShowFilter = false;
                //sheet.Cells[sheet.Dimension.Address].Style.WrapText = true;

                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }

        public static MemoryStream CreateExcel(List<(DataTable dataTable, string sheetName, List<(string cells, System.Enum hAlign, System.Enum vAlign)> mergeCells)> dtSourceList, bool styling = false)
        {
            ExcelPackage package = new ExcelPackage();
            foreach ((DataTable dataTable, string sheetName, List<(string, System.Enum, System.Enum)> mergeCells) in dtSourceList)
            {
                var sheet = package.Workbook.Worksheets.Add(sheetName);
                sheet.Cells["A1"].LoadFromDataTable(dataTable, true, (styling == true) ? OfficeOpenXml.Table.TableStyles.Light16 : OfficeOpenXml.Table.TableStyles.None);
                foreach ((string cells, System.Enum hAlign, System.Enum vAlign) in mergeCells)
                {
                    sheet.Cells[cells].Merge = true;
                    sheet.Cells[cells].Style.HorizontalAlignment = (OfficeOpenXml.Style.ExcelHorizontalAlignment)hAlign;
                    sheet.Cells[cells].Style.VerticalAlignment = (OfficeOpenXml.Style.ExcelVerticalAlignment)hAlign;
                }
                sheet.Cells[sheet.Dimension.Address].AutoFitColumns();
            }
            MemoryStream stream = new MemoryStream();
            package.SaveAs(stream);
            return stream;
        }
    }
}
