using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.ClearaceVB;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.ClearaceVB
{
    public class ClearaceVBDataUtil
    {
        private ClearaceVBService service;

        public ClearaceVBDataUtil(ClearaceVBService service)
        {
            this.service = service;
        }
        public VBRequestDocumentModel GetNewData()
        {
            VBRequestDocumentModel TestData = new VBRequestDocumentModel()
            {
                Id = 1,
                
                CreatedBy = "CreatedBy",
            };

            return TestData;
        }

        public virtual async Task<VBRequestDocumentModel> GetTestData()
        {
            VBRequestDocumentModel model = GetNewData();
            await service.CreateAsync(model);
            return model;
        }
    }
}
