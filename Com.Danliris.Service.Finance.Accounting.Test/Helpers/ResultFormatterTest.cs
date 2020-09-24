using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.MasterCOA;
using Com.Danliris.Service.Finance.Accounting.WebApi.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Xunit;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Com.Danliris.Service.Finance.Accounting.Test.Helpers
{
    public class ResultFormatterTest
    {
        [Fact]
        public void Fail_Return_Success()
        {
            //Setup
            string ApiVersion = "V1";
            int StatusCode = 200;
            string Message = "OK";

            COAViewModel viewModel = new COAViewModel();
            ResultFormatter formatter = new ResultFormatter(ApiVersion, StatusCode, Message);
            var validationContext = new ValidationContext(viewModel);

            var errorData = new
            {
                WarningError = "Format Not Match"
            };

            string error = JsonConvert.SerializeObject(errorData);
            var exception = new ServiceValidationException(validationContext, new List<ValidationResult>() { new ValidationResult(error, new List<string>() { "WarningError" }) });

            //Act
            var result = formatter.Fail(exception);

            //Assert
            Assert.True(0 < result.Count());
        }

        [Fact]
        public void Fail_Throws_Exception()
        {
            //Setup
            string ApiVersion = "V1";
            int StatusCode = 200;
            string Message = "OK";

            COAViewModel viewModel = new COAViewModel();
            ResultFormatter formatter = new ResultFormatter(ApiVersion, StatusCode, Message);
            var validationContext = new ValidationContext(viewModel);
            var exception = new ServiceValidationException(validationContext, new List<ValidationResult>() { new ValidationResult("errorMessaage", new List<string>() { "WarningError" }) });

            //Act
            var result = formatter.Fail(exception);

            //Assert
            Assert.True(0 < result.Count());
        }
    }
}
