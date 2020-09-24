using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.Memo;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.Memo;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.Memo;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.Memo;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.Memo
{
    public class MemoControllerTest
    {
        private MemoViewModel ViewModel
        {
            get { return new MemoViewModel(); }
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

        private MemoController GetController(IServiceProvider serviceProvider)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            var controller = (MemoController)Activator.CreateInstance(typeof(MemoController), serviceProvider);
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

            var serviceMock = new Mock<IMemoService>();
            serviceMock
                .Setup(service => service.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<MemoList>(new List<MemoList>(), 1, new Dictionary<string, string>(), new List<string>()));
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoService))).Returns(serviceMock.Object);

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

            var serviceMock = new Mock<IMemoService>();
            serviceMock
                .Setup(service => service.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoService))).Returns(serviceMock.Object);

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

            var serviceMock = new Mock<IMemoService>();
            serviceMock
                .Setup(service => service.CreateAsync(It.IsAny<MemoModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<MemoViewModel>()))
                .Verifiable();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<MemoModel>(It.IsAny<MemoViewModel>()))
                .Returns(It.IsAny<MemoModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new MemoViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public async Task Post_WithValidationException_ReturnBadRequest()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoService>();
            serviceMock
                .Setup(service => service.CreateAsync(It.IsAny<MemoModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<MemoViewModel>()))
                .Throws(GetServiceValidationExeption());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<MemoModel>(It.IsAny<MemoViewModel>()))
                .Returns(It.IsAny<MemoModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new MemoViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Post_WithException_ReturnInternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoService>();
            serviceMock
                .Setup(service => service.CreateAsync(It.IsAny<MemoModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<MemoViewModel>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<MemoModel>(It.IsAny<MemoViewModel>()))
                .Returns(It.IsAny<MemoModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new MemoViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Put_WithoutException_ReturnNoContent()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoService>();
            serviceMock
                .Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<MemoModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<MemoViewModel>()))
                .Verifiable();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<MemoModel>(It.IsAny<MemoViewModel>()))
                .Returns(It.IsAny<MemoModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Put(It.IsAny<int>(), new MemoViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task Put_WithValidationException_ReturnBadRequest()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoService>();
            serviceMock
                .Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<MemoModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<MemoViewModel>()))
                .Throws(GetServiceValidationExeption());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<MemoModel>(It.IsAny<MemoViewModel>()))
                .Returns(It.IsAny<MemoModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Put(It.IsAny<int>(), new MemoViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Put_WithValidationException_ReturnBadRequest_Diff_Id()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoService>();
            serviceMock
                .Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<MemoModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<MemoViewModel>()))
                .Verifiable();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<MemoModel>(It.IsAny<MemoViewModel>()))
                .Returns(It.IsAny<MemoModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Put(1, new MemoViewModel() { Id = 0 });
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Put_WithException_ReturnInternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoService>();
            serviceMock
                .Setup(service => service.UpdateAsync(It.IsAny<int>(), It.IsAny<MemoModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            validateServiceMock
                .Setup(validateService => validateService.Validate(It.IsAny<MemoViewModel>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<MemoModel>(It.IsAny<MemoViewModel>()))
                .Returns(It.IsAny<MemoModel>());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Put(It.IsAny<int>(), new MemoViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetById_WithoutException_ReturnOK()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync(It.IsAny<int>()))
                .ReturnsAsync(new MemoModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<MemoViewModel>(It.IsAny<MemoModel>()))
                .Returns(new MemoViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetById(It.IsAny<int>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GetbySalesInvoice_WithoutException_ReturnOK()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoService>();
            serviceMock
                .Setup(service => service.ReadBySalesInvoiceAsync(It.IsAny<string>()))
                .ReturnsAsync(new MemoModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<MemoViewModel>(It.IsAny<MemoModel>()))
                .Returns(new MemoViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetBySalesInvoice(It.IsAny<string>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GetById_WithException_ReturnInternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync(It.IsAny<int>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<MemoViewModel>(It.IsAny<MemoModel>()))
                .Returns(new MemoViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetById(It.IsAny<int>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }
        [Fact]
        public async Task GetbySalesInvoice_WithException_ReturnByInternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoService>();
            serviceMock
                .Setup(service => service.ReadBySalesInvoiceAsync(It.IsAny<string>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<MemoViewModel>(It.IsAny<MemoModel>()))
                .Returns(new MemoViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetBySalesInvoice(It.IsAny<string>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnNotFound()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((MemoModel)null);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<MemoViewModel>(It.IsAny<MemoModel>()))
                .Returns(new MemoViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetById(It.IsAny<int>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task GetbySalesInvoice_WithInvalidId_ReturnNotFound()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoService>();
            serviceMock
                .Setup(service => service.ReadBySalesInvoiceAsync(""))
                .ReturnsAsync((MemoModel)null);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<MemoViewModel>(It.IsAny<MemoModel>()))
                .Returns(new MemoViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetBySalesInvoice("");
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task DeleteById_WithoutException_ReturnNoContent()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IMemoService>();
            serviceMock
                .Setup(service => service.DeleteAsync(It.IsAny<int>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<MemoViewModel>(It.IsAny<MemoModel>()))
                .Returns(new MemoViewModel());
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

            var serviceMock = new Mock<IMemoService>();
            serviceMock
                .Setup(service => service.DeleteAsync(It.IsAny<int>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMemoService))).Returns(serviceMock.Object);

            var validateServiceMock = new Mock<IValidateService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);
            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<MemoViewModel>(It.IsAny<MemoModel>()))
                .Returns(new MemoViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Delete(It.IsAny<int>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }
    }
}
