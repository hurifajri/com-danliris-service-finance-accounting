using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.NewIntegrationViewModel
{
    public class CurrencyViewModelTest
    {
        [Fact]
        public void Succes_Instantiate_CurrencyViewModel()
        {
            long id = 1;
            string Code = "code test";
            string Symbol = "symbol test";
            double Rate = 1;
            string Description = "Description test";

            CurrencyViewModel viewModel = new CurrencyViewModel()
            {
                Id = id,
                Code = Code,
                Symbol = Symbol,
                Rate = Rate,
                Description = Description,
            };


            Assert.Equal(id, viewModel.Id);
            Assert.Equal(Code, viewModel.Code);
            Assert.Equal(Rate, viewModel.Rate);
            Assert.Equal(Symbol, viewModel.Symbol);
            Assert.Equal(Description, viewModel.Description);


        }
    }
}
