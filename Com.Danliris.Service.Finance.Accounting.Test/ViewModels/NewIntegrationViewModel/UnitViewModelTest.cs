using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.NewIntegrationViewModel
{
    public class UnitViewModelTest
    {
        [Fact]
        public void Succes_Instantiate_UnitViewModel()
        {
            string id = "id test";
            string code = "code test";
            string name = "name test";

            DivisionViewModel divisonViewModel = new DivisionViewModel()
            {
                Id = "Id test",
                Code = "Code Test",
                Name = "Name test",
            };

            UnitViewModel unitViewModel = new UnitViewModel()
            {
                Id = id,
                Name = name,
                Code = code,
                Division = divisonViewModel,
            };

            Assert.Equal(id, unitViewModel.Id);
            Assert.Equal(name, unitViewModel.Name);
            Assert.Equal(code, unitViewModel.Code);
            Assert.NotNull(unitViewModel.Division);


        }

        [Fact]
        public void Succes_Instantiate_NewUnitViewModel()
        {
            int id = 1;
            string code = "code test";
            string name = "name test";

            DivisionViewModel divison = new DivisionViewModel()
            {
                Id = "Id test",
                Code = "Code Test",
                Name = "Name test",
            };


            NewUnitViewModel uvm = new NewUnitViewModel()
            {
                Id = 1,
                Name = name,
                Code = code,
                Division = divison,
            };


            Assert.Equal(id, uvm.Id);
            Assert.Equal(name, uvm.Name);
            Assert.Equal(code, uvm.Code);
            Assert.NotNull(uvm.Division);


        }

    }
}
