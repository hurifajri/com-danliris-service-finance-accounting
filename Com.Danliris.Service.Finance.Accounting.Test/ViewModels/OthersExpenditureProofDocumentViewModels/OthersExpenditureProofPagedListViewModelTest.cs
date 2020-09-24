using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.OthersExpenditureProofDocumentViewModels;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.OthersExpenditureProofDocumentViewModels
{
   public class OthersExpenditureProofPagedListViewModelTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            var viewModel = new OthersExpenditureProofListViewModel()
            {
                DocumentNo = "1",
                AccountBankId =1,
                Total =1,
                Date =DateTimeOffset.Now,
                Type = "Type",
                Id=1
            };

            Assert.Equal("1", viewModel.DocumentNo);
            Assert.True(DateTimeOffset.MinValue < viewModel.Date);
            Assert.Equal(1, viewModel.AccountBankId);
            Assert.Equal("Type", viewModel.Type);
            Assert.Equal(1, viewModel.Id);
        }
    }
}
