using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.JournalTransaction;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.JournalTransaction
{
   public class JournalTransactionItemViewModelTest
    {
        [Fact]
        public void should_success_intantiate()
        {
            JournalTransactionItemViewModel viewModel = new JournalTransactionItemViewModel()
            {
                Remark = "Remark"
            };

            Assert.Equal("Remark", viewModel.Remark);
          
        }

        [Fact]
        public void Validate_Throws_NotImplementedException()
        {
            JournalTransactionItemViewModel viewModel = new JournalTransactionItemViewModel()
            {
                Remark = "Remark"
            };

            Assert.Throws<NotImplementedException>(()=> viewModel.Validate(null));

        }
    }
}
