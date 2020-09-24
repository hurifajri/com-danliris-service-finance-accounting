using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.ClearaceVB;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.ClearaceVB
{
    public class ClearaceVBViewModelTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            string RqstNo = "RqstNo";
            //string VBCategory = "VBCategory";
            VBType VBCategory = VBType.WithPO;
            DateTimeOffset RqstDate = DateTimeOffset.Now;
            Unit uvm = new Unit();
            string Appliciant = "Appliciant";
            string RealNo = "RealNo";
            DateTimeOffset RealDate = DateTimeOffset.Now;
            DateTimeOffset? VerDate = DateTimeOffset.Now;
            string DiffStatus = "DiffStatus";
            string Status = "Status";
            decimal DiffAmount = 100;
            DateTimeOffset? ClearanceDate = DateTimeOffset.Now;
            bool IsPosted = true;

            ClearaceVBViewModel cvvm = new ClearaceVBViewModel();
            cvvm.RqstNo = RqstNo;
            cvvm.VBCategory = VBCategory;
            cvvm.RqstDate = RqstDate;
            cvvm.Unit = uvm;
            cvvm.Appliciant = Appliciant;
            cvvm.RealNo = RealNo;
            cvvm.RealDate = RealDate;
            cvvm.VerDate = VerDate;
            cvvm.DiffStatus = DiffStatus;
            cvvm.Status = Status;
            cvvm.Unit = uvm;
            cvvm.DiffAmount = DiffAmount;
            cvvm.ClearanceDate = ClearanceDate;
            cvvm.IsPosted = IsPosted;


            Assert.Equal(RqstNo, cvvm.RqstNo);
            Assert.Equal(VBCategory, cvvm.VBCategory);
            Assert.Equal(RqstDate, cvvm.RqstDate);
            Assert.Equal(uvm, cvvm.Unit);
            Assert.Equal(Appliciant, cvvm.Appliciant);
            Assert.Equal(RealNo, cvvm.RealNo);
            Assert.Equal(RealDate, cvvm.RealDate);
            Assert.Equal(VerDate, cvvm.VerDate);
            Assert.Equal(DiffStatus, cvvm.DiffStatus);
            Assert.Equal(Status, cvvm.Status);
            Assert.Equal(uvm, cvvm.Unit);
            Assert.Equal(DiffAmount, cvvm.DiffAmount);
            Assert.Equal(ClearanceDate, cvvm.ClearanceDate);
            Assert.Equal(IsPosted, cvvm.IsPosted);

        }
    }
}
