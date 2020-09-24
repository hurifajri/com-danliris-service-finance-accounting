using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.SalesReceipt;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.SalesReceipt
{
   public class SalesInvoiceReportSalesReceiptViewModelTest
    {

        [Fact]
        public void should_success_intantiate()
        {
            SalesInvoiceReportSalesReceiptViewModel viewModel = new SalesInvoiceReportSalesReceiptViewModel()
            {
                CurrencySymbol = "CurrencySymbol",
                Nominal =1,
                SalesInvoiceId =1,
                SalesReceiptDate =DateTimeOffset.Now,
                SalesReceiptNo ="1",
                TotalPaid =1,
                UnPaid =1
            };

            Assert.Equal(1, viewModel.Nominal);
            Assert.True(DateTimeOffset.MinValue < viewModel.SalesReceiptDate);
            Assert.Equal(1, viewModel.SalesInvoiceId);
            Assert.Equal("1", viewModel.SalesReceiptNo);
            Assert.Equal(1, viewModel.TotalPaid);
            Assert.Equal(1, viewModel.UnPaid);
        }
    }
}
