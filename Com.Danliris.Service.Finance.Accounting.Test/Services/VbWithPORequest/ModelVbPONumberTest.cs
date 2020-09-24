using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VbWIthPORequest;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Services.VbWithPORequest
{
    public class ModelVbPONumberTest
    {
        [Fact]
        public void Should_Succes_Instantiate()
        {

            ModelVbPONumber modelVbPONumber = new ModelVbPONumber()
            {
                DealQuantity = 1,
                POId = 1,
                PONo = "PONo",
                Price = 1,
                VBId = 1,
            };

            Assert.Equal(1, modelVbPONumber.DealQuantity);
            Assert.Equal(1, modelVbPONumber.POId);
            Assert.Equal("PONo", modelVbPONumber.PONo);
            Assert.Equal(1, modelVbPONumber.Price);
            Assert.Equal(1, modelVbPONumber.VBId);

        }
    }
}
