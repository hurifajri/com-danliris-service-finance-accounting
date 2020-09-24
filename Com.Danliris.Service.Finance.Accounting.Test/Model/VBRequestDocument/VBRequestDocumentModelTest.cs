using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRequestDocument;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.Model.VBRequestDocument
{
    public class VBRequestDocumentModelTest
    {
        [Fact]
        public void should_success_instantiate()
        {
            VBRequestDocumentModel model = new VBRequestDocumentModel();

            model.SetCurrency(1, "IDR", "Rp", 2, "description", "user", "userAgent");
            model.SetAmount(1, "user", "userAgent");
            model.SetPurpose("newPurpose", "user", "userAgent");
        }
    }
}
