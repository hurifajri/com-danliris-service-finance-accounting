using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.LockTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.LockTransaction;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.LockTransaction;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.LockTransaction
{
    public class LockTransactionServiceTest
    {
        private const string ENTITY = "LockTransaction";
        //private PurchasingDocumentAcceptanceDataUtil pdaDataUtil;
        //private readonly IIdentityService identityService;

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

        private LockTransactionDataUtil _dataUtil(LockTransactionService service)
        {

            GetServiceProvider();
            return new LockTransactionDataUtil(service);
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test" });


            return serviceProvider;
        }

        [Fact]
        public async Task Should_Success_Get_Data()
        {
            var service = new LockTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            await _dataUtil(service).GetTestData();
            var Response = service.Read(1, 25, "{}", null, null, "{}");
            Assert.NotEmpty(Response.Data);
        }

        [Fact]
        public async Task Should_Success_Get_Data_By_Id()
        {
            var service = new LockTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await _dataUtil(service).GetTestData();
            var Response = await service.ReadByIdAsync(model.Id);
            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_Get_Data_By_Type()
        {
            var service = new LockTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await _dataUtil(service).GetTestData();
            var Response = await service.GetLastActiveLockTransaction(model.Type);
            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_Create_Data()
        {
            var service = new LockTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = _dataUtil(service).GetNewData();
            var Response = await service.CreateAsync(model);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public void Should_Success_Validate_All_Null_Data()
        {
            var vm = new LockTransactionViewModel();

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_NotValidDate()
        {
            var vm = new LockTransactionViewModel
            {
                //BeginLockDate = DateTimeOffset.Now.AddDays(1),
                LockDate = DateTimeOffset.Now
            };

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_NotValidDate_GreaterThanNow()
        {
            var vm = new LockTransactionViewModel
            {
                LockDate = DateTimeOffset.Now.AddDays(1)
            };

            Assert.True(vm.Validate(null).Count() > 0);
        }

        [Fact]
        public void Should_Success_Validate_LockDateNull()
        {
            var vm = new LockTransactionViewModel
            {
                LockDate = null
            };

            Assert.True(vm.Validate(null).Count() > 0);
        }


        [Fact]
        public async Task Should_Success_Update_Data()
        {
            var service = new LockTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await _dataUtil(service).GetTestData();
            var newModel = await service.ReadByIdAsync(model.Id);
            newModel.Description += "[Updated]";
            var Response = await service.UpdateAsync(newModel.Id, newModel);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async Task Should_Success_Delete_Data()
        {
            var service = new LockTransactionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var model = await _dataUtil(service).GetTestData();
            var newModel = await service.ReadByIdAsync(model.Id);

            var Response = await service.DeleteAsync(newModel.Id);
            //var deletedModel = await service.ReadByIdAsync(newModel.Id);
            Assert.NotEqual(0, Response);
        }
    }
}
