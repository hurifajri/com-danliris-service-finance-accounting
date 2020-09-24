using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.NewIntegrationViewModel
{
    public class AccountBankViewModelTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            int id = 1;
            string code = "code test";
            string BankCode = "BankCode test";
            string AccountName = "AccountName test";
            string AccountNumber = "AccountNumber test";
            string BankName = "BankName test";
            CurrencyViewModel cvm = new CurrencyViewModel();

            AccountBankViewModel viewmodel = new AccountBankViewModel()
            {
                Id = id,
                Code = code,
                BankCode = BankCode,
                AccountName = AccountName,
                AccountNumber = AccountNumber,
                BankName = BankName,
                Currency = cvm
            };



            Assert.Equal(id, viewmodel.Id);
            Assert.Equal(code, viewmodel.Code);
            Assert.Equal(BankCode, viewmodel.BankCode);
            Assert.Equal(AccountName, viewmodel.AccountName);
            Assert.Equal(AccountNumber, viewmodel.AccountNumber);
            Assert.Equal(cvm, viewmodel.Currency);

        }
    }
}
