using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CreditorAccount;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.CreditorAccount
{
  public  class CreditorAccountPostedViewModelTest
    {
        [Fact]
        public void validate_default()
        {
            CreditorAccountPostedViewModel dto = new CreditorAccountPostedViewModel();
            var result = dto.Validate(null);
            Assert.True(0 < result.Count());
        }
    }
}
