using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.NewIntegrationViewModel
{
    public class SupplierViewModelTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            long id = 1;
            string code = "code test";
            string nama = "name test";
            bool import = true;
            string PIC = "PIC test";
            string contact = "conatct test";

            SupplierViewModel viewModel = new SupplierViewModel()
            {
                Id = id,
                Code = code,
                Name = nama,
                Import = import,
                PIC = PIC,
                Contact = contact,
            };

            Assert.Equal(id, viewModel.Id);
            Assert.Equal(nama, viewModel.Name);
            Assert.Equal(code, viewModel.Code);
            Assert.Equal(import, viewModel.Import);
            Assert.Equal(PIC, viewModel.PIC);
            Assert.Equal(contact, viewModel.Contact);
        }
    }


    public class NewSupplierViewModelTest
    {
        [Fact]
        public void Should_Succes_Instantiate_NewSupplierViewModel()
        {
            int id = 1;
            string code = "code test";
            string nama = "name test";
            bool import = true;


            NewSupplierViewModel nspm = new NewSupplierViewModel();
            nspm._id = id;
            nspm.code = code;
            nspm.name = nama;
            nspm.import = import;

            Assert.Equal(id, nspm._id);
            Assert.Equal(nama, nspm.name);
            Assert.Equal(code, nspm.code);
            Assert.Equal(import, nspm.import);

        }
    }
}
