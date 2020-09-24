using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.VBNonPORequest;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VbNonPORequest;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VbNonPORequest;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.VbNonPORequest
{
    public class VbNonPORequestServiceTest
    {
        private const string ENTITY = "VbNonPORequest";

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
            var viewModel = new VbNonPORequestViewModel();

            Assert.True(viewModel.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_All_Null_ObjectProperty()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new VbNonPORequestService(dbContext, serviceProviderMock.Object);
            var dataUtil = new VbNonPORequestDataUtil(service);
            var viewModel = dataUtil.GetViewModelToValidate();

            Assert.True(viewModel.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_All_Null_ObjectProperty2()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new VbNonPORequestService(dbContext, serviceProviderMock.Object);
            var dataUtil = new VbNonPORequestDataUtil(service);
            var viewModel = dataUtil.GetViewModelToValidateOthers();

            Assert.True(viewModel.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_All_Null_ObjectProperty3()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new VbNonPORequestService(dbContext, serviceProviderMock.Object);
            var dataUtil = new VbNonPORequestDataUtil(service);
            var viewModel = dataUtil.GetViewModelToValidateOthers3();

            Assert.True(viewModel.Validate(null).Count() > 0);
        }

        [Fact]
        public async Task Should_Success_Create_Model()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new VbNonPORequestService(dbContext, serviceProviderMock.Object);
            var dataUtil = new VbNonPORequestDataUtil(service);
            var modelToCreate = dataUtil.GetVbRequestModelToCreate();
            var viewmodelToCreate = dataUtil.GetViewModel();
            var result = await service.CreateAsync(modelToCreate, viewmodelToCreate);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_Create_Maping()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new VbNonPORequestService(dbContext, serviceProviderMock.Object);
            var dataUtil = new VbNonPORequestDataUtil(service);
            var modelToCreate = dataUtil.GetVbRequestModelToCreate();
            var viewmodelToCreate = dataUtil.GetViewModel();
            var viewmodelToCreate2 = dataUtil.GetViewModel2(); 
            await service.CreateAsync(modelToCreate, viewmodelToCreate);

            var result = await service.MappingData(viewmodelToCreate2);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_Create_Same_()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new VbNonPORequestService(dbContext, serviceProviderMock.Object);
            var dataUtil = new VbNonPORequestDataUtil(service);
            await dataUtil.GetCreatedData();
            var modelToCreate = dataUtil.GetVbRequestModelToCreate();
            var viewmodelToCreate = dataUtil.GetViewModel();
            var result = await service.CreateAsync(modelToCreate, viewmodelToCreate);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_UpdateAsync()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new VbNonPORequestService(dbContext, serviceProviderMock.Object);
            var dataUtil = new VbNonPORequestDataUtil(service);
            var modelToUpdate = await dataUtil.GetCreatedData();
            var viewmodelToCreate = dataUtil.GetViewModel();

            var result = await service.UpdateAsync(modelToUpdate.Id, viewmodelToCreate);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_UpdateStatusAsync()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new VbNonPORequestService(dbContext, serviceProviderMock.Object);
            var dataUtil = new VbNonPORequestDataUtil(service);

            List<VbRequestModel> modelList = new List<VbRequestModel>();
            VbRequestModel model = await dataUtil.GetTestData();
            modelList.Add(model);

            var result = await service.UpdateStatusAsync(modelList, "user");

            Assert.NotEqual(1, result);
        }

        [Fact]
        public async Task Should_ThrowsException_UpdateStatusAsync()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new VbNonPORequestService(dbContext, serviceProviderMock.Object);
            var dataUtil = new VbNonPORequestDataUtil(service);


            await Assert.ThrowsAsync<Exception>(()=>service.UpdateStatusAsync(null, "user"));
           
        }

        [Fact]
        public async Task Should_Success_Update_Model_Remove_Items()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new VbNonPORequestService(dbContext, serviceProviderMock.Object);
            var dataUtil = new VbNonPORequestDataUtil(service);
            var modelToUpdate = await dataUtil.GetCreatedData();
            var viewmodelToCreate = dataUtil.GetViewModel();

            var result = await service.UpdateAsync(modelToUpdate.Id, viewmodelToCreate);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_Read_Data()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new VbNonPORequestService(dbContext, serviceProviderMock.Object);
            var dataUtil = new VbNonPORequestDataUtil(service);
            await dataUtil.GetCreatedData();

            var result = service.Read(1, 10, "{}", new List<string>(), "", "{}");

            Assert.NotEmpty(result.Data);
        }

        [Fact]
        public async Task Should_Success_Read_ById()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new VbNonPORequestService(dbContext, serviceProviderMock.Object);
            var dataUtil = new VbNonPORequestDataUtil(service);
            var data = await dataUtil.GetCreatedData();

            var result = await service.ReadByIdAsync(data.Id);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Should_Success_Read_ById_2()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new VbNonPORequestService(dbContext, serviceProviderMock.Object);
            var dataUtil = new VbNonPORequestDataUtil(service);
            var data = await dataUtil.GetCreatedData();

            var result = await service.ReadByIdAsync2(data.Id);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Should_Success_MapToViewModel()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new VbNonPORequestService(dbContext, serviceProviderMock.Object);
            var dataUtil = new VbNonPORequestDataUtil(service);
            var data = await dataUtil.GetCreatedData();

            var result = await service.ReadByIdAsync(data.Id);

            Assert.NotNull(result);
        }

        [Fact]
        public async Task Should_Success_Delete_ById()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new VbNonPORequestService(dbContext, serviceProviderMock.Object);
            var dataUtil = new VbNonPORequestDataUtil(service);
            var data = await dataUtil.GetCreatedData();

            var result = await service.DeleteAsync(data.Id);

            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task Should_Success_ReadWithDateFilter_Data()
        {
            var dbContext = GetDbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProviderMock();
            var service = new VbNonPORequestService(dbContext, serviceProviderMock.Object);
            var dataUtil = new VbNonPORequestDataUtil(service);
            await dataUtil.GetCreatedData();

            var result = service.ReadWithDateFilter(DateTimeOffset.Now, 7, 1, 10, "{}", new List<string>(), "", "{}");

            Assert.NotNull(result.Data);
        }
    }
}
