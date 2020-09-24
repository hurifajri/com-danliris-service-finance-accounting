using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.PaymentDispositionNotVerifiedReport;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PurchasingDispositionExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.PurchasingDispositionVerification;
using Com.Danliris.Service.Finance.Accounting.Test.DataUtils.PurchasingDispositionExpedition;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.PaymentDispositionNotVerifiedReport
{
    public class PaymentDispositionNotVerifiedReportServiceTest
    {
        private const string ENTITY = "PurchasingDispositionExpeditions";
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

        private PurchasingDispositionExpeditionDataUtil _dataUtil(PurchasingDispositionExpeditionService service)
        {
            return new PurchasingDispositionExpeditionDataUtil(service);
        }

        private Mock<IServiceProvider> GetServiceProvider()
        {
            var httpClientService = new Mock<IHttpClientService>();
            httpClientService
                .Setup(x => x.PutAsync(It.Is<string>(s => s.Contains("purchasing-dispositions/update/position")), It.IsAny<StringContent>()))
                .ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK));

            var serviceProvider = new Mock<IServiceProvider>();

            serviceProvider
                .Setup(x => x.GetService(typeof(IHttpClientService)))
                .Returns(httpClientService.Object);

            serviceProvider
                .Setup(x => x.GetService(typeof(IIdentityService)))
                .Returns(new IdentityService() { Token = "Token", Username = "Test", TimezoneOffset = 7 });


            return serviceProvider;
        }



        [Fact]
        public async Task Should_Success_Get_All_Data()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PurchasingDispositionExpeditionModel model = await _dataUtil(service).GetTestData();

            PurchasingDispositionVerificationViewModel data = new PurchasingDispositionVerificationViewModel()
            {
                DispositionNo = model.DispositionNo,
                Id = 0,
                Reason = "Reason",
                SubmitPosition = ExpeditionPosition.SEND_TO_PURCHASING_DIVISION,
                VerifyDate = DateTimeOffset.UtcNow
            };
            await service.PurchasingDispositionVerification(data);

            DateTimeOffset tomorrow = DateTimeOffset.UtcNow.AddDays(1);
            PaymentDispositionNotVerifiedReportService report= new PaymentDispositionNotVerifiedReportService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var response = report.GetReport("", "", "", model.VerifyDate.GetValueOrDefault().AddDays(-30), tomorrow, 1, 25, "{}", 7, "notHistory");
            Assert.NotNull(response.Item1);
        }

        [Fact]
        public async Task Should_Success_Get_Excel()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PurchasingDispositionExpeditionModel model = await _dataUtil(service).GetTestData();

            PurchasingDispositionVerificationViewModel data = new PurchasingDispositionVerificationViewModel()
            {
                DispositionNo = model.DispositionNo,
                Id = 0,
                Reason = "Reason",
                SubmitPosition = ExpeditionPosition.SEND_TO_PURCHASING_DIVISION,
                VerifyDate = DateTimeOffset.UtcNow
            };
            await service.PurchasingDispositionVerification(data);

            PaymentDispositionNotVerifiedReportService report = new PaymentDispositionNotVerifiedReportService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            DateTimeOffset tomorrow = DateTimeOffset.UtcNow.AddDays(1);
            var reportResponse = report.GenerateExcel("", "", "", model.VerifyDate, tomorrow, 7, "notHistory");
            Assert.NotNull(reportResponse);
        }

        [Fact]
        public async Task Should_Success_Get_All_Data_History()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PurchasingDispositionExpeditionModel model = await _dataUtil(service).GetTestData();

            PurchasingDispositionVerificationViewModel data = new PurchasingDispositionVerificationViewModel()
            {
                DispositionNo = model.DispositionNo,
                Id = 0,
                Reason = "Reason",
                SubmitPosition = ExpeditionPosition.SEND_TO_PURCHASING_DIVISION,
                VerifyDate = DateTimeOffset.UtcNow
            };
            await service.PurchasingDispositionVerification(data);

            DateTimeOffset tomorrow = DateTimeOffset.UtcNow.AddDays(1);
            PaymentDispositionNotVerifiedReportService report = new PaymentDispositionNotVerifiedReportService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            var response = report.GetReport("", "", "", model.VerifyDate, tomorrow, 1, 25, "{}", 7, "history");
            Assert.NotNull(response.Item1);
        }

        [Fact]
        public async Task Should_Success_Get_Excel_History()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));
            PurchasingDispositionExpeditionModel model = await _dataUtil(service).GetTestData();

            PurchasingDispositionVerificationViewModel data = new PurchasingDispositionVerificationViewModel()
            {
                DispositionNo = model.DispositionNo,
                Id = 0,
                Reason = "Reason",
                SubmitPosition = ExpeditionPosition.SEND_TO_PURCHASING_DIVISION,
                VerifyDate = DateTimeOffset.UtcNow
            };
            await service.PurchasingDispositionVerification(data);

            PaymentDispositionNotVerifiedReportService report = new PaymentDispositionNotVerifiedReportService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            DateTimeOffset tomorrow = DateTimeOffset.UtcNow.AddDays(1);
            var reportResponse = report.GenerateExcel("", "", "", model.VerifyDate, tomorrow, 7, "history");
            Assert.NotNull(reportResponse);
        }

        [Fact]
        public void Should_Success_GenerateExcel_When_DataNoExist()
        {
            PurchasingDispositionExpeditionService service = new PurchasingDispositionExpeditionService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            PaymentDispositionNotVerifiedReportService report = new PaymentDispositionNotVerifiedReportService(GetServiceProvider().Object, _dbContext(GetCurrentMethod()));

            DateTimeOffset tomorrow = DateTimeOffset.UtcNow.AddDays(1);
            var reportResponse = report.GenerateExcel("", "", "", DateTimeOffset.Now, tomorrow, 7, "history");
            Assert.NotNull(reportResponse);
        }
    }
}
