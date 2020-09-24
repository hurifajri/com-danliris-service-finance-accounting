using Com.Danliris.Service.Finance.Accounting.Lib.Models.Memo;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Helpers
{
    public class QueryHelperTest
    {
        [Fact]
        public void Filter_Success()
        {
            var query = new List<MemoModel>()
            {
                new MemoModel()
                {
                    BuyerName ="value"
                }
            }.AsQueryable();

            Dictionary<string, object> filterDictionary = new Dictionary<string, object>();
            filterDictionary.Add("BuyerName", "value");

            var result = QueryHelper<MemoModel>.Filter(query, filterDictionary);
            Assert.NotNull(result);
            Assert.True(0 < result.Count());
        }

        [Fact]
        public void Order_Success()
        {
            var query = new List<MemoModel>()
            {
                new MemoModel()
                {
                    BuyerName ="value"
                }
            }.AsQueryable();

            Dictionary<string, string> orderDictionary = new Dictionary<string, string>();
            orderDictionary.Add("BuyerName", "desc");
            var result = QueryHelper<MemoModel>.Order(query, orderDictionary);

            Assert.True(0 < result.Count());
            Assert.NotNull(result);

        }

        [Fact]
        public void Search_Success()
        {
            var query = new List<MemoModel>()
            {
                new MemoModel()
                {
                    BuyerName ="value"
                }
            }.AsQueryable();

            List<string> searchAttributes = new List<string>()
            {
                "BuyerName"
            };

            var result = QueryHelper<MemoModel>.Search(query, searchAttributes, "value", true);
            Assert.NotNull(result);
            Assert.True(0 < result.Count());
        }

    }
}
