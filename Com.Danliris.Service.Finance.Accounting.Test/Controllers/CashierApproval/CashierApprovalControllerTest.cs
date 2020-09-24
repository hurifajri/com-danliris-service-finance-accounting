using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.CashierApproval;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CashierApproval;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.CashierApproval;
using Com.Moonlay.NetCore.Lib.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.CashierApproval
{
    public class CashierApprovalControllerTest
    {
        public (Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<ICashierAprovalService> Service, Mock<IMapper> Mapper) GetMocks()
        {
            return (IdentityService: new Mock<IIdentityService>(), ValidateService: new Mock<IValidateService>(), Service: new Mock<ICashierAprovalService>(), Mapper: new Mock<IMapper>());
        }

        protected CashierApprovalController GetController((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<ICashierAprovalService> Service, Mock<IMapper> Mapper) mocks)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                    new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            CashierApprovalController controller = new CashierApprovalController(mocks.IdentityService.Object, mocks.ValidateService.Object, mocks.Mapper.Object, mocks.Service.Object);
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

        private ServiceValidationException GetServiceValidationExeption()
        {
            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            List<ValidationResult> validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(this.ViewModel, serviceProvider.Object, null);
            return new ServiceValidationException(validationContext, validationResults);
        }

        private CashierApprovalViewModel ViewModel
        {
            get
            {
                return new CashierApprovalViewModel()
                {
                    VBRequestCategory = "VBRequestCategory",
                    CashierApproval = new List<CashierApprovalItemViewModel>()
                    {
                        new CashierApprovalItemViewModel()
                        {
                            VBNo = "VBNo",
                            Id = 1
                        }
                    }
                };
            }
        }


        [Fact]
        public async Task Post_Success()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.CashierAproval(It.IsAny<CashierApprovalViewModel>())).ReturnsAsync(1);

            var controller = GetController(mocks);
            
            var response = await controller.Post(ViewModel);

            Assert.Equal((int)HttpStatusCode.NoContent, GetStatusCode(response));
        }

        [Fact]
        public void Post_Throws_Validation_Exception()
        {
            var validateMock = new Mock<IValidateService>();
            validateMock.Setup(s => s.Validate(It.IsAny<CashierApprovalViewModel>())).Throws(GetServiceValidationExeption());
            var mockMapper = new Mock<IMapper>();

            var mockFacade = new Mock<ICashierAprovalService>();
            var mockIdentity = new Mock<IIdentityService>();
            var response = GetController((mockIdentity, validateMock, mockFacade, mockMapper)).Post(ViewModel).Result;
            Assert.Equal((int)HttpStatusCode.BadRequest, GetStatusCode(response));
        }

        [Fact]
        public void Post_Throws_Internal_Error()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.CashierAproval(It.IsAny<CashierApprovalViewModel>())).Throws(new Exception());

            var response = GetController(mocks).Post(ViewModel).Result;
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void Post_Throws_Internal_Error_Aggregate()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.CashierAproval(It.IsAny<CashierApprovalViewModel>())).Throws(new AggregateException());

            var response = GetController(mocks).Post(ViewModel).Result;
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void Delete_Success()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteCashierAproval(It.IsAny<int>())).ReturnsAsync(1);

            var response = GetController(mocks).Delete(1).Result;
            Assert.Equal((int)HttpStatusCode.NoContent, GetStatusCode(response));
        }

        [Fact]
        public void Delete_Throws_Internal_Error()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteCashierAproval(It.IsAny<int>())).Throws(new Exception());

            var response = GetController(mocks).Delete(1).Result;
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public void Delete_Throws_Internal_Error_Aggregate()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.DeleteCashierAproval(It.IsAny<int>())).Throws(new AggregateException());

            var response = GetController(mocks).Delete(1).Result;
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
    }
}
