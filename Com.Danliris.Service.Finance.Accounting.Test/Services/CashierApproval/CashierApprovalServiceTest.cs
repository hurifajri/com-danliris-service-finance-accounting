using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.CashierApproval;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CashierApproval;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.CashierApproval;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xunit;


//CashierAproval
//DeleteCashierAproval

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.Non_POApproval
{
    public class Non_POApprovalServiceTest
    {
        private const string ENTITY = "CashierApprovals";

        [MethodImpl(MethodImplOptions.NoInlining)]
        public string GetCurrentMethod()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);

            return string.Concat(sf.GetMethod().Name, "_", ENTITY);
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

        protected string GetCurrentAsyncMethod([CallerMemberName] string methodName = "")
        {
            var method = new StackTrace()
                .GetFrames()
                .Select(frame => frame.GetMethod())
                .FirstOrDefault(item => item.Name == methodName);

            return method.Name;

        }

        private CashierApprovalDataUtil _dataUtil(CashierApprovalService service)
        {
            return new CashierApprovalDataUtil(service);
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

        [Fact]
        public void Validate_Validation_ViewModel()
        {
            List<CashierApprovalViewModel> viewModels = new List<CashierApprovalViewModel>
            {
                new CashierApprovalViewModel{
                    VBRequestCategory = "PO",
                    CashierApproval = new List<CashierApprovalItemViewModel>{
                    }
                }
            };
            foreach (var viewModel in viewModels)
            {
                var defaultValidationResult = viewModel.Validate(null);
                Assert.True(defaultValidationResult.Count() > 0);
            }
        }

        [Fact]
        public async Task Should_Success_Post_Approval_With_PO()
        {
            CashierApprovalService service = new CashierApprovalService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            VbRequestModel model = await _dataUtil(service).GetTestData();

            CashierApprovalViewModel data = new CashierApprovalViewModel()
            {
                VBRequestCategory = "PO",
                CashierApproval = new List<CashierApprovalItemViewModel>()
                {
                    new CashierApprovalItemViewModel()
                    {
                        VBNo  = model.VBNo,
                        Id = model.Id,
                        Amount = model.Amount,
                    }
                }
            };

            var response = await service.CashierAproval(data);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Success_Post_Approval_Non_PO()
        {
            CashierApprovalService service = new CashierApprovalService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            VbRequestModel model = await _dataUtil(service).GetTestData();

            CashierApprovalViewModel data = new CashierApprovalViewModel()
            {
                VBRequestCategory = "NONPO",
                CashierApproval = new List<CashierApprovalItemViewModel>()
                {
                    new CashierApprovalItemViewModel()
                    {
                        VBNo  = model.VBNo,
                        Id = model.Id
                    }
                }
            };

            var response = await service.CashierAproval(data);
            Assert.NotEqual(0, response);
        }

        [Fact]
        public async Task Should_Fail_Post_Approval_With_PO()
        {

            CashierApprovalService service = new CashierApprovalService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            VbRequestModel model = await _dataUtil(service).GetTestData();

            CashierApprovalViewModel data = new CashierApprovalViewModel()
            {
                VBRequestCategory = "PO",
                CashierApproval = new List<CashierApprovalItemViewModel>()
                {
                    new CashierApprovalItemViewModel()
                    {
                        VBNo  = model.VBNo,
                        Id = model.Id,
                        Amount = model.Amount
                    }
                }
            };

            await Assert.ThrowsAnyAsync<Exception>(() => service.CashierAproval(null));

        }

        [Fact]
        public async Task Should_Fail_Post_Approval_Non_PO()
        {

            CashierApprovalService service = new CashierApprovalService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            VbRequestModel model = await _dataUtil(service).GetTestData();

            CashierApprovalViewModel data = new CashierApprovalViewModel()
            {
                VBRequestCategory = "NONPO",
                CashierApproval = new List<CashierApprovalItemViewModel>()
                {
                    new CashierApprovalItemViewModel()
                    {
                        VBNo  = model.VBNo,
                        Id = model.Id
                    }
                }
            };

            await Assert.ThrowsAnyAsync<Exception>(() => service.CashierAproval(null));

        }

        [Fact]
        public async Task Should_Success_Delete_Approval_With_PO()
        {
            CashierApprovalService service = new CashierApprovalService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            VbRequestModel model = await _dataUtil(service).GetTestData();
            model.VBRequestCategory = "PO";
            CashierApprovalViewModel data = new CashierApprovalViewModel()
            {
                VBRequestCategory = "PO",
                CashierApproval = new List<CashierApprovalItemViewModel>()
                {
                    new CashierApprovalItemViewModel()
                    {
                        VBNo  = model.VBNo,
                        Id = model.Id,
                        Amount = model.Amount
                    }
                }
            };
            var acceptedResponse = await service.CashierAproval(data);
            var newModel = await service.ReadByIdAsync(model.Id);
            var deleteResponse = await service.DeleteCashierAproval(newModel.Id);
            Assert.NotEqual(0, deleteResponse);
        }

        [Fact]
        public async Task Should_Success_Delete_Approval_Non_PO()
        {
            CashierApprovalService service = new CashierApprovalService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            VbRequestModel model = await _dataUtil(service).GetTestData();

            CashierApprovalViewModel data = new CashierApprovalViewModel()
            {
                VBRequestCategory = "NONPO",
                CashierApproval = new List<CashierApprovalItemViewModel>()
                {
                    new CashierApprovalItemViewModel()
                    {
                        VBNo  = model.VBNo,
                        Id = model.Id
                    }
                }
            };
            var acceptedResponse = await service.CashierAproval(data);
            var newModel = await service.ReadByIdAsync(model.Id);
            var deleteResponse = await service.DeleteCashierAproval(newModel.Id);
            Assert.NotEqual(0, deleteResponse);
        }

        [Fact]
        public async Task Should_Fail_Delete_Approval_Non_PO()
        {

            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(new HttpClientTestService());

            Mock<IIdentityService> mockIdentity = new Mock<IIdentityService>();
            mockIdentity
                .Setup(s => s.Username)
                .Throws(new Exception());

            serviceProvider
               .Setup(x => x.GetService(typeof(IIdentityService)))
               .Returns(mockIdentity.Object);

            CashierApprovalService service = new CashierApprovalService(serviceProvider.Object, _dbContext(GetCurrentAsyncMethod()));
            VbRequestModel vbRequest = new VbRequestModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.UtcNow,
                DateEstimate = DateTimeOffset.UtcNow,
                UnitId = 1,
                UnitCode = "UnitCode",
                UnitName = "UnitName",
                CurrencyId = 1,
                CurrencyCode = "CurrencyCode",
                CurrencyRate = 123,
                CurrencySymbol = "CurrencySymbol",
                Amount = 123,
                Usage = "Usage",
                UnitLoad = "UnitLoad",
                Apporve_Status = false,
                Complete_Status = false,
                VBRequestCategory = "NONPO",
                CreatedBy = "CreatedBy"
            };

            service.DbContext.VbRequests.Add(vbRequest);
            service.DbContext.SaveChanges();


            await Assert.ThrowsAnyAsync<Exception>(() => service.DeleteCashierAproval(1));

        }

        [Fact]
        public async Task Should_Fail_Delete_Empty_Id()
        {
            CashierApprovalService service = new CashierApprovalService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var deleteResponse = await service.DeleteCashierAproval(-1);
            Assert.Equal(0, deleteResponse);
        }

    }
}
