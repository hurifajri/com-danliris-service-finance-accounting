using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VbWIthPORequest;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.VBWIthPORequest;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.VbWithPORequest
{
    public class VbWithPORequestControllerTest
    {
        private VbWithPORequestViewModel ViewModel
        {
            get { return new VbWithPORequestViewModel(); }
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

        private VbWithPORequestController GetController(IServiceProvider serviceProvider)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            var controller = (VbWithPORequestController)Activator.CreateInstance(typeof(VbWithPORequestController), serviceProvider);
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

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<VbRequestWIthPOList>(new List<VbRequestWIthPOList>(), 1, new Dictionary<string, string>(), new List<string>()));
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

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

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

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

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.CreateAsync(It.IsAny<VbRequestModel>(), It.IsAny<VbWithPORequestViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<VbWithPORequestViewModel>()))
                .Verifiable();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<VbRequestModel>(It.IsAny<VbWithPORequestViewModel>()))
                .Returns(It.IsAny<VbRequestModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new VbWithPORequestViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public async Task Post_WithValidationException_ReturnBadRequest()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.CreateAsync(It.IsAny<VbRequestModel>(), It.IsAny<VbWithPORequestViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<VbWithPORequestViewModel>()))
                .Throws(GetServiceValidationExeption());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<VbRequestModel>(It.IsAny<VbWithPORequestViewModel>()))
                .Returns(It.IsAny<VbRequestModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new VbWithPORequestViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Post_WithException_ReturnInternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.CreateAsync(It.IsAny<VbRequestModel>(), It.IsAny<VbWithPORequestViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<VbWithPORequestViewModel>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<VbRequestModel>(It.IsAny<VbWithPORequestViewModel>()))
                .Returns(It.IsAny<VbRequestModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new VbWithPORequestViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Put_WithoutException_ReturnNoContent()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<VbWithPORequestViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<VbWithPORequestViewModel>()))
                .Verifiable();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<VbRequestModel>(It.IsAny<VbWithPORequestViewModel>()))
                .Returns(It.IsAny<VbRequestModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Put(It.IsAny<int>(), new VbWithPORequestViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task Put_WithValidationException_ReturnBadRequest()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<VbWithPORequestViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<VbWithPORequestViewModel>()))
                .Throws(GetServiceValidationExeption());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<VbRequestModel>(It.IsAny<VbWithPORequestViewModel>()))
                .Returns(It.IsAny<VbRequestModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Put(It.IsAny<int>(), new VbWithPORequestViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Put_WithValidationException_ReturnBadRequest_Diff_Id()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<VbWithPORequestViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<VbWithPORequestViewModel>()))
                .Verifiable();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<VbRequestModel>(It.IsAny<VbWithPORequestViewModel>()))
                .Returns(It.IsAny<VbRequestModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Put(1, new VbWithPORequestViewModel() { Id = 0 });
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Put_WithException_ReturnInternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<VbWithPORequestViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<VbWithPORequestViewModel>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<VbRequestModel>(It.IsAny<VbWithPORequestViewModel>()))
                .Returns(It.IsAny<VbRequestModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Put(It.IsAny<int>(), new VbWithPORequestViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetById_WithoutException_ReturnOK()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(new VbWithPORequestViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbRequestModel>()))
                .Returns(new VbWithPORequestViewModel());
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

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbRequestModel>()))
                .Returns(new VbWithPORequestViewModel());
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

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync((VbWithPORequestViewModel)null);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbRequestModel>()))
                .Returns(new VbWithPORequestViewModel());
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

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbRequestModel>()))
                .Returns(new VbWithPORequestViewModel());
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

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.DeleteAsync(It.IsAny<int>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbRequestModel>()))
                .Returns(new VbWithPORequestViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Delete(It.IsAny<int>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Get_VBWithPORequest_PDF_NotFound()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync((VbWithPORequestViewModel)null);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<VbNonPORequestViewModel>(It.IsAny<VbRequestModel>()))
                .Returns(new VbNonPORequestViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetVbNonPORequestPDF(1);
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);

        }

        [Fact]
        public async Task Get_VBWithPO_PDF_Exception()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<VbNonPORequestViewModel>(It.IsAny<VbRequestModel>()))
                .Returns(new VbNonPORequestViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetVbNonPORequestPDF(1);
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_IDRAsync()
        {
            var vm = new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.Now,
                VBMoney = 1,
                Usage = "Usaage",
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVB()
                {
                    Id = 1,
                    Code = "IDR",
                    Rate = 1,
                    Symbol = "$",
                    Description = "DOLLAR"
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "Name",
                        },
                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Conversion = 1,
                                dealQuantity = 1,
                                dealUom = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 1,
                                defaultUom = new defaultUom()
                                {
                                    _id = "id",
                                    unit ="unit"
                                },
                                priceBeforeTax = 1,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }

                    }


                },

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            //var mapperMock = new Mock<IMapper>();
            //mapperMock
            //    .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbWithPORequestViewModel>()))
            //    .Returns(vm);
            //serviceProviderMock
            //    .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetVbNonPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_IDRAsync_S1()
        {
            var vm = new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.Now,
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVB()
                {
                    Id = 1,
                    Code = "IDR",
                    Rate = 1,
                    Symbol = "$",
                    Description = "DOLLAR"
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "SPINNING 1",
                        },
                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Conversion = 1,
                                dealQuantity = 1,
                                dealUom = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 1,
                                defaultUom = new defaultUom()
                                {
                                    _id = "id",
                                    unit ="unit"
                                },
                                priceBeforeTax = 1,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }

                    }


                },

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            //var mapperMock = new Mock<IMapper>();
            //mapperMock
            //    .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbWithPORequestViewModel>()))
            //    .Returns(vm);
            //serviceProviderMock
            //    .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetVbNonPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_IDRAsync_S2()
        {
            var vm = new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = null,
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVB()
                {
                    Id = 1,
                    Code = "USD",
                    Rate = 1,
                    Symbol = "$",
                    Description = "DOLLAR"
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "SPINNING 2",
                        },
                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Conversion = 1,
                                dealQuantity = 1,
                                dealUom = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 1,
                                defaultUom = new defaultUom()
                                {
                                    _id = "id",
                                    unit ="unit"
                                },
                                priceBeforeTax = 1,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }

                    }


                },

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            //var mapperMock = new Mock<IMapper>();
            //mapperMock
            //    .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbWithPORequestViewModel>()))
            //    .Returns(vm);
            //serviceProviderMock
            //    .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetVbNonPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_IDRAsync_S3()
        {
            var vm = new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.Now,
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVB()
                {
                    Id = 1,
                    Code = "IDR",
                    Rate = 1,
                    Symbol = "$",
                    Description = "DOLLAR"
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "SPINNING 3",
                        },
                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Conversion = 1,
                                dealQuantity = 1,
                                dealUom = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 1,
                                defaultUom = new defaultUom()
                                {
                                    _id = "id",
                                    unit ="unit"
                                },
                                priceBeforeTax = 1,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }

                    }


                },

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            //var mapperMock = new Mock<IMapper>();
            //mapperMock
            //    .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbWithPORequestViewModel>()))
            //    .Returns(vm);
            //serviceProviderMock
            //    .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetVbNonPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_IDRAsync_W1()
        {
            var vm = new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.Now,
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVB()
                {
                    Id = 1,
                    Code = "IDR",
                    Rate = 1,
                    Symbol = "$",
                    Description = "DOLLAR"
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "WEAVING 1",
                        },
                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Conversion = 1,
                                dealQuantity = 1,
                                dealUom = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 1,
                                defaultUom = new defaultUom()
                                {
                                    _id = "id",
                                    unit ="unit"
                                },
                                priceBeforeTax = 1,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }

                    }


                },

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            //var mapperMock = new Mock<IMapper>();
            //mapperMock
            //    .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbWithPORequestViewModel>()))
            //    .Returns(vm);
            //serviceProviderMock
            //    .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetVbNonPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_IDRAsync_W2()
        {
            var vm = new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.Now,
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVB()
                {
                    Id = 1,
                    Code = "IDR",
                    Rate = 1,
                    Symbol = "$",
                    Description = "DOLLAR"
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "WEAVING 2",
                        },
                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Conversion = 1,
                                dealQuantity = 1,
                                dealUom = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 1,
                                defaultUom = new defaultUom()
                                {
                                    _id = "id",
                                    unit ="unit"
                                },
                                priceBeforeTax = 1,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }

                    }


                },

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            //var mapperMock = new Mock<IMapper>();
            //mapperMock
            //    .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbWithPORequestViewModel>()))
            //    .Returns(vm);
            //serviceProviderMock
            //    .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetVbNonPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_IDRAsync_PR()
        {
            var vm = new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.Now,
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVB()
                {
                    Id = 1,
                    Code = "IDR",
                    Rate = 1,
                    Symbol = "$",
                    Description = "DOLLAR"
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "PRINTING",
                        },
                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Conversion = 1,
                                dealQuantity = 1,
                                dealUom = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 1,
                                defaultUom = new defaultUom()
                                {
                                    _id = "id",
                                    unit ="unit"
                                },
                                priceBeforeTax = 1,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }

                    }


                },

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            //var mapperMock = new Mock<IMapper>();
            //mapperMock
            //    .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbWithPORequestViewModel>()))
            //    .Returns(vm);
            //serviceProviderMock
            //    .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetVbNonPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_IDRAsync_UMUM()
        {
            var vm = new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.Now,
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVB()
                {
                    Id = 1,
                    Code = "IDR",
                    Rate = 1,
                    Symbol = "$",
                    Description = "DOLLAR"
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "UMUM",
                        },
                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Conversion = 1,
                                dealQuantity = 1,
                                dealUom = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 1,
                                defaultUom = new defaultUom()
                                {
                                    _id = "id",
                                    unit ="unit"
                                },
                                priceBeforeTax = 1,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }

                    }


                },

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            //var mapperMock = new Mock<IMapper>();
            //mapperMock
            //    .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbWithPORequestViewModel>()))
            //    .Returns(vm);
            //serviceProviderMock
            //    .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetVbNonPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_IDRAsync_FIN()
        {
            var vm = new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.Now,
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVB()
                {
                    Id = 1,
                    Code = "IDR",
                    Rate = 1,
                    Symbol = "$",
                    Description = "DOLLAR"
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "FINISHING",
                        },
                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Conversion = 1,
                                dealQuantity = 1,
                                dealUom = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 1,
                                defaultUom = new defaultUom()
                                {
                                    _id = "id",
                                    unit ="unit"
                                },
                                priceBeforeTax = 1,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }

                    }


                },

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            //var mapperMock = new Mock<IMapper>();
            //mapperMock
            //    .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbWithPORequestViewModel>()))
            //    .Returns(vm);
            //serviceProviderMock
            //    .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetVbNonPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_IDRAsync_K1A()
        {
            var vm = new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.Now,
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVB()
                {
                    Id = 1,
                    Code = "IDR",
                    Rate = 1,
                    Symbol = "$",
                    Description = "DOLLAR"
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "KONFEKSI 1A",
                        },
                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Conversion = 1,
                                dealQuantity = 1,
                                dealUom = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 1,
                                defaultUom = new defaultUom()
                                {
                                    _id = "id",
                                    unit ="unit"
                                },
                                priceBeforeTax = 1,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }

                    }


                },

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            //var mapperMock = new Mock<IMapper>();
            //mapperMock
            //    .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbWithPORequestViewModel>()))
            //    .Returns(vm);
            //serviceProviderMock
            //    .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetVbNonPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_IDRAsync_K1B()
        {
            var vm = new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.Now,
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVB()
                {
                    Id = 1,
                    Code = "IDR",
                    Rate = 1,
                    Symbol = "$",
                    Description = "DOLLAR"
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "KONFEKSI 1B",
                        },
                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Conversion = 1,
                                dealQuantity = 1,
                                dealUom = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 1,
                                defaultUom = new defaultUom()
                                {
                                    _id = "id",
                                    unit ="unit"
                                },
                                priceBeforeTax = 1,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }

                    }


                },

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            //var mapperMock = new Mock<IMapper>();
            //mapperMock
            //    .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbWithPORequestViewModel>()))
            //    .Returns(vm);
            //serviceProviderMock
            //    .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetVbNonPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_IDRAsync_K2A()
        {
            var vm = new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.Now,
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVB()
                {
                    Id = 1,
                    Code = "IDR",
                    Rate = 1,
                    Symbol = "$",
                    Description = "DOLLAR"
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "KONFEKSI 2A",
                        },
                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Conversion = 1,
                                dealQuantity = 1,
                                dealUom = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 1,
                                defaultUom = new defaultUom()
                                {
                                    _id = "id",
                                    unit ="unit"
                                },
                                priceBeforeTax = 1,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }

                    }


                },

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            //var mapperMock = new Mock<IMapper>();
            //mapperMock
            //    .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbWithPORequestViewModel>()))
            //    .Returns(vm);
            //serviceProviderMock
            //    .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetVbNonPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_IDRAsync_K2B()
        {
            var vm = new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.Now,
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVB()
                {
                    Id = 1,
                    Code = "IDR",
                    Rate = 1,
                    Symbol = "$",
                    Description = "DOLLAR"
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "KONFEKSI 2B",
                        },
                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Conversion = 1,
                                dealQuantity = 1,
                                dealUom = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 1,
                                defaultUom = new defaultUom()
                                {
                                    _id = "id",
                                    unit ="unit"
                                },
                                priceBeforeTax = 1,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }

                    }


                },

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            //var mapperMock = new Mock<IMapper>();
            //mapperMock
            //    .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbWithPORequestViewModel>()))
            //    .Returns(vm);
            //serviceProviderMock
            //    .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetVbNonPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_IDRAsync_K2C()
        {
            var vm = new VbWithPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.Now,
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVB()
                {
                    Id = 1,
                    Code = "IDR",
                    Rate = 1,
                    Symbol = "$",
                    Description = "DOLLAR"
                },
                Items = new List<VbWithPORequestDetailViewModel>()
                {
                    new VbWithPORequestDetailViewModel()
                    {
                        no = "no",
                        unit = new Unit()
                        {
                            Id = 1,
                            Code = "Code",
                            Name = "KONFEKSI 2C",
                        },
                        Details = new List<VbWithPORequestDetailItemsViewModel>()
                        {
                            new VbWithPORequestDetailItemsViewModel()
                            {
                                Conversion = 1,
                                dealQuantity = 1,
                                dealUom = new dealUom()
                                {
                                    _id = "id",
                                    unit = "unit"
                                },
                                defaultQuantity = 1,
                                defaultUom = new defaultUom()
                                {
                                    _id = "id",
                                    unit ="unit"
                                },
                                priceBeforeTax = 1,
                                product = new Product_VB()
                                {
                                    _id = "id",
                                    code = "code",
                                    name = "name"
                                },
                                productRemark = "productRemark"
                            }
                        }

                    }


                },

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            //var mapperMock = new Mock<IMapper>();
            //mapperMock
            //    .Setup(mapper => mapper.Map<VbWithPORequestViewModel>(It.IsAny<VbWithPORequestViewModel>()))
            //    .Returns(vm);
            //serviceProviderMock
            //    .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetVbNonPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public void GetWithDateFilter_WithoutException_ReturnOK()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadWithDateFilter(It.IsAny<DateTimeOffset?>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<VbRequestWIthPOList>(new List<VbRequestWIthPOList>(), 1, new Dictionary<string, string>(), new List<string>()));
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

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

            var response = controller.GetWithDateFilter(DateTimeOffset.UtcNow, "7", 1, 1, "string", new List<string>(), "string", "string");
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetWithDateFilter_WithException_InternalServerError()
        {

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.ReadWithDateFilter(It.IsAny<DateTimeOffset?>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

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

            var response = controller.GetWithDateFilter(DateTimeOffset.UtcNow, "7", 1, 1, "string", new List<string>(), "string", "string");
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }
    }
}
