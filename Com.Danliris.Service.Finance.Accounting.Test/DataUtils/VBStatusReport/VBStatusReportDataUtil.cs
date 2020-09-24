using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VBStatusReport;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VbNonPORequest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VBStatusReport
{
    public class VBStatusReportDataUtil
    {
        private VBStatusReportService service;

        public VBStatusReportDataUtil(VBStatusReportService service)
        {
            this.service = service;
        }

        public VbRequestModel GetNewData()
        {
            VbRequestModel TestData = new VbRequestModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.UtcNow,
                DateEstimate = DateTimeOffset.UtcNow,
                UnitId = 1,
                UnitName = "UnitName",
                CreatedBy = "CreatedBy",
                Usage = "Usage",
                Amount = 1000,
                Apporve_Status = true,
                Realization_Status = true,
                Complete_Status = true,
                IsDeleted = false,
            };

            return TestData;
        }

        public async Task<VbRequestModel> GetTestData_ById()
        {
            VbRequestModel model = GetNewData();
            await service.CreateAsync(model);
            return await service.ReadByIdAsync(model.Id);
        }
    }
}
