using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.NewIntegrationViewModel
{
  public  class ProductViewModelTest
    {
        [Fact]
        public void Should_Succes_Instantiate_ProductViewModel()
        {
            string id = "id test";
            string code = "code test";
            string nama = "name test";
            double Price = 1.1;

            UomViewModel uomViewModel = new UomViewModel();
            

            ProductViewModel productViewModel = new ProductViewModel();
            productViewModel.Id = id;
            productViewModel.Name = nama;
            productViewModel.Code = code;
            productViewModel.Price = Price;
            productViewModel.UOM = uomViewModel;

            Assert.Equal(id, productViewModel.Id);
            Assert.Equal(nama, productViewModel.Name);
            Assert.Equal(code, productViewModel.Code);
            Assert.Equal(Price, productViewModel.Price);
            Assert.NotNull( productViewModel.UOM);

        }
    }
}
