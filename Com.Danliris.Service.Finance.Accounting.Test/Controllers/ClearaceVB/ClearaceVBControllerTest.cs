using AutoMapper;
using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.ClearaceVB;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.ValidateService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.ClearaceVB;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.ClearaceVB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.ClearaceVB
{
    public class ClearaceVBControllerTest
    {
        //protected DbSet<VbRequestModel> _RequestDbSet;
        //protected DbSet<RealizationVbModel> _RealizationDbSet;
        //private FinanceDbContext dbContext2;

        //public ClearaceVBControllerTest()
        //{
        //    _RequestDbSet = dbContext2.Set<VbRequestModel>();
        //    _RealizationDbSet = dbContext2.Set<RealizationVbModel>();
        //}

        protected VBRequestDocumentModel Model
        {
            get { return new VBRequestDocumentModel(); }
        }
        protected List<ClearaceVBViewModel> ViewModels2
        {
            get { return new List<ClearaceVBViewModel>(); }
        }
        protected List<ClearaceVBViewModel> ViewModels
        {
            get
            {
                return new List<ClearaceVBViewModel>() {
                new ClearaceVBViewModel()
                {
                    Id = 1,
                    RqstNo = "VBNo",
                    VBCategory = Lib.BusinessLogic.VBRequestDocument.VBType.WithPO,
                    RqstDate = DateTimeOffset.Now,
                    Unit = new Unit()
                    {
                        Id = 1,
                        Name = "UnitName",
                    },
                    Appliciant = "CreatedBy",
                    RealNo = "VBNoRealize",
                    RealDate = DateTimeOffset.Now,
                    VerDate = DateTimeOffset.Now,
                    DiffStatus = "StatusReqReal",
                    DiffAmount = 100,
                    ClearanceDate = DateTimeOffset.Now,
                    IsPosted = true,
                    Status = "Completed",
                    LastModifiedUtc = DateTime.Now,
                }
            };
            }
        }

        public (Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IClearaceVBService> Service, Mock<IMapper> Mapper) GetMocks()
        {
            return (IdentityService: new Mock<IIdentityService>(), ValidateService: new Mock<IValidateService>(), Service: new Mock<IClearaceVBService>(), Mapper: new Mock<IMapper>());
        }

        protected ClearaceVBController GetController((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IClearaceVBService> Service, Mock<IMapper> Mapper) mocks)
        {
            var user = new Mock<ClaimsPrincipal>();
            var claims = new Claim[]
            {
                    new Claim("username", "unittestusername")
            };
            user.Setup(u => u.Claims).Returns(claims);
            ClearaceVBController controller = new ClearaceVBController(mocks.IdentityService.Object, mocks.ValidateService.Object, mocks.Mapper.Object, mocks.Service.Object);
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

        private ClearaceVBViewModel ViewModel
        {
            get
            {
                return new ClearaceVBViewModel()
                {
                    Id = 1,
                    RqstNo = "VBNo",
                    VBCategory = Lib.BusinessLogic.VBRequestDocument.VBType.WithPO,
                    RqstDate = DateTimeOffset.Now,
                    Unit = new Unit()
                    {
                        Id = 1,
                        Name = "UnitName",
                    },
                    Appliciant = "CreatedBy",
                    RealNo = "VBNoRealize",
                    RealDate = DateTimeOffset.Now,
                    VerDate = DateTimeOffset.Now,
                    DiffStatus = "StatusReqReal",
                    DiffAmount = 100,
                    ClearanceDate = DateTimeOffset.Now,
                    IsPosted = true,
                    Status = "Completed",
                    LastModifiedUtc = DateTime.Now,
                };
            }
        }

        private int GetStatusCodeGet((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IClearaceVBService> Service, Mock<IMapper> Mapper) mocks)
        {
            ClearaceVBController controller = GetController(mocks);
            IActionResult response = controller.Get();

            return GetStatusCode(response);
        }

        //[Fact]
        //public void Get_Ovveride_Order_Filter_Search_ReturnOK()
        //{
        //    var mocks = GetMocks();

        //    mocks.Service
        //        //.Setup(f => f.Read(1, It.IsAny<int>(), "%7B%7D", It.IsAny<List<string>>(), "vb", "%7B%22Status%22%3A%22Uncompleted%22%7D"))
        //        .Setup(f => f.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
        //        .Returns(new ReadResponse<ClearaceVBViewModel>(new List<ClearaceVBViewModel>() { new ClearaceVBViewModel() }, 0, new Dictionary<string, string>(), new List<string>()));

        //    mocks.Mapper
        //        .Setup(f => f.Map<List<ClearaceVBViewModel>>(It.IsAny<List<ClearaceVBViewModel>>()))
        //        .Returns(new List<ClearaceVBViewModel>() { new ClearaceVBViewModel() });

        //    int statusCode = GetStatusCodeGet(mocks);
        //    Assert.Equal((int)HttpStatusCode.OK, statusCode);
        //}

        [Fact]
        public void Get_WithoutException_ReturnOK()
        {
            var mocks = GetMocks();

            mocks.Service
                .Setup(f => f.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(new ReadResponse<ClearaceVBViewModel>(new List<ClearaceVBViewModel>() { new ClearaceVBViewModel() }, 0, new Dictionary<string, string>(), new List<string>()));

            mocks.Mapper
                .Setup(f => f.Map<List<ClearaceVBViewModel>>(It.IsAny<List<ClearaceVBViewModel>>()))
                .Returns(new List<ClearaceVBViewModel>() { new ClearaceVBViewModel() });

            int statusCode = GetStatusCodeGet(mocks);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void Get_ReadThrowException_ReturnInternalServerError()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.Read(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception());

            int statusCode = GetStatusCodeGet(mocks);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        private async Task<int> GetStatusCodeGetById((Mock<IIdentityService> IdentityService, Mock<IValidateService> ValidateService, Mock<IClearaceVBService> Service, Mock<IMapper> Mapper) mocks)
        {
            ClearaceVBController controller = GetController(mocks);
            IActionResult response = await controller.GetById(1);

            return GetStatusCode(response);
        }

        [Fact]
        public async Task GetById_NotNullModel_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.ReadByIdAsync(1)).ReturnsAsync(Model);

            int statusCode = await GetStatusCodeGetById(mocks);
            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public async Task GetById_NullModel_ReturnNotFound()
        {
            var mocks = GetMocks();
            mocks.Mapper.Setup(f => f.Map<ClearaceVBViewModel>(It.IsAny<VbRequestModel>())).Returns(ViewModel);
            mocks.Service.Setup(f => f.ReadByIdAsync(1)).ReturnsAsync((VBRequestDocumentModel)null);

            int statusCode = await GetStatusCodeGetById(mocks);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);
        }

        [Fact]
        public async Task GetById_ThrowException_ReturnInternalServerError()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.ReadByIdAsync(1)).ThrowsAsync(new Exception());

            int statusCode = await GetStatusCodeGetById(mocks);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public async Task Should_Success_ClearanceVBPost()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<ClearaceVBViewModel>())).Verifiable();
            var id = 1;
            var viewModel = new ClearaceVBViewModel()
            {
                Id = id
            };
            mocks.Mapper.Setup(m => m.Map<ClearaceVBViewModel>(It.IsAny<VbRequestModel>())).Returns(viewModel);
            mocks.Service.Setup(f => f.ClearanceVBPost(It.IsAny<List<ClearencePostId>>())).ReturnsAsync(1);
            var listId = new List<ClearencePostId> { new ClearencePostId() { VBRequestId = (int)viewModel.Id } };

            var controller = GetController(mocks);
            var response = await controller.ClearanceVBPost(listId);
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public async Task Should_Fail_ClearanceVBPost()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<ClearaceVBViewModel>())).Verifiable();
            var id = 1;
            var viewModel = new ClearaceVBViewModel()
            {
                Id = id
            };
            mocks.Mapper.Setup(m => m.Map<ClearaceVBViewModel>(It.IsAny<VbRequestModel>())).Returns(viewModel);
            mocks.Service.Setup(f => f.ClearanceVBPost(It.IsAny<List<ClearencePostId>>())).ThrowsAsync(new Exception());
            var listId = new List<ClearencePostId> { new ClearencePostId() { VBRequestId = (int)viewModel.Id } };

            var controller = GetController(mocks);
            var response = await controller.ClearanceVBPost(listId);
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }

        [Fact]
        public async Task Should_Success_ClearanceVBUnpost()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<ClearaceVBViewModel>())).Verifiable();
            var id = 1;
            var viewModel = new ClearaceVBViewModel()
            {
                Id = id
            };
            mocks.Mapper.Setup(m => m.Map<ClearaceVBViewModel>(It.IsAny<VbRequestModel>())).Returns(viewModel);
            mocks.Service.Setup(f => f.ClearanceVBUnpost(It.IsAny<long>())).ReturnsAsync(1);
            List<long> listId = new List<long> { viewModel.Id };

            var controller = GetController(mocks);
            var response = await controller.ClearanceVBUnpost(id);
            Assert.Equal((int)HttpStatusCode.OK, GetStatusCode(response));
        }

        [Fact]
        public async Task Should_Fail_ClearanceVBUnpost()
        {
            var mocks = GetMocks();
            mocks.ValidateService.Setup(vs => vs.Validate(It.IsAny<ClearaceVBViewModel>())).Verifiable();
            var id = 1;
            var viewModel = new ClearaceVBViewModel()
            {
                Id = id
            };
            mocks.Mapper.Setup(m => m.Map<ClearaceVBViewModel>(It.IsAny<VbRequestModel>())).Returns(viewModel);
            mocks.Service.Setup(f => f.ClearanceVBUnpost(It.IsAny<long>())).ThrowsAsync(new Exception());
            List<long> listId = new List<long> { viewModel.Id };

            var controller = GetController(mocks);
            var response = await controller.ClearanceVBUnpost(id);
            Assert.Equal((int)HttpStatusCode.InternalServerError, GetStatusCode(response));
        }
    }
}
