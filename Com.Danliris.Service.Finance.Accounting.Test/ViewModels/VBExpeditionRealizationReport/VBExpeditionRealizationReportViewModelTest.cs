using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBExpeditionRealizationReport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.VBExpeditionRealizationReport
{
    public class VBExpeditionRealizationReportViewModelTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            int Id = 1;
            string RequestVBNo = "VBNo";
            string RealizationVBNo = "RealizationVBNo";
            string Applicant = "Applicant";
            Unit uvm = new Unit();
            DateTimeOffset DateUnitSend = DateTimeOffset.Now;
            string Usage = "Usage";
            string RequestCurrency = "RequestCurrency";
            string RealizationCurrency = "RealizationCurrency";
            decimal RequestAmount = 1;
            decimal RealizationAmount = 1;
            DateTimeOffset RealizationDate = DateTimeOffset.Now;
            DateTimeOffset DateVerifReceive = DateTimeOffset.Now;
            string Verificator = "Verificator";
            DateTimeOffset DateVerifSend = DateTimeOffset.Now;
            string Status = "Status";
            string VerificationStatus = "VerificationStatus";
            string Notes = "Notes";
            DateTimeOffset DateCashierReceive = DateTimeOffset.Now;
            DateTime LastModifiedUtc = DateTime.Now;

            VBExpeditionRealizationReportViewModel vrrvm = new VBExpeditionRealizationReportViewModel();
            vrrvm.Id = Id;
            vrrvm.RequestVBNo = RequestVBNo;
            vrrvm.RealizationVBNo = RealizationVBNo;
            vrrvm.Applicant = Applicant;
            vrrvm.Unit = uvm;
            vrrvm.DateUnitSend = DateUnitSend;
            vrrvm.Usage = Usage;
            vrrvm.RequestCurrency = RequestCurrency;
            vrrvm.RealizationCurrency = RealizationCurrency;
            vrrvm.RequestAmount = RequestAmount;
            vrrvm.RealizationAmount = RealizationAmount;
            vrrvm.RealizationDate = RealizationDate;
            vrrvm.DateVerifReceive = DateVerifReceive;
            vrrvm.Verificator = Verificator;
            vrrvm.DateVerifSend = DateVerifSend;
            vrrvm.Status = Status;
            vrrvm.VerificationStatus = VerificationStatus;
            vrrvm.Notes = Notes;
            vrrvm.DateCashierReceive = DateCashierReceive;
            vrrvm.LastModifiedUtc = LastModifiedUtc;


            Assert.Equal(Id, vrrvm.Id);
            Assert.Equal(RequestVBNo, vrrvm.RequestVBNo);
            Assert.Equal(RealizationVBNo, vrrvm.RealizationVBNo);
            Assert.Equal(Applicant, vrrvm.Applicant);
            Assert.Equal(uvm, vrrvm.Unit);
            Assert.Equal(DateUnitSend, vrrvm.DateUnitSend);
            Assert.Equal(Usage, vrrvm.Usage);
            Assert.Equal(RequestCurrency, vrrvm.RequestCurrency);
            Assert.Equal(RealizationCurrency, vrrvm.RealizationCurrency);
            Assert.Equal(RequestAmount, vrrvm.RequestAmount);
            Assert.Equal(RealizationAmount, vrrvm.RealizationAmount);
            Assert.Equal(RealizationDate, vrrvm.RealizationDate);
            Assert.Equal(DateVerifReceive, vrrvm.DateVerifReceive);
            Assert.Equal(Verificator, vrrvm.Verificator);
            Assert.Equal(DateVerifSend, vrrvm.DateVerifSend);
            Assert.Equal(Status, vrrvm.Status);
            Assert.Equal(VerificationStatus, vrrvm.VerificationStatus);
            Assert.Equal(Notes, vrrvm.Notes);
            Assert.Equal(DateCashierReceive, vrrvm.DateCashierReceive);
            Assert.Equal(LastModifiedUtc, vrrvm.LastModifiedUtc);

        }
    }
}
