using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Com.Danliris.Service.Finance.Accounting.Test.Services.OthersExpenditureProofDocument.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.VBRequestDocument
{
  public  class VBRequestDocumentServiceTest
    {
        private const string ENTITY = "VBRequestDocuments";
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
                .Setup(x => x.GetService(typeof(IAutoJournalService)))
                .Returns(new AutoJournalServiceTestHelper());

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });

            return serviceProvider;
        }

        private VBRequestDocumentDataUtil GetdataUtil(VBRequestDocumentService service)
        {
            return new VBRequestDocumentDataUtil(service);
        }

        [Fact]
        public async Task CreateNonPO_Return_Success()
        {
            //Setup
            FinanceDbContext dbContext = _dbContext(GetCurrentAsyncMethod());

            VBRequestDocumentService service = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            VBRequestDocumentNonPOFormDto form = GetdataUtil(service).GetNewData_VBRequestDocumentNonPOFormDto();
            VBRequestDocumentNonPODto data = await GetdataUtil(service).GetTestData_VBRequestDocumentNonPO();

            //Act
            int result = await service.CreateNonPO(form);

            //Assert
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task CreateNonPO_Throw_Exception()
        {
            //Setup
            FinanceDbContext dbContext = _dbContext(GetCurrentAsyncMethod());

            VBRequestDocumentService service = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            VBRequestDocumentNonPOFormDto form = GetdataUtil(service).GetNewData_VBRequestDocumentNonPOFormDto();
            form.Items = null;

            //Assert
            await Assert.ThrowsAsync<System.ArgumentNullException>(()=>service.CreateNonPO(form));
        }


        [Fact]
        public void CreateWithPO_Return_Success()
        {
            //Setup
            FinanceDbContext dbContext = _dbContext(GetCurrentAsyncMethod());

            VBRequestDocumentService service = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            VBRequestDocumentWithPOFormDto form = GetdataUtil(service).GetNewData_VBRequestDocumentWithPOFormDto();
            VBRequestDocumentWithPODto data = GetdataUtil(service).GetTestData_VBRequestDocumentWithPO();

            //Act
            int result =  service.CreateWithPO(form);

            //Assert
            Assert.NotEqual(0, result);
        }

        [Fact]
        public async Task DeleteNonPO_Return_Success()
        {
            //Setup
            FinanceDbContext dbContext = _dbContext(GetCurrentAsyncMethod());

            VBRequestDocumentService service = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            VBRequestDocumentNonPODto data = await GetdataUtil(service).GetTestData_VBRequestDocumentNonPO();
            
            //Act
            int result = await service.DeleteNonPO(data.Id);

            //Assert
            Assert.NotEqual(0, result);
        }


        [Fact]
        public void DeleteWithPO_Return_Success()
        {
            //Setup
            FinanceDbContext dbContext = _dbContext(GetCurrentAsyncMethod());

            VBRequestDocumentService service = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            VBRequestDocumentWithPODto data = GetdataUtil(service).GetTestData_VBRequestDocumentWithPO();

            //Act
            int result =  service.DeleteWithPO(data.Id);

            //Assert
            Assert.NotEqual(0, result);
        }


        [Fact]
        public void Get_Return_Success()
        {
            //Setup
            FinanceDbContext dbContext = _dbContext(GetCurrentAsyncMethod());

            VBRequestDocumentService service = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            VBRequestDocumentWithPODto data = GetdataUtil(service).GetTestData_VBRequestDocumentWithPO();

            var orderData = new
            {
                DocumentNo = "desc"
            };
            string order =JsonConvert.SerializeObject(orderData);

            //Act
            var result = service.Get(1,1, order,new List<string>(),"","{}");

            //Assert
            Assert.NotNull( result);
            Assert.True(0 < result.Data.Count());
        }

        [Fact]
        public async Task GetNonPOById_Return_Success()
        {
            //Setup
            FinanceDbContext dbContext = _dbContext(GetCurrentAsyncMethod());

            VBRequestDocumentService service = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            VBRequestDocumentNonPODto data =await GetdataUtil(service).GetTestData_VBRequestDocumentNonPO();

            //Act
            VBRequestDocumentNonPODto result = await service.GetNonPOById(data.Id);

            //Assert
            Assert.NotEqual(0, result.Id);
            Assert.NotNull(result);
        }

        [Fact]
        public async Task GetNonPOById_Return_Null()
        {
            //Setup
            FinanceDbContext dbContext = _dbContext(GetCurrentAsyncMethod());

            VBRequestDocumentService service = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            VBRequestDocumentNonPODto data = await GetdataUtil(service).GetTestData_VBRequestDocumentNonPO();

            //Act
            var result =await service.GetNonPOById(data.Id+1);

            //Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetWithPOById_Return_Success()
        {
            //Setup
            FinanceDbContext dbContext = _dbContext(GetCurrentAsyncMethod());

            VBRequestDocumentService service = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            var data = GetdataUtil(service).GetTestData_VBRequestDocumentWithPO();

            //Act
            var result =  service.GetWithPOById(data.Id );

            //Assert
            Assert.NotNull(result);
            Assert.NotEqual(0,result.Id);
        }

        [Fact]
        public async Task UpdateNonPO_Return_Success()
        {
            //Setup
            FinanceDbContext dbContext = _dbContext(GetCurrentAsyncMethod());

            VBRequestDocumentService service = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            VBRequestDocumentNonPOFormDto form = GetdataUtil(service).GetNewData_VBRequestDocumentNonPOFormDto();
            VBRequestDocumentNonPODto data = await GetdataUtil(service).GetTestData_VBRequestDocumentNonPO();

            //Act
            int result = await service.UpdateNonPO(data.Id, form);

            //Assert
            Assert.NotEqual(0,result);
        }

        [Fact]
        public void UpdateWithPO_Return_Succes()
        {
            //Setup
            FinanceDbContext dbContext = _dbContext(GetCurrentMethod());
            
            VBRequestDocumentService service = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            
            VBRequestDocumentWithPODto data = GetdataUtil(service).GetTestData_VBRequestDocumentWithPO();
            VBRequestDocumentWithPOFormDto form = GetdataUtil(service).GetNewData_VBRequestDocumentWithPOFormDto();
            
            //Act
            int result =  service.UpdateWithPO(data.Id, form);

            //Assert
            Assert.NotEqual(0, result);
        }

        [Fact]
        public void GetNotApprovedData_Return_Succes()
        {
            //Setup
            FinanceDbContext dbContext = _dbContext(GetCurrentMethod());

            VBRequestDocumentService service = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            VBRequestDocumentWithPODto data = GetdataUtil(service).GetTestData_VBRequestDocumentWithPO();

            //Act
            var result = service.GetNotApprovedData((int)VBType.WithPO, data.Id,(int)data.SuppliantUnit.Id,DateTime.Now,"{}");

            //Assert
            Assert.NotNull(result);
            //Assert.True(0 <result.Count());
        }

        [Fact]
        public async Task ApproveData_Return_Succes()
        {
            //Setup
            FinanceDbContext dbContext = _dbContext(GetCurrentAsyncMethod());

            VBRequestDocumentService service = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            VBRequestDocumentWithPODto data = GetdataUtil(service).GetTestData_VBRequestDocumentWithPO();

            ApprovalVBFormDto approvalVBFormDto = new ApprovalVBFormDto()
            {
                IsApproved=true,
                Ids = new List<int>() { data.Id }
            };
            //Act
            int result = await service.ApprovalData(approvalVBFormDto);

            //Assert
            Assert.True(0 < result);
        }

        [Fact]
        public async Task CancelDocument_Return_Succes()
        {
            //Setup
            var dbContext = _dbContext(GetCurrentAsyncMethod());

            var service = new VBRequestDocumentService(dbContext, GetServiceProvider().Object);
            var data = GetdataUtil(service).GetTestData_VBRequestDocumentWithPO();

            var form = new CancellationFormDto()
            {
                Ids = new List<int>() { data.Id }
            };
            //Act
            int result = await service.CancellationDocuments(form);

            //Assert
            Assert.True(0 < result);
        }
    }
}
