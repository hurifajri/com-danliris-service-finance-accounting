using Com.Danliris.Service.Finance.Accounting.Lib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.DownPayment
{
  public  class DownPaymentListTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            DownPaymentList downPaymentList = new DownPaymentList()
            {
                BuyerName= "BuyerName",
                CategoryAcceptance = "CategoryAcceptance",
                CurrencyCode = "CurrencyCode",
                DatePayment =DateTimeOffset.Now,
                DocumentNo = "DocumentNo",
                TotalPayment=1
            };
            Assert.Equal("BuyerName", downPaymentList.BuyerName);
            Assert.Equal("CategoryAcceptance", downPaymentList.CategoryAcceptance);
            Assert.Equal("CurrencyCode", downPaymentList.CurrencyCode);
            Assert.Equal("DocumentNo", downPaymentList.DocumentNo);
            Assert.True(DateTimeOffset.MinValue < downPaymentList.DatePayment);
            Assert.Equal(1, downPaymentList.TotalPayment);
        }
        }
}
