using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.VBRealizationDocument
{
    public class FormDtoTest
    {
        [Fact]
        public void ShouldhaveError_Validate_When_AllData_Null()
        {
            FormDto dto = new FormDto();

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveNoError_Validate_Data()
        {
            FormDto dto = new FormDto()
            {
                Id = 1,
                Type = "Tanpa Nomor VB",
                Currency = new CurrencyDto()
                {
                    Id = 1,
                    Code = "Code"
                },
                Date = DateTimeOffset.Now,
                SuppliantUnit = new UnitDto()
                {
                    Id = 1,
                    Code = "code",
                    Division = new DivisionDto()
                    {
                        Id = 1,
                        Code = "Code",
                        Name = "Name",

                    },
                    Name = "Name",
                    VBDocumentLayoutOrder = 1
                },
                VBRequestDocument = new VBRequestDocumentDto()
                {
                    Id = 1
                },
                Items = new List<FormItemDto>()
                {
                    new FormItemDto()
                    {
                        UnitPaymentOrder=new UnitPaymentOrderDto()
                        {
                            Id=1,
                            Amount=1,
                            Date=DateTimeOffset.Now,
                            IncomeTax=new IncomeTaxDto()
                            {
                                Id=1,
                                Name="Name",
                                Rate=1
                            },
                            IncomeTaxBy="supplier",
                            No="1",
                            UseIncomeTax=true,
                            UseVat=true
                        }
                    }
                }
            };

            var result = dto.Validate(null);

            Assert.True(0 == result.Count());
        }


        [Fact]
        public void ShouldHaveError_Validate_When_SuppliantUnit_and_Currency_Null()
        {
            FormDto dto = new FormDto()
            {
                Type = "Tanpa Nomor VB",
                SuppliantUnit = null,
                Currency = null,
                Items = new List<FormItemDto>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_SuppliantUnitId_and_CurrencyId_Equal_Zero()
        {
            FormDto dto = new FormDto()
            {
                Type = "Tanpa Nomor VB",
                SuppliantUnit = new UnitDto()
                {
                    Id = 0
                },
                Currency = new CurrencyDto()
                {
                    Id = 0
                },
                Items = new List<FormItemDto>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }


        [Fact]
        public void ShouldHaveError_Validate_When_VBRequestDocument_Null()
        {
            FormDto dto = new FormDto()
            {
                Type = "Dengan Nomor VB",

                Items = new List<FormItemDto>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_VBRequestDocumentId_Equal_Zero()
        {
            FormDto dto = new FormDto()
            {
                Type = "Dengan Nomor VB",
                VBRequestDocument = new VBRequestDocumentDto()
                {
                    Id = 0
                },
                Items = new List<FormItemDto>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }


        [Fact]
        public void ShouldHaveError_Validate_When_ItemsCount_Equal_Zero()
        {
            FormDto dto = new FormDto()
            {
                Items = new List<FormItemDto>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }


        [Fact]
        public void ShouldHaveError_Validate_When_UnitPaymentOrder_Null()
        {
            FormDto dto = new FormDto()
            {
                Items = new List<FormItemDto>()
                {
                    new FormItemDto()
                    {
                        UnitPaymentOrder=null
                    }
                }
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_UnitPaymentOrderId_Equal_Zero()
        {
            FormDto dto = new FormDto()
            {
                Items = new List<FormItemDto>()
                {
                    new FormItemDto()
                    {
                        UnitPaymentOrder=new UnitPaymentOrderDto()
                        {
                            Id=0
                        }
                    }
                }
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }
    }
}
