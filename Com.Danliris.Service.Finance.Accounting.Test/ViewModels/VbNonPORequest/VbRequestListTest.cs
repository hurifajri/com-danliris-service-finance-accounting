using Com.Danliris.Service.Finance.Accounting.Lib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.VbNonPORequest
{
 public   class VbRequestListTest
    {
        [Fact]
        public void should_success_intantiate()
        {
            VbRequestList viewModel = new VbRequestList()
            {
                CurrencySymbol = "CurrencySymbol",
                Amount = 1,
                Complete_Status = true,
                CurrencyCode = "RP",
                CurrencyRate = 1,
                Date = DateTimeOffset.Now,
                DateEstimate = DateTimeOffset.Now,
                UnitCode = "UnitCode",
                UnitLoad = "UnitLoad",
                UnitId = 1,
                UnitName = "UnitName",
                VBNo = "VBNo",
                VBRequestCategory = "VBRequestCategory",
                Id = 1,
                Apporve_Status =true,
                CurrencyDescription = "CurrencyDescription",
                RealizationStatus =true,
                PONo =new List<Lib.Models.VbNonPORequest.VbRequestDetailModel>()
                {
                    new Lib.Models.VbNonPORequest.VbRequestDetailModel()
                },
                UnitDivisionId =1,
                Status_Post =true,
                UnitDivisionName = "UnitDivisionName",
                Usage = "Usage"
            };

            Assert.Equal("CurrencySymbol", viewModel.CurrencySymbol);
            Assert.Equal("RP", viewModel.CurrencyCode);
            Assert.Equal(1, viewModel.Amount);
            Assert.True(viewModel.Apporve_Status);
            Assert.True(viewModel.Complete_Status);
            Assert.Equal("RP", viewModel.CurrencyCode);
            Assert.Equal(1, viewModel.CurrencyRate);
            Assert.Equal(1, viewModel.CurrencyRate);
            Assert.True(DateTimeOffset.MinValue < viewModel.Date);
            Assert.True(DateTimeOffset.MinValue < viewModel.DateEstimate);
            Assert.Equal("UnitCode", viewModel.UnitCode);
            Assert.Equal("UnitLoad", viewModel.UnitLoad);
            Assert.Equal("UnitName", viewModel.UnitName);
            Assert.Equal("VBNo", viewModel.VBNo);
            Assert.Equal("VBRequestCategory", viewModel.VBRequestCategory);
            Assert.Equal(1, viewModel.Id);
            Assert.Equal(1, viewModel.UnitDivisionId);
            Assert.NotNull(viewModel.PONo);
            Assert.True(viewModel.Status_Post);
            Assert.Equal("UnitDivisionName", viewModel.UnitDivisionName);
            Assert.Equal("Usage", viewModel.Usage);
        }
    }
}
