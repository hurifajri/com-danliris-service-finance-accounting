using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionReport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.PurchasingDispositionReport
{
  public  class PurchasingDispositionReportPostedViewModelTest
    {
        [Fact]
        public void should_success_intantiate()
        {
            PurchasingDispositionReportPostedViewModel viewModel = new PurchasingDispositionReportPostedViewModel()
            {
                Data =new List<PurchasingDispositionViewModel>()
                {
                    new PurchasingDispositionViewModel()
                },
                DateFrom =DateTimeOffset.MinValue,
                DateTo=DateTimeOffset.MaxValue
            };

            Assert.NotNull(viewModel.Data);
            Assert.Equal(DateTimeOffset.MinValue, viewModel.DateFrom);
            Assert.Equal(DateTimeOffset.MaxValue, viewModel.DateTo);
        }
    }
}
