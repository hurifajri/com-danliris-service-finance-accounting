using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.VBRequestDocument
{
    public class VBRequestDocumentWithPOFormDtoTest
    {
        [Fact]
        public void ShouldhaveError_Validate_When_Data_Null()
        {
            VBRequestDocumentWithPOFormDto dto = new VBRequestDocumentWithPOFormDto();
          
            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldhaveError_Validate_Items_When_ElementCount_Empty()
        {
            VBRequestDocumentWithPOFormDto dto = new VBRequestDocumentWithPOFormDto()
            {
                Items = new List<VBRequestDocumentWithPOItemFormDto>()
                {

                }
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }


        [Fact]
        public void ShouldHaveError_Validate_When_CurrencyId_Equal_Zero()
        {
            VBRequestDocumentWithPOFormDto dto = new VBRequestDocumentWithPOFormDto()
            {
                Currency = new CurrencyDto()
                {
                    Id = 0
                },
                Items = new List<VBRequestDocumentWithPOItemFormDto>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_SuppliantUnitId_Equal_Zero()
        {
            VBRequestDocumentWithPOFormDto dto = new VBRequestDocumentWithPOFormDto()
            {
                SuppliantUnit = new UnitDto()
                {
                    Id = 0
                },
                Items = new List<VBRequestDocumentWithPOItemFormDto>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_Items_when_NoExist_Unit()
        {
            VBRequestDocumentWithPOFormDto dto = new VBRequestDocumentWithPOFormDto()
            {

                Items = new List<VBRequestDocumentWithPOItemFormDto>()
                
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_Items_when_NoExist_PurchaseOrderExternal()
        {
            VBRequestDocumentWithPOFormDto dto = new VBRequestDocumentWithPOFormDto()
            {
                Items = new List<VBRequestDocumentWithPOItemFormDto>()
                {
                    new VBRequestDocumentWithPOItemFormDto()
                }
            };

            var result = dto.Validate(null);
            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_Items_when_PurchaseOrderExternal_Equal_Zero()
        {
            VBRequestDocumentWithPOFormDto dto = new VBRequestDocumentWithPOFormDto()
            {
                Items = new List<VBRequestDocumentWithPOItemFormDto>()
                {
                    new VBRequestDocumentWithPOItemFormDto()
                    {
                        PurchaseOrderExternal=new PurchaseOrderExternal()
                        {
                            Id=0
                        }
                    }
                }

            };

            var result = dto.Validate(null);
            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveNoError_Validate_Data()
        {
            VBRequestDocumentWithPOFormDto dto = new VBRequestDocumentWithPOFormDto()
            {
                Id=1,
                Amount=1,
                Currency=new CurrencyDto()
                {
                    Id=1,
                },
                Date=DateTimeOffset.Now,
                Purpose= "Purpose",
                RealizationEstimationDate=DateTimeOffset.Now,
                SuppliantUnit=new UnitDto()
                {
                    Id=1,
                    Code="Code"
                },
                Items = new List<VBRequestDocumentWithPOItemFormDto>()
                {
                    new VBRequestDocumentWithPOItemFormDto()
                    {
                        PurchaseOrderExternal=new PurchaseOrderExternal()
                        {
                            Id=1,
                            Items =new List<PurchaseOrderExternalItem>()
                            {
                                new PurchaseOrderExternalItem()
                                {
                                    Id=1
                                }
                            }
                        }
                    }
                }

            };

            var result = dto.Validate(null);
            Assert.True(0 == result.Count());
        }
    }
}
