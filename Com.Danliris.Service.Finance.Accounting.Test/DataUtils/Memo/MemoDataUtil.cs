using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.Memo;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.Memo;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.Memo;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.Memo
{
    public class MemoDataUtil
    {
        private readonly MemoService _service;

        public MemoDataUtil(MemoService service)
        {
            _service = service;
        }

        public MemoViewModel GetViewModelToValidate()
        {
            return new MemoViewModel()
            {

                SalesInvoice = new SalesInvoice()
                {
                    Id = 0,
                    Buyer = new Buyer()
                    {
                        Id = 0
                    }
                },
                Unit = new Unit()
                {
                    Id = 0
                },
                Items = new List<MemoItemViewModel>()
                {
                    new MemoItemViewModel()
                    {
                        Interest = -1
                    },
                    new MemoItemViewModel()
                    {
                        Currency = new Currency()
                        {
                            Id = 0
                        }
                    }
                }
            };
        }

        public MemoModel GetMemoModelToCreate()
        {
            return new MemoModel()
            {
                BuyerCode = "BuyerCode",
                BuyerId = 1,
                BuyerName = "BuyerName",
                Date = DateTime.Now.ToUniversalTime(),
                Items = new List<MemoItemModel>()
                {
                    new MemoItemModel()
                    {
                        CurrencyCode = "CurrencyCode",
                        CurrencyId = 1,
                        CurrencyRate = 1,
                        Interest = 1,
                        PaymentAmount = 1
                    }
                },
                MemoType = "MemoType",
                SalesInvoiceId = 1,
                SalesInvoiceNo = "SalesInvoiceNo",
                UnitCode = "UnitCode",
                UnitId = 1,
                UnitName = "UnitName"
            };
        }

        public MemoModel GetMemoModelToCreate_SalesInvoice()
        {
            return new MemoModel()
            {
                BuyerCode = "BuyerCode",
                BuyerId = 1,
                BuyerName = "BuyerName",
                Date = DateTime.Now.ToUniversalTime(),
                Items = new List<MemoItemModel>()
                {
                    new MemoItemModel()
                    {
                        CurrencyCode = "CurrencyCode",
                        CurrencyId = 1,
                        CurrencyRate = 1,
                        Interest = 1,
                        PaymentAmount = 1
                    }
                },
                MemoType = "MemoType",
                SalesInvoiceId = 1,
                SalesInvoiceNo = "SalesInvoiceNo",
                UnitCode = "UnitCode",
                UnitId = 1,
                UnitName = "UnitName"
            };
        }

        public async Task<MemoModel> GetCreatedData()
        {
            var model = GetMemoModelToCreate();
            await _service.CreateAsync(model);
            return await _service.ReadByIdAsync(model.Id);
        }

        public async Task<MemoModel> GetCreatedSalesInvoiceData()
        {
            var model = GetMemoModelToCreate_SalesInvoice();
            await _service.CreateAsync(model);
            return await _service.ReadBySalesInvoiceAsync(model.SalesInvoiceNo);
        }
    }
}
