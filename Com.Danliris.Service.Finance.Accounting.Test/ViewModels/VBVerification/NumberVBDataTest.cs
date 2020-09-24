using Com.Danliris.Service.Finance.Accounting.Lib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.VBVerification
{
    public class NumberVBDataTest
    {
        [Fact]
        public void should_success_intantiate()
        {
            NumberVBData viewModel = new NumberVBData()
            {
                Amount_Realization = 1,
                Amount_Request = 1,
                Amount_Vat = 1,
                Currency = "Currency",
                DateEstimate = DateTimeOffset.Now,
                DateRealization = DateTimeOffset.Now,
                DateVB = DateTimeOffset.Now,
                Diff = 1,
                RequestVbName = "RequestVbName",
                Status_ReqReal = "Status_ReqReal",
                Usage = "Usage",
                VBNoRealize = "VBNoRealize",
                VBRealizeCategory = "VBRealizeCategory",
                UnitName = "UnitName",
                VBNo = "VBNo",
                DetailItems = new List<VbVerificationDetailViewModel>()
                {
                    new VbVerificationDetailViewModel()
                },
                Id = 1
            };

            Assert.Equal(1, viewModel.Amount_Realization);
            Assert.Equal(1, viewModel.Amount_Request);
            Assert.Equal(1, viewModel.Amount_Vat);
            Assert.Equal("Currency", viewModel.Currency);
            Assert.True(DateTimeOffset.MinValue < viewModel.DateEstimate);
            Assert.True(DateTimeOffset.MinValue < viewModel.DateRealization);
            Assert.True(DateTimeOffset.MinValue < viewModel.DateVB);
            Assert.Equal(1, viewModel.Diff);
            Assert.Equal("RequestVbName", viewModel.RequestVbName);
            Assert.Equal("Status_ReqReal", viewModel.Status_ReqReal);
            Assert.Equal("Usage", viewModel.Usage);
            Assert.Equal("VBNoRealize", viewModel.VBNoRealize);
            Assert.Equal("VBRealizeCategory", viewModel.VBRealizeCategory);
            Assert.Equal("UnitName", viewModel.UnitName);
            Assert.Equal("VBNo", viewModel.VBNo);
            Assert.NotNull(viewModel.DetailItems);

            Assert.Equal(1, viewModel.Id);

        }
    }
}
