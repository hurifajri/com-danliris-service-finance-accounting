using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VBRequestAll;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRequestAll;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBRequestAll
{
    public class VBRequestAllDataUtil
    {
        private VBRequestAllService service;

        public VBRequestAllDataUtil(VBRequestAllService service)
        {
            this.service = service;
        }

        public VbRequestModel GetNewData()
        {
            return new VbRequestModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.UtcNow,
                DateEstimate = DateTimeOffset.UtcNow,
                UnitId = 1,
                UnitCode = "UnitCode",
                UnitName = "UnitName",
                CurrencyId = 1,
                CurrencyCode = "CurrencyCode",
                CurrencyRate = 123,
                CurrencySymbol = "CurrencySymbol",
                Amount = 123,
                Usage = "Usage",
                UnitLoad = "UnitLoad",
                Apporve_Status = true,
                Complete_Status = true,
                VBRequestCategory = "PO",
                CreatedBy = "CreatedBy",
                VBMoney = 1,
                Usage_Input = "Usage_Input",
            };
        }

        public async Task<VbRequestModel> GetTestDataById()
        {
            VbRequestModel model = GetNewData();
            await service.CreateAsync(model);
            return await service.ReadByIdAsync(model.Id);
        }
    }
}
