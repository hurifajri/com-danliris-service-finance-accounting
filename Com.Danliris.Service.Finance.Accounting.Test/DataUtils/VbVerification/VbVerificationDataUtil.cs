using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.VBVerification;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VbNonPORequest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.VbVerification
{
    public class VbVerificationDataUtil
    {
        private readonly VbVerificationService _service;
        private FinanceDbContext _dbContext;
        public VbVerificationDataUtil(VbVerificationService service, FinanceDbContext dbContext)
        {
            _service = service;
            _dbContext = dbContext;

        }

        public VbVerificationViewModel Get_Verified_VbVerificationViewModel()
        {
            return new VbVerificationViewModel()
            {
                numberVB = new NumberVBData()
                {
                    Id = 1,
                    Amount_Realization = 1,
                    Amount_Request = 1,
                    Currency = "USD",
                    DateEstimate = DateTimeOffset.UtcNow,
                    DateRealization = DateTimeOffset.UtcNow,
                    DateVB = DateTimeOffset.UtcNow,
                    Diff = 1,
                    RequestVbName = "RequestVbName",
                    UnitName = "UnitName",
                    Usage = "Usage",
                    VBNo = "VBNo",
                    VBNoRealize = "VBNoRealize",
                    VBRealizeCategory = "VBRealizeCategory",
                    Status_ReqReal = "Status_ReqReal",
                    Amount_Vat = 1,
                    DetailItems = new List<VbVerificationDetailViewModel>()
                    {
                        new VbVerificationDetailViewModel()
                        {
                            Date = DateTimeOffset.UtcNow,
                            Remark = "Remark",
                            Amount = 1,
                            isGetPPn = true,
                            Total = 1
                        }
                    }
                },
                Reason = "Reason",
                Remark = "Remark",
                VerifyDate = DateTimeOffset.UtcNow,
                isNotVeridied = true,
                isVerified = true

            };
        }



        public VbVerificationViewModel Get_NotVerified_VbVerificationViewModel()
        {
            return new VbVerificationViewModel()
            {
                numberVB = new NumberVBData()
                {
                    Id=1,
                    Amount_Realization = 1,
                    Amount_Request = 1,
                    Currency = "USD",
                    DateEstimate = DateTimeOffset.UtcNow,
                    DateRealization = DateTimeOffset.UtcNow,
                    DateVB = DateTimeOffset.UtcNow,
                    Diff = 1,
                    RequestVbName = "RequestVbName",
                    UnitName = "UnitName",
                    Usage = "Usage",
                    VBNo = "VBNo",
                    VBNoRealize = "VBNoRealize",
                    VBRealizeCategory = "VBRealizeCategory",
                    Status_ReqReal = "Status_ReqReal",
                    Amount_Vat = 1,
                    DetailItems = new List<VbVerificationDetailViewModel>()
                    {
                        new VbVerificationDetailViewModel()
                        {
                            Date = DateTimeOffset.UtcNow,
                            Remark = "Remark",
                            Amount = 1,
                            isGetPPn = true,
                            Total = 1
                        }
                    }
                },
                Reason = "",
                Remark = "Remark",
                VerifyDate = DateTimeOffset.UtcNow,
                isNotVeridied = true,
                isVerified = false
            };
        }

        public async Task<VbRequestModel> GetTestData_VbRequestModel()
        {
            VbRequestModel data = GetNewData_VbRequestModel();

            _dbContext.VbRequests.Add(data);
            await _dbContext.SaveChangesAsync();
            return data;
        }

        public VbRequestModel GetNewData_VbRequestModel()
        {

            return new VbRequestModel()
            {
                VbRequestDetail =new List<VbRequestDetailModel>()
                {
                    new VbRequestDetailModel()
                }
            };
        }


        public async Task<RealizationVbModel> GetTestData_RealizationVbModel()
        {
            RealizationVbModel data = GetNewData_RealizationVbModel();

            _dbContext.RealizationVbs.Add(data);
            await _dbContext.SaveChangesAsync();
            return data;
        }

        public RealizationVbModel GetNewData_RealizationVbModel()
        {

            return new RealizationVbModel()
            {
                Position = (int)VBRealizationPosition.VerifiedToCashier,
                DateEstimate = DateTimeOffset.Now,
                CloseDate = DateTimeOffset.Now,
                Date = DateTimeOffset.Now,
                isClosed = true,
                isNotVeridied = false,
                isVerified = true,
                RequestVbName = "RequestVbName",
                UnitCode = "UnitCode",
                UnitName = "UnitName",
                VBNo = "VBNo",
                VBNoRealize = "VBNoRealize",
                VBRealizeCategory = "NONPO",
                VerifiedDate = DateTimeOffset.Now,
                RealizationVbDetail = new List<RealizationVbDetailModel>()
                {
                    new RealizationVbDetailModel()
                    {
                        CodeProductSPB ="CodeProductSPB",
                        NameProductSPB ="NameProductSPB",
                        NoPOSPB ="NoPOSPB",
                        NoSPB="NoSPB",
                        PriceTotalSPB=1,
                        RealizationVbDetail =new RealizationVbModel()
                        {
                            DateEstimate = DateTimeOffset.Now,
                            CloseDate = DateTimeOffset.Now,
                            Date = DateTimeOffset.Now,
                            isClosed = true,
                            isNotVeridied = false,
                            isVerified = true,
                            RequestVbName = "RequestVbName",
                            UnitCode = "UnitCode",
                            UnitName = "UnitName",
                            VBNo = "VBNo",
                            VBNoRealize = "VBNoRealize",
                            VBRealizeCategory = "NONPO",
                            VerifiedDate = DateTimeOffset.Now,
                        },
                        DivisionSPB ="DivisionSPB",
                        VBRealizationId =1,
                        IdProductSPB ="IdProductSPB",
                        DateSPB =DateTimeOffset.Now,

                    }
                }
            };
        }
        

        public VbVerificationViewModel GetNewDataVbVerificationViewModel()
        {
            return new VbVerificationViewModel()
            {

            };
        }

       
    }
}
