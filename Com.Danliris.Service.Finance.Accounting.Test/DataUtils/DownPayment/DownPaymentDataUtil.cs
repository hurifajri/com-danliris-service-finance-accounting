using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DownPayment;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DownPayment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.DownPayment
{
    public class DownPaymentDataUtil
    {
        private readonly DownPaymentService Service;

        public DownPaymentDataUtil(DownPaymentService service)
        {
            Service = service;
        }

        public DownPaymentViewModel GetViewModelToValidate()
        {
            return new DownPaymentViewModel()
            {
                DocumentNo="",
                TotalPayment=-1,
                Currency = new Currency()
                {
                    Id=0,
                    Code="",
                    Rate=-1
                },
                Remark=""
            };
        }

        public DownPaymentModel GetNewData()
        {
            DownPaymentModel TestData = new DownPaymentModel()
            {
                DocumentNo = "DocumentNo",
                AccountName = "AccountName",
                AccountNumber = "AccountNumber",
                BankName = "BankName",
                CodeBankCurrency = "CodeBankCurrency",
                BuyerId = 1,
                BuyerName = "BuyerName",
                BuyerCode = "BuyerCode",
                CurrencyId = 1,
                CurrencyCode = "USD",
                CurrencyRate = 14447,
                DatePayment = DateTimeOffset.UtcNow,
                PaymentFor = "PaymentFor",
                Remark = "Remark",
                TotalPayment = 14447,
            };

            return TestData;
        }

        public DownPaymentViewModel GetDataToValidate()
        {

            DownPaymentViewModel TestData = new DownPaymentViewModel()
            {
                DocumentNo = "DocumentNo",
                DatePayment = DateTimeOffset.UtcNow,
                Bank = new Bank()
                {
                    AccountName = "AccountName",
                    BankName = "BankName",
                    AccountNumber= "AccountNumber",

                    Currency = new BankCurrency()
                    {
                        Code = "Code",
                    },
                },
                Buyer = new Buyer()
                {
                    Id = 1,
                    Code= "Code",
                    Name = "UnitName",
                },
                TotalPayment = 14447,
                Currency = new Currency()
                {
                    Id = 1,
                    Code = "Code",
                    Rate= 14447,
                },
                Remark = "Remark",               
                
            };

            return TestData;
        }

        public async Task<DownPaymentModel> GetTestDataById()
        {
            DownPaymentModel model = GetNewData();
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }
    }
}
