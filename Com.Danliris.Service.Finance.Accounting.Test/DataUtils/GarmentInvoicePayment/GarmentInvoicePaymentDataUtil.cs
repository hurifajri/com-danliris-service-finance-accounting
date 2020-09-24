using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.GarmentInvoicePayment;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.GarmentInvoicePayment;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.GarmentInvoicePayment
{
    public class GarmentInvoicePaymentDataUtil
    {
        private readonly GarmentInvoicePaymentService Service;

        public GarmentInvoicePaymentDataUtil(GarmentInvoicePaymentService service)
        {
            Service = service;
        }

        public GarmentInvoicePaymentModel GetNewData()
        {
            return new GarmentInvoicePaymentModel
            {
                BGNo="no",
                BuyerId=1,
                BuyerCode="code",
                BuyerName="name",
                CurrencyCode="code",
                CurrencyId=1,
                CurrencyRate=1,
                PaymentDate=DateTimeOffset.Now,
                Remark="aa",
                Items= new List<GarmentInvoicePaymentItemModel>
                {
                    new GarmentInvoicePaymentItemModel()
                    {
                        InvoiceId=1,
                        InvoiceNo="no",
                        Amount=1,
                        IDRAmount=1,
                        
                    },
                    new GarmentInvoicePaymentItemModel()
                    {
                        InvoiceId=2,
                        InvoiceNo="no1",
                        Amount=1,
                        IDRAmount=1,

                    },
                }
            };
        }

        public async Task<GarmentInvoicePaymentModel> GetTestData()
        {
            GarmentInvoicePaymentModel model = GetNewData();
            await Service.CreateAsync(model);
            return await Service.ReadByIdAsync(model.Id);
        }
    }
}
