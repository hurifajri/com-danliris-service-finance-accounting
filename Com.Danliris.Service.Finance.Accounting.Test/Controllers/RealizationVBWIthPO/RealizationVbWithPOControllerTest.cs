using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBWIthPO;
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
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.RealizationVBWIthPO
{
    public class RealizationVbWithPOControllerTest
    {

        protected RealizationVbWithPOController GetController(Mock<IServiceProvider> serviceProvider)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                    new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);

            serviceProvider
              .Setup(s => s.GetService(typeof(IIdentityService)))
              .Returns(new IdentityService() { TimezoneOffset = 1, Token = "token", Username = "username" });

            var validateService = new Mock<IValidateService>();
            serviceProvider
              .Setup(s => s.GetService(typeof(IValidateService)))
              .Returns(validateService.Object);

            Mock<IMapper> mapper = new Mock<IMapper>();
            serviceProvider
             .Setup(s => s.GetService(typeof(IMapper)))
             .Returns(mapper.Object);

            RealizationVbWithPOController controller = new RealizationVbWithPOController(serviceProvider.Object);
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

        protected ServiceValidationException GetServiceValidationException()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var validationResults = new List<ValidationResult>();
            System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(realizationVbWithPOViewModel, serviceProvider.Object, null);
            return new ServiceValidationException(validationContext, validationResults);
        }

        public RealizationVbWithPOViewModel realizationVbWithPOViewModel
        {
            get
            {
                return new RealizationVbWithPOViewModel()
                {
                    Id = 1,
                    Date = DateTimeOffset.Now,
                    numberVB = new DetailVB()
                    {
                        CreateBy = "CreateBy",
                        DateEstimate = DateTimeOffset.Now,
                        UnitCode = "UnitCode",
                        UnitId = 1,
                        VBNo = "VBNo",
                        UnitName = "UnitName",
                        PONo = new List<PODetail>()
                        {
                            new PODetail()
                            {
                                PONo="PONo",
                                Price =1
                            }
                        }
                    },
                    Items = new List<DetailSPB>()
                    {
                        new DetailSPB()
                        {
                            date=DateTimeOffset.Now,
                            division ="division",
                            IsSave =true,
                            no ="no",
                            item=new List<DetailItemSPB>()
                            {
                                new DetailItemSPB()
                                {
                                    IsDeleted =false,
                                    unitReceiptNote =new DetailunitReceiptNote()
                                    {
                                        no="no",
                                        items =new List<DetailitemunitReceiptNote>()
                                        {
                                            new DetailitemunitReceiptNote()
                                            {
                                                PriceTotal=1,
                                                Product=new Product_VB()
                                                {
                                                    code="code",
                                                    name="name"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                };
            }
        }



        [Fact]
        public async Task RealizationVbWithPORequestPDF_Return_NotFound()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var RealizationVbWithPOMock = new Mock<IRealizationVbWithPOService>();
            RealizationVbWithPOMock.Setup(s => s.ReadByIdAsync2(It.IsAny<int>())).ReturnsAsync(() => null);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService)))
               .Returns(RealizationVbWithPOMock.Object);

            IActionResult response = await GetController(serviceProviderMock).RealizationVbWithPORequestPDF(1);
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task RealizationVbWithPORequestPDF_Return_InternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var RealizationVbWithPOMock = new Mock<IRealizationVbWithPOService>();
            RealizationVbWithPOMock.Setup(s => s.ReadByIdAsync2(It.IsAny<int>())).Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService)))
               .Returns(RealizationVbWithPOMock.Object);

            IActionResult response = await GetController(serviceProviderMock).RealizationVbWithPORequestPDF(1);
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_IDRAsync()
        {
            var vm = new RealizationVbWithPOViewModel()
            {
                Id = 1,
                Date = DateTimeOffset.Now,
                numberVB = new DetailVB()
                {
                    CreateBy = "CreateBy",
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    VBNo = "VBNo",
                    UnitName = "UnitName",
                    Amount = 0,
                    PONo = new List<PODetail>()
                        {
                            new PODetail()
                            {
                                PONo="PONo",
                                Price =1
                            }
                        }
                },
                Items = new List<DetailSPB>()
                    {
                        new DetailSPB()
                        {
                            date=DateTimeOffset.Now,
                            division ="division",
                            IsSave =true,
                            no ="no",
                            supplier = new SupplierViewModel()
                            {
                                _id = "id",
                                code = "code",
                                name = "name",
                            },
                            currency = new CurrencyViewModel()
                            {
                                _id = "id",
                                code = "IDR",
                                rate = 123
                            },
                            item=new List<DetailItemSPB>()
                            {
                                new DetailItemSPB()
                                {
                                    IsDeleted =false,
                                    unitReceiptNote =new DetailunitReceiptNote()
                                    {
                                        no="no",
                                        items =new List<DetailitemunitReceiptNote>()
                                        {
                                            new DetailitemunitReceiptNote()
                                            {
                                                PriceTotal=1,
                                                Product=new Product_VB()
                                                {
                                                    code="code",
                                                    name="name"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbWithPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService))).Returns(serviceMock.Object);

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

            var controller = GetController(serviceProviderMock);

            var response = await controller.RealizationVbWithPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_Currency_USDAsync()
        {
            var vm = new RealizationVbWithPOViewModel()
            {
                Id = 1,
                Date = DateTimeOffset.Now,
                numberVB = new DetailVB()
                {
                    CreateBy = "CreateBy",
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    VBNo = "VBNo",
                    UnitName = "UnitName",
                    Amount = 123,
                    PONo = new List<PODetail>()
                        {
                            new PODetail()
                            {
                                PONo="PONo",
                                Price =1
                            }
                        }
                },
                Items = new List<DetailSPB>()
                    {
                        new DetailSPB()
                        {
                            date=DateTimeOffset.Now,
                            division ="division",
                            IsSave =true,
                            no ="no",
                            supplier = new SupplierViewModel()
                            {
                                _id = "id",
                                code = "code",
                                name = "name",
                            },
                            currency = new CurrencyViewModel()
                            {
                                _id = "id",
                                code = "USD",
                                rate = 123
                            },
                            item=new List<DetailItemSPB>()
                            {
                                new DetailItemSPB()
                                {
                                    IsDeleted =false,
                                    unitReceiptNote =new DetailunitReceiptNote()
                                    {
                                        no="no",
                                        items =new List<DetailitemunitReceiptNote>()
                                        {
                                            new DetailitemunitReceiptNote()
                                            {
                                                PriceTotal=1,
                                                Product=new Product_VB()
                                                {
                                                    code="code",
                                                    name="name"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbWithPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService))).Returns(serviceMock.Object);

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

            var controller = GetController(serviceProviderMock);

            var response = await controller.RealizationVbWithPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_SPINNING1()
        {
            var vm = new RealizationVbWithPOViewModel()
            {
                Id = 1,
                Date = null,
                numberVB = new DetailVB()
                {
                    CreateBy = "CreateBy",
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    VBNo = "VBNo",
                    UnitName = "UnitName",
                    Amount = 123,
                    PONo = new List<PODetail>()
                        {
                            new PODetail()
                            {
                                PONo="PONo",
                                Price =1
                            }
                        }
                },
                Items = new List<DetailSPB>()
                    {
                        new DetailSPB()
                        {
                            date = null,
                            division ="SPINNING 1",
                            IsSave =true,
                            no ="no",
                            supplier = new SupplierViewModel()
                            {
                                _id = "id",
                                code = "code",
                                name = "name",
                            },
                            currency = new CurrencyViewModel()
                            {
                                _id = "id",
                                code = "USD",
                                rate = 123
                            },
                            item=new List<DetailItemSPB>()
                            {
                                new DetailItemSPB()
                                {
                                    IsDeleted =false,
                                    unitReceiptNote =new DetailunitReceiptNote()
                                    {
                                        no="no",
                                        items =new List<DetailitemunitReceiptNote>()
                                        {
                                            new DetailitemunitReceiptNote()
                                            {
                                                PriceTotal=1,
                                                Product=new Product_VB()
                                                {
                                                    code="code",
                                                    name="name"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbWithPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService))).Returns(serviceMock.Object);

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

            var controller = GetController(serviceProviderMock);

            var response = await controller.RealizationVbWithPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_SPINNING2()
        {
            var vm = new RealizationVbWithPOViewModel()
            {
                Id = 1,
                Date = DateTimeOffset.Now,
                numberVB = new DetailVB()
                {
                    CreateBy = "CreateBy",
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    VBNo = "VBNo",
                    UnitName = "UnitName",
                    Amount = 123,
                    PONo = new List<PODetail>()
                        {
                            new PODetail()
                            {
                                PONo="PONo",
                                Price =1
                            }
                        }
                },
                Items = new List<DetailSPB>()
                    {
                        new DetailSPB()
                        {
                            date=DateTimeOffset.Now,
                            division ="SPINNING 2",
                            IsSave =true,
                            no ="no",
                            supplier = new SupplierViewModel()
                            {
                                _id = "id",
                                code = "code",
                                name = "name",
                            },
                            currency = new CurrencyViewModel()
                            {
                                _id = "id",
                                code = "USD",
                                rate = 123
                            },
                            item=new List<DetailItemSPB>()
                            {
                                new DetailItemSPB()
                                {
                                    IsDeleted =false,
                                    unitReceiptNote =new DetailunitReceiptNote()
                                    {
                                        no="no",
                                        items =new List<DetailitemunitReceiptNote>()
                                        {
                                            new DetailitemunitReceiptNote()
                                            {
                                                PriceTotal=1,
                                                Product=new Product_VB()
                                                {
                                                    code="code",
                                                    name="name"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbWithPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService))).Returns(serviceMock.Object);

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

            var controller = GetController(serviceProviderMock);

            var response = await controller.RealizationVbWithPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_SPINNING3()
        {
            var vm = new RealizationVbWithPOViewModel()
            {
                Id = 1,
                Date = DateTimeOffset.Now,
                numberVB = new DetailVB()
                {
                    CreateBy = "CreateBy",
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    VBNo = "VBNo",
                    UnitName = "UnitName",
                    Amount = 123,
                    PONo = new List<PODetail>()
                        {
                            new PODetail()
                            {
                                PONo="PONo",
                                Price =1
                            }
                        }
                },
                Items = new List<DetailSPB>()
                    {
                        new DetailSPB()
                        {
                            date=DateTimeOffset.Now,
                            division ="SPINNING 3",
                            IsSave =true,
                            no ="no",
                            supplier = new SupplierViewModel()
                            {
                                _id = "id",
                                code = "code",
                                name = "name",
                            },
                            currency = new CurrencyViewModel()
                            {
                                _id = "id",
                                code = "USD",
                                rate = 123
                            },
                            item=new List<DetailItemSPB>()
                            {
                                new DetailItemSPB()
                                {
                                    IsDeleted =false,
                                    unitReceiptNote =new DetailunitReceiptNote()
                                    {
                                        no="no",
                                        items =new List<DetailitemunitReceiptNote>()
                                        {
                                            new DetailitemunitReceiptNote()
                                            {
                                                PriceTotal=1,
                                                Product=new Product_VB()
                                                {
                                                    code="code",
                                                    name="name"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbWithPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService))).Returns(serviceMock.Object);

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

            var controller = GetController(serviceProviderMock);

            var response = await controller.RealizationVbWithPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_WEAVING1()
        {
            var vm = new RealizationVbWithPOViewModel()
            {
                Id = 1,
                Date = DateTimeOffset.Now,
                numberVB = new DetailVB()
                {
                    CreateBy = "CreateBy",
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    VBNo = "VBNo",
                    UnitName = "UnitName",
                    Amount = 123,
                    PONo = new List<PODetail>()
                        {
                            new PODetail()
                            {
                                PONo="PONo",
                                Price =1
                            }
                        }
                },
                Items = new List<DetailSPB>()
                    {
                        new DetailSPB()
                        {
                            date=DateTimeOffset.Now,
                            division ="WEAVING 1",
                            IsSave =true,
                            no ="no",
                            supplier = new SupplierViewModel()
                            {
                                _id = "id",
                                code = "code",
                                name = "name",
                            },
                            currency = new CurrencyViewModel()
                            {
                                _id = "id",
                                code = "USD",
                                rate = 123
                            },
                            item=new List<DetailItemSPB>()
                            {
                                new DetailItemSPB()
                                {
                                    IsDeleted =false,
                                    unitReceiptNote =new DetailunitReceiptNote()
                                    {
                                        no="no",
                                        items =new List<DetailitemunitReceiptNote>()
                                        {
                                            new DetailitemunitReceiptNote()
                                            {
                                                PriceTotal=1,
                                                Product=new Product_VB()
                                                {
                                                    code="code",
                                                    name="name"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbWithPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService))).Returns(serviceMock.Object);

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

            var controller = GetController(serviceProviderMock);

            var response = await controller.RealizationVbWithPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_WEAVING2()
        {
            var vm = new RealizationVbWithPOViewModel()
            {
                Id = 1,
                Date = DateTimeOffset.Now,
                numberVB = new DetailVB()
                {
                    CreateBy = "CreateBy",
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    VBNo = "VBNo",
                    UnitName = "UnitName",
                    Amount = 123,
                    PONo = new List<PODetail>()
                        {
                            new PODetail()
                            {
                                PONo="PONo",
                                Price =1
                            }
                        }
                },
                Items = new List<DetailSPB>()
                    {
                        new DetailSPB()
                        {
                            date=DateTimeOffset.Now,
                            division ="WEAVING 2",
                            IsSave =true,
                            no ="no",
                            supplier = new SupplierViewModel()
                            {
                                _id = "id",
                                code = "code",
                                name = "name",
                            },
                            currency = new CurrencyViewModel()
                            {
                                _id = "id",
                                code = "USD",
                                rate = 123
                            },
                            item=new List<DetailItemSPB>()
                            {
                                new DetailItemSPB()
                                {
                                    IsDeleted =false,
                                    unitReceiptNote =new DetailunitReceiptNote()
                                    {
                                        no="no",
                                        items =new List<DetailitemunitReceiptNote>()
                                        {
                                            new DetailitemunitReceiptNote()
                                            {
                                                PriceTotal=1,
                                                Product=new Product_VB()
                                                {
                                                    code="code",
                                                    name="name"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbWithPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService))).Returns(serviceMock.Object);

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

            var controller = GetController(serviceProviderMock);

            var response = await controller.RealizationVbWithPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_PRINTING()
        {
            var vm = new RealizationVbWithPOViewModel()
            {
                Id = 1,
                Date = DateTimeOffset.Now,
                numberVB = new DetailVB()
                {
                    CreateBy = "CreateBy",
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    VBNo = "VBNo",
                    UnitName = "UnitName",
                    Amount = 123,
                    PONo = new List<PODetail>()
                        {
                            new PODetail()
                            {
                                PONo="PONo",
                                Price =1
                            }
                        }
                },
                Items = new List<DetailSPB>()
                    {
                        new DetailSPB()
                        {
                            date=DateTimeOffset.Now,
                            division ="PRINTING",
                            IsSave =true,
                            no ="no",
                            supplier = new SupplierViewModel()
                            {
                                _id = "id",
                                code = "code",
                                name = "name",
                            },
                            currency = new CurrencyViewModel()
                            {
                                _id = "id",
                                code = "USD",
                                rate = 123
                            },
                            item=new List<DetailItemSPB>()
                            {
                                new DetailItemSPB()
                                {
                                    IsDeleted =false,
                                    unitReceiptNote =new DetailunitReceiptNote()
                                    {
                                        no="no",
                                        items =new List<DetailitemunitReceiptNote>()
                                        {
                                            new DetailitemunitReceiptNote()
                                            {
                                                PriceTotal=1,
                                                Product=new Product_VB()
                                                {
                                                    code="code",
                                                    name="name"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbWithPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService))).Returns(serviceMock.Object);

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

            var controller = GetController(serviceProviderMock);

            var response = await controller.RealizationVbWithPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_DYEING()
        {
            var vm = new RealizationVbWithPOViewModel()
            {
                Id = 1,
                Date = DateTimeOffset.Now,
                numberVB = new DetailVB()
                {
                    CreateBy = "CreateBy",
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    VBNo = "VBNo",
                    UnitName = "UnitName",
                    Amount = 123,
                    PONo = new List<PODetail>()
                        {
                            new PODetail()
                            {
                                PONo="PONo",
                                Price =1
                            }
                        }
                },
                Items = new List<DetailSPB>()
                    {
                        new DetailSPB()
                        {
                            date=DateTimeOffset.Now,
                            division ="DYEING",
                            IsSave =true,
                            no ="no",
                            supplier = new SupplierViewModel()
                            {
                                _id = "id",
                                code = "code",
                                name = "name",
                            },
                            currency = new CurrencyViewModel()
                            {
                                _id = "id",
                                code = "USD",
                                rate = 123
                            },
                            item=new List<DetailItemSPB>()
                            {
                                new DetailItemSPB()
                                {
                                    IsDeleted =false,
                                    unitReceiptNote =new DetailunitReceiptNote()
                                    {
                                        no="no",
                                        items =new List<DetailitemunitReceiptNote>()
                                        {
                                            new DetailitemunitReceiptNote()
                                            {
                                                PriceTotal=1,
                                                Product=new Product_VB()
                                                {
                                                    code="code",
                                                    name="name"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbWithPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService))).Returns(serviceMock.Object);

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

            var controller = GetController(serviceProviderMock);

            var response = await controller.RealizationVbWithPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_KONFEKSI1A()
        {
            var vm = new RealizationVbWithPOViewModel()
            {
                Id = 1,
                Date = DateTimeOffset.Now,
                numberVB = new DetailVB()
                {
                    CreateBy = "CreateBy",
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    VBNo = "VBNo",
                    UnitName = "UnitName",
                    Amount = 123,
                    PONo = new List<PODetail>()
                        {
                            new PODetail()
                            {
                                PONo="PONo",
                                Price =1
                            }
                        }
                },
                Items = new List<DetailSPB>()
                    {
                        new DetailSPB()
                        {
                            date=DateTimeOffset.Now,
                            division ="KONFEKSI 1A",
                            IsSave =true,
                            no ="no",
                            supplier = new SupplierViewModel()
                            {
                                _id = "id",
                                code = "code",
                                name = "name",
                            },
                            currency = new CurrencyViewModel()
                            {
                                _id = "id",
                                code = "USD",
                                rate = 123
                            },
                            item=new List<DetailItemSPB>()
                            {
                                new DetailItemSPB()
                                {
                                    IsDeleted =false,
                                    unitReceiptNote =new DetailunitReceiptNote()
                                    {
                                        no="no",
                                        items =new List<DetailitemunitReceiptNote>()
                                        {
                                            new DetailitemunitReceiptNote()
                                            {
                                                PriceTotal=1,
                                                Product=new Product_VB()
                                                {
                                                    code="code",
                                                    name="name"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbWithPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService))).Returns(serviceMock.Object);

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

            var controller = GetController(serviceProviderMock);

            var response = await controller.RealizationVbWithPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_KONFEKSI1B()
        {
            var vm = new RealizationVbWithPOViewModel()
            {
                Id = 1,
                Date = DateTimeOffset.Now,
                numberVB = new DetailVB()
                {
                    CreateBy = "CreateBy",
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    VBNo = "VBNo",
                    UnitName = "UnitName",
                    Amount = 123,
                    PONo = new List<PODetail>()
                        {
                            new PODetail()
                            {
                                PONo="PONo",
                                Price =1
                            }
                        }
                },
                Items = new List<DetailSPB>()
                    {
                        new DetailSPB()
                        {
                            date=DateTimeOffset.Now,
                            division ="KONFEKSI 1B",
                            IsSave =true,
                            no ="no",
                            supplier = new SupplierViewModel()
                            {
                                _id = "id",
                                code = "code",
                                name = "name",
                            },
                            currency = new CurrencyViewModel()
                            {
                                _id = "id",
                                code = "USD",
                                rate = 123
                            },
                            item=new List<DetailItemSPB>()
                            {
                                new DetailItemSPB()
                                {
                                    IsDeleted =false,
                                    unitReceiptNote =new DetailunitReceiptNote()
                                    {
                                        no="no",
                                        items =new List<DetailitemunitReceiptNote>()
                                        {
                                            new DetailitemunitReceiptNote()
                                            {
                                                PriceTotal=1,
                                                Product=new Product_VB()
                                                {
                                                    code="code",
                                                    name="name"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbWithPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService))).Returns(serviceMock.Object);

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

            var controller = GetController(serviceProviderMock);

            var response = await controller.RealizationVbWithPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_KONFEKSI2A()
        {
            var vm = new RealizationVbWithPOViewModel()
            {
                Id = 1,
                Date = DateTimeOffset.Now,
                numberVB = new DetailVB()
                {
                    CreateBy = "CreateBy",
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    VBNo = "VBNo",
                    UnitName = "UnitName",
                    Amount = 123,
                    PONo = new List<PODetail>()
                        {
                            new PODetail()
                            {
                                PONo="PONo",
                                Price =1
                            }
                        }
                },
                Items = new List<DetailSPB>()
                    {
                        new DetailSPB()
                        {
                            date=DateTimeOffset.Now,
                            division ="KONFEKSI 2A",
                            IsSave =true,
                            no ="no",
                            supplier = new SupplierViewModel()
                            {
                                _id = "id",
                                code = "code",
                                name = "name",
                            },
                            currency = new CurrencyViewModel()
                            {
                                _id = "id",
                                code = "USD",
                                rate = 123
                            },
                            item=new List<DetailItemSPB>()
                            {
                                new DetailItemSPB()
                                {
                                    IsDeleted =false,
                                    unitReceiptNote =new DetailunitReceiptNote()
                                    {
                                        no="no",
                                        items =new List<DetailitemunitReceiptNote>()
                                        {
                                            new DetailitemunitReceiptNote()
                                            {
                                                PriceTotal=1,
                                                Product=new Product_VB()
                                                {
                                                    code="code",
                                                    name="name"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbWithPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService))).Returns(serviceMock.Object);

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

            var controller = GetController(serviceProviderMock);

            var response = await controller.RealizationVbWithPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_KONFEKSI2B()
        {
            var vm = new RealizationVbWithPOViewModel()
            {
                Id = 1,
                Date = DateTimeOffset.Now,
                numberVB = new DetailVB()
                {
                    CreateBy = "CreateBy",
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    VBNo = "VBNo",
                    UnitName = "UnitName",
                    Amount = 123,
                    PONo = new List<PODetail>()
                        {
                            new PODetail()
                            {
                                PONo="PONo",
                                Price =1
                            }
                        }
                },
                Items = new List<DetailSPB>()
                    {
                        new DetailSPB()
                        {
                            date=DateTimeOffset.Now,
                            division ="KONFEKSI 2B",
                            IsSave =true,
                            no ="no",
                            supplier = new SupplierViewModel()
                            {
                                _id = "id",
                                code = "code",
                                name = "name",
                            },
                            currency = new CurrencyViewModel()
                            {
                                _id = "id",
                                code = "USD",
                                rate = 123
                            },
                            item=new List<DetailItemSPB>()
                            {
                                new DetailItemSPB()
                                {
                                    IsDeleted =false,
                                    unitReceiptNote =new DetailunitReceiptNote()
                                    {
                                        no="no",
                                        items =new List<DetailitemunitReceiptNote>()
                                        {
                                            new DetailitemunitReceiptNote()
                                            {
                                                PriceTotal=1,
                                                Product=new Product_VB()
                                                {
                                                    code="code",
                                                    name="name"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbWithPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService))).Returns(serviceMock.Object);

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

            var controller = GetController(serviceProviderMock);

            var response = await controller.RealizationVbWithPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_KONFEKSI2C()
        {
            var vm = new RealizationVbWithPOViewModel()
            {
                Id = 1,
                Date = DateTimeOffset.Now,
                numberVB = new DetailVB()
                {
                    CreateBy = "CreateBy",
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    VBNo = "VBNo",
                    UnitName = "UnitName",
                    Amount = 123,
                    PONo = new List<PODetail>()
                        {
                            new PODetail()
                            {
                                PONo="PONo",
                                Price =1
                            }
                        }
                },
                Items = new List<DetailSPB>()
                    {
                        new DetailSPB()
                        {
                            date=DateTimeOffset.Now,
                            division ="KONFEKSI 2C",
                            IsSave =true,
                            no ="no",
                            supplier = new SupplierViewModel()
                            {
                                _id = "id",
                                code = "code",
                                name = "name",
                            },
                            currency = new CurrencyViewModel()
                            {
                                _id = "id",
                                code = "USD",
                                rate = 123
                            },
                            item=new List<DetailItemSPB>()
                            {
                                new DetailItemSPB()
                                {
                                    IsDeleted =false,
                                    unitReceiptNote =new DetailunitReceiptNote()
                                    {
                                        no="no",
                                        items =new List<DetailitemunitReceiptNote>()
                                        {
                                            new DetailitemunitReceiptNote()
                                            {
                                                PriceTotal=1,
                                                Product=new Product_VB()
                                                {
                                                    code="code",
                                                    name="name"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbWithPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService))).Returns(serviceMock.Object);

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

            var controller = GetController(serviceProviderMock);

            var response = await controller.RealizationVbWithPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public async Task Get_Sales_Receipt_PDF_Success_UMUM()
        {
            var vm = new RealizationVbWithPOViewModel()
            {
                Id = 1,
                Date = DateTimeOffset.Now,
                numberVB = new DetailVB()
                {
                    CreateBy = "CreateBy",
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    VBNo = "VBNo",
                    UnitName = "UnitName",
                    Amount = 123,
                    PONo = new List<PODetail>()
                        {
                            new PODetail()
                            {
                                PONo="PONo",
                                Price =1
                            }
                        }
                },
                Items = new List<DetailSPB>()
                    {
                        new DetailSPB()
                        {
                            date=DateTimeOffset.Now,
                            division ="UMUM",
                            IsSave =true,
                            no ="no",
                            supplier = new SupplierViewModel()
                            {
                                _id = "id",
                                code = "code",
                                name = "name",
                            },
                            currency = new CurrencyViewModel()
                            {
                                _id = "id",
                                code = "USD",
                                rate = 123
                            },
                            item=new List<DetailItemSPB>()
                            {
                                new DetailItemSPB()
                                {
                                    IsDeleted =false,
                                    unitReceiptNote =new DetailunitReceiptNote()
                                    {
                                        no="no",
                                        items =new List<DetailitemunitReceiptNote>()
                                        {
                                            new DetailitemunitReceiptNote()
                                            {
                                                PriceTotal=1,
                                                Product=new Product_VB()
                                                {
                                                    code="code",
                                                    name="name"
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

            };

            var serviceProviderMock = new Mock<IServiceProvider>();

            var serviceMock = new Mock<IRealizationVbWithPOService>();
            serviceMock
                .Setup(service => service.ReadByIdAsync2(It.IsAny<int>()))
                .ReturnsAsync(vm);
            serviceProviderMock
                .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService))).Returns(serviceMock.Object);

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

            var controller = GetController(serviceProviderMock);

            var response = await controller.RealizationVbWithPORequestPDF(It.IsAny<int>());
            //var statusCode = GetStatusCode(response);

            Assert.NotNull(response);
        }

        [Fact]
        public void Get_Return_OK()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var RealizationVbWithPOMock = new Mock<IRealizationVbWithPOService>();
            Dictionary<string, string> order = new Dictionary<string, string>();
            order.Add("CreatedBy", "desc");

            var queryResult = new ReadResponse<RealizationVbList>(new List<RealizationVbList>(), 1, order, new List<string>() { "CreatedBy", "CreatedBy" });

            RealizationVbWithPOMock.Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>())).Returns(queryResult);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService)))
               .Returns(RealizationVbWithPOMock.Object);

            IActionResult response = GetController(serviceProviderMock).Get(1, 25, "{}", new List<string>() { "CreatedBy" }, "", "{}");
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
            //OK
        }


        [Fact]
        public void Get_Return_InternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var RealizationVbWithPOMock = new Mock<IRealizationVbWithPOService>();

            RealizationVbWithPOMock.Setup(s => s.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService)))
               .Returns(RealizationVbWithPOMock.Object);

            IActionResult response = GetController(serviceProviderMock).Get(1, 25, "{}", new List<string>() { "RequestVbName" }, "", "{}");
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }


        [Fact]
        public async Task Post_Return_OK()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var RealizationVbWithPOMock = new Mock<IRealizationVbWithPOService>();

            RealizationVbWithPOMock.Setup(s => s.CreateAsync(It.IsAny<RealizationVbModel>(), It.IsAny<RealizationVbWithPOViewModel>())).ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService)))
               .Returns(RealizationVbWithPOMock.Object);

            IActionResult response = await GetController(serviceProviderMock).Post(realizationVbWithPOViewModel);
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.Created, statusCode);
        }

        [Fact]
        public async Task Post_Return_BadRequest()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var RealizationVbWithPOMock = new Mock<IRealizationVbWithPOService>();

            RealizationVbWithPOMock.Setup(s => s.CreateAsync(It.IsAny<RealizationVbModel>(), It.IsAny<RealizationVbWithPOViewModel>())).ThrowsAsync(GetServiceValidationException());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService)))
               .Returns(RealizationVbWithPOMock.Object);

            IActionResult response = await GetController(serviceProviderMock).Post(realizationVbWithPOViewModel);
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Post_Return_InternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var RealizationVbWithPOMock = new Mock<IRealizationVbWithPOService>();

            RealizationVbWithPOMock.Setup(s => s.CreateAsync(It.IsAny<RealizationVbModel>(), It.IsAny<RealizationVbWithPOViewModel>())).ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService)))
               .Returns(RealizationVbWithPOMock.Object);

            IActionResult response = await GetController(serviceProviderMock).Post(realizationVbWithPOViewModel);
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetById_Return_InternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var RealizationVbWithPOMock = new Mock<IRealizationVbWithPOService>();

            RealizationVbWithPOMock.Setup(s => s.ReadByIdAsync2(It.IsAny<int>())).ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService)))
               .Returns(RealizationVbWithPOMock.Object);

            IActionResult response = await GetController(serviceProviderMock).GetById(1);
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task GetById_Return_NotFound()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var RealizationVbWithPOMock = new Mock<IRealizationVbWithPOService>();

            RealizationVbWithPOMock.Setup(s => s.ReadByIdAsync2(It.IsAny<int>())).ReturnsAsync(() => null);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService)))
               .Returns(RealizationVbWithPOMock.Object);

            IActionResult response = await GetController(serviceProviderMock).GetById(1);
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }


        [Fact]
        public async Task GetById_Return_OK()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var RealizationVbWithPOMock = new Mock<IRealizationVbWithPOService>();

            RealizationVbWithPOMock.Setup(s => s.ReadByIdAsync2(It.IsAny<int>())).ReturnsAsync(realizationVbWithPOViewModel);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService)))
               .Returns(RealizationVbWithPOMock.Object);

            IActionResult response = await GetController(serviceProviderMock).GetById(1);
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task Put_Return_OK()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var RealizationVbWithPOMock = new Mock<IRealizationVbWithPOService>();

            RealizationVbWithPOMock.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<RealizationVbWithPOViewModel>())).ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService)))
               .Returns(RealizationVbWithPOMock.Object);

            IActionResult response = await GetController(serviceProviderMock).Put(1, realizationVbWithPOViewModel);
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }

        [Fact]
        public async Task Put_Return_BadRequest()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var RealizationVbWithPOMock = new Mock<IRealizationVbWithPOService>();

            RealizationVbWithPOMock.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<RealizationVbWithPOViewModel>())).ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService)))
               .Returns(RealizationVbWithPOMock.Object);


            IActionResult response = await GetController(serviceProviderMock).Put(0, realizationVbWithPOViewModel);
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Put_Throws_ServiceValidationException()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var RealizationVbWithPOMock = new Mock<IRealizationVbWithPOService>();

            RealizationVbWithPOMock.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<RealizationVbWithPOViewModel>())).ThrowsAsync(GetServiceValidationException());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService)))
               .Returns(RealizationVbWithPOMock.Object);


            IActionResult response = await GetController(serviceProviderMock).Put(1, realizationVbWithPOViewModel);
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.BadRequest, statusCode);
        }

        [Fact]
        public async Task Put_Return_InternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var RealizationVbWithPOMock = new Mock<IRealizationVbWithPOService>();

            RealizationVbWithPOMock.Setup(s => s.UpdateAsync(It.IsAny<int>(), It.IsAny<RealizationVbWithPOViewModel>())).ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService)))
               .Returns(RealizationVbWithPOMock.Object);


            IActionResult response = await GetController(serviceProviderMock).Put(1, realizationVbWithPOViewModel);
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Delete_Return_InternalServerError()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var RealizationVbWithPOMock = new Mock<IRealizationVbWithPOService>();

            RealizationVbWithPOMock.Setup(s => s.DeleteAsync(It.IsAny<int>())).ThrowsAsync(new Exception());

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService)))
               .Returns(RealizationVbWithPOMock.Object);


            IActionResult response = await GetController(serviceProviderMock).Delete(1);
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Delete_Succees_Return_NoContent()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();

            var RealizationVbWithPOMock = new Mock<IRealizationVbWithPOService>();

            RealizationVbWithPOMock.Setup(s => s.DeleteAsync(It.IsAny<int>())).ReturnsAsync(1);

            serviceProviderMock
               .Setup(serviceProvider => serviceProvider.GetService(typeof(IRealizationVbWithPOService)))
               .Returns(RealizationVbWithPOMock.Object);


            IActionResult response = await GetController(serviceProviderMock).Delete(1);
            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NoContent, statusCode);
        }
    }
}
