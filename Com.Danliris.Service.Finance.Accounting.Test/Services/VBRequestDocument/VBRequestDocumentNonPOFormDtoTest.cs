using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.VBRequestDocument
{
   public class VBRequestDocumentNonPOFormDtoTest
    {
        [Fact]
        public void ShouldHaveError_Validate_When_Data_Null()
        {
            VBRequestDocumentNonPOFormDto dto = new VBRequestDocumentNonPOFormDto()
            {
                Items = new List<VBRequestDocumentNonPOItemFormDto>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_SuppliantUnitId_Equal_Zero()
        {
            VBRequestDocumentNonPOFormDto dto = new VBRequestDocumentNonPOFormDto()
            {
                SuppliantUnit=new UnitDto()
                {
                    Id=0
                },
                Items = new List<VBRequestDocumentNonPOItemFormDto>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_When_CurrencyId_Equal_Zero()
        {
            VBRequestDocumentNonPOFormDto dto = new VBRequestDocumentNonPOFormDto()
            {
                Currency = new CurrencyDto()
                {
                    Id = 0
                },
                Items = new List<VBRequestDocumentNonPOItemFormDto>()
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_Items_when_NoExist_UnitName()
        {
            VBRequestDocumentNonPOFormDto dto = new VBRequestDocumentNonPOFormDto()
            {
                
                Items = new List<VBRequestDocumentNonPOItemFormDto>()
                {
                    new VBRequestDocumentNonPOItemFormDto()
                    {
                      
                        IsSelected=true
                    }
                }
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }


        [Fact]
        public void ShouldHaveError_Validate_Items_when_NoExist_Unit_with_UnitId_Equal_Zero()
        {
            VBRequestDocumentNonPOFormDto dto = new VBRequestDocumentNonPOFormDto()
            {

                Items = new List<VBRequestDocumentNonPOItemFormDto>()
                {
                    new VBRequestDocumentNonPOItemFormDto()
                    {
                        Unit=new UnitDto()
                        {
                            Id=0
                        },
                        IsSelected=true
                    }
                }
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveError_Validate_Items_when_NoExist_Selected_UnitItem()
        {
            VBRequestDocumentNonPOFormDto dto = new VBRequestDocumentNonPOFormDto()
            {
                Items = new List<VBRequestDocumentNonPOItemFormDto>()
                {
                    new VBRequestDocumentNonPOItemFormDto()
                    {
                        IsSelected=false
                    }
                }
            };

            var result = dto.Validate(null);

            Assert.True(0 < result.Count());
        }

        [Fact]
        public void ShouldHaveNoError_Validate_Data()
        {
            VBRequestDocumentNonPOFormDto dto = new VBRequestDocumentNonPOFormDto()
            {
                Id = 1,
                Amount = 1,
                Currency = new CurrencyDto()
                {
                    Id = 1
                },
                Date = DateTimeOffset.Now,
                Purpose = "Purpose",
                RealizationEstimationDate = DateTimeOffset.Now,
                SuppliantUnit = new UnitDto()
                {
                    Id = 1,
                },
                Items = new List<VBRequestDocumentNonPOItemFormDto>()
                {
                    new VBRequestDocumentNonPOItemFormDto()
                    {
                        Unit=new UnitDto()
                        {
                            Id=1
                        },
                        IsSelected=true
                    }
                }
            };

            var result = dto.Validate(null);

            Assert.True(0 == result.Count());
        }

    }
}
