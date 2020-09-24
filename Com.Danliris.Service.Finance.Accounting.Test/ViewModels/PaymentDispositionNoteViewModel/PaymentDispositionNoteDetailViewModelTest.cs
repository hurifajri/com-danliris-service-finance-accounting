using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNoteViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.PaymentDispositionNoteViewModel
{
  public  class PaymentDispositionNoteDetailViewModelTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            var viewModel = new PaymentDispositionNoteDetailViewModel()
            {
                epoId = "1",
                product = new ProductViewModel(),
                quantity = 1,
                unit = new UnitViewModel(),
                uom = new UomViewModel(),
                purchasingDispositionDetailId = 1,
                purchasingDispositionExpeditionItemId = 1
            };

            Assert.Equal("1", viewModel.epoId);
            Assert.NotNull(viewModel.product);
            Assert.Equal(1, viewModel.quantity);
            Assert.NotNull(viewModel.unit);
            Assert.NotNull(viewModel.uom);
            Assert.Equal(1, viewModel.purchasingDispositionDetailId);
            Assert.Equal(1, viewModel.purchasingDispositionExpeditionItemId);
        }
    }
}
