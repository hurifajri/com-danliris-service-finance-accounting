using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.VBRealizationDocumentExpedition
{
    public class VBRealizationExpeditionControllerTest
    {
        
        protected VBRealizationExpeditionController GetController(Mock<IServiceProvider> serviceProvider)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);

            VBRealizationExpeditionController controller = new VBRealizationExpeditionController(serviceProvider.Object);
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

        Mock<IServiceProvider> GetServiceProvider()
        {
            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            serviceProvider
              .Setup(s => s.GetService(typeof(IIdentityService)))
              .Returns(new IdentityService() { TimezoneOffset = 1, Token = "token", Username = "username" });

            var validateService = new Mock<IValidateService>();
            serviceProvider
              .Setup(s => s.GetService(typeof(IValidateService)))
              .Returns(validateService.Object);
            return serviceProvider;
        }

        protected ServiceValidationException GetServiceValidationException()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(vBRealizationIdListDto, serviceProvider.Object, null);
            return new ServiceValidationException(validationContext, validationResults);
        }

        protected int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        public VBRealizationIdListDto vBRealizationIdListDto
        {
            get
            {
                return new VBRealizationIdListDto()
                {
                    VBRealizationIds = new List<int>()
                    {
                        1
                    }
                };
            }
        }

        VBRealizationExpeditionRejectDto rejectDto
        {
            get
            {
                return new VBRealizationExpeditionRejectDto()
                {
                    Reason = "Reason"
                };
            }
        }

        ReportDto vBRealizationDocumentExpeditionModel
        {
            get
            {
                return new ReportDto()
                {
                    CashierReceiptBy = "Cashier",
                    CashierReceiptDate = DateTime.Now.ToUniversalTime(),
                    CurrencyCode = "Code",
                    CurrencyRate = 1,
                    DivisionId = 1,
                    DivisionName = "Name",
                    NotVerifiedBy = "Not Verified By",
                    NotVerifiedDate = DateTime.Now.ToUniversalTime(),
                    NotVerifiedReason = "reason",
                    Position = VBRealizationPosition.Purchasing,
                    SendToVerificationBy = "Verification By",
                    SendToVerificationDate = DateTime.Now.ToUniversalTime(),
                    UnitId = 1,
                    UnitName = "Nam",
                    VBAmount = 1,
                    VBId = 1,
                    VBNo = " no",
                    VBRealizationAmount = 1,
                    VBRealizationDate = DateTime.Now.ToUniversalTime(),
                    VBRealizationId = 1,
                    VBRealizationNo = "No",
                    VBRequestName = "name",
                    VBType = VBType.NonPO,
                    VerificationReceiptBy = "ReceiptBy",
                    VerificationReceiptDate = DateTime.Now.ToUniversalTime(),
                    VerifiedToCashierBy = "CashierBy",
                    VerifiedToCashierDate = DateTime.Now.ToUniversalTime()
                };
               
            }
        }

        [Fact]
        public void GetVbRealizationToVerification_Return_OK()
        {
            //Setup
            var serviceProviderMock = GetServiceProvider();
            
            var service = new Mock<IVBRealizationDocumentExpeditionService>();
            Dictionary<string, string> order = new Dictionary<string, string>();
            service
                .Setup(s => s.ReadRealizationToVerification(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new ReadResponse<VBRealizationDocumentModel>(new List<VBRealizationDocumentModel>(), 1, order, new List<string>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetVbRealizationToVerification(0, 0, null, null, 0);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetVbRealizationToVerification_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();
           
            service
                .Setup(s => s.ReadRealizationToVerification(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<int>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).GetVbRealizationToVerification(0, 0, null, null, 0);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void Get_Return_OK()
        {
            //Setup
            var serviceProviderMock = GetServiceProvider(); ;
            
            var service = new Mock<IVBRealizationDocumentExpeditionService>();
            Dictionary<string, string> order = new Dictionary<string, string>();
            service
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<VBRealizationPosition>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new ReadResponse<VBRealizationDocumentExpeditionModel>(new List<VBRealizationDocumentExpeditionModel>(), 1, order, new List<string>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).Get(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<VBRealizationPosition>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void Get_Return_InternalServerError()
        {
            //Setup
            var serviceProviderMock = new Mock<IServiceProvider>();
            
            var service = new Mock<IVBRealizationDocumentExpeditionService>();
            service
                .Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<VBRealizationPosition>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<int>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = GetController(serviceProviderMock).Get(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<VBRealizationPosition>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>());
            
            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public void GetVerification_Return_OK()
        {
            //Setup
            var serviceProviderMock = GetServiceProvider(); ;
            
            var service = new Mock<IVBRealizationDocumentExpeditionService>();
            Dictionary<string, string> order = new Dictionary<string, string>();
            service
                .Setup(s => s.ReadVerification(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<VBRealizationPosition>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<int>()))
                .Returns(new ReadResponse<VBRealizationDocumentExpeditionModel>(new List<VBRealizationDocumentExpeditionModel>(), 1, order, new List<string>()));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);
            //Act
            IActionResult response = GetController(serviceProviderMock).GetVerification(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<VBRealizationPosition>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>());
            
            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetVerification_Return_InternalServerError()
        {
            //Setup
            var serviceProviderMock = GetServiceProvider(); ;
            
            var service = new Mock<IVBRealizationDocumentExpeditionService>();
            Dictionary<string, string> order = new Dictionary<string, string>();
            service
                .Setup(s => s.ReadVerification(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<VBRealizationPosition>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<int>()))
                .Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);
            //Act
            IActionResult response = GetController(serviceProviderMock).GetVerification(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset?>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<VBRealizationPosition>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>());

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Post_Succes_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.SubmitToVerification(It.IsAny<List<int>>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).Post(new VBRealizationIdListDto() { VBRealizationIds = new List<int>() { 1 } });

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }


        [Fact]
        public async Task Post_Throws_ServiceValidationExeption()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.SubmitToVerification(It.IsAny<List<int>>()))
                .ThrowsAsync(GetServiceValidationException());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).Post(vBRealizationIdListDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Post_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.SubmitToVerification(It.IsAny<List<int>>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).Post(vBRealizationIdListDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public async Task Verify_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service.Setup(s => s.VerifiedToCashier(It.IsAny<int>())).ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).Verify(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }


        [Fact]
        public async Task Verify_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.VerifiedToCashier(It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).Verify(1);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }
        [Fact]
        public async Task reject_Return_Created()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.Reject(It.IsAny<int>(), It.IsAny<string>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).reject(1, rejectDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task reject_Throws_ServiceValidationException()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.Reject(It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(GetServiceValidationException());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).reject(1, rejectDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }


        [Fact]
        public async Task reject_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.Reject(It.IsAny<int>(), It.IsAny<string>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).reject(1, rejectDto);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public async Task AcceptForVerification_Sucees_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.VerificationDocumentReceipt(It.IsAny<List<int>>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).AcceptForVerification(new VBRealizationIdListDto() { VBRealizationIds = new List<int>() { 1 } });

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task AcceptForVerification_Throws_ServiceValidationException()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.VerificationDocumentReceipt(It.IsAny<List<int>>()))
                .ThrowsAsync(GetServiceValidationException());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).AcceptForVerification(new VBRealizationIdListDto() { VBRealizationIds = new List<int>() { 1 } });

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }


        [Fact]
        public async Task AcceptForVerification_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.VerificationDocumentReceipt(It.IsAny<List<int>>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).AcceptForVerification(new VBRealizationIdListDto() { VBRealizationIds = new List<int>() { 1 } });

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task AcceptForCashier_Sucees_Return_NoContent()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.CashierReceipt(It.IsAny<List<int>>()))
                .ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).AcceptForCashier(new VBRealizationIdListDto() { VBRealizationIds = new List<int>() { 1 } });

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task AcceptForCashier_Throws_ServiceValidationException()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.CashierReceipt(It.IsAny<List<int>>()))
                .ThrowsAsync(GetServiceValidationException());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).AcceptForCashier(new VBRealizationIdListDto() { VBRealizationIds = new List<int>() { 1 } });

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task AcceptForCashier_REturn_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.CashierReceipt(It.IsAny<List<int>>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).AcceptForCashier(new VBRealizationIdListDto() { VBRealizationIds = new List<int>() { 1 } });

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetReport_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.GetReports(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new VBRealizationDocumentExpeditionReportDto(new List<ReportDto>() { new ReportDto()},1,1,25));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetReport(1,1,"",1,1,null,null,null,25);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GetReport_With_dateStart_and_dateEnd_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.GetReports(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new VBRealizationDocumentExpeditionReportDto(new List<ReportDto>() { new ReportDto() }, 1, 1, 25));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetReport(1, 1, "", 1, 1,DateTime.Now, DateTime.Now.AddDays(2), null, 25);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GetReport_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service.Setup(s => s.GetReports(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>())).ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetReport(1, 1, "", 1, 1, null, null, null, 25);

            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetReportXls_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.GetReports(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new VBRealizationDocumentExpeditionReportDto(new List<ReportDto>() { vBRealizationDocumentExpeditionModel }, 1, 1, 25));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetReportXls(1, 1, "", 1, 1, null, null,null);

            //Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", response.GetType().GetProperty("ContentType").GetValue(response, null));
            
        }

        [Fact]
        public async Task GetReportXls_when_DataNull_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.GetReports(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new VBRealizationDocumentExpeditionReportDto(new List<ReportDto>(), 1, 1, 25));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);
            
            //Act
            IActionResult response = await GetController(serviceProviderMock).GetReportXls(1, 1, "", 1, 1, null, null,null);
            
            //Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", response.GetType().GetProperty("ContentType").GetValue(response, null));
        }

        [Fact]
        public async Task GetReportXls_when_DataEmpty_Return_OK()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.GetReports(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(new VBRealizationDocumentExpeditionReportDto(new List<ReportDto>() { }, 1, 1, 25));

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetReportXls(1, 1, "", 1, 1, null, null,null);

            //Assert
            Assert.Equal("application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", response.GetType().GetProperty("ContentType").GetValue(response, null));
        }

        [Fact]
        public async Task GetReportXls_Return_InternalServerError()
        {
            //Setup
            Mock<IServiceProvider> serviceProviderMock = GetServiceProvider();
            var service = new Mock<IVBRealizationDocumentExpeditionService>();

            service
                .Setup(s => s.GetReports(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<DateTimeOffset>(), It.IsAny<DateTimeOffset>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<int>()))
                .ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IVBRealizationDocumentExpeditionService)))
               .Returns(service.Object);

            //Act
            IActionResult response = await GetController(serviceProviderMock).GetReportXls(1, 1, "", 1, 1, null, null,null);
            
            //Assert
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }
    }
}
