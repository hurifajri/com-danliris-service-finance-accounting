using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Model.VBRequestDocument
{
     public class VBRequestDocumentItemModelTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            VBRequestDocumentItemModel model = new VBRequestDocumentItemModel();

            model.SetUnit(1, "newUnitName", "newUnitCode", "user", "userAgent");
            model.SetDivision(1, "newDivisionName", "newDivisionCode", "user", "userAgent");
            model.SetIsSelected(true, "user", "userAgent");
            model.SetVBDocumentLayoutOrder(1, "user", "userAgent");
        }
    }
}
