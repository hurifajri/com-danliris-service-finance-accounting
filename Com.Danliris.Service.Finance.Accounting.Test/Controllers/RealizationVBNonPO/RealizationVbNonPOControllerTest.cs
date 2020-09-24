using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBNonPO;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.RealizationVBNonPO
{
    public class RealizationVbNonPOControllerTest
    {
        private RealizationVbNonPOViewModel ViewModel
        {
            get { return new RealizationVbNonPOViewModel(); }
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

        private RealizationVbNonPOController GetController(IServiceProvider serviceProvider)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            var controller = (RealizationVbNonPOController)Activator.CreateInstance(typeof(RealizationVbNonPOController), serviceProvider);
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

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<RealizationVbList>(new List<RealizationVbList>(), 1, new Dictionary<string, string>(), new List<string>()));
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

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

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

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

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.CreateAsync(It.IsAny<RealizationVbModel>(), It.IsAny<RealizationVbNonPOViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<RealizationVbNonPOViewModel>()))
                .Verifiable();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<RealizationVbModel>(It.IsAny<RealizationVbNonPOViewModel>()))
                .Returns(It.IsAny<RealizationVbModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new RealizationVbNonPOViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public async Task Post_WithValidationException_ReturnBadRequest()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.CreateAsync(It.IsAny<RealizationVbModel>(), It.IsAny<RealizationVbNonPOViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<RealizationVbNonPOViewModel>()))
                .Throws(GetServiceValidationExeption());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<RealizationVbModel>(It.IsAny<RealizationVbNonPOViewModel>()))
                .Returns(It.IsAny<RealizationVbModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new RealizationVbNonPOViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Post_WithException_ReturnInternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.CreateAsync(It.IsAny<RealizationVbModel>(), It.IsAny<RealizationVbNonPOViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<RealizationVbNonPOViewModel>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<RealizationVbModel>(It.IsAny<RealizationVbNonPOViewModel>()))
                .Returns(It.IsAny<RealizationVbModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new RealizationVbNonPOViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Post_WithoutException_ReturnCreated_Mapping()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.MappingData(It.IsAny<RealizationVbNonPOViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<RealizationVbNonPOViewModel>()))
                .Verifiable();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<RealizationVbModel>(It.IsAny<RealizationVbNonPOViewModel>()))
                .Returns(It.IsAny<RealizationVbModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new RealizationVbNonPOViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public async Task Post_WithValidationException_ReturnBadRequest_Mapping()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.MappingData(It.IsAny<RealizationVbNonPOViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<RealizationVbNonPOViewModel>()))
                .Throws(GetServiceValidationExeption());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<RealizationVbModel>(It.IsAny<RealizationVbNonPOViewModel>()))
                .Returns(It.IsAny<RealizationVbModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new RealizationVbNonPOViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Post_WithException_ReturnInternalServerError_Mapping()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.MappingData(It.IsAny<RealizationVbNonPOViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<RealizationVbNonPOViewModel>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<RealizationVbModel>(It.IsAny<RealizationVbNonPOViewModel>()))
                .Returns(It.IsAny<RealizationVbModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new RealizationVbNonPOViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Put_WithoutException_ReturnNoContent()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<RealizationVbNonPOViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<RealizationVbNonPOViewModel>()))
                .Verifiable();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<RealizationVbModel>(It.IsAny<RealizationVbNonPOViewModel>()))
                .Returns(It.IsAny<RealizationVbModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Put(It.IsAny<int>(), new RealizationVbNonPOViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task Put_WithValidationException_ReturnBadRequest()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<RealizationVbNonPOViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<RealizationVbNonPOViewModel>()))
                .Throws(GetServiceValidationExeption());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<RealizationVbModel>(It.IsAny<RealizationVbNonPOViewModel>()))
                .Returns(It.IsAny<RealizationVbModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Put(It.IsAny<int>(), new RealizationVbNonPOViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Put_WithValidationException_ReturnBadRequest_Diff_Id()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<RealizationVbNonPOViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<RealizationVbNonPOViewModel>()))
                .Verifiable();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<RealizationVbModel>(It.IsAny<RealizationVbNonPOViewModel>()))
                .Returns(It.IsAny<RealizationVbModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Put(1, new RealizationVbNonPOViewModel() { Id = 0 });
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Put_WithException_ReturnInternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<RealizationVbNonPOViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<RealizationVbNonPOViewModel>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<RealizationVbModel>(It.IsAny<RealizationVbNonPOViewModel>()))
                .Returns(It.IsAny<RealizationVbModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Put(It.IsAny<int>(), new RealizationVbNonPOViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetById_WithoutException_ReturnOK()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(new RealizationVbNonPOViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<RealizationVbNonPOViewModel>(It.IsAny<RealizationVbModel>()))
                .Returns(new RealizationVbNonPOViewModel());
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

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<RealizationVbNonPOViewModel>(It.IsAny<RealizationVbModel>()))
                .Returns(new RealizationVbNonPOViewModel());
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

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync((RealizationVbNonPOViewModel)null);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<RealizationVbNonPOViewModel>(It.IsAny<RealizationVbModel>()))
                .Returns(new RealizationVbNonPOViewModel());
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

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<RealizationVbNonPOViewModel>(It.IsAny<RealizationVbModel>()))
                .Returns(new RealizationVbNonPOViewModel());
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

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.DeleteAsync(It.IsAny<int>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<RealizationVbNonPOViewModel>(It.IsAny<RealizationVbModel>()))
                .Returns(new RealizationVbNonPOViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Delete(It.IsAny<int>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_NotFound()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync((RealizationVbNonPOViewModel)null);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<RealizationVbNonPOViewModel>(It.IsAny<RealizationVbModel>()))
                .Returns(new RealizationVbNonPOViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(1);
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);

        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Exception()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<RealizationVbNonPOViewModel>(It.IsAny<RealizationVbModel>()))
                .Returns(new RealizationVbNonPOViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(1);
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_IDRAsync()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "IDR",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "Rp",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    UnitLoad = "UnitLoad",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = true
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_USDAsync()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    UnitLoad = "UnitLoad",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = true
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);
            controller.ControllerContext.HttpContext.Request.Headers["Accept"] = "application/pdf";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "0";
            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_PPn_False()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    UnitLoad = "UnitLoad",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_SPINNING1()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = null,
                TypeVBNonPO = "Tanpa Nomor VB",
                AmountSpinning1 = 1,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    UnitLoad = "SPINNING 1",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = null,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_SPINNING1VB()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = null,
                TypeVBNonPO = "Dengan Nomor VB",
                AmountSpinning1VB = 1,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    UnitLoad = "SPINNING 1",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = null,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_SPINNING2()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = null,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "SPINNING 2",
                    UnitId = 1,
                    UnitLoad = "SPINNING 2",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_SPINNING2VB()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",
                AmountSpinning2VB = 1,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = null,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "SPINNING 2",
                    UnitId = 1,
                    UnitLoad = "SPINNING 2",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_SPINNING3()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "SPINNING 3",
                    UnitId = 1,
                    UnitLoad = "SPINNING 3",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_SPINNING3VB()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",
                AmountSpinning3VB = 1,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "SPINNING 3",
                    UnitId = 1,
                    UnitLoad = "SPINNING 3",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_WEAVING1()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "WEAVING 1",
                    UnitId = 1,
                    UnitLoad = "WEAVING 1",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_WEAVING1VB()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",
                AmountWeaving1VB = 1,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "WEAVING 1",
                    UnitId = 1,
                    UnitLoad = "WEAVING 1",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_WEAVING2()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "WEAVING 2",
                    UnitId = 1,
                    UnitLoad = "WEAVING 2",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_WEAVING2VB()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",
                AmountWeaving2VB = 1,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "WEAVING 2",
                    UnitId = 1,
                    UnitLoad = "WEAVING 2",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_PRINTING()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "PRINTING",
                    UnitId = 1,
                    UnitLoad = "PRINTING",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_PRINTINGVB()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "PRINTING",
                    UnitId = 1,
                    UnitLoad = "PRINTING",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_DYEING()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "DYEING",
                    UnitId = 1,
                    UnitLoad = "DYEING",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_FINISHING()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "FINISHING",
                    UnitId = 1,
                    UnitLoad = "FINISHING",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_FINISHINGVB()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",
                AmountFinishingVB = 1,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "FINISHING",
                    UnitId = 1,
                    UnitLoad = "FINISHING",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_KONFEKSI1A()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "KONFEKSI 1A",
                    UnitId = 1,
                    UnitLoad = "KONFEKSI 1A",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            controller.ControllerContext.HttpContext.Request.Headers["Accept"] = "application/pdf";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "0";

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_KONFEKSI1AVB()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",
                AmountKonfeksi1AVB = 1,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "KONFEKSI 1A",
                    UnitId = 1,
                    UnitLoad = "KONFEKSI 1A",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            controller.ControllerContext.HttpContext.Request.Headers["Accept"] = "application/pdf";
            controller.ControllerContext.HttpContext.Request.Headers["x-timezone-offset"] = "0";

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_KONFEKSI1B()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Tanpa Nomor VB",
                Spinning1 = true,
                Spinning2 = true,
                Spinning3 = true,
                Weaving1 = true,
                Weaving2 = true,
                Finishing = true,
                Printing = true,
                Konfeksi1A = true,
                Konfeksi1B = true,
                Konfeksi2A = true,
                Konfeksi2B = true,
                Konfeksi2C = true,
                Umum = true,
                Others = true,

                AmountSpinning1 = 123,
                AmountSpinning2 = 123,
                AmountSpinning3 = 123,
                AmountWeaving1 = 123,
                AmountWeaving2 = 123,
                AmountFinishing = 123,
                AmountPrinting = 123,
                AmountKonfeksi1A = 123,
                AmountKonfeksi1B = 123,
                AmountKonfeksi2A = 123,
                AmountKonfeksi2B = 123,
                AmountKonfeksi2C = 123,
                AmountUmum = 123,
                AmountOthers = 123,

                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "KONFEKSI 1B",
                    UnitId = 1,
                    UnitLoad = "KONFEKSI 1B",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_KONFEKSI2A()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,

                TypeVBNonPO = "Dengan Nomor VB",
                Spinning1VB = true,
                Spinning2VB = true,
                Spinning3VB = true,
                Weaving1VB = true,
                Weaving2VB = true,
                FinishingVB = true,
                PrintingVB = true,
                Konfeksi1AVB = true,
                Konfeksi1BVB = true,
                Konfeksi2AVB = true,
                Konfeksi2BVB = true,
                Konfeksi2CVB = true,
                UmumVB = true,
                OthersVB = true,

                AmountSpinning1VB = 123,
                AmountSpinning2VB = 123,
                AmountSpinning3VB = 123,
                AmountWeaving1VB = 123,
                AmountWeaving2VB = 123,
                AmountFinishingVB = 123,
                AmountPrintingVB = 123,
                AmountKonfeksi1AVB = 123,
                AmountKonfeksi1BVB = 123,
                AmountKonfeksi2AVB = 123,
                AmountKonfeksi2BVB = 123,
                AmountKonfeksi2CVB = 123,
                AmountUmumVB = 123,
                AmountOthersVB = 123,

                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "KONFEKSI 2A",
                    UnitId = 1,
                    UnitLoad = "KONFEKSI 2A",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_KONFEKSI2B()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "KONFEKSI 2B",
                    UnitId = 1,
                    UnitLoad = "KONFEKSI 2B",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 0,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_KONFEKSI2BVB()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",
                AmountKonfeksi2BVB = 1,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "KONFEKSI 2B",
                    UnitId = 1,
                    UnitLoad = "KONFEKSI 2B",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 0,
                        isGetPPn = false
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_KONFEKSI2C()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 0,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "KONFEKSI 2C",
                    UnitId = 1,
                    UnitLoad = "KONFEKSI 2C",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1.5
                        },
                        IncomeTaxBy = "Supplier"
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_KONFEKSI2CVB()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",
                AmountKonfeksi2CVB = 1,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 0,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "KONFEKSI 2C",
                    UnitId = 1,
                    UnitLoad = "KONFEKSI 2C",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1.5
                        },
                        IncomeTaxBy = "Supplier"
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_UMUM()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UMUM",
                    UnitId = 1,
                    UnitLoad = "UMUM",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1.5
                        },
                        IncomeTaxBy = "Danliris"
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_UMUMVB()
        {
            var vm = new RealizationVbNonPOViewModel()
            {
                Id = 1,
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",
                AmountUmumVB = 1,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "$",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UMUM",
                    UnitId = 1,
                    UnitLoad = "UMUM",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1.5
                        },
                        IncomeTaxBy = "Danliris"
                    }
                }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbNonPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbNonPOService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.RealizationVbNonPORequestPDF(It.IsAny<int>());

            Assert.NotNull(response);
        }
    }
}
