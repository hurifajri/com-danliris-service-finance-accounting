using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.RealizationVBNonPO;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VbNonPORequest;
using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBNonPO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Test.DataUtils.RealizationVBNonPO
{
    public class RealizationVBNonPODataUtil
    {
        private readonly RealizationVbNonPOService Service;

        public RealizationVBNonPODataUtil(RealizationVbNonPOService service)
        {
            Service = service;
        }

        public RealizationVbModel GetNewData()
        {
            return new RealizationVbModel()
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
                TypeWithOrWithoutVB = "Supplier",
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
                        IncomeTaxId = 1,
                        IncomeTaxName = "IncomeTaxName",
                        IncomeTaxBy = "IncomeTaxBy",
                        IncomeTaxRate = 1

                    }
                }
            };
        }

        public RealizationVbNonPOViewModel GetNewViewModel()
        {
            return new RealizationVbNonPOViewModel()
            {
                IsDeleted = false,
                Active = true,
                CreatedUtc = DateTime.Now,
                CreatedBy = "CreatedBy",
                CreatedAgent = "CreatedAgent",
                LastModifiedUtc = DateTime.Now,
                LastModifiedBy = "LastModifiedBy",
                LastModifiedAgent = "LastModifiedAgent",
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,

                TypeVBNonPO = "Tanpa Nomor VB",
                AmountVB = 100,
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
                DetailOthers = "DetailOthers",

                AmountSpinning1 = 123,
                AmountSpinning2 = 123,
                AmountSpinning3 = 123,
                AmountWeaving1 = 123,
                AmountWeaving2 = 123,
                AmountFinishing = 123,
                AmountPrinting = 123,
                AmountKonfeksi1A = 123,
                AmountKonfeksi1B = 123,
                AmountKonfeksi2A = 123,
                AmountKonfeksi2B = 123,
                AmountKonfeksi2C = 123,
                AmountUmum = 123,
                AmountOthers = 123,
                DateEstimateVB = DateTimeOffset.Now,
                Unit = new Unit()
                {
                    Code = "code",
                    Name = "name"
                },
                DateVB = DateTimeOffset.Now,
                Currency = new CurrencyVBRequest()
                {
                    Code = "code",
                    Rate = 1,
                    Description = "des",
                    Symbol = "Rp"
                },
                Division = new Division()
                {
                    Id = 1,
                    Name = "name"
                },

                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "IDR",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "CurrencySymbol",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    UnitLoad = "UnitLoad",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = true,
                        isGetPPh = true,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1
                        },
                        IncomeTaxBy = "Supplier"
                    },
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = true,
                        isGetPPh = true,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1
                        },
                        IncomeTaxBy = "Danliris"
                    }
                }
            };
        }

        public RealizationVbNonPOViewModel GetNewViewModelNew()
        {
            return new RealizationVbNonPOViewModel()
            {
                IsDeleted = false,
                Active = true,
                CreatedUtc = DateTime.Now,
                CreatedBy = "CreatedBy",
                CreatedAgent = "CreatedAgent",
                LastModifiedUtc = DateTime.Now,
                LastModifiedBy = "LastModifiedBy",
                LastModifiedAgent = "LastModifiedAgent",
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",

                Spinning1VB = true,
                Spinning2VB = true,
                Spinning3VB = true,
                Weaving1VB = true,
                Weaving2VB = true,
                FinishingVB = true,
                PrintingVB = true,
                Konfeksi1AVB = true,
                Konfeksi1BVB = true,
                Konfeksi2AVB = true,
                Konfeksi2BVB = true,
                Konfeksi2CVB = true,
                UmumVB = true,
                OthersVB = true,
                DetailOthersVB = "DetailOthers",

                AmountSpinning1VB = 123,
                AmountSpinning2VB = 123,
                AmountSpinning3VB = 123,
                AmountWeaving1VB = 123,
                AmountWeaving2VB = 123,
                AmountFinishingVB = 123,
                AmountPrintingVB = 123,
                AmountKonfeksi1AVB = 123,
                AmountKonfeksi1BVB = 123,
                AmountKonfeksi2AVB = 123,
                AmountKonfeksi2BVB = 123,
                AmountKonfeksi2CVB = 123,
                AmountUmumVB = 123,
                AmountOthersVB = 123,

                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "IDR",
                    CurrencyRate = 1,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "CurrencySymbol",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    UnitLoad = "UnitLoad",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1
                        },
                        IncomeTaxBy = "IncomeTaxBy"
                    }
                }
            };
        }

        public RealizationVbNonPOViewModel GetNewViewModelNew1()
        {
            return new RealizationVbNonPOViewModel()
            {
                //Id = 1,
                IsDeleted = false,
                Active = true,
                CreatedUtc = DateTime.Now,
                CreatedBy = "CreatedBy",
                CreatedAgent = "CreatedAgent",
                LastModifiedUtc = DateTime.Now,
                LastModifiedBy = "LastModifiedBy",
                LastModifiedAgent = "LastModifiedAgent",
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",

                Spinning1VB = true,
                Spinning2VB = true,
                Spinning3VB = true,
                Weaving1VB = true,
                Weaving2VB = true,
                FinishingVB = true,
                PrintingVB = true,
                Konfeksi1AVB = true,
                Konfeksi1BVB = true,
                Konfeksi2AVB = true,
                Konfeksi2BVB = true,
                Konfeksi2CVB = true,
                UmumVB = true,
                OthersVB = true,
                DetailOthersVB = "DetailOthers",

                AmountSpinning1VB = 123,
                AmountSpinning2VB = 123,
                AmountSpinning3VB = 123,
                AmountWeaving1VB = 123,
                AmountWeaving2VB = 123,
                AmountFinishingVB = 123,
                AmountPrintingVB = 123,
                AmountKonfeksi1AVB = 123,
                AmountKonfeksi1BVB = 123,
                AmountKonfeksi2AVB = 123,
                AmountKonfeksi2BVB = 123,
                AmountKonfeksi2CVB = 123,
                AmountUmumVB = 123,
                AmountOthersVB = 123,

                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "IDR",
                    CurrencyRate = 1,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "CurrencySymbol",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    UnitLoad = "UnitLoad",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1
                        },
                        IncomeTaxBy = "IncomeTaxBy"
                    }
                }
            };
        }

        public RealizationVbNonPOViewModel GetNewViewModel2()
        {
            return new RealizationVbNonPOViewModel()
            {
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",

                Spinning1VB = true,
                Spinning2VB = true,
                Spinning3VB = true,
                Weaving1VB = true,
                Weaving2VB = true,
                FinishingVB = true,
                PrintingVB = true,
                Konfeksi1AVB = true,
                Konfeksi1BVB = true,
                Konfeksi2AVB = true,
                Konfeksi2BVB = true,
                Konfeksi2CVB = true,
                UmumVB = true,
                OthersVB = true,
                DetailOthersVB = "DetailOthers",

                AmountSpinning1VB = 123,
                AmountSpinning2VB = 123,
                AmountSpinning3VB = 123,
                AmountWeaving1VB = 123,
                AmountWeaving2VB = 123,
                AmountFinishingVB = 123,
                AmountPrintingVB = 123,
                AmountKonfeksi1AVB = 123,
                AmountKonfeksi1BVB = 123,
                AmountKonfeksi2AVB = 123,
                AmountKonfeksi2BVB = 123,
                AmountKonfeksi2CVB = 123,
                AmountUmumVB = 123,
                AmountOthersVB = 123,

                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "IDR",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "CurrencySymbol",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    UnitLoad = "UnitLoad",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }
            };
        }

        public RealizationVbNonPOViewModel GetNewViewModel3()
        {
            return new RealizationVbNonPOViewModel()
            {
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,

                Spinning1VB = true,
                Spinning2VB = true,
                Spinning3VB = true,
                Weaving1VB = true,
                Weaving2VB = true,
                FinishingVB = true,
                PrintingVB = true,
                Konfeksi1AVB = true,
                Konfeksi1BVB = true,
                Konfeksi2AVB = true,
                Konfeksi2BVB = true,
                Konfeksi2CVB = true,
                UmumVB = true,
                OthersVB = true,
                DetailOthersVB = "DetailOthers",

                AmountSpinning1VB = 123,
                AmountSpinning2VB = 123,
                AmountSpinning3VB = 123,
                AmountWeaving1VB = 123,
                AmountWeaving2VB = 123,
                AmountFinishingVB = 123,
                AmountPrintingVB = 123,
                AmountKonfeksi1AVB = 123,
                AmountKonfeksi1BVB = 123,
                AmountKonfeksi2AVB = 123,
                AmountKonfeksi2BVB = 123,
                AmountKonfeksi2CVB = 123,
                AmountUmumVB = 123,
                AmountOthersVB = 123,

                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "CurrencySymbol",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    UnitLoad = "UnitLoad",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false
                    }
                }
            };
        }

        public RealizationVbNonPOViewModel GetNewViewModel4()
        {
            return new RealizationVbNonPOViewModel()
            {
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Tanpa Nomor VB",
                AmountVB = 100,
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
                DetailOthers = "DetailOthers",

                AmountSpinning1 = 123,
                AmountSpinning2 = 123,
                AmountSpinning3 = 123,
                AmountWeaving1 = 123,
                AmountWeaving2 = 123,
                AmountFinishing = 123,
                AmountPrinting = 123,
                AmountKonfeksi1A = 123,
                AmountKonfeksi1B = 123,
                AmountKonfeksi2A = 123,
                AmountKonfeksi2B = 123,
                AmountKonfeksi2C = 123,
                AmountUmum = 123,
                AmountOthers = 123,
                DateEstimateVB = DateTimeOffset.Now,
                Unit = new Unit()
                {
                    Code = "code",
                    Name = "name"
                },
                DateVB = DateTimeOffset.Now,
                Currency = new CurrencyVBRequest()
                {
                    Code = "code",
                    Rate = 1,
                    Description = "des",
                    Symbol = "Rp"
                },
                Division = new Division()
                {
                    Id = 1,
                    Name = "name"
                },

                numberVB = new DetailRequestNonPO()
                {
                    Amount = 0,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "CurrencySymbol",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    UnitLoad = "UnitLoad",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 123,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1
                        },
                        IncomeTaxBy = "Dan Liris"
                    }
                }
            };
        }

        public RealizationVbNonPOViewModel GetNewViewModel5()
        {
            return new RealizationVbNonPOViewModel()
            {
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",

                Spinning1VB = true,
                Spinning2VB = true,
                Spinning3VB = true,
                Weaving1VB = true,
                Weaving2VB = true,
                FinishingVB = true,
                PrintingVB = true,
                Konfeksi1AVB = true,
                Konfeksi1BVB = true,
                Konfeksi2AVB = true,
                Konfeksi2BVB = true,
                Konfeksi2CVB = true,
                UmumVB = true,
                OthersVB = true,
                DetailOthersVB = "DetailOthers",

                AmountSpinning1VB = 123,
                AmountSpinning2VB = 123,
                AmountSpinning3VB = 123,
                AmountWeaving1VB = 123,
                AmountWeaving2VB = 123,
                AmountFinishingVB = 123,
                AmountPrintingVB = 123,
                AmountKonfeksi1AVB = 123,
                AmountKonfeksi1BVB = 123,
                AmountKonfeksi2AVB = 123,
                AmountKonfeksi2BVB = 123,
                AmountKonfeksi2CVB = 123,
                AmountUmumVB = 123,
                AmountOthersVB = 123,

                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "CurrencySymbol",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    UnitLoad = "UnitLoad",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 0,
                        isGetPPn = false,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1
                        },
                        IncomeTaxBy = "IncomeTaxBy"
                    }
                }
            };
        }

        public RealizationVbNonPOViewModel GetNewViewModel6()
        {
            return new RealizationVbNonPOViewModel()
            {

                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",

                Spinning1VB = true,
                Spinning2VB = true,
                Spinning3VB = true,
                Weaving1VB = true,
                Weaving2VB = true,
                FinishingVB = true,
                PrintingVB = true,
                Konfeksi1AVB = true,
                Konfeksi1BVB = true,
                Konfeksi2AVB = true,
                Konfeksi2BVB = true,
                Konfeksi2CVB = true,
                UmumVB = true,
                OthersVB = true,
                DetailOthersVB = "DetailOthers",

                AmountSpinning1VB = 123,
                AmountSpinning2VB = 123,
                AmountSpinning3VB = 123,
                AmountWeaving1VB = 123,
                AmountWeaving2VB = 123,
                AmountFinishingVB = 123,
                AmountPrintingVB = 123,
                AmountKonfeksi1AVB = 123,
                AmountKonfeksi1BVB = 123,
                AmountKonfeksi2AVB = 123,
                AmountKonfeksi2BVB = 123,
                AmountKonfeksi2CVB = 123,
                AmountUmumVB = 123,
                AmountOthersVB = 123,

                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "USD",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "CurrencySymbol",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    UnitLoad = "UnitLoad",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "Remark",
                        Amount = 0,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1
                        },
                        IncomeTaxBy = "IncomeTaxBy"
                    }
                }
            };
        }

        public RealizationVbNonPOViewModel GetNewViewModelFalse()
        {
            return new RealizationVbNonPOViewModel()
            {
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Tanpa Nomor VB",
                DateVB = DateTimeOffset.Now,
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 123,
                    CreatedBy = "CreateBy",
                    CurrencyCode = "IDR",
                    CurrencyRate = 123,
                    CurrencyDescription = "CurrencyDescription",
                    CurrencySymbol = "CurrencySymbol",
                    Date = DateTimeOffset.Now,
                    DateEstimate = DateTimeOffset.Now,
                    UnitCode = "UnitCode",
                    UnitId = 1,
                    UnitLoad = "UnitLoad",
                    UnitName = "UnitName",
                    UnitDivisionId = 1,
                    UnitDivisionName = "UnitDivisionName",
                    VBNo = "VBNo",
                    VBRequestCategory = "NONPO"

                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now.AddDays(5),
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false
                    },
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = null,
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false
                    },
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now.AddDays(-5),
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false
                    },
                }
            };
        }

        public RealizationVbNonPOViewModel GetNewViewModelFalse2()
        {
            return new RealizationVbNonPOViewModel()
            {
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",
                numberVB = new DetailRequestNonPO()
                {
                    Amount = 1,
                    Date = DateTimeOffset.Now,
                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now.AddDays(5),
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1
                        },
                        IncomeTaxBy = "income"
                    },
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = null,
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTaxBy = ""
                    },
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now.AddDays(-5),
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false
                    },
                }
            };
        }

        public RealizationVbNonPOViewModel GetNewViewModelFalse3()
        {
            return new RealizationVbNonPOViewModel()
            {
                VBRealizationNo = "VBRealizationNo",
                TypeVBNonPO = "Tanpa Nomor VB",
                AmountVB = 100,
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
                DetailOthers = "DetailOthers",

                AmountSpinning1 = 123,
                AmountSpinning2 = 123,
                AmountSpinning3 = 123,
                AmountWeaving1 = 123,
                AmountWeaving2 = 123,
                AmountFinishing = 123,
                AmountPrinting = 123,
                AmountKonfeksi1A = 123,
                AmountKonfeksi1B = 123,
                AmountKonfeksi2A = 123,
                AmountKonfeksi2B = 123,
                AmountKonfeksi2C = 123,
                AmountUmum = 123,
                AmountOthers = 123,
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now.AddDays(5),
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1
                        },
                        IncomeTaxBy = "income"
                    },
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = null,
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTaxBy = ""
                    },
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now.AddDays(-5),
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false
                    },
                }
            };
        }

        public RealizationVbNonPOViewModel GetNewViewModelFalse3a()
        {
            return new RealizationVbNonPOViewModel()
            {
                VBRealizationNo = "VBRealizationNo",
                TypeVBNonPO = "Tanpa Nomor VB",
                AmountVB = 100,
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

                AmountSpinning1 = 0,
                AmountSpinning2 = 0,
                AmountSpinning3 = 0,
                AmountWeaving1 = 0,
                AmountWeaving2 = 0,
                AmountFinishing = 0,
                AmountPrinting = 0,
                AmountKonfeksi1A = 0,
                AmountKonfeksi1B = 0,
                AmountKonfeksi2A = 0,
                AmountKonfeksi2B = 0,
                AmountKonfeksi2C = 0,
                AmountUmum = 0,
                AmountOthers = 0,
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now.AddDays(5),
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1
                        },
                        IncomeTaxBy = "income"
                    },
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = null,
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTaxBy = ""
                    },
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now.AddDays(-5),
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false
                    },
                }
            };
        }

        public RealizationVbNonPOViewModel GetNewViewModelFalse3b()
        {
            return new RealizationVbNonPOViewModel()
            {
                VBRealizationNo = "VBRealizationNo",
                TypeVBNonPO = "Tanpa Nomor VB",
                AmountVB = 100,
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
                DetailOthers = "DetailOthers",

                AmountSpinning1 = -1,
                AmountSpinning2 = -1,
                AmountSpinning3 = -1,
                AmountWeaving1 = -1,
                AmountWeaving2 = -1,
                AmountFinishing = -1,
                AmountPrinting = -1,
                AmountKonfeksi1A = -1,
                AmountKonfeksi1B = -1,
                AmountKonfeksi2A = -1,
                AmountKonfeksi2B = -1,
                AmountKonfeksi2C = -1,
                AmountUmum = -1,
                AmountOthers = -1,
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now.AddDays(5),
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1
                        },
                        IncomeTaxBy = "income"
                    },
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = null,
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTaxBy = ""
                    },
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now.AddDays(-5),
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false
                    },
                }
            };
        }

        public RealizationVbNonPOViewModel GetNewViewModelFalse4()
        {
            return new RealizationVbNonPOViewModel()
            {
                VBRealizationNo = "VBRealizationNo",
                DateVB = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",
                Spinning1VB = false,
                Spinning2VB = false,
                Spinning3VB = false,
                Weaving1VB = false,
                Weaving2VB = false,
                FinishingVB = false,
                PrintingVB = false,
                Konfeksi1AVB = false,
                Konfeksi1BVB = false,
                Konfeksi2AVB = false,
                Konfeksi2BVB = false,
                Konfeksi2CVB = false,
                UmumVB = false,
                OthersVB = false,
                DetailOthersVB = "",

                AmountSpinning1VB = 0,
                AmountSpinning2VB = 0,
                AmountSpinning3VB = 0,
                AmountWeaving1VB = 0,
                AmountWeaving2VB = 0,
                AmountFinishingVB = 0,
                AmountPrintingVB = 0,
                AmountKonfeksi1AVB = 0,
                AmountKonfeksi1BVB = 0,
                AmountKonfeksi2AVB = 0,
                AmountKonfeksi2BVB = 0,
                AmountKonfeksi2CVB = 0,
                AmountUmumVB = 0,
                AmountOthersVB = 0,

                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now.AddDays(5),
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1
                        },
                        IncomeTaxBy = "income"
                    },
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = null,
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTaxBy = ""
                    },
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now.AddDays(-5),
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1
                        },
                        IncomeTaxBy = "income"
                    },
                }
            };
        }

        public RealizationVbNonPOViewModel GetNewViewModelFalse4a()
        {
            return new RealizationVbNonPOViewModel()
            {
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                DateVB = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",
                Spinning1VB = true,
                Spinning2VB = true,
                Spinning3VB = true,
                Weaving1VB = true,
                Weaving2VB = true,
                FinishingVB = true,
                PrintingVB = true,
                Konfeksi1AVB = true,
                Konfeksi1BVB = true,
                Konfeksi2AVB = true,
                Konfeksi2BVB = true,
                Konfeksi2CVB = true,
                UmumVB = true,
                OthersVB = true,
                DetailOthersVB = "detail",

                AmountSpinning1VB = 123,
                AmountSpinning2VB = 123,
                AmountSpinning3VB = 123,
                AmountWeaving1VB = 123,
                AmountWeaving2VB = 123,
                AmountFinishingVB = 123,
                AmountPrintingVB = 123,
                AmountKonfeksi1AVB = 123,
                AmountKonfeksi1BVB = 123,
                AmountKonfeksi2AVB = 123,
                AmountKonfeksi2BVB = 123,
                AmountKonfeksi2CVB = 123,
                AmountUmumVB = 123,
                AmountOthersVB = 123,

                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now.AddDays(5),
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1
                        },
                        IncomeTaxBy = "income"
                    },
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = null,
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTaxBy = ""
                    },
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now.AddDays(-5),
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTaxBy = ""
                    },
                }
            };
        }

        public RealizationVbNonPOViewModel GetNewViewModelFalse4b()
        {
            return new RealizationVbNonPOViewModel()
            {
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now,
                DateVB = DateTimeOffset.Now,
                TypeVBNonPO = "Dengan Nomor VB",
                Spinning1VB = true,
                Spinning2VB = true,
                Spinning3VB = true,
                Weaving1VB = true,
                Weaving2VB = true,
                FinishingVB = true,
                PrintingVB = true,
                Konfeksi1AVB = true,
                Konfeksi1BVB = true,
                Konfeksi2AVB = true,
                Konfeksi2BVB = true,
                Konfeksi2CVB = true,
                UmumVB = true,
                OthersVB = true,
                DetailOthersVB = "detail",

                AmountSpinning1VB = -1,
                AmountSpinning2VB = -1,
                AmountSpinning3VB = -1,
                AmountWeaving1VB = -1,
                AmountWeaving2VB = -1,
                AmountFinishingVB = -1,
                AmountPrintingVB = -1,
                AmountKonfeksi1AVB = -1,
                AmountKonfeksi1BVB = -1,
                AmountKonfeksi2AVB = -1,
                AmountKonfeksi2BVB = -1,
                AmountKonfeksi2CVB = -1,
                AmountUmumVB = -1,
                AmountOthersVB = -1,

                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now.AddDays(5),
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1
                        },
                        IncomeTaxBy = "income"
                    },
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = null,
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTaxBy = ""
                    },
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now.AddDays(-5),
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false
                    },
                }
            };
        }

        public RealizationVbNonPOViewModel GetNewViewModelFalse5()
        {
            return new RealizationVbNonPOViewModel()
            {
                VBRealizationNo = "VBRealizationNo",
                Date = DateTimeOffset.Now.AddDays(10),
                numberVB = new DetailRequestNonPO()
                {
                    Date = DateTimeOffset.Now.AddDays(-10),
                },
                Items = new List<VbNonPORequestDetailViewModel>()
                {
                    new VbNonPORequestDetailViewModel()
                    {
                        DateDetail = DateTimeOffset.Now,
                        Remark = "",
                        Amount = -1,
                        isGetPPn = false,
                        isGetPPh = true,
                        IncomeTax = new IncomeTaxNew()
                        {
                            Id = 1,
                            name = "name",
                            rate = 1
                        },
                        IncomeTaxBy = "income"
                    }
                }
            };
        }

        public VbRequestModel GetDataRequestVB()
        {
            return new VbRequestModel()
            {
                VBNo = "VBNo",
                Realization_Status = false,
                IsDeleted = false
            };
        }

        public RealizationVbModel GetDataRealizationVB()
        {
            return new RealizationVbModel()
            {
                VBNo = "VBNo",
                isVerified = true,
                IsDeleted = false,
            };
        }

        public async Task<RealizationVbNonPOViewModel> GetCreatedData()
        {
            var model = GetNewData();
            var viewmodel = GetNewViewModel();
            await Service.CreateAsync(model, viewmodel);
            return await Service.ReadByIdAsync2(model.Id);
        }

        public async Task<RealizationVbNonPOViewModel> GetCreatedData2()
        {
            var model = GetNewData();
            var viewmodel = GetNewViewModelNew();
            await Service.CreateAsync(model, viewmodel);
            return await Service.ReadByIdAsync2(model.Id);
        }
    }
}
