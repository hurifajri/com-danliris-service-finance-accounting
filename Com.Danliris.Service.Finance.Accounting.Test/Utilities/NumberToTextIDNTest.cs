using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Utilities
{
    public class NumberToTextIDNTest
    {
        [Fact]
        public void Should_Success_Terbilang_IsNegative()
        {
            var terbilang = NumberToTextIDN.terbilang(-1);
            Assert.NotNull(terbilang);
        }

        [Fact]
        public void Should_Success_Terbilang_IsNegative_Decimal()
        {
            var terbilang = NumberToTextIDN.terbilang(-1.2);
            Assert.NotNull(terbilang);
        }


        //[Fact]
        //public void Should_Success_Terbilang_IsNegative_Decimal()
        //{
        //    var terbilang = NumberToTextIDN.terbilang(-1.2);
        //    Assert.NotNull(terbilang);
        //}

        [Fact]
        public void Should_Success_TerbilangKoma_MoreThan_4_DecimalPlace()
        {
            var terbilangKoma = NumberToTextIDN.terbilangKoma(123.45678);
            Assert.NotNull(terbilangKoma);
        }

        [Fact]
        public void Should_Success_TerbilangKoma_LessThanEqual_4_DecimalPlace()
        {
            var terbilangKoma = NumberToTextIDN.terbilangKoma(1004);
            Assert.NotNull(terbilangKoma);
        }
    }
}
