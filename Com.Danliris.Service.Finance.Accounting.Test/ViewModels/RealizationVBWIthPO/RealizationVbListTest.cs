using Com.Danliris.Service.Finance.Accounting.Lib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.RealizationVBWIthPO
{
  public  class RealizationVbListTest
    {
        

        [Fact]
        public void should_success_intantiate()
        {
            RealizationVbList viewModel = new RealizationVbList()
            {
               Id =1,
               DateEstimate =DateTimeOffset.Now,
               VBNo = "VBNo",
               isVerified =true,
               VBNoRealize = "VBNoRealize",
               Date =DateTimeOffset.Now,
               CreatedBy="adepamungkas",
               VBRealizeCategory = "VBRealizeCategory"
            };

            Assert.Equal(1,viewModel.Id);
            Assert.True(DateTimeOffset.MinValue < viewModel.DateEstimate);
            Assert.True(DateTimeOffset.MinValue < viewModel.Date);
            Assert.Equal("VBNoRealize", viewModel.VBNoRealize);
            Assert.Equal("adepamungkas", viewModel.CreatedBy);
            Assert.Equal("VBNo", viewModel.VBNo);
            Assert.Equal("VBRealizeCategory", viewModel.VBRealizeCategory);
            Assert.True(viewModel.isVerified);
        }
    }
}
