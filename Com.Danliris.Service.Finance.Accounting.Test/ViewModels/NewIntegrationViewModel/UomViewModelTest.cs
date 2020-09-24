using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.NewIntegrationViewModel
{

    public class UomViewModelTest
    {
        [Fact]
        public void Succes_Instantiate_UomViewModel()
        {
            string id = "id test";
            string Unit = "Unit test";

            UomViewModel uvm = new UomViewModel()
            {
                Id = id,
                Unit = Unit,
            };

            Assert.Equal(id, uvm.Id);
            Assert.Equal(Unit, uvm.Unit);

        }
    }
}
