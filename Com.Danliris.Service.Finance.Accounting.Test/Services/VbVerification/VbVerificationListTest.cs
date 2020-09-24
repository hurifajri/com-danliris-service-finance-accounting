using Com.Danliris.Service.Finance.Accounting.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.VbVerification
{
    public class VbVerificationListTest
    {

        [Fact]
        public void Should_Succes_Instantiate()
        {
            VbVerificationList vbVerificationList = new VbVerificationList()
            {
                Amount_Realization = 1,
                Amount_Request = 1,
                Amount_Vat = 1,
                DateEstimate = DateTimeOffset.Now,
                DateRealization = DateTimeOffset.Now,
                DateVB = DateTimeOffset.Now,
                Diff = 1,
                DetailItems = new List<ModelVbItem>()
                {
                       new ModelVbItem()
                },
                RequestVbName = "RequestVbName",
                Status_ReqReal = "Status_ReqReal",
                UnitLoad = "UnitLoad",
                UnitName = "UnitName",
                VBNo = "VBNo",
                VBNoRealize = "VBNoRealize",
                VBRealizeCategory = "VBRealizeCategory",
                Currency = "rupiah",
                Id = 1,
                Usage = "Usage",
            };

            Assert.Equal(1, vbVerificationList.Id);
            Assert.Equal(1, vbVerificationList.Amount_Realization);
            Assert.Equal(1, vbVerificationList.Amount_Request);
            Assert.Equal(1, vbVerificationList.Amount_Vat);
            Assert.Equal("rupiah", vbVerificationList.Currency);
            Assert.True(DateTimeOffset.MinValue < vbVerificationList.DateEstimate);
            Assert.True(DateTimeOffset.MinValue < vbVerificationList.DateRealization);
            Assert.True(DateTimeOffset.MinValue < vbVerificationList.DateVB);
            Assert.Equal(1, vbVerificationList.Diff);
            Assert.True(vbVerificationList.DetailItems.Count() > 0);
            Assert.Equal("RequestVbName", vbVerificationList.RequestVbName);
            Assert.Equal("Status_ReqReal", vbVerificationList.Status_ReqReal);
            Assert.Equal("UnitLoad", vbVerificationList.UnitLoad);
            Assert.Equal("UnitName", vbVerificationList.UnitName);
            Assert.Equal("VBNo", vbVerificationList.VBNo);
            Assert.Equal("VBNoRealize", vbVerificationList.VBNoRealize);
            Assert.Equal("VBRealizeCategory", vbVerificationList.VBRealizeCategory);
            Assert.Equal("Usage", vbVerificationList.Usage);
        }
    }
}
