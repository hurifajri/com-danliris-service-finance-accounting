using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PaymentDispositionNoteViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.PurchasingDispositionExpedition
{
    public class PaymentDispositionNoteItemViewModelTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            var viewModel = new PaymentDispositionNoteItemViewModel()
            {
                proformaNo = "proformaNo",
                division = new DivisionViewModel(),
                purchasingDispositionExpeditionId = 1,
                dispositionDate = DateTimeOffset.Now,
                paymentDueDate = DateTimeOffset.Now,
                totalPaid = 1,
                dispositionId = "dispositionId",
                dispositionNo = "dispositionNo",
                dpp = 1,
                vatValue = 1,
                incomeTaxValue = 1,
                category = new CategoryViewModel(),
                payToSupplier = 1,
                Details = new List<PaymentDispositionNoteDetailViewModel>()
                {
                    new PaymentDispositionNoteDetailViewModel()
                }
            };

            Assert.Equal("proformaNo", viewModel.proformaNo);
            Assert.NotNull(viewModel.category);
            Assert.Equal(1, viewModel.purchasingDispositionExpeditionId);
            Assert.NotNull(viewModel.division);
            Assert.NotNull(viewModel.Details);
            Assert.Equal(1, viewModel.totalPaid);
            Assert.Equal(1, viewModel.dpp);
            Assert.Equal(1, viewModel.vatValue);
            Assert.Equal(1, viewModel.incomeTaxValue);
            Assert.Equal(1, viewModel.payToSupplier);
            Assert.Equal("dispositionId", viewModel.dispositionId);
        }
    }
}
