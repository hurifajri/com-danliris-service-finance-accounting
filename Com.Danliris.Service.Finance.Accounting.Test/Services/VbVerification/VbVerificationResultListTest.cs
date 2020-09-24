using Com.Danliris.Service.Finance.Accounting.Lib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.VbVerification
{
    public class VbVerificationResultListTest
    {
        [Fact]
        public void Should_Succes_Instantiate()
        {

            VbVerificationResultList vbVerificationResultList = new VbVerificationResultList()
            {
                Amount = 1,
                Currency = "rupiah",
                DateRealize = DateTimeOffset.Now,
                DateVerified = DateTimeOffset.Now,
                isNotVeridied = true,
                isVerified = true,
                IsVerified = true,
                RealizeNo = "RealizeNo",
                Reason_NotVerified = "Reason_NotVerified",
                RequestName = "RequestName",
                UnitRequest = "UnitRequest",
                SendTo = "SendTo",
                Usage = "Usage",
                VBCategory = "VBCategory",
                VbNo = "VbNo"
            };

            Assert.Equal(1, vbVerificationResultList.Amount);
            Assert.Equal("rupiah", vbVerificationResultList.Currency);
            Assert.True(DateTimeOffset.MinValue < vbVerificationResultList.DateRealize);
            Assert.True(DateTimeOffset.MinValue < vbVerificationResultList.DateVerified);
            Assert.True(vbVerificationResultList.isNotVeridied);
            Assert.True(vbVerificationResultList.isVerified);
            Assert.True(vbVerificationResultList.IsVerified);
            Assert.Equal("RealizeNo", vbVerificationResultList.RealizeNo);
            Assert.Equal("Reason_NotVerified", vbVerificationResultList.Reason_NotVerified);
            Assert.Equal("RequestName", vbVerificationResultList.RequestName);
            Assert.Equal("UnitRequest", vbVerificationResultList.UnitRequest);
            Assert.Equal("SendTo", vbVerificationResultList.SendTo);
            Assert.Equal("Usage", vbVerificationResultList.Usage);
            Assert.Equal("VBCategory", vbVerificationResultList.VBCategory);
            Assert.Equal("VbNo", vbVerificationResultList.VbNo);
        }
    }
}
