using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Interfaces.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.MasterCOA;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Danliris.Service.Finance.Accounting.Lib.ViewModels.OthersExpenditureProofDocumentViewModels;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction
{
    public class AutoJournalService : IAutoJournalService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IJournalTransactionService _journalTransactionService;

        public AutoJournalService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _journalTransactionService = serviceProvider.GetService<IJournalTransactionService>();
        }

        private async Task<string> GetAccountBankCOA(int accountBankId)
        {
            var http = _serviceProvider.GetService<IHttpClientService>();

            var response = await http.GetAsync(APIEndpoint.Core + $"master/account-banks/{accountBankId}");

            var responseString = await response.Content.ReadAsStringAsync();
            var jsonSerializationSetting = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore };

            var result = JsonConvert.DeserializeObject<APIDefaultResponse<AccountBank>>(responseString, jsonSerializationSetting);

            return result.data.AccountCOA;
        }

        public async Task<int> AutoJournalFromOthersExpenditureProof(OthersExpenditureProofDocumentCreateUpdateViewModel viewModel, string documentNo)
        {
            var model = new JournalTransactionModel()
            {
                Date = viewModel.Date.GetValueOrDefault(),
                Description = "Auto Journal Bukti Pengeluaran Bank Lain - Lain",
                ReferenceNo = documentNo,
                Status = "POSTED",
                Items = new List<JournalTransactionItemModel>()
            };

            model.Items = viewModel.Items.Select(item => new JournalTransactionItemModel()
            {
                COA = new COAModel()
                {
                    Id = item.COAId.GetValueOrDefault(),
                },
                Debit = item.Debit.GetValueOrDefault()
            }).ToList();

            var accountBankCOA = await GetAccountBankCOA(viewModel.AccountBankId.GetValueOrDefault());
            var creditItem = new JournalTransactionItemModel()
            {
                COA = new COAModel() { Code = accountBankCOA },
                Credit = viewModel.Items.Sum(item => item.Debit.GetValueOrDefault())
            };
            model.Items.Add(creditItem);

            return await _journalTransactionService.CreateAsync(model);
        }

        public Task<int> AutoJournalReverseFromOthersExpenditureProof(string documentNo)
        {
            return _journalTransactionService.ReverseJournalTransactionByReferenceNo(documentNo);
        }

        public async Task<int> AutoJournalVBNonPOClearence(List<int> vbRealizationIds)
        {
            var dbContext = _serviceProvider.GetService<FinanceDbContext>();

            var vbRealizations = dbContext.VBRealizationDocuments.Where(entity => vbRealizationIds.Contains(entity.Id)).ToList();
            var vbRequestIds = vbRealizations.Select(element => element.VBRequestDocumentId).ToList();
            var vbRequests = dbContext.VBRequestDocuments.Where(entity => vbRequestIds.Contains(entity.Id)).ToList();

            foreach (var vbRealization in vbRealizations)
            {


                if (vbRealization.VBRequestDocumentId > 0)
                {
                    var vbRequest = vbRequests.FirstOrDefault(element => element.Id == vbRealization.VBRequestDocumentId);
                    if (vbRequest.IsInklaring)
                    {
                        if (vbRequest.CurrencyCode == "IDR")
                        {
                            var modelInklaring = new JournalTransactionModel()
                            {
                                Date = vbRealization.Date,
                                Description = "Clearance VB Inklaring",
                                ReferenceNo = vbRealization.DocumentNo,
                                Status = "POSTED",
                                Items = new List<JournalTransactionItemModel>()
                            };

                            modelInklaring.Items.Add(new JournalTransactionItemModel()
                            {
                                COA = new COAModel()
                                {
                                    Code = $"1804.00.0.00"
                                },
                                Debit = vbRealization.Amount
                            });

                            modelInklaring.Items.Add(new JournalTransactionItemModel()
                            {
                                COA = new COAModel()
                                {
                                    Code = $"1503.00.0.00"
                                },
                                Credit = vbRealization.Amount
                            });

                            if (modelInklaring.Items.Any(element => element.COA.Code.Contains("9999")))
                                modelInklaring.Status = "DRAFT";

                            await _journalTransactionService.CreateAsync(modelInklaring);
                        }
                    }
                    else
                    {
                        var model = new JournalTransactionModel()
                        {
                            Date = vbRealization.Date,
                            Description = "Clearance VB",
                            ReferenceNo = vbRealization.DocumentNo,
                            Status = "DRAFT",
                            Items = new List<JournalTransactionItemModel>()
                        };

                        //model.Items.Add(new JournalTransactionItemModel()
                        //{
                        //    COA = new COAModel()
                        //    {
                        //        Code = $"9999.00.0.00"
                        //    },
                        //    Debit = vbRealization.Amount
                        //});

                        if (vbRealization.CurrencyCode == "IDR")
                        {
                            model.Items.Add(new JournalTransactionItemModel()
                            {
                                COA = new COAModel()
                                {
                                    Code = $"1011.00.0.00"
                                },
                                Credit = vbRealization.Amount
                            });
                        }
                        else
                        {
                            model.Items.Add(new JournalTransactionItemModel()
                            {
                                COA = new COAModel()
                                {
                                    Code = $"1012.00.0.00"
                                },
                                Credit = vbRealization.Amount
                            });
                        }

                        //if (model.Items.Any(element => element.COA.Code.Contains("9999")))
                        //    model.Status = "DRAFT";

                        await _journalTransactionService.CreateAsync(model);
                    }
                }
                else
                {
                    var model = new JournalTransactionModel()
                    {
                        Date = vbRealization.Date,
                        Description = "Clearance VB",
                        ReferenceNo = vbRealization.DocumentNo,
                        Status = "DRAFT",
                        Items = new List<JournalTransactionItemModel>()
                    };

                    //model.Items.Add(new JournalTransactionItemModel()
                    //{
                    //    COA = new COAModel()
                    //    {
                    //        Code = $"9999.00.0.00"
                    //    },
                    //    Debit = vbRealization.Amount
                    //});

                    if (vbRealization.CurrencyCode == "IDR")
                    {
                        model.Items.Add(new JournalTransactionItemModel()
                        {
                            COA = new COAModel()
                            {
                                Code = $"1011.00.0.00"
                            },
                            Credit = vbRealization.Amount
                        });
                    }
                    else
                    {
                        model.Items.Add(new JournalTransactionItemModel()
                        {
                            COA = new COAModel()
                            {
                                Code = $"1012.00.0.00"
                            },
                            Credit = vbRealization.Amount
                        });
                    }

                    //if (model.Items.Any(element => element.COA.Code.Contains("9999")))
                    //    model.Status = "DRAFT";

                    await _journalTransactionService.CreateAsync(model);
                }
            }

            return vbRealizations.Count;
        }

        public async Task<int> AutoJournalInklaring(List<int> vbRequestIds)
        {
            var dbContext = _serviceProvider.GetService<FinanceDbContext>();
            var vbRequests = dbContext.VBRequestDocuments.Where(entity => vbRequestIds.Contains(entity.Id)).ToList();

            foreach (var vbRequest in vbRequests)
            {
                if (vbRequest.IsInklaring && vbRequest.CurrencyCode == "IDR")
                {
                    var modelInklaring = new JournalTransactionModel()
                    {
                        Date = vbRequest.Date,
                        Description = "Approval VB Inklaring",
                        ReferenceNo = vbRequest.DocumentNo,
                        Status = "DRAFT",
                        Items = new List<JournalTransactionItemModel>()
                    };

                    modelInklaring.Items.Add(new JournalTransactionItemModel()
                    {
                        COA = new COAModel()
                        {
                            Code = $"1503.00.0.00"
                        },
                        Debit = vbRequest.Amount
                    });

                    //modelInklaring.Items.Add(new JournalTransactionItemModel()
                    //{
                    //    COA = new COAModel()
                    //    {
                    //        Code = $"9999.00.0.00"
                    //    },
                    //    Credit = vbRequest.Amount
                    //});

                    //if (modelInklaring.Items.Any(element => element.COA.Code.Contains("9999")))
                    //modelInklaring.Status = "DRAFT";

                    await _journalTransactionService.CreateAsync(modelInklaring);
                }
            }
            return vbRequests.Count;
        }
    }
}