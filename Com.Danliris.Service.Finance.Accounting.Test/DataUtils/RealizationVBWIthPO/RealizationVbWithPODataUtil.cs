using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.IntegrationViewModel;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBWIthPO;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.RealizationVBWIthPO
{
    public class RealizationVbWithPODataUtil
    {
        private readonly RealizationVbWithPOService Service;

        public RealizationVbWithPODataUtil(RealizationVbWithPOService service)
        {
            Service = service;
        }

        public RealizationVbModel GetNewData()
        {
            return new RealizationVbModel()
            {  
                Position=4,
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
                VBRealizeCategory = "VBRealizeCategory",
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
                            VBRealizeCategory = "VBRealizeCategory",
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

        public RealizationVbWithPOViewModel GetNewViewModel()
        {
            return new RealizationVbWithPOViewModel()
            {
                TypeVBNonPO= "Dengan Nomor VB",
                Date = DateTimeOffset.Now,
                numberVB = new DetailVB()
                {
                    DateEstimate = DateTimeOffset.Now,
                    PONo = new List<PODetail>()
                    {
                        new PODetail()
                        {
                            PONo="PONo",
                            Price=1,
                        }
                    },
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    UnitName = "UnitName",
                    VBNo = "VBNo",

                },
                VBRealizationNo = "VBRealizationNo",
                IsDeleted = false,

                Items = new List<DetailSPB>()
                {
                    new DetailSPB()
                    {
                        date =DateTimeOffset.Now,
                        division ="division",
                        IsSave =true,
                        no ="no",
                        supplier = new SupplierViewModel()
                        {
                            _id = "1",
                            code = "code",
                            name = "name"
                        },
                        currency = new CurrencyViewModel()
                        {
                            _id = "1",
                            code = "code",
                            symbol = "$",
                            rate = 1
                        },
                        item =new List<DetailItemSPB>()
                        {
                            new DetailItemSPB()
                            {
                                IsDeleted =false,
                                unitReceiptNote =new DetailunitReceiptNote()
                                {
                                    no ="no",
                                    items =new List<DetailitemunitReceiptNote>()
                                    {
                                        new DetailitemunitReceiptNote()
                                        {
                                            PriceTotal=1,
                                            Product =new Product_VB()
                                            {
                                                _id = "1",
                                                code ="code",
                                                name ="name",

                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        
        public VbRequestModel GetDataRequestVB()
        {
            return new VbRequestModel()
            {
                Id = 1,
                VBNo = "VBNo",
                Realization_Status = false,
                IsDeleted = false,
            };
        }

      

        //public async Task<RealizationVbModel> GetTestData()
        //{
        //    RealizationVbModel model = GetNewData();
        //    RealizationVbWithPOViewModel viewModel = GetNewViewModel();
        //    await Service.CreateAsync(model, viewModel);

        //}
    }



}
