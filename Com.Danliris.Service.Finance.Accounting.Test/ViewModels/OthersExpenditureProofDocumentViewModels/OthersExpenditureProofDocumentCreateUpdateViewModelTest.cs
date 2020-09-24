using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.OthersExpenditureProofDocumentViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.OthersExpenditureProofDocumentViewModels
{
    public class OthersExpenditureProofDocumentCreateUpdateViewModelTest
    {
        [Fact]
        public void validate_default()
        {
            var dto = new OthersExpenditureProofDocumentCreateUpdateViewModel();
            var result = dto.Validate(null);
            Assert.True(0 < result.Count());
        }

        [Fact]
        public void validate_Items()
        {
            var dto = new OthersExpenditureProofDocumentCreateUpdateViewModel()
            {
                Items = new List<OthersExpenditureProofDocumentCreateUpdateItemViewModel>()
                {
                      new OthersExpenditureProofDocumentCreateUpdateItemViewModel()
                }
            };
            var result = dto.Validate(null);
            Assert.True(0 < result.Count());
        }
    }
}
