using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VBExpeditionRealizationReport;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBExpeditionRealizationReport;
using Com.Danliris.Service.Finance.Accounting.Test.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.VBExpeditionRealizationReport
{
    public class VBExpeditionRealizationReportServiceTest
    {
        private const string ENTITY = "VBExpeditionRealizationReport";

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

        private VBExpeditionRealizationReportDataUtil _dataUtil(VBExpeditionRealizationReportService service)
        {
            return new VBExpeditionRealizationReportDataUtil(service);
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
        public async Task Should_Success_GetReport()
        {
            VBExpeditionRealizationReportService service = new VBExpeditionRealizationReportService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestData_ById();

            var Response = service.GetReport(data.Id, data.Id, "CreatedBy", data.UnitId, data.UnitDivisionId, "All", data.Date, data.Date, 7);
            Assert.NotNull(Response);

            Response = service.GetReport(data.Id, data.Id, "", data.UnitId, data.UnitDivisionId, "Cashier", null, data.Date, 7);
            Assert.NotNull(Response);

            Response = service.GetReport(data.Id, data.Id, null, data.UnitId, data.UnitDivisionId, "Return", data.Date, null, 7);
            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_GenerateExcel()
        {
            VBExpeditionRealizationReportService service = new VBExpeditionRealizationReportService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestData_ById();

            var dataRealization = new RealizationVbModel()
            {
                VBNo = "VBNo",
                VBNoRealize = "VBNoRealize",
                CurrencyCode = "IDR",
                Amount = 100,
                Date = DateTimeOffset.Now,
                VerifiedName = "VerifiedName",
                VerifiedDate = DateTimeOffset.Now,
                isVerified = true,
                isNotVeridied = false,
                Reason_NotVerified = "Reason_NotVerified",
                LastModifiedUtc = DateTime.Now,
                Position = 5
            };
            service._DbContext.RealizationVbs.Add(dataRealization);
            service._DbContext.SaveChanges();

            var Response = service.GenerateExcel(0, 0, null, 0, 0, "All", null, null, 7);
            Assert.NotNull(Response);

            Response = service.GenerateExcel(0, 0, null, 0, 0, "Cashier", data.Date, null, 7);
            Assert.NotNull(Response);

            Response = service.GenerateExcel(0, 0, null, 0, 0, "Return", null, data.Date, 7);
            Assert.NotNull(Response);
        }
    }
}
