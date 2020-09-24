using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.Memo;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.Memo
{
  public  class MemoListTest
    {
        [Fact]
        public void should_success_Instantiate()
        {
            MemoList memoList = new MemoList()
            {
                BuyerName = "BuyerName",
                CurrencyCodes = "CurrencyCodes",
                Date =DateTimeOffset.Now,
                DocumentNo = "DocumentNo",
                MemoType = "MemoType",
                SalesInvoiceNo = "SalesInvoiceNo",
                Id =1,
                LastModifiedUtc=DateTime.Now
            };

            Assert.Equal("BuyerName", memoList.BuyerName);
            Assert.Equal("CurrencyCodes", memoList.CurrencyCodes);
            Assert.Equal("DocumentNo", memoList.DocumentNo);
            Assert.Equal("MemoType", memoList.MemoType);
            Assert.Equal("SalesInvoiceNo", memoList.SalesInvoiceNo);
            Assert.Equal(1, memoList.Id);
            Assert.True(DateTime.MinValue < memoList.LastModifiedUtc);
            Assert.True(DateTimeOffset.MinValue < memoList.Date);
        }
    }
}
