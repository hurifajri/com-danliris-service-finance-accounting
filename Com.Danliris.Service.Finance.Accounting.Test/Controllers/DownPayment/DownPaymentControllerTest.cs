using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DownPayment;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DownPayment;
using Com.Danliris.Service.Finance.Accounting.Test.Controller.Utils;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.DownPayment;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.DownPayment
{
    public class DownPaymentControllerTest 
    {

        private DownPaymentViewModel ViewModel
        {
            get { return new DownPaymentViewModel(); }
        }

        protected ServiceValidationException GetServiceValidationExeption()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(ViewModel, serviceProvider.Object, null);
            return new ServiceValidationException(validationContext, validationResults);
        }

        private int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        private DownPaymentController GetController(IServiceProvider serviceProvider)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            var controller = (DownPaymentController)Activator.CreateInstance(typeof(DownPaymentController), serviceProvider);
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext()
                {
                    User = user.Object
                }
            };
            controller.ControllerContext.HttpContext.Request.Headers["Authorization"] = "Bearer unittesttoken";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "7";
            controller.ControllerContext.HttpContext.Request.Path = new PathString("/v1/unit-test");
            return controller;
        }

        [Fact]
        public void Get_WithoutException_ReturnOK()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<DownPaymentList>(new List<DownPaymentList>(), 1, new Dictionary<string, string>(), new List<string>()));
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = controller.Get(select: new List<string>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void Get_WithException_ReturnInternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = controller.Get(select: new List<string>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Post_WithoutException_ReturnCreated()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.CreateAsync(It.IsAny<DownPaymentModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<DownPaymentViewModel>()))
                .Verifiable();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<DownPaymentModel>(It.IsAny<DownPaymentViewModel>()))
                .Returns(It.IsAny<DownPaymentModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new DownPaymentViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public async Task Post_WithValidationException_ReturnBadRequest()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.CreateAsync(It.IsAny<DownPaymentModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<DownPaymentViewModel>()))
                .Throws(GetServiceValidationExeption());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<DownPaymentModel>(It.IsAny<DownPaymentViewModel>()))
                .Returns(It.IsAny<DownPaymentModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new DownPaymentViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Post_WithException_ReturnInternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.CreateAsync(It.IsAny<DownPaymentModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<DownPaymentViewModel>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<DownPaymentModel>(It.IsAny<DownPaymentViewModel>()))
                .Returns(It.IsAny<DownPaymentModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new DownPaymentViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Put_WithoutException_ReturnNoContent()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<DownPaymentModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<DownPaymentViewModel>()))
                .Verifiable();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<DownPaymentModel>(It.IsAny<DownPaymentViewModel>()))
                .Returns(It.IsAny<DownPaymentModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Put(It.IsAny<int>(), new DownPaymentViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task Put_WithValidationException_ReturnBadRequest()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<DownPaymentModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<DownPaymentViewModel>()))
                .Throws(GetServiceValidationExeption());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<DownPaymentModel>(It.IsAny<DownPaymentViewModel>()))
                .Returns(It.IsAny<DownPaymentModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Put(It.IsAny<int>(), new DownPaymentViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Put_WithValidationException_ReturnBadRequest_Diff_Id()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<DownPaymentModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<DownPaymentViewModel>()))
                .Verifiable();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<DownPaymentModel>(It.IsAny<DownPaymentViewModel>()))
                .Returns(It.IsAny<DownPaymentModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Put(1, new DownPaymentViewModel() { Id = 0 });
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Put_WithException_ReturnInternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<DownPaymentModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<DownPaymentViewModel>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<DownPaymentModel>(It.IsAny<DownPaymentViewModel>()))
                .Returns(It.IsAny<DownPaymentModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Put(It.IsAny<int>(), new DownPaymentViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetById_WithoutException_ReturnOK()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new DownPaymentModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<DownPaymentViewModel>(It.IsAny<DownPaymentModel>()))
                .Returns(new DownPaymentViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetById(It.IsAny<int>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GetById_WithException_ReturnInternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync(It.IsAny<int>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<DownPaymentViewModel>(It.IsAny<DownPaymentModel>()))
                .Returns(new DownPaymentViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetById(It.IsAny<int>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnNotFound()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((DownPaymentModel)null);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<DownPaymentViewModel>(It.IsAny<DownPaymentModel>()))
                .Returns(new DownPaymentViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetById(It.IsAny<int>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task DeleteById_WithoutException_ReturnNoContent()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<DownPaymentViewModel>(It.IsAny<DownPaymentModel>()))
                .Returns(new DownPaymentViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Delete(It.IsAny<int>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task DeleteById_WithException_ReturnInternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.DeleteAsync(It.IsAny<int>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<DownPaymentViewModel>(It.IsAny<DownPaymentModel>()))
                .Returns(new DownPaymentViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Delete(It.IsAny<int>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_IDRAsync()
        {
            var vm = new DownPaymentViewModel()
            {
                DatePayment = DateTimeOffset.Now,
                Bank = new Bank()
                {
                    AccountName = "Name",
                    BankName= "BankName",
                    AccountNumber = "Account",

                    Currency = new BankCurrency()
                    {
                        Code = "IDR"
                    },

                },
                Buyer = new Buyer()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                TotalPayment = 12345,

                Currency = new Currency()
                {
                    Id = 1,
                    Code = "IDR",
                    Rate = 1,
                },
                Remark="remark"

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new DownPaymentModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<DownPaymentViewModel>(It.IsAny<DownPaymentModel>()))
                .Returns(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetDownPaymentPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_Not_IDR()
        {
            
            var vm = new DownPaymentViewModel()
            {  
                CategoryAcceptance= "CategoryAcceptance",
                DatePayment = DateTimeOffset.Now,
                Bank = new Bank()
                {
                    AccountName = "Name",
                    BankName = "BankName",
                    AccountNumber = "Account",
                    Currency = new BankCurrency()
                    {
                        Code = "USD"
                    },
                    
                },
                Buyer = new Buyer()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                TotalPayment = 12345678,

                Currency = new Currency()
                {
                    Id = 1,
                    Code = "USD",
                    Rate = 14447,
                },
                Remark = "remark"

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new DownPaymentModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<DownPaymentViewModel>(It.IsAny<DownPaymentModel>()))
                .Returns(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetDownPaymentPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_NotFound()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((DownPaymentModel)null);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<DownPaymentViewModel>(It.IsAny<DownPaymentModel>()))
                .Returns(new DownPaymentViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetDownPaymentPDF(1);
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);

        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Exception()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IDownPaymentService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync(It.IsAny<int>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IDownPaymentService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<DownPaymentViewModel>(It.IsAny<DownPaymentModel>()))
                .Returns(new DownPaymentViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetDownPaymentPDF(1);
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }
    }
}
