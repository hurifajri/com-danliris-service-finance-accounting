using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VbWIthPORequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.VbWithPORequest
{
    public class VbRequestWIthPOListTest
    {
        [Fact]
        public void Should_Succes_Instantiate()
        {

            VbRequestWIthPOList vbRequestWIthPOList = new VbRequestWIthPOList()
            {
                Amount = 1,
                Apporve_Status = true,
                ApproveDate = DateTimeOffset.Now,
                Complete_Status = true,
                CurrencyCode = "Rp",
                CurrencyId = 1,
                CurrencyRate = 1,
                CurrencySymbol = "Rp",
                Date = DateTimeOffset.Now,
                DateEstimate = DateTimeOffset.Now,
                UnitCode = "UnitCode",
                UnitId = 1,
                UnitLoad = "UnitLoad",
                PONo = new List<ModelVbPONumber>()
                {
                    new ModelVbPONumber()
                },
                UnitName = "UnitName",
                VBNo = "VBNo",
                VBRequestCategory = "VBRequestCategory",

            };

            Assert.Equal(1, vbRequestWIthPOList.Amount);
            Assert.True(vbRequestWIthPOList.Apporve_Status);
            Assert.True(DateTimeOffset.MinValue < vbRequestWIthPOList.ApproveDate);
            Assert.True(vbRequestWIthPOList.Complete_Status);
            Assert.Equal("Rp", vbRequestWIthPOList.CurrencyCode);
            Assert.Equal(1, vbRequestWIthPOList.CurrencyId);
            Assert.Equal(1, vbRequestWIthPOList.CurrencyRate);
            Assert.Equal("Rp", vbRequestWIthPOList.CurrencySymbol);
            Assert.True(DateTimeOffset.MinValue < vbRequestWIthPOList.Date);
            Assert.True(DateTimeOffset.MinValue < vbRequestWIthPOList.DateEstimate);
            Assert.Equal(1, vbRequestWIthPOList.CurrencyRate);
            Assert.Equal("UnitCode", vbRequestWIthPOList.UnitCode);
            Assert.Equal(1, vbRequestWIthPOList.UnitId);
            Assert.Equal("UnitLoad", vbRequestWIthPOList.UnitLoad);
            Assert.True(vbRequestWIthPOList.PONo.Count() > 0);
            Assert.Equal("UnitName", vbRequestWIthPOList.UnitName);
            Assert.Equal("VBNo", vbRequestWIthPOList.VBNo);
            Assert.Equal("VBRequestCategory", vbRequestWIthPOList.VBRequestCategory);
        }
    }
}
