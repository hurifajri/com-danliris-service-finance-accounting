using Com.Danliris.Service.Finance.Accounting.Lib;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.VBVerification
{
    public class VbVerificationDetailViewModelTest
    {
        [Fact]
        public void should_success_intantiate()
        {
            VbVerificationDetailViewModel viewModel = new VbVerificationDetailViewModel()
            {
               Amount =1,
               Date =DateTimeOffset.Now,
               DateSPB =DateTimeOffset.Now,
               isGetPPn =true,
               NoSPB = "NoSPB",
               PriceTotalSPB =1,
               Remark = "Remark",
               SupplierName = "SupplierName",
               Total=1
            };

            Assert.Equal(1, viewModel.Amount);
            Assert.True(DateTimeOffset.MinValue < viewModel.Date);
            Assert.True(DateTimeOffset.MinValue < viewModel.DateSPB);
            Assert.True( viewModel.isGetPPn);
            Assert.Equal("NoSPB", viewModel.NoSPB);
            Assert.Equal(1, viewModel.PriceTotalSPB);
            Assert.Equal("Remark", viewModel.Remark);
            Assert.Equal("SupplierName", viewModel.SupplierName);
            Assert.Equal(1, viewModel.Total);

            

        }
    }
}
