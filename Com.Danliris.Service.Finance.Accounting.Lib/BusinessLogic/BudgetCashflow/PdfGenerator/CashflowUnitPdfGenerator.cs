using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow.PdfGenerator
{
    public static class CashflowUnitPdfGenerator
    {
        private static readonly Font _headerFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 11);
        private static readonly Font _subHeaderFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 10);
        private static readonly Font _normalFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _smallFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerFont = FontFactory.GetFont(BaseFont.HELVETICA, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
        private static readonly Font _normalBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9);
        private static readonly Font _normalBoldWhiteFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 9, 0, BaseColor.White);
        private static readonly Font _smallBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 8);
        private static readonly Font _smallerBoldFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7);
        private static readonly Font _smallerBoldWhiteFont = FontFactory.GetFont(BaseFont.HELVETICA_BOLD, BaseFont.CP1250, BaseFont.NOT_EMBEDDED, 7, 0, BaseColor.White);

        public static MemoryStream Generate(UnitDto unit, DateTimeOffset dueDate, int offset, List<BudgetCashflowItemDto> data)
        {
            var document = new Document(PageSize.A4.Rotate(), 20, 20, 20, 20);
            var stream = new MemoryStream();
            var writer = PdfWriter.GetInstance(document, stream);
            document.Open();

            SetTitle(document, unit, dueDate, offset);
            SetTable(document, data);

            document.Close();
            byte[] byteInfo = stream.ToArray();
            stream.Write(byteInfo, 0, byteInfo.Length);
            stream.Position = 0;

            return stream;
        }

        private static void SetTable(Document document, List<BudgetCashflowItemDto> data)
        {
            var table = new PdfPTable(8)
            {
                WidthPercentage = 100,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            table.SetWidths(new float[] { 2f, 2f, 1f, 12f, 6f, 8f, 8f, 8f });
            var cellRotate = new PdfPCell()
            {
                Border = Rectangle.RECTANGLE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_TOP,
                Rotation = 270
            };

            var cellCenter = new PdfPCell()
            {
                Border = Rectangle.RECTANGLE,
                HorizontalAlignment = Element.ALIGN_CENTER,
                VerticalAlignment = Element.ALIGN_CENTER
            };

            var cellLeft = new PdfPCell()
            {
                Border = Rectangle.RECTANGLE,
                HorizontalAlignment = Element.ALIGN_LEFT,
                VerticalAlignment = Element.ALIGN_CENTER
            };

            var cellRight = new PdfPCell()
            {
                Border = Rectangle.RECTANGLE,
                HorizontalAlignment = Element.ALIGN_RIGHT,
                VerticalAlignment = Element.ALIGN_CENTER
            };

            cellCenter.Colspan = 4;
            cellCenter.Phrase = new Phrase("KETERANGAN", _smallBoldFont);
            table.AddCell(cellCenter);
            cellCenter.Colspan = 1;
            cellCenter.Phrase = new Phrase("MATA UANG", _smallBoldFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("NOMINAL VALAS", _smallBoldFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("NOMINAL IDR", _smallBoldFont);
            table.AddCell(cellCenter);
            cellCenter.Phrase = new Phrase("ACTUAL", _smallBoldFont);
            table.AddCell(cellCenter);

            foreach (var item in data)
            {
                if (item.IsUseSection)
                {
                    cellRotate.Rowspan = item.SectionRowSpan > 0 ? item.SectionRowSpan : 1;
                    cellRotate.Phrase = new Phrase(item.CashflowTypeName, _smallFont);
                    table.AddCell(cellRotate);
                }
                //else
                //{
                //    cellRotate.Rowspan = 1;
                //    cellRotate.Phrase = new Phrase(item.CashflowTypeName, _smallFont);
                //    table.AddCell(cellRotate);
                //}

                if (item.IsUseGroup)
                {
                    cellRotate.Rowspan = item.GroupRowSpan > 0 ? item.GroupRowSpan : 1;
                    cellRotate.Phrase = new Phrase(item.TypeName, _smallFont);
                    table.AddCell(cellRotate);
                }
                //else
                //{
                //    cellRotate.Rowspan = 1;
                //    cellRotate.Phrase = new Phrase(item.TypeName, _smallFont);
                //    table.AddCell(cellRotate);
                //}

                if (item.CashflowCategoryId > 0)
                {
                    cellLeft.Colspan = 6;
                    cellLeft.Phrase = new Phrase(item.CashflowCategoryName, _smallFont);
                    table.AddCell(cellLeft);
                }

                if (item.SubCategoryId > 0)
                {
                    cellLeft.Colspan = 1;
                    cellLeft.Phrase = new Phrase("", _smallFont);
                    table.AddCell(cellLeft);

                    if (item.IsShowSubCategoryLabel)
                        cellLeft.Phrase = new Phrase(item.SubCategoryName, _smallFont);
                    else
                        cellLeft.Phrase = new Phrase("", _smallFont);
                    table.AddCell(cellLeft);

                    cellCenter.Colspan = 1;
                    cellCenter.Phrase = new Phrase(item.Currency?.Code, _smallFont);
                    table.AddCell(cellCenter);

                    cellRight.Colspan = 1;
                    cellRight.Phrase = new Phrase(item.Nominal.ToString(), _smallFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.CurrencyNominal.ToString(), _smallFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.Total.ToString(), _smallFont);
                    table.AddCell(cellRight);
                }

                if (!string.IsNullOrWhiteSpace(item.TotalLabel))
                {
                    cellLeft.Colspan = 2;
                    if (item.IsShowTotalLabel)
                        cellLeft.Phrase = new Phrase(item.TotalLabel, _smallFont);
                    else
                        cellLeft.Phrase = new Phrase("", _smallFont);
                    table.AddCell(cellLeft);

                    cellCenter.Colspan = 1;
                    cellCenter.Phrase = new Phrase(item.Currency?.Code, _smallFont);
                    table.AddCell(cellCenter);

                    cellRight.Colspan = 1;
                    cellRight.Phrase = new Phrase(item.Nominal.ToString(), _smallFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.CurrencyNominal.ToString(), _smallFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.Total.ToString(), _smallFont);
                    table.AddCell(cellRight);
                }

                if (!string.IsNullOrWhiteSpace(item.DifferenceLabel))
                {
                    cellLeft.Colspan = 3;
                    if (item.IsShowDifferenceLabel)
                        cellLeft.Phrase = new Phrase(item.DifferenceLabel, _smallFont);
                    else
                        cellLeft.Phrase = new Phrase("", _smallFont);
                    table.AddCell(cellLeft);

                    cellCenter.Colspan = 1;
                    cellCenter.Phrase = new Phrase(item.Currency?.Code, _smallFont);
                    table.AddCell(cellCenter);

                    cellRight.Colspan = 1;
                    cellRight.Phrase = new Phrase(item.Nominal.ToString(), _smallFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.CurrencyNominal.ToString(), _smallFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.Total.ToString(), _smallFont);
                    table.AddCell(cellRight);
                }

                if (item.IsSummaryBalance)
                {
                    cellLeft.Colspan = 4;
                    if (item.IsShowSummaryBalance)
                        cellLeft.Phrase = new Phrase(item.SummaryBalanceLabel, _smallFont);
                    else
                        cellLeft.Phrase = new Phrase("", _smallFont);
                    table.AddCell(cellLeft);

                    cellCenter.Colspan = 1;
                    cellCenter.Phrase = new Phrase(item.Currency?.Code, _smallFont);
                    table.AddCell(cellCenter);

                    cellRight.Colspan = 1;
                    cellRight.Phrase = new Phrase(item.Nominal.ToString(), _smallFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.CurrencyNominal.ToString(), _smallFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.Total.ToString(), _smallFont);
                    table.AddCell(cellRight);
                }

                if (item.IsShowSummary)
                {
                    cellLeft.Colspan = 4;
                    if (item.IsShowSummaryLabel)
                        cellLeft.Phrase = new Phrase(item.SummaryLabel, _smallFont);
                    else
                        cellLeft.Phrase = new Phrase("", _smallFont);
                    table.AddCell(cellLeft);

                    cellCenter.Colspan = 1;
                    cellCenter.Phrase = new Phrase(item.Currency?.Code, _smallFont);
                    table.AddCell(cellCenter);

                    cellRight.Colspan = 1;
                    cellRight.Phrase = new Phrase(item.Nominal.ToString(), _smallFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.CurrencyNominal.ToString(), _smallFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.Total.ToString(), _smallFont);
                    table.AddCell(cellRight);
                }

                if (item.IsRealCashBalance)
                {
                    cellLeft.Colspan = 4;
                    if (item.IsShowRealCashBalanceLabel)
                        cellLeft.Phrase = new Phrase("Saldo Real Kas", _smallFont);
                    else
                        cellLeft.Phrase = new Phrase("", _smallFont);
                    table.AddCell(cellLeft);

                    cellCenter.Colspan = 1;
                    cellCenter.Phrase = new Phrase(item.Currency?.Code, _smallFont);
                    table.AddCell(cellCenter);

                    cellRight.Colspan = 1;
                    cellRight.Phrase = new Phrase(item.Nominal.ToString(), _smallFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.CurrencyNominal.ToString(), _smallFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.Total.ToString(), _smallFont);
                    table.AddCell(cellRight);
                }

                if (item.IsShowCurrencyRate)
                {
                    cellLeft.Colspan = 4;
                    if (item.IsShowCurrencyRateLabel)
                        cellLeft.Phrase = new Phrase("Rate", _smallFont);
                    else
                        cellLeft.Phrase = new Phrase("", _smallFont);
                    table.AddCell(cellLeft);

                    cellCenter.Colspan = 1;
                    cellCenter.Phrase = new Phrase(item.Currency?.Code, _smallFont);
                    table.AddCell(cellCenter);

                    cellRight.Colspan = 1;
                    cellRight.Phrase = new Phrase(item.Currency == null ? "" : item.Currency.Rate.ToString(), _smallFont);
                    table.AddCell(cellRight);
                    cellRight.Colspan = 2;
                    cellRight.Phrase = new Phrase("", _smallFont);
                    table.AddCell(cellRight);
                }

                if (item.IsShowRealCashDifference)
                {
                    cellLeft.Colspan = 4;
                    if (item.IsShowRealCashDifferenceLabel)
                        cellLeft.Phrase = new Phrase(item.RealCashDifferenceLabel, _smallFont);
                    else
                        cellLeft.Phrase = new Phrase("", _smallFont);
                    table.AddCell(cellLeft);

                    cellCenter.Colspan = 1;
                    cellCenter.Phrase = new Phrase(item.Currency?.Code, _smallFont);
                    table.AddCell(cellCenter);

                    cellRight.Colspan = 1;
                    cellRight.Phrase = new Phrase(item.Nominal.ToString(), _smallFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.CurrencyNominal.ToString(), _smallFont);
                    table.AddCell(cellRight);
                    cellRight.Phrase = new Phrase(item.Total.ToString(), _smallFont);
                    table.AddCell(cellRight);
                }

                if (item.IsEquivalentDifference)
                {
                    cellLeft.Colspan = 4;
                    cellLeft.Phrase = new Phrase("Total Surplus (Defisit) Equivalent", _smallFont);
                    table.AddCell(cellLeft);

                    cellCenter.Colspan = 1;
                    cellCenter.Phrase = new Phrase("IDR", _smallFont);
                    table.AddCell(cellCenter);
                    cellRight.Colspan = 1;
                    cellRight.Phrase = new Phrase(item.Total.ToString(), _smallFont);
                    table.AddCell(cellRight);
                    cellRight.Colspan = 2;
                    cellRight.Phrase = new Phrase("", _smallFont);
                    table.AddCell(cellRight);
                }
                //if (item.SubCategoryId > 0)
                //{

                //}
            }

            document.Add(table);
        }

        private static void SetTitle(Document document, UnitDto unit, DateTimeOffset dueDate, int offset)
        {
            var company = "PT DAN LIRIS";
            var title = "LAPORAN BUDGET CASHFLOW";
            var unitName = "UNIT: ";
            if (unit != null)
                unitName += unit.Name;

            var dueDateString = $"{dueDate.AddMonths(1).AddHours(offset).DateTime.ToString("MMMM yyyy", new CultureInfo("id-ID")).ToUpper()}";
            var date = $"PERIODE S.D. {dueDateString}";

            var table = new PdfPTable(1)
            {
                WidthPercentage = 100,
                HorizontalAlignment = Element.ALIGN_LEFT
            };

            var cell = new PdfPCell()
            {
                Border = Rectangle.NO_BORDER,
                HorizontalAlignment = Element.ALIGN_LEFT,
                Phrase = new Phrase(company, _headerFont),
            };
            table.AddCell(cell);

            cell.Phrase = new Phrase(title, _headerFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase(unitName, _headerFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase(date, _headerFont);
            table.AddCell(cell);

            cell.Phrase = new Phrase("\n", _headerFont);
            table.AddCell(cell);

            document.Add(table);
        }
    }
}
