using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNoteViewModel;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.PaymentDispositionNoteViewModel
{
   public class PaymentDispositionNoteViewModelTest
    {
        [Fact]
        public void validate_Currency()
        {
            var viewModel = new Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNoteViewModel.PaymentDispositionNoteViewModel()
            {
                AccountBank =new Lib.ViewModels.NewIntegrationViewModel.AccountBankViewModel()
                {
                    Id=1,
                    AccountName="ade",
                    Currency=new Lib.ViewModels.NewIntegrationViewModel.CurrencyViewModel()
                    {
                        Code = "IDR"
                    }
                }
            };
            var result = viewModel.Validate(null);
            Assert.True(0 < result.Count());
        }
    }
}
