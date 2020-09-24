using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.NewIntegrationViewModel
{
    public class SalesInvoiceViewModelTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            int id = 1;
            string salesInvoiceNo = "salesInvoiceNo test";
            string vatType = "vatType test";
            CurrencyViewModel cvm = new CurrencyViewModel();

            SalesInvoiceViewModel viewModel = new SalesInvoiceViewModel()
            {
                Id = id,
                SalesInvoiceNo = salesInvoiceNo,
                VatType = vatType,
                Currency = cvm
            };

            Assert.Equal(id, viewModel.Id);
            Assert.Equal(salesInvoiceNo, viewModel.SalesInvoiceNo);
            Assert.Equal(vatType, viewModel.VatType);
            Assert.Equal(cvm, viewModel.Currency);

        }
    }
}