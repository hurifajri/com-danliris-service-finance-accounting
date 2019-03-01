﻿using Com.Danliris.Service.Finance.Accounting.Lib.Models.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities.BaseInterface;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.JournalTransaction;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.JournalTransaction
{
    public interface IJournalTransactionService : IBaseService<JournalTransactionModel>
    {
        (ReadResponse<JournalTransactionReportViewModel>, double, double) GetReport(int page, int size, DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offSet);
        MemoryStream GenerateExcel(DateTimeOffset? dateFrom, DateTimeOffset? dateTo, int offSet);
        Task<int> ReverseJournalTransactionByReferenceNo(string referenceNo);
        Task<SubLedgerReportViewModel> GetSubLedgerReport(int coaId, int month, int year, int timeoffset);
        Task<SubLedgerXlsFormat> GetSubLedgerReportXls(int coaId, int month, int year, int timeoffset);
        Task<int> PostTransactionAsync(int id);
    }
}
