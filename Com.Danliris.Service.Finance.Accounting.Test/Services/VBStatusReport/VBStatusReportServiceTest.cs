using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VBStatusReport;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBStatusReport;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBStatusReport;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.VBStatusReport
{
    public class VBStatusReportServiceTest
    {
        private const string ENTITY = "VBStatusReprt";

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

        private VBStatusReportDataUtil _dataUtil(VBStatusReportService service)
        {
            return new VBStatusReportDataUtil(service);
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
            VBStatusReportService service = new VBStatusReportService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestData_ById();

            var Response = service.GetReport(data.UnitId, data.Id, "CreatedBy", "All", data.Date, data.Date, data.Date, data.Date, data.Date, data.Date, 7);
            Assert.NotNull(Response);

            Response = service.GetReport(0, 0, null, "Outstanding", data.Date, null, data.Date, null, data.Date, null, 7);
            Assert.NotNull(Response);

            Response = service.GetReport(0, 0, null, "Clearance", null, data.Date, null, data.Date, null, data.Date, 7);
            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_GenerateExcel()
        {
            VBStatusReportService service = new VBStatusReportService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestData_ById();

            var dataRealisation = new RealizationVbModel()
            {
                DateEstimate = DateTimeOffset.Now,
                VBNoRealize = "VBNoRealize",
                VBNo = "VBNo",
                Date = DateTimeOffset.Now,
                Amount = 100,
                isVerified = false,
                LastModifiedUtc = DateTime.Now,
            };
            service._DbContext.RealizationVbs.Add(dataRealisation);
            service._DbContext.SaveChanges();

            var Response = service.GenerateExcel(0, 0, null, "Clearance", null, null, null, null, null, null, 7);
            Assert.NotNull(Response);
            Response = service.GenerateExcel(0, 0, null, "Outstanding", null, null, null, null, null, null, 7);
            Assert.NotNull(Response);
            Response = service.GenerateExcel(0, 0, null, "All", null, null, null, null, null, null, 7);
            Assert.NotNull(Response);
        }

        [Fact]
        public async Task Should_Success_Get_Data_By_Applicant_Name()
        {
            VBStatusReportService service = new VBStatusReportService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var data = await _dataUtil(service).GetTestData_ById();
            var Response = await service.GetByApplicantName(data.CreatedBy);
            Assert.NotNull(Response);
        }
    }
}
