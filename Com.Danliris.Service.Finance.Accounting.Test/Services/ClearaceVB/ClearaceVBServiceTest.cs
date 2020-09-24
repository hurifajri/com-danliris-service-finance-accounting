using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.ClearaceVB;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.ClearaceVB;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.ClearaceVB;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.ClearaceVB
{
    public class ClearaceVBServiceTest
    {
        private const string ENTITY = "ClearanceVB";

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

        private ClearaceVBDataUtil _dataUtil(ClearaceVBService service)
        {
            return new ClearaceVBDataUtil(service);
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

        //[Fact]
        //public async Task Should_Success_Read_Data()
        //{
        //    var dbContext = _dbContext(GetCurrentMethod());
        //    var serviceProvider = GetServiceProvider().Object;
        //    var service = new ClearaceVBService(serviceProvider, dbContext);
        //    var dataUtil = new ClearaceVBDataUtil(service);
        //    var data = await dataUtil.GetTestData();
        //    var dataRealisation = new RealizationVbModel()
        //    {
        //        VBNo = "VBNo",
        //        VBNoRealize = "VBNoRealize",
        //        Date = DateTimeOffset.Now,
        //        DifferenceReqReal = 100,
        //        LastModifiedUtc = DateTime.Now,
        //        Position = 5,
        //        isVerified = true,
        //    };
        //    service._DbContext.RealizationVbs.Add(dataRealisation);
        //    service._DbContext.SaveChanges();

        //    var result = service.Read(1, 10, "{LastModifiedUtc: 'desc'}", new List<string>(), "VB", "{'Status':'Completed'}");

        //    Assert.NotEmpty(result.Data);
        //}

        //[Fact]
        //public async Task Should_Success_Read_Data_2()
        //{
        //    var dbContext = _dbContext(GetCurrentMethod());
        //    var serviceProvider = GetServiceProvider().Object;
        //    var service = new ClearaceVBService(serviceProvider, dbContext);
        //    var dataUtil = new ClearaceVBDataUtil(service);
        //    var data = await dataUtil.GetTestData();
        //    var dataRealisation = new RealizationVbModel()
        //    {
        //        VBNo = "VBNo",
        //        VBNoRealize = "VBNoRealize",
        //        Date = DateTimeOffset.Now,
        //        DifferenceReqReal = 100,
        //        LastModifiedUtc = DateTime.Now,
        //        Position = 6,
        //        isVerified = true,
        //    };
        //    service._DbContext.RealizationVbs.Add(dataRealisation);
        //    service._DbContext.SaveChanges();

        //    var result = service.Read(1, 10, "{}", new List<string>(), "", "{}");

        //    Assert.NotEmpty(result.Data);
        //}

        [Fact]
        public async void PreSalesPost_Success()
        {
            var dbContext = _dbContext(GetCurrentMethod());
            var serviceProvider = GetServiceProvider().Object;
            ClearaceVBService service = new ClearaceVBService(serviceProvider, dbContext);

            var data = await _dataUtil(service).GetTestData();
            var listData = new List<ClearencePostId> { new ClearencePostId() { VBRequestId = data.Id } };
            var Response = await service.ClearanceVBPost(listData);
            Assert.NotEqual(0, Response);
        }

        [Fact]
        public async void PreSalesUnPost_Success()
        {
            var dbContext = _dbContext(GetCurrentMethod());
            var serviceProvider = GetServiceProvider().Object;
            ClearaceVBService service = new ClearaceVBService(serviceProvider, dbContext);

            var data = await _dataUtil(service).GetTestData();
            var Response = await service.ClearanceVBUnpost(data.Id);
            Assert.NotEqual(0, Response);
        }
    }
}
