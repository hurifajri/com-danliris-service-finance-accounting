using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBWIthPO;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.RealizationVBWIthPO
{
    public class PODetailTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            PODetail pODetail = new PODetail()
            {
                PONo = "PONo",
                Price = 1
            };

            Assert.Equal("PONo", pODetail.PONo);
            Assert.Equal(1, pODetail.Price);
        }
    }
}
