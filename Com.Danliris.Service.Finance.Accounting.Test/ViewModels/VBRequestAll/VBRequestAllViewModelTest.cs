using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRequestAll;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.VBRequestAll
{
    public class VBRequestAllViewModelTest
    {
        [Fact]
        public void should_success_intantiate()
        {
            VBRequestAllViewModel viewModel = new VBRequestAllViewModel()
            {
                CurrencySymbol = "CurrencySymbol",
                Amount =1,
                ApproveDate =DateTimeOffset.Now,
                Approve_Status =true,
                Complete_Status =true,
                CurrencyCode ="RP",
                CurrencyId =1,
                CurrencyRate =1,
                Date =DateTimeOffset.Now,
                DateEstimate =DateTimeOffset.Now,
                UnitCode = "UnitCode",
                UnitLoad = "UnitLoad",
                UnitId =1,
                UnitName = "UnitName",
                VBNo = "VBNo",
                VBRequestCategory = "VBRequestCategory",
                Id =1
            };

            Assert.Equal("CurrencySymbol", viewModel.CurrencySymbol);
            Assert.Equal(1, viewModel.Amount);
            Assert.True(DateTimeOffset.MinValue < viewModel.ApproveDate);
            Assert.True(viewModel.Approve_Status);
            Assert.True(viewModel.Complete_Status);
            Assert.Equal("RP", viewModel.CurrencyCode);
            Assert.Equal(1, viewModel.CurrencyId);
            Assert.Equal(1, viewModel.CurrencyRate);
            Assert.True(DateTimeOffset.MinValue < viewModel.Date);
            Assert.True(DateTimeOffset.MinValue < viewModel.DateEstimate);
            Assert.Equal("UnitCode", viewModel.UnitCode);
            Assert.Equal("UnitLoad", viewModel.UnitLoad);
            Assert.Equal("UnitName", viewModel.UnitName);
            Assert.Equal("VBNo", viewModel.VBNo);
            Assert.Equal("VBRequestCategory", viewModel.VBRequestCategory);
            Assert.Equal(1, viewModel.Id);
        }
    }
}
