using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.SalesReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.SalesReceipt;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.NewIntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.SalesReceipt;
using Com.Danliris.Service.Finance.Accounting.Test.Controller.Utils;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.SalesReceipt;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Controllers.SalesReceipt
{
    public class SalesReceiptControllerTest : BaseControllerTest<SalesReceiptController, SalesReceiptModel, SalesReceiptViewModel, ISalesReceiptService>
    {
        [Fact]
        public void Get_Sales_Receipt_PDF_Success_Currency_IDR()
        {
            var vm = new SalesReceiptViewModel()
            {
                SalesReceiptDate = DateTimeOffset.Now,
                Unit = new NewUnitViewModel()
                {
                    Name = "Name",
                },
                Buyer = new NewBuyerViewModel()
                {
                    Name = "Name",
                    Address = "Address",
                },
                Currency = new CurrencyViewModel()
                {
                    Code = "IDR",
                    Symbol = "Rp",
                    Rate = 1,
                },
                Bank = new AccountBankViewModel()
                {
                    BankName = "BCA",
                },
                SalesReceiptDetails = new List<SalesReceiptDetailViewModel>()
                {
                    new SalesReceiptDetailViewModel()
                    {
                        VatType = "PPN BUMN",
                        SalesInvoice = new SalesInvoiceViewModel()
                        {
                            SalesInvoiceNo = "no",
                            Currency = new CurrencyViewModel()
                            {
                                Code = "IDR",
                                Symbol = "Rp",
                                Rate = 1,
                            },
                        },
                    }
                }

            };
            var mocks = GetMocks();
            mocks.Service.Setup(x => x.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(Model);
            mocks.Mapper.Setup(s => s.Map<SalesReceiptViewModel>(It.IsAny<SalesReceiptModel>()))
                .Returns(vm);
            var controller = GetController(mocks);
            var response = controller.GetSalesReceiptPDF(1).Result;

            Assert.NotNull(response);
        }

        [Fact]
        public void Get_Sales_Receipt_PDF_Success_Currency_Not_IDR()
        {
            var vm = new SalesReceiptViewModel()
            {
                SalesReceiptDate = DateTimeOffset.Now,
                Unit = new NewUnitViewModel()
                {
                    Name = "Name",
                },
                Buyer = new NewBuyerViewModel()
                {
                    Name = "Name",
                    Address = "Address",
                },
                Currency = new CurrencyViewModel()
                {
                    Code = "USD",
                    Symbol = "$",
                    Rate = 14447,
                },
                Bank = new AccountBankViewModel()
                {
                    BankName = "BCA",
                },
                SalesReceiptDetails = new List<SalesReceiptDetailViewModel>()
                {
                    new SalesReceiptDetailViewModel()
                    {
                        VatType = "PPN BUMN",
                        SalesInvoice = new SalesInvoiceViewModel()
                        {
                            SalesInvoiceNo = "no",
                            Currency = new CurrencyViewModel()
                            {
                                Code = "USD",
                                Symbol = "$",
                                Rate = 14447,
                            },
                        },
                    }
                }

            };
            var mocks = GetMocks();
            mocks.Service.Setup(x => x.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(Model);
            mocks.Mapper.Setup(s => s.Map<SalesReceiptViewModel>(It.IsAny<SalesReceiptModel>()))
                .Returns(vm);
            var controller = GetController(mocks);
            var response = controller.GetSalesReceiptPDF(1).Result;

            Assert.NotNull(response);
        }

        [Fact]
        public void Get_Sales_Receipt_PDF_NotFound()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(x => x.ReadByIdAsync(It.IsAny<int>())).ReturnsAsync(default(SalesReceiptModel));
            var controller = GetController(mocks);
            var response = controller.GetSalesReceiptPDF(1).Result;

            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.NotFound, statusCode);

        }

        [Fact]
        public void Get_Sales_Receipt_PDF_Exception()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(x => x.ReadByIdAsync(It.IsAny<int>())).ThrowsAsync(new Exception("error"));
            var controller = GetController(mocks);
            var response = controller.GetSalesReceiptPDF(1).Result;

            int statusCode = this.GetStatusCode(response);
            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);

        }

        [Fact]
        public void GetSalesInvoiceData_WithoutException_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetSalesInvoice(It.IsAny<SalesInvoicePostForm>())).Returns(new List<SalesInvoiceReportSalesReceiptViewModel>());
            
            var controller = GetController(mocks);
            var response = controller.GetSalesInvoiceData(new SalesInvoicePostForm());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetSalesInvoiceData_WithException_InternalServerError()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetSalesInvoice(It.IsAny<SalesInvoicePostForm>())).Throws(new Exception());

            var controller = GetController(mocks);
            var response = controller.GetSalesInvoiceData(new SalesInvoicePostForm());
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetReportAll_WithoutException_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReport(It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>()))
                .Returns(new List<SalesReceiptReportViewModel>());

            var controller = GetController(mocks);
            var response = controller.GetReportAll(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, "7");
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.OK, statusCode);
        }

        [Fact]
        public void GetReportAll_WithException_InternalServerError()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GetReport(It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>()))
                .Throws(new Exception());

            var controller = GetController(mocks);
            var response = controller.GetReportAll(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, "7");
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }

        [Fact]
        public void GetXlsAll_WithoutException_ReturnOK()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcel(It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>()))
                .Returns(new System.IO.MemoryStream());

            var controller = GetController(mocks);
            var response = controller.GetXlsAll(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, "7");
            Assert.NotNull(response);
        }

        [Fact]
        public void GetXlsAll_WithException_InternalServerError()
        {
            var mocks = GetMocks();
            mocks.Service.Setup(f => f.GenerateExcel(It.IsAny<DateTimeOffset?>(), It.IsAny<DateTimeOffset?>(), It.IsAny<int>()))
               .Throws(new Exception());

            var controller = GetController(mocks);
            var response = controller.GetXlsAll(DateTimeOffset.UtcNow, DateTimeOffset.UtcNow, "7");
            var statusCode = GetStatusCode(response);

            Assert.Equal((int)HttpStatusCode.InternalServerError, statusCode);
        }
    }
}
