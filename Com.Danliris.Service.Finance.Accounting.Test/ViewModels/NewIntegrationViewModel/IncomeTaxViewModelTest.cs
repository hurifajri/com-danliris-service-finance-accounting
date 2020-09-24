
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.NewIntegrationViewModel
{
    public class IncomeTaxViewModelTest
    {
        [Fact]
        public void Should_Succes_Intantiate_IncomeTaxViewModel()
        {
            int id = 1;
            string nama = "name test";
            double rate = 1.1;

            IncomeTaxViewModel viewModel = new IncomeTaxViewModel();
            viewModel.Id = id;
            viewModel.Name = nama;
            viewModel.Rate = rate;

            Assert.Equal(id, viewModel.Id);
            Assert.Equal(nama, viewModel.Name);
            Assert.Equal(rate, viewModel.Rate);
        }
    }
}
