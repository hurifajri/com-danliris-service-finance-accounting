using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.NewIntegrationViewModel
{
    public class DivisionViewModelTest
    {
        [Fact]
        public void Should_succes_Instantiate_DivisionViewModel()
        {
            string id = "id test";
            string nama = "name test";
            string code = "code test";

            DivisionViewModel viewModel = new DivisionViewModel();
            viewModel.Id = id;
            viewModel.Name = nama;
            viewModel.Code = code;

            Assert.Equal(id, viewModel.Id);
            Assert.Equal(nama, viewModel.Name);
            Assert.Equal(code, viewModel.Code);
        }
    }
}
