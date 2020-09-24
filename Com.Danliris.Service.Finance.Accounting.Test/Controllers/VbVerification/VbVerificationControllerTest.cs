using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.VBVerification;
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

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.VbVerification
{
    public class VbVerificationControllerTest
    {

        private VBVerificationController GetController(IServiceProvider serviceProvider)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            var controller = (VBVerificationController)Activator.CreateInstance(typeof(VBVerificationController), serviceProvider);
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

        private Mock<IServiceProvider> GetServiceProvider()
        {
            Mock<IServiceProvider> serviceProviderMock = new Mock<IServiceProvider>();
            var validateServiceMock = new Mock<IValidateService>();

            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IValidateService))).Returns(validateServiceMock.Object);
            var identityServiceMock = new Mock<IIdentityService>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IIdentityService))).Returns(identityServiceMock.Object);

            var mapperMock = new Mock<IMapper>();
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IMapper))).Returns(mapperMock.Object);

            return serviceProviderMock;
        }

        private VbVerificationViewModel ViewModel
        {
            get { return new VbVerificationViewModel(); }
        }

        private VbVerificationList vbVerificationList
        {
            get
            {
                return new VbVerificationList()
                {
                    Amount_Realization=1,
                    Amount_Request =1,
                    Amount_Vat=1,
                    Currency="RP",
                    DateEstimate =DateTimeOffset.Now,
                    DateRealization =DateTimeOffset.Now,
                    DateVB =DateTimeOffset.Now,
                    Diff =1,
                    RequestVbName = "RequestVbName",
                    Status_ReqReal = "RequestVbName",
                    UnitLoad = "UnitLoad",
                    UnitName = "UnitName",
                    Usage = "Usage",
                    VBNo = "VBNo",
                    VBNoRealize= "VBNoRealize",
                    VBRealizeCategory = "VBRealizeCategory",
                    DetailItems =new List<ModelVbItem>()
                    {
                        new ModelVbItem()
                        {
                            Amount=1,
                            DateSPB =DateTimeOffset.Now,
                            NoSPB ="NoSPB",
                            PriceTotalSPB=1,
                            Remark ="Remark",
                            isGetPPn =true,
                            SupplierName ="SupplierName",
                            Total =1,
                            Date =DateTimeOffset.Now
                        }
                    }
                };
            }
        }

        private int GetStatusCode(IActionResult response)
        {
            return (int)response.GetType().GetProperty("StatusCode").GetValue(response, null);
        }

        protected ServiceValidationException GetServiceValidationExeption()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(ViewModel, serviceProvider.Object, null);
            return new ServiceValidationException(validationContext, validationResults);
        }

        [Fact]
        public void Get_WithoutException_ReturnOK()
        {
            var serviceProviderMock = GetServiceProvider();

            var serviceMock = new Mock<IVbVerificationService>();
            serviceMock
                .Setup(service => service.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<VbVerificationList>(new List<VbVerificationList>(), 1, new Dictionary<string, string>(), new List<string>()));
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbVerificationService))).Returns(serviceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = controller.Get(select: new List<string>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void Get_WithException_ReturnInternalServerError()
        {
            var serviceProviderMock = GetServiceProvider();

            var serviceMock = new Mock<IVbWithPORequestService>();
            serviceMock
                .Setup(service => service.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);


            var controller = GetController(serviceProviderMock.Object);

            var response = controller.Get(select: new List<string>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        //===========================
        [Fact]
        public void Get_WithoutException_ReturnOKReadVerification()
        {
            var serviceProviderMock = GetServiceProvider();

            var serviceMock = new Mock<IVbVerificationService>();
            serviceMock
                .Setup(service => service.ReadVerification(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<VbVerificationResultList>(new List<VbVerificationResultList>(), 1, new Dictionary<string, string>(), new List<string>()));
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbVerificationService))).Returns(serviceMock.Object);


            var controller = GetController(serviceProviderMock.Object);

            var response = controller.GetVerification(select: new List<string>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void Get_WithException_ReturnInternalServerErrorReadVerification()
        {
            var serviceProviderMock = GetServiceProvider(); ;

            var serviceMock = new Mock<IVbVerificationService>();
            serviceMock
                .Setup(service => service.ReadVerification(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbVerificationService))).Returns(serviceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = controller.GetVerification(select: new List<string>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Post_WithoutException_ReturnCreated()
        {
            var serviceProviderMock = GetServiceProvider();

            var serviceMock = new Mock<IVbVerificationService>();
            serviceMock
                .Setup(service => service.CreateAsync(It.IsAny<VbVerificationViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbVerificationService))).Returns(serviceMock.Object);


            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new VbVerificationViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public async Task Post_WithValidationException_ReturnBadRequest()
        {
            var serviceProviderMock = GetServiceProvider();

            var serviceMock = new Mock<IVbVerificationService>();
            serviceMock
                .Setup(service => service.CreateAsync(It.IsAny<VbVerificationViewModel>()))
                .Throws(GetServiceValidationExeption());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbVerificationService))).Returns(serviceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new VbVerificationViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Post_WithException_ReturnInternalServerError()
        {
            var serviceProviderMock = GetServiceProvider();

            var serviceMock = new Mock<IVbVerificationService>();
            serviceMock
                .Setup(service => service.CreateAsync(It.IsAny<VbVerificationViewModel>()))
                .ReturnsAsync(1);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbWithPORequestService))).Returns(serviceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.Post(new VbVerificationViewModel());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }



        [Fact]
        public async Task GetById_WithoutException_ReturnOK()
        {
            var serviceProviderMock = GetServiceProvider();

            var serviceMock = new Mock<IVbVerificationService>();
            serviceMock
                .Setup(service => service.ReadById(It.IsAny<int>()))
                .ReturnsAsync(new VbVerificationViewModel());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbVerificationService))).Returns(serviceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetById(It.IsAny<int>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }


        [Fact]
        public void GetToVerified_Return_OK()
        {
            //Setup
            var serviceProviderMock = GetServiceProvider();
            var serviceMock = new Mock<IVbVerificationService>();
            serviceMock
                .Setup(service => service.ReadToVerified(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<VbVerificationList>(new List<VbVerificationList>() { vbVerificationList }, 1, new Dictionary<string, string>(), new List<string>()));
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbVerificationService))).Returns(serviceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            //Act
            var response =  controller.GetToVerified(1,25,"{}",new List<string>() { "UnitName" },"","{}");
            var statusCode = GetStatusCode(response);

            //Assert
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetToVerified_Return_InternalServerError()
        {
            //Setup
            var serviceProviderMock = GetServiceProvider();
            var serviceMock = new Mock<IVbVerificationService>();
            serviceMock
                .Setup(service => service.ReadToVerified(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbVerificationService))).Returns(serviceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            //Act
            var response = controller.GetToVerified();
            var statusCode = GetStatusCode(response);

            //Assert
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetById_WithException_ReturnInternalServerError()
        {
            var serviceProviderMock = GetServiceProvider();

            var serviceMock = new Mock<IVbVerificationService>();
            serviceMock
                .Setup(service => service.ReadById(It.IsAny<int>()))
                .Throws(new Exception());
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbVerificationService))).Returns(serviceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetById(It.IsAny<int>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetById_WithInvalidId_ReturnNotFound()
        {
            var serviceProviderMock = GetServiceProvider();

            var serviceMock = new Mock<IVbVerificationService>();
            serviceMock
                .Setup(service => service.ReadById(It.IsAny<int>()))
                .ReturnsAsync((VbVerificationViewModel)null);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IVbVerificationService))).Returns(serviceMock.Object);

            var controller = GetController(serviceProviderMock.Object);

            var response = await controller.GetById(It.IsAny<int>());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }
    }
}
