using Com.Danliris.Service.Finance.Accounting.Lib;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.VBStatusReport;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Com.Danliris.Service.Finance.Accounting.Test.ViewModels.VBStatusReport
{
    public class VBStatusReportViewModelTest
    {
        [Fact]
        public void Should_Success_Instantiate()
        {
            long Id = 1;
            string VBNo = "VBNo";
            string CreateBy = "CreateBy";
            string RealizationNo = "RealizationNo";
            string Usage = "Usage";
            int Aging = 1;
            decimal Amount = 1;
            decimal RealizationAmount = 1;
            decimal Difference = 1;
            string Status = "Status";
            Unit uvm = new Unit();
            string RealizationDate = "string";
            string Date = "string";
            string DateEstimate = "string";
            string LastModifiedUtc = "string";
            ICollection<RealizationVbDetailModel> Details = new List<RealizationVbDetailModel>();

            VBStatusReportViewModel viewModel = new VBStatusReportViewModel()
            {
                Id = Id,
                VBNo = VBNo,
                CreateBy = CreateBy,
                RealizationNo = RealizationNo,
                Usage = Usage,
                Aging = Aging,
                Amount = Amount,
                RealizationAmount = RealizationAmount,
                Difference = Difference,
                Status = Status,
                Unit = uvm,
                RealizationDate = RealizationDate,
                Date = Date,
                DateEstimate = DateEstimate,
                LastModifiedUtc = LastModifiedUtc,
                Details = (ICollection<RealizationVbDetailModel>)Details,
            };



            Assert.Equal(Id, viewModel.Id);
            Assert.Equal(VBNo, viewModel.VBNo);
            Assert.Equal(CreateBy, viewModel.CreateBy);
            Assert.Equal(RealizationNo, viewModel.RealizationNo);
            Assert.Equal(Usage, viewModel.Usage);
            Assert.Equal(Aging, viewModel.Aging);
            Assert.Equal(Amount, viewModel.Amount);
            Assert.Equal(RealizationAmount, viewModel.RealizationAmount);
            Assert.Equal(Difference, viewModel.Difference);
            Assert.Equal(Status, viewModel.Status);
            Assert.Equal(uvm, viewModel.Unit);
            Assert.Equal(RealizationDate, viewModel.RealizationDate);
            Assert.Equal(Date, viewModel.Date);
            Assert.Equal(DateEstimate, viewModel.DateEstimate);
            Assert.Equal(LastModifiedUtc, viewModel.LastModifiedUtc);
            Assert.Equal((IEnumerable<RealizationVbDetailModel>)Details, viewModel.Details);


        }
    }
}