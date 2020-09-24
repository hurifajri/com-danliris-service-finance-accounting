using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBRealizationDocumentNonPO;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Model.VBRealizationDocument
{
    public class VBRealizationDocumentUnitCostsItemModelTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            VBRealizationDocumentNonPOUnitCostViewModel viewModel = new VBRealizationDocumentNonPOUnitCostViewModel()
            {
                Unit = new UnitViewModel()
                {
                    Division = new DivisionViewModel()
                    {
                        Code = "Code"
                    }
                }
            };
            VBRealizationDocumentUnitCostsItemModel model = new VBRealizationDocumentUnitCostsItemModel(1, viewModel);
            model.SetUnit(1, "newUnitName", "newUnitCode", "user", "userAgent");
            model.SetDivision(1, "newDivisionName", "newDivisionCode", "user", "userAgent");
            model.SetIsSelected(true, "user", "userAgent");
            model.SetAmount(1, "user", "userAgent");
        }

        [Fact]
        public void should_success_instantiate_with_dto()
        {
            UnitPaymentOrderItemDto element = new UnitPaymentOrderItemDto()
            {
                IncomeTax = new IncomeTaxDto()
                {
                    Name = "Name",
                    Rate = 1,
                    Id = 1
                },
                UseIncomeTax = true,
            };
            VBRealizationDocumentUnitCostsItemModel model = new VBRealizationDocumentUnitCostsItemModel(1, element);

        }

    }
}
