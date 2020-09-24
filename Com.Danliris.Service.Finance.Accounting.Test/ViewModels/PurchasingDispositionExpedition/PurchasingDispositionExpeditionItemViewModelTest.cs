using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionExpedition;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.PurchasingDispositionExpedition
{
    public class PurchasingDispositionExpeditionItemViewModelTest
    {
        [Fact]
        public void Succes_Instantiate()
        {
            var viewModel = new PurchasingDispositionExpeditionItemViewModel()
            {
                epoId = "1",
                price = 1,
                product = new ProductViewModel(),
                purchasingDispositionDetailId = 1,
                quantity = 1,
                unit = new UnitViewModel(),
                uom = new UomViewModel()
            };

            Assert.Equal("1", viewModel.epoId);
            Assert.Equal(1, viewModel.price);
            Assert.Equal(1, viewModel.purchasingDispositionDetailId);
            Assert.Equal(1, viewModel.quantity);
            Assert.NotNull(viewModel.product);
            Assert.NotNull(viewModel.unit);
            Assert.NotNull(viewModel.uom);
        }
    }

}
