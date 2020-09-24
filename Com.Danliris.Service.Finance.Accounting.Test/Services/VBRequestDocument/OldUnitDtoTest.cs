using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.VBRequestDocument
{
   public class OldUnitDtoTest
    {

        [Fact]
        public void Should_Success_Instantiate()
        {
            OldUnitDto dto = new OldUnitDto()
            {
                code= "code",
                division=new OldDivisionDto()
                {
                    _id=1,
                    code="code",
                    name="name"
                },
                name="name",
                _id=1
            };

            Assert.Equal(1, dto._id);
            Assert.Equal("code", dto.code);
            Assert.Equal("name", dto.name);
            Assert.NotNull(dto.division);
        }
    }
}
