﻿using Com.Danliris.Service.Sales.Lib.Utilities;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Utilities
{
    public class NumbertoTextENTest
    {
        [Fact]
        public void toWords_With_DozenNumber_Return_Success()
        {
            var result = NumberToTextEN.toWords(16);
            Assert.NotEmpty(result);
            Assert.Equal("Sixteen  ", result);
        }

        [Fact]
        public void toWords_withHundredNumber_Return_Success()
        {
            var result = NumberToTextEN.toWords(200);
            Assert.NotEmpty(result);
            Assert.Equal("Two  Hundred  ", result);

        }

        [Fact]
        public void toWords_withTwentiesNumber_Return_Success()
        {
            var result = NumberToTextEN.toWords(21);
            Assert.NotEmpty(result);
            Assert.Equal("Twenty One  ", result);
        }

        //[Fact]
        //public void toWords_withThousandNumber_Return_Success()
        //{
        //    var result = NumberToTextEN.toWords(2000000000000000);
        //    Assert.NotEmpty(result);
        //    Assert.Equal("Two Thousand  ", result);
        //}
    }
}
