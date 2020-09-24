using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.AutoMapperProfiles.VBNonPORequest;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VbNonPORequest;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VbNonPORequest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VbNonPORequest
{
    public class VbNonPORequestDataUtil
    {
        private readonly VbNonPORequestService _service;

        public VbNonPORequestDataUtil(VbNonPORequestService service)
        {
            _service = service;
        }

        public VbNonPORequestViewModel GetViewModelToValidate()
        {
            return new VbNonPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.UtcNow,
                //ApprovedDate = DateTimeOffset.UtcNow,
                DateEstimate = DateTimeOffset.UtcNow,
                Unit = new Unit()
                {
                    Id = 0,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVBRequest()
                {
                    Id = 0,
                    Code = "",
                    Rate = 123,
                    Symbol = "$"
                },
                Amount = 123,
                Usage = "Usage",
                Spinning1 = false,
                Spinning2 = false,
                Spinning3 = false,
                Weaving1 = false,
                Weaving2 = false,
                Finishing = false,
                Printing = false,
                Konfeksi1A = false,
                Konfeksi1B = false,
                Konfeksi2A = false,
                Konfeksi2B = false,
                Konfeksi2C = false,
                Umum = false,
                Others = false,
                DetailOthers = "",
                UnitLoad = "UnitLoad,UnitLoad,UnitLoad"
            };
        }

        public VbNonPORequestViewModel GetViewModelToValidateOthers()
        {
            return new VbNonPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.UtcNow,
                //ApprovedDate = DateTimeOffset.UtcNow,
                DateEstimate = DateTimeOffset.UtcNow,
                Unit = new Unit()
                {
                    Id = 0,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVBRequest()
                {
                    Id = 0,
                    Code = "",
                    Rate = 123,
                    Symbol = "$"
                },
                Amount = 123,
                Usage = "Usage",
                Spinning1 = false,
                Spinning2 = false,
                Spinning3 = false,
                Weaving1 = false,
                Weaving2 = false,
                Finishing = false,
                Printing = false,
                Konfeksi1A = false,
                Konfeksi1B = false,
                Konfeksi2A = false,
                Konfeksi2B = false,
                Konfeksi2C = false,
                Umum = false,
                Others = true,
                DetailOthers = "",
                UnitLoad = "UnitLoad,UnitLoad,UnitLoad"
            };
        }

        public VbNonPORequestViewModel GetViewModelToValidateOthers3()
        {
            return new VbNonPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.UtcNow,
                //ApprovedDate = DateTimeOffset.UtcNow,
                DateEstimate = DateTimeOffset.UtcNow,
                Unit = new Unit()
                {
                    Id = 0,
                    Code = "Code",
                    Name = "Name",
                },
                Currency = new CurrencyVBRequest()
                {
                    Id = 0,
                    Code = "",
                    Rate = 123,
                    Symbol = "$"
                },
                Amount = 123,
                Usage = "Usage",
                Spinning1 = true,
                Spinning2 = true,
                Spinning3 = true,
                Weaving1 = true,
                Weaving2 = true,
                Finishing = true,
                Printing = true,
                Konfeksi1A = true,
                Konfeksi1B = true,
                Konfeksi2A = true,
                Konfeksi2B = true,
                Konfeksi2C = true,
                Umum = true,
                Others = false,
                DetailOthers = "",
                UnitLoad = "UnitLoad,UnitLoad,UnitLoad"
            };
        }

        public VbNonPORequestViewModel GetViewModel()
        {
            return new VbNonPORequestViewModel()
            {
                VBNo = "VBNo",
                Date = DateTimeOffset.UtcNow,
                DateEstimate = DateTimeOffset.UtcNow,
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Division = new Division()
                {
                    Id = 1,
                    Name = "DivisionName"
                },
                Currency = new CurrencyVBRequest()
                {
                    Id = 0,
                    Code = "",
                    Rate = 123,
                    Symbol = "$"
                },
                Amount = 123,
                Usage = "Usage",
                Spinning1 = true,
                Spinning2 = true,
                Spinning3 = true,
                Weaving1 = true,
                Weaving2 = true,
                Finishing = true,
                Printing = true,
                Konfeksi1A = true,
                Konfeksi1B = true,
                Konfeksi2A = true,
                Konfeksi2B = true,
                Konfeksi2C = true,
                Umum = true,
                Others = true,
                DetailOthers = "",
                UnitLoad = "UnitLoad,UnitLoad,UnitLoad"

            };
        }

        public VbNonPORequestViewModel GetViewModel2()
        {
            return new VbNonPORequestViewModel()
            {
                Id = 1,
                VBNo = "VBNo",
                Date = DateTimeOffset.UtcNow,
                DateEstimate = DateTimeOffset.UtcNow,
                Unit = new Unit()
                {
                    Id = 1,
                    Code = "Code",
                    Name = "Name",
                },
                Division = new Division()
                {
                    Id = 1,
                    Name = "DivisionName"
                },
                Currency = new CurrencyVBRequest()
                {
                    Id = 0,
                    Code = "",
                    Rate = 123,
                    Symbol = "$"
                },
                Amount = 123,
                Usage = "Usage",
                Spinning1 = true,
                Spinning2 = true,
                Spinning3 = true,
                Weaving1 = true,
                Weaving2 = true,
                Finishing = true,
                Printing = true,
                Konfeksi1A = true,
                Konfeksi1B = true,
                Konfeksi2A = true,
                Konfeksi2B = true,
                Konfeksi2C = true,
                Umum = true,
                Others = true,
                DetailOthers = "",
                UnitLoad = "UnitLoad,UnitLoad,UnitLoad"

            };
        }

        public VbRequestModel GetVbRequestModelToCreate()
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
                VBRequestCategory = "NONPO",
                CreatedBy = "CreatedBy"

            };
        }

        public async Task<VbRequestModel> GetCreatedData()
        {
            var viewmodel = GetViewModel();
            var model = GetVbRequestModelToCreate();
            await _service.CreateAsync(model, viewmodel);
            await _service.MappingData(viewmodel);
            return await _service.ReadByIdAsync(model.Id);
        }

        public async Task<VbRequestModel> GetTestData()
        {
            VbRequestModel vbRequest = GetVbRequestModelToCreate();
            var viewmodel = GetViewModel();
            await _service.CreateAsync(vbRequest, viewmodel);

            return vbRequest;
        }
    }
}
