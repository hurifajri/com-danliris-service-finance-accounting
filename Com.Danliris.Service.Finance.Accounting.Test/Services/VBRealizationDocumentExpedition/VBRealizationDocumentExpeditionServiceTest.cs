using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRealizationDocumentExpedition;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.VBRealizationDocumentExpedition
{
    public class VBRealizationDocumentExpeditionServiceTest
    {
        private const string ENTITY = "VBRealizationDocumentExpeditionService";
        [MethodImpl(MethodImplOptions.NoInlining)]
        protected string GetCurrentMethod()
        {

            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
        }

        protected string GetCurrentAsyncMethod([CallerMemberName] string methodName = "")
        {
            var method = new StackTrace()
                .GetFrames()
                .Select(frame => frame.GetMethod())
                .FirstOrDefault(item => item.Name == methodName);

            return method.Name;

        }
        private FinanceDbContext _dbContext(string testName)
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

        private VBRealizationDocumentExpeditionDataUtil _dataUtil(VBRealizationDocumentExpeditionService service, FinanceDbContext financeDbContext)
        {
            return new VBRealizationDocumentExpeditionDataUtil(service, financeDbContext);
        }

        private VBRealizationDocumentDataUtil _dataUtil(VBRealizationWithPOService service)
        {
            return new VBRealizationDocumentDataUtil(service);
        }

        

        [Fact]
        public async Task CashierReceipt_Return_Success()
        {
            FinanceDbContext dbContext = _dbContext(GetCurrentAsyncMethod());
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();

            VBRealizationWithPOService vBRealizationWithPOService = new VBRealizationWithPOService(dbContext, serviceProviderMock.Object);
            var vBRealizationWithPODto = _dataUtil(vBRealizationWithPOService).GetTestData_TanpaNomorVB();


            VBRealizationDocumentExpeditionService service = new VBRealizationDocumentExpeditionService(dbContext, GetServiceProvider().Object);
            VBRealizationDocumentExpeditionModel model = _dataUtil(service, dbContext).GetTestData_VBRealizationDocumentExpedition();

            var data = model.VBRealizationId;
            int result = await service.CashierReceipt(new List<int>() { data });
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task InitializeExpedition_Return_Success()
        {
            FinanceDbContext dbContext = _dbContext(GetCurrentMethod());
            var serviceProvider = GetServiceProvider();

            VBRealizationWithPOService vBRealizationWithPOService = new VBRealizationWithPOService(dbContext, serviceProvider.Object);
            var vBRealizationWithPODto = _dataUtil(vBRealizationWithPOService).GetTestData_TanpaNomorVB();

            VBRealizationDocumentExpeditionService service = new VBRealizationDocumentExpeditionService(dbContext, serviceProvider.Object);
            var realizationVbModel = _dataUtil(service, dbContext).GetTestData_RealizationVbs();


            int result = await service.InitializeExpedition(realizationVbModel.Id);
            Assert.NotEqual(0, result);
        }


        [Fact]
        public async Task GetReports_Return_Success()
        {
            //Setup
            FinanceDbContext dbContext = _dbContext(GetCurrentMethod());
            var serviceProviderMock = GetServiceProvider();

            VBRealizationDocumentExpeditionService service = new VBRealizationDocumentExpeditionService(dbContext, serviceProviderMock.Object);

            VBRealizationWithPOService vBRealizationWithPOService = new VBRealizationWithPOService(dbContext, serviceProviderMock.Object);
            var vBRealizationWithPODto = _dataUtil(vBRealizationWithPOService).GetTestData_TanpaNomorVB();

            VBRealizationDocumentExpeditionModel vbRealization = _dataUtil(service, dbContext).GetTestData_VBRealizationDocumentExpedition();
           
            //Act
             var  result = await service.GetReports(vbRealization.Id, vbRealization.VBRealizationId, vbRealization.VBRequestName, vbRealization.UnitId, vbRealization.DivisionId,DateTimeOffset.Now.AddDays(-2), DateTimeOffset.Now.AddDays(2),"UNIT", 1, 25);

            //Assert
            Assert.NotNull(result);
        
        }


        [Fact]
        public void Read_Return_Success()
        {
            FinanceDbContext dbContext = _dbContext(GetCurrentMethod());

            VBRealizationDocumentExpeditionService service = new VBRealizationDocumentExpeditionService(dbContext, GetServiceProvider().Object);

            var result = service.Read(1, 1, "{}", "", 0, 0, 0, null, null, 0);
            Assert.NotNull(result);
        }

        [Fact]
        public void Read_with_Data_Return_Success()
        {
            //Setup
            FinanceDbContext dbContext = _dbContext(GetCurrentMethod());
            VBRealizationDocumentExpeditionService service = new VBRealizationDocumentExpeditionService(dbContext, GetServiceProvider().Object);
            var data = _dataUtil(service, dbContext).GetTestData_VBRealizationDocumentExpedition();

            //Act
            var result = service.Read(1, 1, "{}", "", data.Position, data.VBId, data.VBRealizationId, data.VBRealizationDate, data.VBRequestName, data.UnitId);

            //Assert
            Assert.NotNull(result);
        }

        //[Fact]
        //public async Task Reject_Return_Success()
        //{
        //    FinanceDbContext dbContext = _dbContext(GetCurrentMethod());
        //    Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();

        //    VBRealizationWithPOService vBRealizationWithPOService = new VBRealizationWithPOService(dbContext, serviceProviderMock.Object);
        //    var vBRealizationWithPODto = _dataUtil(vBRealizationWithPOService).GetTestData_TanpaNomorVB();

        //    VBRealizationDocumentExpeditionService service = new VBRealizationDocumentExpeditionService(dbContext, serviceProviderMock.Object);
        //    VBRealizationDocumentExpeditionModel model = _dataUtil(service, dbContext).GetTestData_VBRealizationDocumentExpedition();
           
        //    int result = await service.Reject(model.VBRealizationId, null);
        //    Assert.NotEqual(0, result);
        //}



        [Fact]
        public async Task SubmitToVerification_Return_Success()
        {
            FinanceDbContext dbContext = _dbContext(GetCurrentAsyncMethod());
            var serviceProvider = GetServiceProvider();

            VBRealizationDocumentExpeditionService service = new VBRealizationDocumentExpeditionService(dbContext, serviceProvider.Object);
            
            VBRealizationWithPOService vBRealizationWithPOService = new VBRealizationWithPOService(dbContext, serviceProvider.Object);
            var data = _dataUtil(vBRealizationWithPOService).GetTestData_TanpaNomorVB();

            int result = await service.SubmitToVerification(new List<int>() { data.Id });
            Assert.NotEqual(0, result);
        }


        [Fact]
        public async Task VerifiedToCashier_with_multipleData_Return_Success()
        {
            FinanceDbContext dbContext = _dbContext(GetCurrentMethod());

            VBRealizationDocumentExpeditionService service = new VBRealizationDocumentExpeditionService(dbContext, GetServiceProvider().Object);
            VBRealizationDocumentExpeditionModel model = _dataUtil(service, dbContext).GetTestData_VBRealizationDocumentExpedition();

            int result = await service.VerifiedToCashier(new List<int>() { model.VBRealizationId });

            Assert.NotEqual(0, result);
        }

        

        [Fact]
        public async Task VerificationDocumentReceipt_Return_Success()
        {
            FinanceDbContext dbContext = _dbContext(GetCurrentMethod());

            VBRealizationDocumentExpeditionService service = new VBRealizationDocumentExpeditionService(dbContext, GetServiceProvider().Object);
            VBRealizationDocumentExpeditionModel model = _dataUtil(service, dbContext).GetTestData_VBRealizationDocumentExpedition();
            var data = model.VBRealizationId;
            int result = await service.VerificationDocumentReceipt(new List<int>() { data });
            Assert.NotEqual(0, result);
        }

        //[Fact]
        //public async Task VerifiedToCashier_Return_Success()
        //{
        //    FinanceDbContext dbContext = _dbContext(GetCurrentMethod());
        //    Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();

        //    VBRealizationWithPOService vBRealizationWithPOService = new VBRealizationWithPOService(dbContext, serviceProviderMock.Object);
        //    VBRealizationWithPODto vBRealizationWithPODto = _dataUtil(vBRealizationWithPOService).GetTestData_TanpaNomorVB();

        //    VBRealizationDocumentExpeditionService service = new VBRealizationDocumentExpeditionService(dbContext, serviceProviderMock.Object);
        //    VBRealizationDocumentExpeditionModel model = _dataUtil(service, dbContext).GetTestData_VBRealizationDocumentExpedition();
            
        //    int result = await service.VerifiedToCashier(model.VBRealizationId);
        //    Assert.NotEqual(0, result);
        //}

        [Fact]
        public void ReadRelizationToVerification_Return_Success()
        {
            FinanceDbContext dbContext = _dbContext(GetCurrentMethod());

            VBRealizationDocumentExpeditionService service = new VBRealizationDocumentExpeditionService(dbContext, GetServiceProvider().Object);

            ReadResponse<VBRealizationDocumentModel> result = service.ReadRealizationToVerification(0, 0, null, null, 0);
            Assert.NotNull(result);
        }


        [Fact]
        public void ReadRelizationToVerification_with_dataTest_Return_Success()
        {
            FinanceDbContext dbContext = _dbContext(GetCurrentMethod());

            VBRealizationDocumentExpeditionService service = new VBRealizationDocumentExpeditionService(dbContext, GetServiceProvider().Object);
            VBRealizationDocumentExpeditionModel model = _dataUtil(service, dbContext).GetTestData_VBRealizationDocumentExpedition();
            var result = service.ReadRealizationToVerification(model.VBId, model.VBRealizationId, model.VBRealizationDate, model.VBRequestName, model.UnitId);
            Assert.NotNull(result);
            //Assert.True(0 < result.Data.Count());

        }

        [Fact]
        public async Task UpdateExpeditionByRealizationId_Return_Success()
        {
            FinanceDbContext dbContext = _dbContext(GetCurrentAsyncMethod());

            VBRealizationDocumentExpeditionService service = new VBRealizationDocumentExpeditionService(dbContext, GetServiceProvider().Object);
            VBRealizationDocumentExpeditionModel model = _dataUtil(service, dbContext).GetTestData_VBRealizationDocumentExpedition();
            int result = await service.UpdateExpeditionByRealizationId(model.VBRealizationId);

            Assert.NotEqual(0, result);

        }

        [Fact]
        public void ReadVerification_Return_Success()
        {
            FinanceDbContext dbContext = _dbContext(GetCurrentAsyncMethod());

            VBRealizationDocumentExpeditionService service = new VBRealizationDocumentExpeditionService(dbContext, GetServiceProvider().Object);
            VBRealizationDocumentExpeditionModel model = _dataUtil(service, dbContext).GetTestData_VBRealizationDocumentExpedition();
            var result =  service.ReadVerification(1,1,"{}","",model.Position,model.VBId,model.VBRealizationId,model.VBRealizationDate,model.VBRequestName,model.UnitId);
           
            Assert.NotNull( result);

        }


    }
}
