using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.NewIntegrationViewModel
{

    public class CategoryViewModelTest
    {
        [Fact]
        public void Should_Success_Instantiate_CategoryViewModel()
        {
            string id = "id test";
            string nama = "name test";
            string code = "code test";

            CategoryViewModel viewModel = new CategoryViewModel()
            {
                Id = id,
                Name = nama,
                Code = code,
            };

            Assert.Equal(id, viewModel.Id);
            Assert.Equal(nama, viewModel.Name);
            Assert.Equal(code, viewModel.Code);
        }



    }
}
