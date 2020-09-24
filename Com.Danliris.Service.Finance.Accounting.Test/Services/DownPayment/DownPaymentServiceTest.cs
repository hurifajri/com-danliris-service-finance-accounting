using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Text;
using System.Diagnostics;
using Com.Danliris.Service.Finance.Accounting.Lib;
using Microsoft.EntityFrameworkCore;
using Moq;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Xunit;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DownPayment;
using System.Linq;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DownPayment;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.DownPayment;
using System.Threading.Tasks;
using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.DownPayment;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.DownPayment
{
    public class DownPaymentServiceTest
    {
        private const string ENTITY = "DownPayment";

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            var st = new StackTrace();
            var sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        private FinanceDbContext GetDbContext(string testName)
        {
            DbContextOptionsBuilder<FinanceDbContext> optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));

            FinanceDbContext dbContext = new FinanceDbContext(optionsBuilder.Options);

            return dbContext;
        }

        private Mock<IServiceProvider> GetServiceProviderMock()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });


            return serviceProvider;
        }

        [Fact]
        public void Should_Success_Validate_All_Null_Data()
        {
            var viewModel = new DownPaymentViewModel();

            Assert.True(viewModel.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_All_Null_ObjectProperty()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new DownPaymentService(dbContext, serviceProviderMock.Object);
            var dataUtil = new DownPaymentDataUtil(service);
            var viewModel = dataUtil.GetViewModelToValidate();

            Assert.True(viewModel.Validate(null).Count() > 0);
        }

        [Fact]
        public async Task Should_Success_Create_Model()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new DownPaymentService(dbContext, serviceProviderMock.Object);
            var dataUtil = new DownPaymentDataUtil(service);
            var modelToCreate = dataUtil.GetNewData();
            var result = await service.CreateAsync(modelToCreate);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_Create_Same_Buyer()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new DownPaymentService(dbContext, serviceProviderMock.Object);
            var dataUtil = new DownPaymentDataUtil(service);
            await dataUtil.GetTestDataById();
            var modelToCreate = dataUtil.GetNewData();
            var result = await service.CreateAsync(modelToCreate);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_Update_Model()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new DownPaymentService(dbContext, serviceProviderMock.Object);
            var dataUtil = new DownPaymentDataUtil(service);
            var modelToUpdate = await dataUtil.GetTestDataById();

            var result = await service.UpdateAsync(modelToUpdate.Id, modelToUpdate);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_Update_Model_Remove_Items()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new DownPaymentService(dbContext, serviceProviderMock.Object);
            var dataUtil = new DownPaymentDataUtil(service);
            var modelToUpdate = await dataUtil.GetTestDataById();

            var result = await service.UpdateAsync(modelToUpdate.Id, modelToUpdate);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_Read_Data()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new DownPaymentService(dbContext, serviceProviderMock.Object);
            var dataUtil = new DownPaymentDataUtil(service);
            await dataUtil.GetTestDataById();

            var result = service.Read(1, 10, "{}", new List<string>(), "", "{}");

            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task Should_Success_Read_ById()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new DownPaymentService(dbContext, serviceProviderMock.Object);
            var dataUtil = new DownPaymentDataUtil(service);
            var data = await dataUtil.GetTestDataById();

            var result = await service.ReadByIdAsync(data.Id);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Should_Success_MapToViewModel()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new DownPaymentService(dbContext, serviceProviderMock.Object);
            var dataUtil = new DownPaymentDataUtil(service);
            var data = await dataUtil.GetTestDataById();

            var result = await service.ReadByIdAsync(data.Id);

            //Mapper.Initialize(mapper => mapper.AddProfile<DownPaymentProfile>());
            //Mapper.AssertConfigurationIsValid();
            //var viewModel = Mapper.Map<DownPaymentViewModel>(result);
            //Assert.NotNull(viewModel);
            //var model = Mapper.Map<DownPaymentModel>(viewModel);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task Should_Success_Delete_ById()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new DownPaymentService(dbContext, serviceProviderMock.Object);
            var dataUtil = new DownPaymentDataUtil(service);
            var data = await dataUtil.GetTestDataById();

            var result = await service.DeleteAsync(data.Id);

            Assert.NotEqual(0, result);
        }
    }
}
