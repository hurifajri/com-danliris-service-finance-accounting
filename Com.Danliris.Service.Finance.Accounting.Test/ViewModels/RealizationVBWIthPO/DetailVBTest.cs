using Com.Danliris.Service.Finance.Accounting.WebApi.Controllers.v1.RealizationVBWIthPO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.RealizationVBWIthPO
{
    public class DetailVBTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            DetailVB detailVB = new DetailVB()
            {
                Id = 1,
                DateEstimate = DateTimeOffset.Now,
                UnitId = 1,
                UnitCode = "UnitCode",
                PONo = new List<PODetail>()
                {
                    new PODetail()
                }

            };

            Assert.Equal(1, detailVB.Id);
            Assert.True(DateTimeOffset.MinValue < detailVB.DateEstimate);
            Assert.Equal(1, detailVB.UnitId);
            Assert.Equal("UnitCode", detailVB.UnitCode);
            Assert.True(detailVB.PONo.Count() > 0);
        }
        }
}
