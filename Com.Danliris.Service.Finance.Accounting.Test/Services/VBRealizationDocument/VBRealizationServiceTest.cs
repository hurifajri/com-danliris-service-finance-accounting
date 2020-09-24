using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.VBRealizationDocument
{
    public class VBRealizationServiceTest
    {
        private FinanceDbContext GetDbContext(string testName)
        {
            var serviceProvider = new ServiceCollection()
              .AddEntityFrameworkInMemoryDatabase()
              .BuildServiceProvider();

            DbContextOptionsBuilder<FinanceDbContext> optionsBuilder = new DbContextOptionsBuilder<FinanceDbContext>();
            optionsBuilder
                .UseInMemoryDatabase(testName)
                .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .UseInternalServiceProvider(serviceProvider);

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

        protected string GetCurrentAsyncMethod([CallerMemberName] string methodName = "")
        {
            var method = new StackTrace()
                .GetFrames()
                .Select(frame => frame.GetMethod())
                .FirstOrDefault(item => item.Name == methodName);

            return method.Name;

        }
        private VBRealizationDocumentDataUtil GetDataUtil(VBRealizationWithPOService service)
        {
            return new VBRealizationDocumentDataUtil(service);
        }

        private VBRequestDocumentDataUtil GetDataUtil(VBRequestDocumentService service)
        {
            return new VBRequestDocumentDataUtil(service);
        }

        [Fact]
        public void Read_Return_Success()
        {
            //Setup
            FinanceDbContext _dbContext = GetDbContext(GetCurrentAsyncMethod());
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();

            var vBRealizationService = new VBRealizationService(_dbContext, serviceProviderMock.Object);

            VBRealizationWithPOService vBRealizationWithPOService = new VBRealizationWithPOService(_dbContext, serviceProviderMock.Object);
            var vBRealizationDocumenData = GetDataUtil(vBRealizationWithPOService).GetTestData_TanpaNomorVB();

            //Act
            ReadResponse<VBRealizationDocumentModel> result = vBRealizationService.Read(1, 1, "{}", new List<string>(), "", "{}");

            //Assert
            Assert.NotNull(result);
            Assert.True(0 < result.Count);

        }

        [Fact]
        public async Task ReadByIdAsync_Return_Null()
        {
            //Setup
            FinanceDbContext dbContext = GetDbContext(GetCurrentAsyncMethod());
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();

            var vBRealizationService = new VBRealizationService(dbContext, serviceProviderMock.Object);

            VBRealizationWithPOService vBRealizationWithPOService = new VBRealizationWithPOService(dbContext, serviceProviderMock.Object);
            
            //Act
            var result = await vBRealizationService.ReadByIdAsync(1);

            //Assert
            Assert.NotNull(result);
            Assert.Null(result.Item1);
        }

        [Fact]
        public async Task ReadByIdAsync_NonPO_Return_Success()
        {
            //Setup
            FinanceDbContext dbContext = GetDbContext(GetCurrentAsyncMethod());
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();

            var vBRealizationService = new VBRealizationService(dbContext, serviceProviderMock.Object);
           
            var vBRequestDocumentService = new VBRequestDocumentService(dbContext, serviceProviderMock.Object);
            var vBRequestDocumentData = GetDataUtil(vBRequestDocumentService).GetTestData_VBRequestDocumentNonPO();

            VBRealizationWithPOService vBRealizationWithPOService = new VBRealizationWithPOService(dbContext, serviceProviderMock.Object);
            var vBRealizationDocumenData = GetDataUtil(vBRealizationWithPOService).GetTestData_TanpaNomorVB();

            //Act
            var result = await vBRealizationService.ReadByIdAsync(vBRealizationDocumenData.Id);

            //Assert
            Assert.NotNull(result);

        }


        [Fact]
        public async Task ReadByIdAsync_withPO_Return_Success()
        {
            //Setup
            FinanceDbContext dbContext = GetDbContext(GetCurrentAsyncMethod());
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();

            var vBRequestDocumentService = new VBRequestDocumentService(dbContext, serviceProviderMock.Object);
            var vBRequestDocumentData = GetDataUtil(vBRequestDocumentService).GetTestData_VBRequestDocumentWithPO();

            var vBRealizationService = new VBRealizationService(dbContext, serviceProviderMock.Object);

            VBRealizationWithPOService vBRealizationWithPOService = new VBRealizationWithPOService(dbContext, serviceProviderMock.Object);
            var vBRealizationDocumenData = GetDataUtil(vBRealizationWithPOService).GetTestData_TanpaNomorVB();

            //Act
            var result =await vBRealizationService.ReadByIdAsync(vBRealizationDocumenData.Id);

            //Assert
            Assert.NotNull(result);
          
        }
    }
}