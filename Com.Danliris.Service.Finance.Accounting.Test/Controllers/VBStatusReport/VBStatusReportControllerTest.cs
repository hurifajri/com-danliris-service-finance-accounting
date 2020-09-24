using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.VBStatusReport;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VBStatusReport;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBStatusReport;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.VBStatusReport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.VBStatusReport
{
    public class VBStatusReportControllerTest
    {
        public (Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IVBStatusReportService> Service, Mock<IMapper> Mapper) GetMocks()
        {
            return (IdentityService: new Mock<IIdentityService>(), ValidateService: new Mock<IValidateService>(), Service: new Mock<IVBStatusReportService>(), Mapper: new Mock<IMapper>());
        }

        protected VBStatusReportController GetController((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IVBStatusReportService> Service, Mock<IMapper> Mapper) mocks)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            VBStatusReportController controller = new VBStatusReportController(mocks.IdentityService.Object, mocks.ValidateService.Object, mocks.Mapper.Object, mocks.Service.Object);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");
            return controller;
        }

        protected int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        [Fact]
        public void GetReportAll_WithoutException_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReport(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>()))
                .Returns(new List<VBStatusReportViewModel>());

            var controller = GetController(mocks);
            var response = controller.GetReportAll(1, 1, "CreatedBy", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, "ALL");
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetReportAll_WithException_InternalServerError()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReportWithCurrency(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>()))
                .Throws(new Exception());

            var controller = GetController(mocks);
            var response = controller.GetReportAll(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<string>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetXlsAll_WithoutException_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcel(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>()))
                .Returns(new System.IO.MemoryStream());

            var controller = GetController(mocks);
            var response = controller.GetXlsAll(1, 1, "CreatedBy", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), "ALL");
            Assert.NotNull(response);
        }

        [Fact]
        public void GetXlsAll_WithException_InternalServerError()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcel(It.IsAny<int>(), It.IsAny<long>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<int>()))
               .Throws(new Exception());

            var controller = GetController(mocks);
            var response = controller.GetXlsAll(1, 1, "CreatedBy", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), "ALL");
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetByApplicantName_NotNullModel_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetByApplicantName(It.IsAny<string>())).ReturnsAsync(new List<VbRequestModel>());

            var controller = GetController(mocks);
            var response = controller.GetByApplicantName("Pemohon");
            var statusCode = GetStatusCode(response.Result);

            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetByApplicantName_ThrowException_ReturnInternalServerError()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetByApplicantName(It.IsAny<string>())).ThrowsAsync(new Exception());

            var controller = GetController(mocks);
            var response = controller.GetByApplicantName("Pemohon");
            var statusCode = GetStatusCode(response.Result);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }
    }
}
