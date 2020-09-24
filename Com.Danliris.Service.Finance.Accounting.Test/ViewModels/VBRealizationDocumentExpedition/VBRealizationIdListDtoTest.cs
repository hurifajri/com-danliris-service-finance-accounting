using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.VBRealizationDocumentExpedition
{
  public  class VBRealizationIdListDtoTest
    {
        [Fact]
        public void validate_default()
        {
            VBRealizationIdListDto dto = new VBRealizationIdListDto()
            {
                VBRealizationIds = new List<int>()
                {
                    
                }
            };
            var result = dto.Validate(null);
            Assert.True(0 < result.Count());
        }

        [Fact]
        public void validate_Null_Data()
        {
            VBRealizationIdListDto dto = new VBRealizationIdListDto()
            {
                VBRealizationIds = null
            };
            var result = dto.Validate(null);
            Assert.True(0 < result.Count());
        }
    }
}
