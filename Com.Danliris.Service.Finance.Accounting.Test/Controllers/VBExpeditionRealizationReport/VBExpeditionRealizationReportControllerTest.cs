using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.VBExpeditionRealizationReport;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBExpeditionRealizationReport;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.VBExpeditionRealizationReport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.VBExpeditionRealizationReport
{
    public class VBExpeditionRealizationReportControllerTest
    {
        public (Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IVBExpeditionRealizationReportService> Service, Mock<IMapper> Mapper) GetMocks()
        {
            return (IdentityService: new Mock<IIdentityService>(), ValidateService: new Mock<IValidateService>(), Service: new Mock<IVBExpeditionRealizationReportService>(), Mapper: new Mock<IMapper>());
        }

        protected VBExpeditionRealizationReportController GetController((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IVBExpeditionRealizationReportService> Service, Mock<IMapper> Mapper) mocks)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            VBExpeditionRealizationReportController controller = new VBExpeditionRealizationReportController(mocks.IdentityService.Object, mocks.ValidateService.Object, mocks.Mapper.Object, mocks.Service.Object);
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
            mocks.Service.Setup(f => f.GetReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>()))
                .ReturnsAsync(new List<VBExpeditionRealizationReportViewModel>());

            var controller = GetController(mocks);
            var response = controller.GetReportAll(1, 1, "CreatedBy", 1, 1, "ALL", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, "7");
            var statusCode = GetStatusCode(response.Result);

            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetReportAll_WithException_InternalServerError()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReport(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>()))
                .Throws(new Exception());

            var controller = GetController(mocks);
            var response = controller.GetReportAll(1, 1, "CreatedBy", 1, 1, "ALL", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, "7");
            var statusCode = GetStatusCode(response.Result);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetXlsAll_WithoutException_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcel(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>()))
                .ReturnsAsync(new System.IO.MemoryStream());

            var controller = GetController(mocks);
            var response = controller.GetXlsAll(1, 1, "CreatedBy", 1, 1, "ALL", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, "7");
            Assert.NotNull(response);
        }

        [Fact]
        public void GetXlsAll_WithException_InternalServerError()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcel(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>()))
               .Throws(new Exception());

            var controller = GetController(mocks);
            var response = controller.GetXlsAll(1, 1, "CreatedBy", 1, 1, "ALL", DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, "7");
            var statusCode = GetStatusCode(response.Result);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }
    }
}
