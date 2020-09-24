using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.JournalTransaction;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.JournalTransaction
{
   public class JournalTransactionReportHeaderViewModelTest
    {
        [Fact]
        public void should_success_intantiate()
        {
            JournalTransactionReportHeaderViewModel viewModel = new JournalTransactionReportHeaderViewModel()
            {
                ReferenceNo = "ReferenceNo",
                Description = "Description",
                Items =new List<JournalTransactionReportViewModel>()
                {
                    new JournalTransactionReportViewModel()
                }
            };

            Assert.NotNull(viewModel.Items);
            Assert.Equal("ReferenceNo", viewModel.ReferenceNo);
            Assert.Equal("Description", viewModel.Description);
        }
    }
}
