using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentInvoicePayment;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentInvoicePayment;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.GarmentInvoicePaymentViewModel;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentInvoicePayment;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.GarmentInvoicePayment
{
    public class GarmentInvoicePaymentServiceTest
    {
        private const string ENTITY = "GarmentInvoicePayments";

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private FinanceDbContext _dbContext(string testName)
        {
            DbContextOptionsBuilder<FinanceDbContext> optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            FinanceDbContext dbContext = new FinanceDbContext(optionsBuilder.Options);

            return dbContext;
        }


        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });


            return serviceProvider;
        }

        private GarmentInvoicePaymentDataUtil _dataUtil(GarmentInvoicePaymentService service, string testname)
        {
            return new GarmentInvoicePaymentDataUtil(service);
        }

        [Fact]
        public async Task Should_Success_Create_Data()
        {
            GarmentInvoicePaymentService service = new GarmentInvoicePaymentService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            GarmentInvoicePaymentModel model = _dataUtil(service, GetCurrentMethod()).GetNewData();
            var Response = await service.CreateAsync(model);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Get_Data()
        {
            GarmentInvoicePaymentService service = new GarmentInvoicePaymentService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var Response = service.Read(1, 25, "{}", null, null, "{}");
            Assert.NotEmpty(Response.Data);
        }

        [Fact]
        public async Task Should_Success_Get_Data_By_Id()
        {
            GarmentInvoicePaymentService service = new GarmentInvoicePaymentService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            GarmentInvoicePaymentModel model = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var Response = await service.ReadByIdAsync(model.Id);
            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_Delete_Data()
        {
            GarmentInvoicePaymentService service = new GarmentInvoicePaymentService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));


            GarmentInvoicePaymentModel model = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var newModel = await service.ReadByIdAsync(model.Id);

            var Response = await service.DeleteAsync(newModel.Id);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Update_Data()
        {
            GarmentInvoicePaymentService service = new GarmentInvoicePaymentService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));


            GarmentInvoicePaymentModel model = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            var newModel = await service.ReadByIdAsync(model.Id);
            //newModel.BGCheckNumber = "newBG";
            var Response1 = await service.UpdateAsync(newModel.Id, newModel);
            Assert.NotEqual(0, Response1);

            GarmentInvoicePaymentModel model2 = await _dataUtil(service, GetCurrentMethod()).GetTestData();
            //var newModel2 = await service.ReadByIdAsync(model.Id);
            GarmentInvoicePaymentModel newModel2 = new GarmentInvoicePaymentModel();
            newModel2.Id = model2.Id;

            newModel2.Items = new List<GarmentInvoicePaymentItemModel> { model2.Items.First() };
            var Response = await service.UpdateAsync(model2.Id, newModel2);
            Assert.NotEqual(0, Response);

            GarmentInvoicePaymentItemModel newItem = new GarmentInvoicePaymentItemModel
            {
                InvoiceId=3,
                InvoiceNo="no3",
                Amount=3,
                IDRAmount=3
            };

            newModel2.Items.Add(newItem);
            var Response3 = await service.UpdateAsync(model2.Id, newModel2);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public void Should_Success_Validate_All_Null_Data()
        {
            GarmentInvoicePaymentViewModel vm = new GarmentInvoicePaymentViewModel();

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_Null_Invoice_Data()
        {
            GarmentInvoicePaymentViewModel vm = new GarmentInvoicePaymentViewModel();
            vm.Items = new List<GarmentInvoicePaymentItemViewModel>
            {
                new GarmentInvoicePaymentItemViewModel()
                {
                    InvoiceId=0,
                    InvoiceNo=" "
                }
            };

            Assert.True(vm.Validate(null).Count() > 0);
        }

    }
}
