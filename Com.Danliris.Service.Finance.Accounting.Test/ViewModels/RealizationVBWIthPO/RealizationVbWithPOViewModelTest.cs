using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBWIthPO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.RealizationVBWIthPO
{
  public  class RealizationVbWithPOViewModelTest
    {

        [Fact]
        public void Validate_DenganNomorVB()
        {
            RealizationVbWithPOViewModel viewModel = new RealizationVbWithPOViewModel()
            {
                TypeVBNonPO = "Dengan Nomor VB",
                numberVB=null
            };

            Assert.True(viewModel.Validate(null).Count() > 0);
        }

        [Fact]
        public void Validate_TanpaNomorVB()
        {
            RealizationVbWithPOViewModel viewModel = new RealizationVbWithPOViewModel()
            {
                TypeVBNonPO = "Tanpa Nomor VB",
                numberVB = null,
                DateEstimate =null,
            };

            Assert.True(viewModel.Validate(null).Count() > 0);
        }
    }
}
