using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.DailyBankTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.OthersExpenditureProofDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.PaymentDispositionNote;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.DailyBankTransaction
{
    public class AutoDailyBankTransactionService : IAutoDailyBankTransactionService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IDailyBankTransactionService _dailyBankTransactionService;

        public AutoDailyBankTransactionService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _dailyBankTransactionService = serviceProvider.GetService<IDailyBankTransactionService>();
        }

        public Task<int> AutoCreateFromPaymentDisposition(PaymentDispositionNoteModel model)
        {
            var nominal = model.Items.Sum(item => (decimal)item.TotalPaid * (decimal)model.CurrencyRate);
            var dailyBankTransactionModel = new DailyBankTransactionModel()
            {
                AccountBankAccountName = model.BankAccountName,
                AccountBankAccountNumber = model.BankAccountNumber,
                AccountBankCode = model.BankCode,
                AccountBankCurrencyCode = model.BankCurrencyCode,
                AccountBankCurrencyId = model.BankCurrencyId,
                AccountBankCurrencySymbol = model.BankCurrencyCode,
                AccountBankId = model.BankId,
                AccountBankName = model.BankName,
                Date = model.PaymentDate,
                Nominal = nominal,
                ReferenceNo = model.PaymentDispositionNo,
                ReferenceType = "Pembayaran Disposisi",
                Remark = model.CurrencyCode != "IDR" ? $"Pembayaran atas {model.BankCurrencyCode} dengan nominal {string.Format("{0:n}", nominal)} dan kurs {model.CurrencyCode}" : "",
                SourceType = model.TransactionType,
                SupplierCode = model.SupplierCode,
                SupplierId = model.SupplierId,
                SupplierName = model.SupplierName,
                Status = "OUT"
            };
            return _dailyBankTransactionService.CreateAsync(dailyBankTransactionModel);
        }

        public Task<int> AutoRevertFromPaymentDisposition(PaymentDispositionNoteModel model)
        {
            var nominal = model.Items.Sum(item => (decimal)item.TotalPaid * (decimal)model.CurrencyRate);
            var dailyBankTransactionModel = new DailyBankTransactionModel()
            {
                AccountBankAccountName = model.BankAccountName,
                AccountBankAccountNumber = model.BankAccountNumber,
                AccountBankCode = model.BankCode,
                AccountBankCurrencyCode = model.BankCurrencyCode,
                AccountBankCurrencyId = model.BankCurrencyId,
                AccountBankCurrencySymbol = model.BankCurrencyCode,
                AccountBankId = model.BankId,
                AccountBankName = model.BankName,
                Date = model.PaymentDate,
                Nominal = nominal,
                ReferenceNo = model.PaymentDispositionNo,
                ReferenceType = "Pembayaran Disposisi",
                Remark = model.CurrencyCode != "IDR" ? $"Pembayaran atas {model.BankCurrencyCode} dengan nominal {string.Format("{0:n}", nominal)} dan kurs {model.CurrencyCode}" : "",
                SourceType = model.TransactionType,
                SupplierCode = model.SupplierCode,
                SupplierId = model.SupplierId,
                SupplierName = model.SupplierName,
                Status = "IN"
            };
            return _dailyBankTransactionService.CreateAsync(dailyBankTransactionModel);
        }

        private async Task<AccountBank> GetAccountBank(int accountBankId)
        {
            var http = _serviceProvider.GetService<IHttpClientService>();

            var response = await http.GetAsync(APIEndpoint.Core + $"master/account-banks/{accountBankId}");

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonSerializationSetting = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore };

            var result = JsonConvert.DeserializeObject<APIDefaultResponse<AccountBank>>(responseString, jsonSerializationSetting);

            return result.data;
        }

        public async Task<int> AutoCreateFromOthersExpenditureProofDocument(OthersExpenditureProofDocumentModel model, List<OthersExpenditureProofDocumentItemModel> itemModels)
        {
            var accountBank = await GetAccountBank(model.AccountBankId);
            var dailyBankTransactionModel = new DailyBankTransactionModel()
            {
                AccountBankAccountName = accountBank.AccountName,
                AccountBankAccountNumber = accountBank.AccountNumber,
                AccountBankCode = accountBank.BankCode,
                AccountBankCurrencyCode = accountBank.Currency.Code,
                AccountBankCurrencyId = accountBank.Currency.Id,
                AccountBankCurrencySymbol = accountBank.Currency.Symbol,
                AccountBankId = accountBank.Id,
                AccountBankName = accountBank.BankName,
                Date = model.Date,
                Nominal = itemModels.Sum(item => item.Debit),
                ReferenceNo = model.DocumentNo,
                Remark = "Pembayaran Lain - lain",
                SourceType = model.Type,
                Status = "OUT"
            };
            return await _dailyBankTransactionService.CreateAsync(dailyBankTransactionModel);
        }

        public async Task<int> AutoRevertFromOthersExpenditureProofDocument(OthersExpenditureProofDocumentModel model, List<OthersExpenditureProofDocumentItemModel> itemModels)
        {
            var accountBank = await GetAccountBank(model.AccountBankId);
            var dailyBankTransactionModel = new DailyBankTransactionModel()
            {
                AccountBankAccountName = accountBank.AccountName,
                AccountBankAccountNumber = accountBank.AccountNumber,
                AccountBankCode = accountBank.BankCode,
                AccountBankCurrencyCode = accountBank.Currency.Code,
                AccountBankCurrencyId = accountBank.Currency.Id,
                AccountBankCurrencySymbol = accountBank.Currency.Symbol,
                AccountBankId = accountBank.Id,
                AccountBankName = accountBank.BankName,
                Date = model.Date,
                Nominal = itemModels.Sum(item => item.Debit),
                ReferenceNo = model.DocumentNo,
                Remark = "Pembayaran Lain - lain",
                SourceType = model.Type,
                Status = "IN"
            };
            return await _dailyBankTransactionService.CreateAsync(dailyBankTransactionModel);
        }
    }
}