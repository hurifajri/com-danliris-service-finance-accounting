using System;
using System.Collections.Generic;
using System.Text;
using Com.Danliris.Service.Finance.Accounting.Lib.Services;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.CreditBalance;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.DownPayment;
using Moq;
using Xunit;


namespace Com.Danliris.Service.Finance.Accounting.Test.ServicesLib.ValidateService
{
   public class ValidateServiceTest
    {

        [Fact]
        public void Should_Succes_Instantiate_validateService()
        {
            var serviceProviderMock = new Mock<IServiceProvider>();
            var service = new Lib.Services.ValidateService.ValidateService(serviceProviderMock.Object);

            CreditBalanceViewModel viewModel = new CreditBalanceViewModel();
            service.Validate(viewModel);

            Assert.NotNull(service);
        }

       

        [Fact]
        public void Validate_Throws_ServiceValidationExeption()
        {
            Mock<IServiceProvider> serviceProvider = new Mock<IServiceProvider>();
            DownPaymentViewModel viewModel = new DownPaymentViewModel();

            var service = new Lib.Services.ValidateService.ValidateService(serviceProvider.Object);
            Assert.Throws<ServiceValidationException>(() => service.Validate(viewModel));

        }


    }
}
