using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.JournalTransaction;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.BudgetCashflow;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.CacheService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Moonlay.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.BudgetCashflow
{
    public class BudgetCashflowService : IBudgetCashflowService
    {
        private const string UserAgent = "finance-service";
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;
        private readonly List<CurrencyDto> _currencies;
        private readonly List<DivisionDto> _divisions;
        private readonly IServiceProvider _serviceProvider;
        private readonly List<UnitDto> _units;

        public BudgetCashflowService(IServiceProvider serviceProvider)
        {
            _dbContext = serviceProvider.GetService<FinanceDbContext>();
            _identityService = serviceProvider.GetService<IIdentityService>();

            var cacheService = serviceProvider.GetService<ICacheService>();
            var jsonCurrencies = cacheService.GetString("Currency");
            _currencies = JsonConvert.DeserializeObject<List<CurrencyDto>>(jsonCurrencies, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            });

            var jsonUnits = cacheService.GetString("Unit");
            _units = JsonConvert.DeserializeObject<List<UnitDto>>(jsonUnits, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            });

            var jsonDivisions = cacheService.GetString("Division");
            _divisions = JsonConvert.DeserializeObject<List<DivisionDto>>(jsonDivisions, new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            });

            _serviceProvider = serviceProvider;
        }

        public ReadResponse<BudgetCashflowMasterDto> GetBudgetCashflowMasterLayout(string keyword, int page, int size)
        {
            var budgetCashflowTypeQuery = _dbContext.BudgetCashflowTypes.OrderBy(entity => entity.LayoutOrder).AsQueryable();
            var budgetCashflowCategoryQuery = _dbContext.BudgetCashflowCategories.OrderBy(entity => entity.LayoutOrder).AsQueryable();
            var budgetCashflowSubCategoryQuery = _dbContext.BudgetCashflowSubCategories.OrderBy(entity => entity.LayoutOrder).AsQueryable();

            var query = from cashflowType in budgetCashflowTypeQuery

                        join cashflowCategory in budgetCashflowCategoryQuery on cashflowType.Id equals cashflowCategory.CashflowTypeId into cashflowTypeWithCategories
                        from cashflowTypeWithCategory in cashflowTypeWithCategories.DefaultIfEmpty()

                        join cashflowSubCategory in budgetCashflowSubCategoryQuery on cashflowTypeWithCategory.Id equals cashflowSubCategory.CashflowCategoryId into cashflowCategoryWithSubCategories
                        from cashflowCategoryWithSubCategory in cashflowCategoryWithSubCategories.DefaultIfEmpty()

                        select new BudgetCashflowMasterDto(cashflowType, cashflowTypeWithCategory, cashflowCategoryWithSubCategory);

            return new ReadResponse<BudgetCashflowMasterDto>(query.OrderBy(entity => entity.CashflowTypeLayoutOrder).ThenBy(entity => entity.CashType).ThenBy(entity => entity.CashflowCategoryLayoutOrder).ThenBy(entity => entity.CashflowSubCategoryLayoutOrder).ToList(), query.Count(), new Dictionary<string, string>(), new List<string>());
        }

        public int CreateBudgetCashflowUnit(CashflowUnitFormDto form)
        {
            var models = new List<BudgetCashflowUnitModel>();

            foreach (var item in form.Items)
            {
                var model = new BudgetCashflowUnitModel(form.CashflowSubCategoryId, form.UnitId, form.DivisionId, form.Date.AddHours(_identityService.TimezoneOffset).AddMonths(1), item.CurrencyId, item.CurrencyNominal, item.Nominal, item.Total);
                EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
                models.Add(model);
            }

            _dbContext.BudgetCashflowUnits.AddRange(models);
            return _dbContext.SaveChanges();
        }

        //public int 

        //public List<BudgetCashflowUnitDto> GetBudgetCashflowUnit(int unitId, DateTimeOffset date)
        //{
        //    var query = from cashflowType in _dbContext.BudgetCashflowTypes

        //                join cashflowCategory in _dbContext.BudgetCashflowCategories on cashflowType.Id equals cashflowCategory.CashflowTypeId into cashflowTypeCategories
        //                from cashflowTypeCategory in cashflowTypeCategories.DefaultIfEmpty()

        //                join cashflowSubCategory in _dbContext.BudgetCashflowSubCategories on cashflowTypeCategory.Id equals cashflowSubCategory.CashflowCategoryId into cashflowCategorySubCategories
        //                from cashflowCategorySubCategory in cashflowCategorySubCategories.DefaultIfEmpty()

        //                select new
        //                {
        //                    CashflowTypeId = cashflowType.Id,
        //                    CashflowTypeName = cashflowType.Name,
        //                    CashflowTypeLayoutOrder = cashflowType.LayoutOrder,
        //                    CashflowCashType = cashflowTypeCategory != null ? cashflowTypeCategory.Type : 0,
        //                    CashflowCategoryId = cashflowTypeCategory != null ? cashflowTypeCategory.Id : 0,
        //                    CashflowCategoryName = cashflowTypeCategory != null ? cashflowTypeCategory.Name : "",
        //                    CashflowCategoryLayoutOrder = cashflowTypeCategory != null ? cashflowTypeCategory.LayoutOrder : 0,
        //                    CashflowSubCategoryId = cashflowCategorySubCategory != null ? cashflowCategorySubCategory.Id : 0,
        //                    CashflowSubCategoryName = cashflowCategorySubCategory != null ? cashflowCategorySubCategory.Name : "",
        //                    CashflowSubCategoryLayoutOrder = cashflowCategorySubCategory != null ? cashflowCategorySubCategory.LayoutOrder : 0,
        //                    CashflowSubCategoryReadOnly = cashflowCategorySubCategory != null && cashflowCategorySubCategory.IsReadOnly
        //                };

        //    query = query.OrderBy(entity => entity.CashflowTypeLayoutOrder).ThenBy(entity => entity.CashflowCashType).ThenBy(entity => entity.CashflowCategoryLayoutOrder).ThenBy(entity => entity.CashflowSubCategoryLayoutOrder);

        //    var result = new List<BudgetCashflowUnitDto>();

        //    var cashflowSubCategoryIds = query.Select(entity => entity.CashflowSubCategoryId).ToList();
        //    var cashflowUnits = _dbContext.BudgetCashflowUnits
        //        .Where(entity => entity.UnitId == unitId && cashflowSubCategoryIds.Contains(entity.BudgetCashflowSubCategoryId) && entity.Month == date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Month && entity.Year == date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Year)
        //        .ToList();

        //    var previousCashflowTypeId = 0;
        //    var previousCashflowCategoryId = 0;
        //    var previousCashInCashOut = (CashType)0;
        //    foreach (var item in query.ToList())
        //    {
        //        var selectedCashflowUnits = cashflowUnits.Where(element => element.BudgetCashflowSubCategoryId == item.CashflowSubCategoryId).ToList();
        //        var cashflowItem = new BudgetCashflowUnitDto(item.CashflowTypeId, item.CashflowTypeName, item.CashflowCategoryId, item.CashflowCategoryName, item.CashflowSubCategoryId, item.CashflowSubCategoryName, item.CashflowSubCategoryReadOnly, item.CashflowCashType);

        //        if (item.CashflowTypeId != previousCashflowTypeId)
        //        {
        //            previousCashflowTypeId = item.CashflowTypeId;
        //            cashflowItem.UseSection();
        //        }

        //        if (item.CashflowCashType != previousCashInCashOut)
        //        {
        //            previousCashInCashOut = item.CashflowCashType;
        //            cashflowItem.UseGroup();
        //        }

        //        if (item.CashflowCategoryId != previousCashflowCategoryId)
        //        {
        //            previousCashflowCategoryId = item.CashflowCategoryId;
        //            cashflowItem.LabelOnly();
        //            result.Add(cashflowItem);
        //            cashflowItem = new BudgetCashflowUnitDto(cashflowItem);
        //        }

        //        var isFirst = true;
        //        if (selectedCashflowUnits.Count > 0)
        //            foreach (var cashflowUnit in selectedCashflowUnits)
        //            {
        //                cashflowItem = new BudgetCashflowUnitDto(cashflowItem, isFirst);
        //                var currency = _currencies.FirstOrDefault(element => element.Id.GetValueOrDefault() == cashflowUnit.CurrencyId);
        //                cashflowItem.SetNominal(currency, cashflowUnit.CurrencyNominal, cashflowUnit.Nominal, cashflowUnit.Total);
        //                result.Add(cashflowItem);
        //                isFirst = false;
        //            }
        //        else if (item.CashflowSubCategoryId > 0)
        //        {
        //            cashflowItem.ShowLabel();
        //            result.Add(cashflowItem);
        //        }
        //    }

        //    var cashflowTypeIds = query.Select(element => element.CashflowTypeId).Distinct().ToList();
        //    var counter = 0;

        //    //foreach (var cashflowTypeId in cashflowTypeIds)
        //    //{
        //    //    var cashInSummary = result.Where(element => element.CashflowTypeId == cashflowTypeId && element.TypeName == CashType.In.ToDescriptionString());
        //    //    var cashInCurrencies = cashInSummary.Where(element => element.Currency != null && element.Currency.Id > 0).Select(element => element.Currency.Id).Distinct().ToList();

        //    //    foreach (var currency in cashInCurrencies)
        //    //    {

        //    //    }
        //    //}

        //    foreach (var cashflowTypeId in cashflowTypeIds)
        //    {
        //        var item = result.FirstOrDefault(element => element.CashflowTypeId == cashflowTypeId);
        //        item.SetCashflowTypeRowspan(result.Count(element => element.CashflowTypeId == cashflowTypeId));

        //        var cashInSummary = result.Where(element => element.CashflowTypeId == cashflowTypeId && element.TypeName == CashType.In.ToDescriptionString());
        //        var cashOutSummary = result.Where(element => element.CashflowTypeId == cashflowTypeId && element.TypeName == CashType.Out.ToDescriptionString());

        //        var cashIn = result.FirstOrDefault(element => element.CashflowTypeId == cashflowTypeId && element.TypeName == CashType.In.ToDescriptionString());
        //        if (cashIn != null)
        //            cashIn.SetGroupRowspan(result.Count(element => element.CashflowTypeId == cashflowTypeId && element.TypeName == CashType.In.ToDescriptionString()));

        //        var cashOut = result.FirstOrDefault(element => element.CashflowTypeId == cashflowTypeId && element.TypeName == CashType.Out.ToDescriptionString());
        //        if (cashOut != null)
        //            cashOut.SetGroupRowspan(result.Count(element => element.CashflowTypeId == cashflowTypeId && element.TypeName == CashType.Out.ToDescriptionString()));


        //    }

        //    return result;
        //}

        private async Task<List<BudgetCashflowByCategoryDto>> GetCurrencyByCategoryAndDivisionId(int unitId, int divisionId, List<int> categoryIds)
        {
            var jsonSerializerSettings = new JsonSerializerSettings
            {
                MissingMemberHandling = MissingMemberHandling.Ignore
            };

            var http = _serviceProvider.GetService<IHttpClientService>();
            var uri = APIEndpoint.Purchasing + $"budget-cashflows/best-case/by-category?unitId={unitId}&divisionId={divisionId}&categoryIds={JsonConvert.SerializeObject(categoryIds)}";
            var response = await http.GetAsync(uri);

            var result = new BaseResponse<List<BudgetCashflowByCategoryDto>>();
            result.data = new List<BudgetCashflowByCategoryDto>();

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                result = JsonConvert.DeserializeObject<BaseResponse<List<BudgetCashflowByCategoryDto>>>(responseContent, jsonSerializerSettings);
            }
            return result.data;
        }

        public async Task<List<BudgetCashflowItemDto>> GetBudgetCashflowUnit(int unitId, DateTimeOffset date)
        {
            var query = from cashflowType in _dbContext.BudgetCashflowTypes

                        join cashflowCategory in _dbContext.BudgetCashflowCategories on cashflowType.Id equals cashflowCategory.CashflowTypeId into cashflowTypeCategories
                        from cashflowTypeCategory in cashflowTypeCategories.DefaultIfEmpty()

                        join cashflowSubCategory in _dbContext.BudgetCashflowSubCategories on cashflowTypeCategory.Id equals cashflowSubCategory.CashflowCategoryId into cashflowCategorySubCategories
                        from cashflowCategorySubCategory in cashflowCategorySubCategories.DefaultIfEmpty()

                        select new
                        {
                            CashflowTypeId = cashflowType.Id,
                            CashflowTypeName = cashflowType.Name,
                            CashflowTypeLayoutOrder = cashflowType.LayoutOrder,
                            CashflowCashType = cashflowTypeCategory != null ? cashflowTypeCategory.Type : 0,
                            CashflowCategoryId = cashflowTypeCategory != null ? cashflowTypeCategory.Id : 0,
                            CashflowCategoryName = cashflowTypeCategory != null ? cashflowTypeCategory.Name : "",
                            CashflowCategoryLayoutOrder = cashflowTypeCategory != null ? cashflowTypeCategory.LayoutOrder : 0,
                            CashflowSubCategoryId = cashflowCategorySubCategory != null ? cashflowCategorySubCategory.Id : 0,
                            CashflowSubCategoryName = cashflowCategorySubCategory != null ? cashflowCategorySubCategory.Name : "",
                            CashflowSubCategoryLayoutOrder = cashflowCategorySubCategory != null ? cashflowCategorySubCategory.LayoutOrder : 0
                        };

            query = query.Where(entity => entity.CashflowSubCategoryId > 0).OrderBy(entity => entity.CashflowTypeLayoutOrder).ThenBy(entity => entity.CashflowCashType).ThenBy(entity => entity.CashflowCategoryLayoutOrder).ThenBy(entity => entity.CashflowSubCategoryLayoutOrder);

            var selectedCashflows = query.Select(entity => new { entity.CashflowTypeId, entity.CashflowTypeLayoutOrder }).Distinct().OrderBy(entity => entity.CashflowTypeLayoutOrder).ToList();

            var month = date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Month;
            var year = date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Year;
            var cashflowSubCategoryIds = query.Select(entity => entity.CashflowSubCategoryId).ToList();
            var cashflowUnits = _dbContext.BudgetCashflowUnits
                .Where(entity => entity.UnitId == unitId && cashflowSubCategoryIds.Contains(entity.BudgetCashflowSubCategoryId) && entity.Month == month && entity.Year == year)
                .ToList();

            var summaries = new List<SummaryPerType>();
            var categories = _dbContext.BudgetCashflowCategories.ToList();
            var cashflowSubCategories = _dbContext.BudgetCashflowSubCategories.ToList();
            foreach (var selectedCashflow in selectedCashflows)
            {
                var cashflowType = _dbContext.BudgetCashflowTypes.FirstOrDefault(entity => entity.Id == selectedCashflow.CashflowTypeId);
                var summary = new SummaryPerType();
                summary.SetCashflowType(cashflowType);
                //var sectionRowSpan = 0;
                var cashTypes = query.Select(entity => entity.CashflowCashType).Distinct().ToList();
                foreach (var cashType in cashTypes)
                {
                    //var groupRowSpan = 0;
                    var selectedCashflowCategories = query.Where(entity => entity.CashflowTypeId == selectedCashflow.CashflowTypeId && entity.CashflowCashType == cashType).Select(entity => new { entity.CashflowCategoryId, entity.CashflowCategoryLayoutOrder }).Distinct().OrderBy(entity => entity.CashflowCategoryLayoutOrder).ToList();

                    foreach (var selectedCashflowCategory in selectedCashflowCategories)
                    {
                        var cashflowCategory = categories.FirstOrDefault(entity => entity.Id == selectedCashflowCategory.CashflowCategoryId);
                        var selectedSubCategories = query.Where(entity => entity.CashflowCategoryId == selectedCashflowCategory.CashflowCategoryId).Select(entity => new { entity.CashflowSubCategoryId, entity.CashflowSubCategoryLayoutOrder }).Distinct().OrderBy(entity => entity.CashflowSubCategoryLayoutOrder).ToList();

                        summary.CashflowCategories.Add(cashflowCategory);

                        foreach (var selectedSubCategory in selectedSubCategories)
                        {
                            var cashflowSubCategory = cashflowSubCategories.FirstOrDefault(entity => entity.Id == selectedSubCategory.CashflowSubCategoryId);

                            if (cashflowSubCategory != null)
                                if (cashflowSubCategory.IsReadOnly)
                                {
                                    var categoryIds = JsonConvert.DeserializeObject<List<int>>(cashflowSubCategory.PurchasingCategoryIds);
                                    //var selectedCashflowUnits = await GetCurrencyByCategoryAndDivisionId(unitId, 0, categoryIds);
                                    var selectedCashflowUnits = new List<BudgetCashflowByCategoryDto>();

                                    if (selectedCashflowUnits != null && selectedCashflowUnits.Count > 0)
                                        foreach (var cashflowUnit in selectedCashflowUnits)
                                        {
                                            summary.Items.Add(new BudgetCashflowUnitDto(cashflowType, cashflowCategory, cashflowSubCategory, new BudgetCashflowUnitModel(0, unitId, 0, date, cashflowUnit.CurrencyId, cashflowUnit.CurrencyNominal, cashflowUnit.Nominal, cashflowUnit.ActualNominal)));
                                        }
                                    else
                                        summary.Items.Add(new BudgetCashflowUnitDto(cashflowType, cashflowCategory, cashflowSubCategory, new BudgetCashflowUnitModel()));
                                }
                                else
                                {
                                    var selectedCashflowUnits = cashflowUnits.Where(element => element.BudgetCashflowSubCategoryId == selectedSubCategory.CashflowSubCategoryId).ToList();

                                    if (selectedCashflowUnits.Count > 0)
                                        foreach (var cashflowUnit in selectedCashflowUnits)
                                        {
                                            summary.Items.Add(new BudgetCashflowUnitDto(cashflowType, cashflowCategory, cashflowSubCategory, cashflowUnit));
                                        }
                                    else
                                        summary.Items.Add(new BudgetCashflowUnitDto(cashflowType, cashflowCategory, cashflowSubCategory, new BudgetCashflowUnitModel()));
                                }
                            else
                            {
                                continue;
                            }
                        }
                    }

                    var totalCashTypes = summary
                        .Items
                        .Where(element => element.CashflowType.Id == selectedCashflow.CashflowTypeId && element.CashflowCategory.Type == cashType && element.CashflowUnit.CurrencyId > 0)
                        .GroupBy(element => element.CashflowUnit.CurrencyId)
                        .Select(element =>
                        {
                            var nominal = element.Sum(sum => sum.CashflowUnit.Nominal);
                            var currencyNominal = element.Sum(sum => sum.CashflowUnit.CurrencyNominal);
                            var total = element.Sum(sum => sum.CashflowUnit.Total);

                            return new TotalCashType(selectedCashflow.CashflowTypeId, cashType, element.Key, nominal, currencyNominal, total);
                        })
                        .ToList();

                    summary.AddTotalCashTypes(totalCashTypes);
                }

                summaries.Add(summary);


            }

            var result = new List<BudgetCashflowItemDto>();

            foreach (var summary in summaries)
            {
                var summaryItem = new BudgetCashflowItemDto(cashflowTypeId: summary.CashflowType.Id, cashflowTypeName: summary.CashflowType.Name, isUseSection: true);

                var cashCategoryRow = summary.CashflowCategories.Where(element => element.CashflowTypeId == summary.CashflowType.Id).Select(element => element.Id).Distinct().Count();
                var itemRow = summary.Items.Where(element => element.CashflowSubCategory.Id > 0 && element.CashflowType.Id > 0).Count();
                var totalInRow = summary.TotalCashTypes.Where(element => element.CashflowTypeId == summary.CashflowType.Id && element.CashType == CashType.In).Count() == 0 ? 1 : summary.TotalCashTypes.Where(element => element.CashflowTypeId == summary.CashflowType.Id && element.CashType == CashType.In).Count();
                var totalOutRow = summary.TotalCashTypes.Where(element => element.CashflowTypeId == summary.CashflowType.Id && element.CashType == CashType.Out).Count() == 0 ? 1 : summary.TotalCashTypes.Where(element => element.CashflowTypeId == summary.CashflowType.Id && element.CashType == CashType.Out).Count();
                var differenceRow = summary.GetDifference(summary.CashflowType.Id).Count == 0 ? 1 : summary.GetDifference(summary.CashflowType.Id).Count;
                summaryItem.SetSectionRowSpan(sectionRowSpan: cashCategoryRow + itemRow + totalInRow + totalOutRow + differenceRow);

                var cashTypes = summary.CashflowCategories.Where(element => element.CashflowTypeId == summary.CashflowType.Id).Select(element => element.Type).Distinct().ToList();

                foreach (var cashType in cashTypes)
                {
                    var selectedCasCategoryRow = summary.CashflowCategories.Where(element => element.CashflowTypeId == summary.CashflowType.Id && element.Type == cashType).Count();
                    var selectedItemRow = summary.Items.Where(element => element.CashflowType.Id == summary.CashflowType.Id && element.CashflowCategory.Type == cashType).Count();
                    var selectedTotalRow = summary.TotalCashTypes.Where(element => element.CashflowTypeId == summary.CashflowType.Id && element.CashType == cashType).Count() == 0 ? 1 : summary.TotalCashTypes.Where(element => element.CashflowTypeId == summary.CashflowType.Id && element.CashType == cashType).Count();
                    summaryItem.SetGroup(isUseGroup: true, groupRowSpan: selectedCasCategoryRow + selectedItemRow + selectedTotalRow, type: cashType);

                    var cashflowCategories = summary.CashflowCategories.Where(element => element.CashflowTypeId == summary.CashflowType.Id && element.Type == cashType).ToList();

                    foreach (var cashflowCategory in cashflowCategories)
                    {
                        summaryItem.SetLabelOnly(cashflowCategory);
                        result.Add(summaryItem);
                        summaryItem = new BudgetCashflowItemDto(cashflowCategory);

                        var items = summary.Items.Where(element => element.CashflowCategory.Id == cashflowCategory.Id).ToList();

                        var isShowSubCategoryLabel = false;
                        var previousSubCategoryId = 0;
                        foreach (var item in items)
                        {
                            if (item.CashflowSubCategory == null)
                                continue;

                            if (item.CashflowSubCategory.Id != previousSubCategoryId)
                            {
                                isShowSubCategoryLabel = true;
                                previousSubCategoryId = item.CashflowSubCategory.Id;
                            }

                            result.Add(new BudgetCashflowItemDto(isShowSubCategoryLabel, item, _currencies));
                            isShowSubCategoryLabel = false;
                        }
                    }

                    var totalCashTypes = summary.TotalCashTypes.Where(element => element.CashflowTypeId == summary.CashflowType.Id && element.CashType == cashType).ToList();
                    var isShowTotalLabel = true;
                    if (totalCashTypes.Count > 0)
                        foreach (var totalCashType in totalCashTypes)
                        {
                            result.Add(new BudgetCashflowItemDto(isShowTotalLabel, totalLabel: cashType == CashType.In ? $"Total Penerimaan {summary.CashflowType.Name}" : $"Total Pengeluaran {summary.CashflowType.Name}", totalCashType, _currencies));
                            isShowTotalLabel = false;
                        }
                    else
                        result.Add(new BudgetCashflowItemDto(isShowTotalLabel, totalLabel: cashType == CashType.In ? $"Total Penerimaan {summary.CashflowType.Name}" : $"Total Pengeluaran {summary.CashflowType.Name}", new TotalCashType(), _currencies));
                }

                var differenceCashTypes = summary.GetDifference(summary.CashflowType.Id);
                var isShowDifferenceLabel = true;
                if (differenceCashTypes.Count > 0)
                    foreach (var differenceCashType in differenceCashTypes)
                    {
                        result.Add(new BudgetCashflowItemDto(isShowDifferenceLabel, differenceLabel: $"Surplus/Deficit-Kas dari {summary.CashflowType.Name}", differenceCashType, _currencies, isShowDifference: true));
                        isShowDifferenceLabel = false;
                    }
                else
                    result.Add(new BudgetCashflowItemDto(isShowDifferenceLabel, differenceLabel: $"Surplus/Deficit-Kas dari {summary.CashflowType.Name}", new TotalCashType(), _currencies, isShowDifference: true));
            }

            // Saldo Awal
            var initialBalances = _dbContext.InitialCashBalances.Where(entity => entity.UnitId == unitId && entity.Month == date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Month && entity.Year == date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Year).ToList();
            var isShowSummaryBalance = true;
            if (initialBalances.Count > 0)
                foreach (var initialBalance in initialBalances)
                {
                    var currency = _currencies.FirstOrDefault(element => element.Id == initialBalance.CurrencyId);
                    result.Add(new BudgetCashflowItemDto(isShowSummaryBalance, "Saldo Awal Kas", currency, initialBalance.Nominal, initialBalance.CurrencyNominal, initialBalance.Total, true, "saldo", false));
                    isShowSummaryBalance = false;
                }
            else
            {
                result.Add(new BudgetCashflowItemDto(isShowSummaryBalance, "Saldo Awal Kas", new CurrencyDto(), 0, 0, 0, true, "saldo", false));
                isShowSummaryBalance = false;
            }

            // Total Surplus/Deficit Kas
            var differenceSummaries = result
                .Where(element => element.IsShowDifference && element.Currency != null)
                .GroupBy(element => element.Currency.Id)
                .Select(element => new
                {
                    CurrencyId = element.Key,
                    CurrencyNominal = element.Sum(s => s.CurrencyNominal),
                    Nominal = element.Sum(s => s.Nominal),
                    Actual = element.Sum(s => s.Total)
                })
                .ToList();
            var isShowSummaryLabel = true;
            foreach (var item in differenceSummaries)
            {
                var currency = _currencies.FirstOrDefault(element => element.Id == item.CurrencyId);
                result.Add(new BudgetCashflowItemDto(isShowSummaryLabel, "TOTAL SURPLUS/DEFISIT KAS", currency, item.Nominal, item.CurrencyNominal, item.Actual, true));
                isShowSummaryLabel = false;
            }

            // Saldo Akhir
            var summaryCurrencyIds = new List<int>();
            summaryCurrencyIds.AddRange(initialBalances.Select(element => element.CurrencyId).Where(element => element > 0).Distinct().ToList());
            summaryCurrencyIds.AddRange(differenceSummaries.Select(element => element.CurrencyId).Where(element => element > 0).Distinct().ToList());

            summaryCurrencyIds = summaryCurrencyIds.Distinct().ToList();
            isShowSummaryBalance = true;
            if (summaryCurrencyIds.Count > 0)
                foreach (var currencyId in summaryCurrencyIds)
                {
                    var currency = _currencies.FirstOrDefault(element => element.Id == currencyId);
                    var initialBalance = initialBalances.FirstOrDefault(element => element.CurrencyId == currencyId);
                    var differenceSummary = differenceSummaries.FirstOrDefault(element => element.CurrencyId == currencyId);

                    var nominal = 0.0;
                    var currencyNominal = 0.0;
                    var total = 0.0;

                    if (initialBalance != null)
                    {
                        nominal += initialBalance.Nominal;
                        currencyNominal += initialBalance.CurrencyNominal;
                        total += initialBalance.Total;
                    }

                    if (differenceSummary != null)
                    {
                        nominal += differenceSummary.Nominal;
                        currencyNominal += differenceSummary.CurrencyNominal;
                        total += differenceSummary.Actual;
                    }

                    result.Add(new BudgetCashflowItemDto(isShowSummaryBalance, "Saldo Akhir Kas", currency, nominal, currencyNominal, total, true, "saldo", true));
                    isShowSummaryBalance = false;
                }
            else
            {
                result.Add(new BudgetCashflowItemDto(isShowSummaryBalance, "Saldo Akhir Kas", new CurrencyDto(), 0, 0, 0, true, "saldo", true));
                isShowSummaryBalance = false;
            }

            // Saldo Awal
            var realCashBalances = _dbContext.RealCashBalances.Where(entity => entity.UnitId == unitId && entity.Month == date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Month && entity.Year == date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Year).ToList();
            var isShowRealCashBalance = true;
            if (realCashBalances.Count > 0)
                foreach (var realCashBalance in realCashBalances)
                {
                    var currency = _currencies.FirstOrDefault(element => element.Id == realCashBalance.CurrencyId);
                    result.Add(new BudgetCashflowItemDto(isShowRealCashBalance, realCashBalance, currency));
                    isShowRealCashBalance = false;
                }
            else
            {
                result.Add(new BudgetCashflowItemDto(isShowRealCashBalance, new RealCashBalanceModel(), new CurrencyDto()));
                isShowSummaryBalance = false;
            }

            summaryCurrencyIds.AddRange(realCashBalances.Select(element => element.CurrencyId));
            summaryCurrencyIds = summaryCurrencyIds.Distinct().ToList();

            if (summaryCurrencyIds.Count > 0)
            {
                var isShowCurrencyRateLabel = true;
                foreach (var summaryCurrencyId in summaryCurrencyIds)
                {
                    var currency = _currencies.FirstOrDefault(element => element.Id == summaryCurrencyId);
                    result.Add(new BudgetCashflowItemDto(isShowCurrencyRateLabel, currency));
                    isShowCurrencyRateLabel = false;
                }
            }
            else
            {
                result.Add(new BudgetCashflowItemDto(true, new CurrencyDto()));
            }

            if (summaryCurrencyIds.Count > 0)
            {
                var endingBalances = result.Where(element => element.SummaryBalanceLabel == "Saldo Akhir Kas").ToList();
                var isShowRealCashDifferenceLabel = true;
                foreach (var currencyId in summaryCurrencyIds)
                {
                    var currency = _currencies.FirstOrDefault(element => element.Id == currencyId);
                    var endingBalance = endingBalances.FirstOrDefault(element => element.Currency.Id == currencyId);
                    var realCashBalance = realCashBalances.FirstOrDefault(element => element.CurrencyId == currencyId);

                    var nominal = 0.0;
                    var currencyNominal = 0.0;
                    var total = 0.0;

                    if (endingBalance != null)
                    {
                        nominal += endingBalance.Nominal;
                        currencyNominal += endingBalance.CurrencyNominal;
                        total += endingBalance.Total;
                    }

                    if (realCashBalance != null)
                    {
                        nominal -= realCashBalance.Nominal;
                        currencyNominal -= realCashBalance.CurrencyNominal;
                        total -= realCashBalance.Total;
                    }

                    result.Add(new BudgetCashflowItemDto(isShowRealCashDifferenceLabel, "Selisih", currency, nominal, currencyNominal, total));
                    isShowRealCashDifferenceLabel = false;
                }
            }
            else
            {
                result.Add(new BudgetCashflowItemDto(true, "Selisih", new CurrencyDto(), 0, 0, 0));
            }


            result.Add(new BudgetCashflowItemDto("Total Surplus (Defisit) Equivalent", result.Where(element => element.RealCashDifferenceLabel == "Selisih").Sum(element => element.Total)));
            return result;
        }

        public async Task<BudgetCashflowDivision> GetBudgetCashflowDivision(int divisionId, DateTimeOffset date)
        {
            var month = date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Month;
            var year = date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Year;

            var cashflowTypes = _dbContext.BudgetCashflowTypes.OrderBy(entity => entity.LayoutOrder).ToList();
            var cashflowTypeIds = cashflowTypes.Select(element => element.Id).ToList();
            var cashflowCategories = _dbContext.BudgetCashflowCategories.Where(entity => cashflowTypeIds.Contains(entity.CashflowTypeId)).ToList();
            var cashflowCategoryIds = cashflowCategories.Select(element => element.Id).ToList();
            var cashflowSubCategories = _dbContext.BudgetCashflowSubCategories.Where(entity => cashflowCategoryIds.Contains(entity.CashflowCategoryId)).ToList();

            var units = divisionId > 0 ? _units.Where(unit => unit.DivisionId == divisionId).ToList() : _units;
            var unitIds = units.Select(element => element.Id).ToList();
            var cashflowUnits = _dbContext.BudgetCashflowUnits.Where(entity => unitIds.Contains(entity.UnitId) && entity.Month == month && entity.Year == year).ToList();

            var existingUnitIds = cashflowUnits.Select(element => element.UnitId).Distinct().ToList();
            var existingDivisionIds = cashflowUnits.Select(element => element.DivisionId).Distinct().ToList();

            units = _units.Where(unit => existingUnitIds.Contains(unit.Id)).ToList();
            var divisions = _divisions.Where(division => existingDivisionIds.Contains(division.Id)).ToList();

            var divisionTemporaryRows = new List<DivisionTemporaryRowDto>();

            foreach (var cashflowType in cashflowTypes)
            {
                foreach (CashType type in Enum.GetValues(typeof(CashType)))
                {
                    var selectedCashflowCategories = cashflowCategories.Where(element => element.Type == type && element.CashflowTypeId == cashflowType.Id).OrderBy(element => element.LayoutOrder).ToList();

                    foreach (var cashflowCategory in selectedCashflowCategories)
                    {
                        var selectedCashflowSubCategories = cashflowSubCategories.Where(element => element.CashflowCategoryId == cashflowCategory.Id).OrderBy(element => element.LayoutOrder).ToList();

                        // category take 1 row;
                        divisionTemporaryRows.Add(new DivisionTemporaryRowDto(cashflowType, type, cashflowCategory));

                        foreach (var cashflowSubCategory in selectedCashflowSubCategories)
                        {
                            var selectedCashflowUnits = cashflowUnits.Where(element => element.BudgetCashflowSubCategoryId == cashflowSubCategory.Id).ToList();

                            var currencyIds = selectedCashflowUnits.Select(element => element.CurrencyId).Where(element => element > 0).Distinct().ToList();

                            if (currencyIds.Count > 0)
                            {
                                foreach (var currencyId in currencyIds)
                                {
                                    var currency = _currencies.FirstOrDefault(element => element.Id == currencyId);
                                    var divisionTemporaryRow = new DivisionTemporaryRowDto(cashflowType, type, cashflowCategory, cashflowSubCategory, currency);
                                    foreach (var division in divisions)
                                    {
                                        var divisionUnits = units.Where(unit => unit.DivisionId == division.Id);

                                        foreach (var divisionUnit in divisionUnits)
                                        {
                                            var cashflowUnit = selectedCashflowUnits.FirstOrDefault(element => element.UnitId == divisionUnit.Id && element.CurrencyId == currencyId);

                                            divisionTemporaryRow.Items.Add(new DivisionTemporaryDivisionUnitDto(cashflowType, type, cashflowCategory, cashflowSubCategory, currency, division, divisionUnit, cashflowUnit));
                                        }

                                        divisionTemporaryRow.Items.Add(new DivisionTemporaryDivisionUnitDto(cashflowType, type, cashflowCategory, cashflowSubCategory, currency, division));
                                    }

                                    divisionTemporaryRows.Add(divisionTemporaryRow);
                                }
                            }
                            else
                            {
                                divisionTemporaryRows.Add(new DivisionTemporaryRowDto(cashflowType, type, cashflowCategory, cashflowSubCategory, null));
                            }
                        }
                    }
                }
            }

            var result = new BudgetCashflowDivision();
            foreach (var division in divisions)
            {
                var divisionUnits = units.Where(element => element.DivisionId == division.Id);
                foreach (var unit in divisionUnits)
                {
                    result.Headers.Add($"UNIT {unit.Name}");
                }

                result.Headers.Add($"DIVISI {division.Name}");
            }

            foreach (var cashflowType in cashflowTypes)
            {
                var item = new BudgetCashflowDivisionItemDto(cashflowType);

                var cashInRows = divisionTemporaryRows.Where(element => element.CashflowType.Id == cashflowType.Id && element.Type == CashType.In).Count();
                cashInRows = cashInRows > 0 ? cashInRows : 1;
                var cashInTotalRows = divisionTemporaryRows.Where(element => element.CashflowType.Id == cashflowType.Id && element.Type == CashType.In && element.Currency != null).Select(element => element.Currency.Id).Where(element => element > 0).Distinct().Count();
                cashInTotalRows = cashInTotalRows > 0 ? cashInTotalRows : 1;
                var cashOutRows = divisionTemporaryRows.Where(element => element.CashflowType.Id == cashflowType.Id && element.Type == CashType.Out).Count();
                cashOutRows = cashOutRows > 0 ? cashOutRows : 1;
                var cashOutTotalRows = divisionTemporaryRows.Where(element => element.CashflowType.Id == cashflowType.Id && element.Type == CashType.Out && element.Currency != null).Select(element => element.Currency.Id).Where(element => element > 0).Distinct().Count();
                cashOutTotalRows = cashOutTotalRows > 0 ? cashOutTotalRows : 1;
                var cashTypeDifferenceRows = divisionTemporaryRows.Where(element => element.CashflowType.Id == cashflowType.Id && element.Currency != null).Select(element => element.Currency.Id).Where(element => element > 0).Distinct().Count();
                cashTypeDifferenceRows = cashTypeDifferenceRows > 0 ? cashTypeDifferenceRows : 1;
                var sectionRows = cashInRows + cashInTotalRows + cashOutRows + cashOutTotalRows + cashTypeDifferenceRows;
                item.SetSection(sectionRows);

                foreach (CashType type in Enum.GetValues(typeof(CashType)))
                {
                    var groupRows = type == CashType.In ? cashInRows + cashInTotalRows : cashOutRows + cashOutTotalRows;
                    item.SetGroup(type, groupRows);
                    var selectedCashflowCategories = cashflowCategories.Where(element => element.Type == type && element.CashflowTypeId == cashflowType.Id).OrderBy(element => element.LayoutOrder).ToList();

                    foreach (var cashflowCategory in selectedCashflowCategories)
                    {
                        item.SetLabelOnly(cashflowCategory);
                        result.Items.Add(item);
                        item = new BudgetCashflowDivisionItemDto();

                        var selectedCashflowSubCategories = cashflowSubCategories.Where(element => element.CashflowCategoryId == cashflowCategory.Id).OrderBy(element => element.LayoutOrder).ToList();

                        foreach (var cashflowSubCategory in selectedCashflowSubCategories)
                        {
                            var selectedCashflowUnits = cashflowUnits.Where(element => element.BudgetCashflowSubCategoryId == cashflowSubCategory.Id).ToList();

                            var currencyIds = selectedCashflowUnits.Select(element => element.CurrencyId).Where(element => element > 0).Distinct().ToList();

                            var isShowSubCategoryLabel = true;

                            if (currencyIds.Count > 0)
                                foreach (var currencyId in currencyIds)
                                {
                                    var currency = _currencies.FirstOrDefault(element => element.Id == currencyId);

                                    var subCategoryItem = new BudgetCashflowDivisionItemDto(cashflowType, type, cashflowCategory, cashflowSubCategory, currency, isShowSubCategoryLabel);
                                    isShowSubCategoryLabel = false;

                                    var divisionCurrencyNominalTotal = 0.0;
                                    var divisionNominalTotal = 0.0;
                                    var divisionActualTotal = 0.0;
                                    foreach (var division in divisions)
                                    {
                                        var divisionUnits = units.Where(unit => unit.DivisionId == division.Id);

                                        var divisionCurrencyNominal = 0.0;
                                        var divisionNominal = 0.0;
                                        var divisionActual = 0.0;
                                        foreach (var divisionUnit in divisionUnits)
                                        {
                                            var cashflowUnit = selectedCashflowUnits.FirstOrDefault(element => element.UnitId == divisionUnit.Id && element.CurrencyId == currencyId);
                                            subCategoryItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(cashflowUnit, divisionUnit));

                                            if (cashflowUnit != null)
                                            {
                                                divisionCurrencyNominal += cashflowUnit.CurrencyNominal;
                                                divisionNominal += cashflowUnit.Nominal;
                                                divisionActual += cashflowUnit.Total;
                                                divisionCurrencyNominalTotal += cashflowUnit.CurrencyNominal;
                                                divisionNominalTotal += cashflowUnit.Nominal;
                                                divisionActualTotal += cashflowUnit.Total;
                                            }
                                        }

                                        subCategoryItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, divisionCurrencyNominal, divisionNominal, divisionActual));
                                    }

                                    subCategoryItem.SetRowSummary(divisionCurrencyNominalTotal, divisionNominalTotal, divisionActualTotal);
                                    result.Items.Add(subCategoryItem);
                                }
                            else
                            {
                                var subCategoryItem = new BudgetCashflowDivisionItemDto(cashflowType, type, cashflowCategory, cashflowSubCategory, null, isShowSubCategoryLabel);
                                isShowSubCategoryLabel = false;

                                var divisionCurrencyNominalTotal = 0.0;
                                var divisionNominalTotal = 0.0;
                                var divisionActualTotal = 0.0;
                                foreach (var division in divisions)
                                {
                                    var divisionUnits = units.Where(unit => unit.DivisionId == division.Id);

                                    var divisionCurrencyNominal = 0.0;
                                    var divisionNominal = 0.0;
                                    var divisionActual = 0.0;
                                    foreach (var divisionUnit in divisionUnits)
                                    {
                                        subCategoryItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(null, divisionUnit));
                                    }

                                    subCategoryItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, divisionCurrencyNominal, divisionNominal, divisionActual));
                                }

                                subCategoryItem.SetRowSummary(divisionCurrencyNominalTotal, divisionNominalTotal, divisionActualTotal);
                                result.Items.Add(subCategoryItem);
                            }

                        }
                    }

                    var summaryByTypeAndCashflowTypes = result
                        .Items
                        .Where(element => element.CashflowType != null && element.CashflowType.Id == cashflowType.Id && element.Type == type && element.Currency != null)
                        .GroupBy(element => element.Currency.Id)
                        .Select(element => new
                        {
                            CurrencyId = element.Key,
                            Items = element.SelectMany(e => e.Items).Where(e => e.CashflowUnit != null).ToList()
                        })
                        .ToList();

                    var summaryCurrencyIds = summaryByTypeAndCashflowTypes.Select(element => element.CurrencyId).Where(element => element > 0).Distinct().ToList();
                    var isShowSummaryLabel = true;
                    if (summaryByTypeAndCashflowTypes.Count > 0)
                        foreach (var summaryByTypeAndCashflowType in summaryByTypeAndCashflowTypes)
                        {
                            var currency = _currencies.FirstOrDefault(element => element.Id == summaryByTypeAndCashflowType.CurrencyId);

                            var typeSummaryItem = new BudgetCashflowDivisionItemDto(cashflowType, type, currency, isShowSummaryLabel);
                            isShowSummaryLabel = false;

                            typeSummaryItem.InitializeItems();
                            var divisionCurrencyNominalTotal = 0.0;
                            var divisionNominalTotal = 0.0;
                            var divisionActualTotal = 0.0;
                            foreach (var division in divisions)
                            {
                                var divisionUnits = units.Where(unit => unit.DivisionId == division.Id);

                                var divisionCurrencyNominal = 0.0;
                                var divisionNominal = 0.0;
                                var divisionActual = 0.0;
                                foreach (var divisionUnit in divisionUnits)
                                {
                                    var nominal = summaryByTypeAndCashflowType.Items.Where(unitItem => unitItem.Unit != null && unitItem.Unit.Id == divisionUnit.Id).Sum(element => element.CashflowUnit.Nominal);
                                    var currencyNominal = summaryByTypeAndCashflowType.Items.Where(unitItem => unitItem.Unit != null && unitItem.Unit.Id == divisionUnit.Id).Sum(element => element.CashflowUnit.CurrencyNominal);
                                    var actual = summaryByTypeAndCashflowType.Items.Where(unitItem => unitItem.Unit != null && unitItem.Unit.Id == divisionUnit.Id).Sum(element => element.CashflowUnit.Total);

                                    typeSummaryItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, divisionUnit, nominal, currencyNominal, actual));

                                    divisionCurrencyNominal += currencyNominal;
                                    divisionNominal += nominal;
                                    divisionActual += actual;
                                    divisionCurrencyNominalTotal += currencyNominal;
                                    divisionNominalTotal += nominal;
                                    divisionActualTotal += actual;
                                }

                                typeSummaryItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, divisionNominal, divisionCurrencyNominal, divisionActual));

                            }

                            typeSummaryItem.SetRowSummary(divisionCurrencyNominalTotal, divisionNominalTotal, divisionActualTotal);

                            result.Items.Add(typeSummaryItem);
                        }
                    else
                    {
                        var typeSummaryItem = new BudgetCashflowDivisionItemDto(cashflowType, type, null, isShowSummaryLabel);

                        typeSummaryItem.InitializeItems();
                        var divisionCurrencyNominalTotal = 0.0;
                        var divisionNominalTotal = 0.0;
                        var divisionActualTotal = 0.0;
                        foreach (var division in divisions)
                        {
                            var divisionUnits = units.Where(unit => unit.DivisionId == division.Id);

                            var divisionCurrencyNominal = 0.0;
                            var divisionNominal = 0.0;
                            var divisionActual = 0.0;
                            foreach (var divisionUnit in divisionUnits)
                            {
                                typeSummaryItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, divisionUnit, 0, 0, 0));
                            }

                            typeSummaryItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, divisionNominal, divisionCurrencyNominal, divisionActual));

                        }

                        typeSummaryItem.SetRowSummary(divisionCurrencyNominalTotal, divisionNominalTotal, divisionActualTotal);

                        result.Items.Add(typeSummaryItem);
                    }
                }

                var differenceByTypeAndCashflowTypes = result
                        .Items
                        .Where(element => element.CashflowType != null && element.CashflowType.Id == cashflowType.Id && element.Currency != null && element.IsSummary)
                        .GroupBy(element => new { element.Currency.Id, element.Type })
                        .Select(element => new
                        {
                            CurrencyId = element.Key.Id,
                            Type = element.Key.Type,
                            Items = element.SelectMany(e => e.Items).ToList()
                        })
                        .ToList();

                var differenceCurrencyIds = differenceByTypeAndCashflowTypes.Select(element => element.CurrencyId).Where(element => element > 0).Distinct().ToList();
                var isShowDifferenceLabel = true;
                if (differenceByTypeAndCashflowTypes.Count > 0)
                    foreach (var differenceCurrencyId in differenceCurrencyIds)
                    {
                        var currency = _currencies.FirstOrDefault(element => element.Id == differenceCurrencyId);

                        var typeDifferenceItem = new BudgetCashflowDivisionItemDto(cashflowType, currency, isShowDifferenceLabel);
                        isShowDifferenceLabel = false;

                        typeDifferenceItem.InitializeItems();
                        var divisionCurrencyNominalTotal = 0.0;
                        var divisionNominalTotal = 0.0;
                        var divisionActualTotal = 0.0;
                        foreach (var division in divisions)
                        {
                            var divisionUnits = units.Where(unit => unit.DivisionId == division.Id);

                            var divisionCurrencyNominal = 0.0;
                            var divisionNominal = 0.0;
                            var divisionActual = 0.0;
                            foreach (var divisionUnit in divisionUnits)
                            {
                                var cashInItems = differenceByTypeAndCashflowTypes.Where(element => element.CurrencyId == differenceCurrencyId && element.Type == CashType.In).SelectMany(element => element.Items).Where(element => element.Unit != null && element.Unit.Id == divisionUnit.Id).ToList();
                                var cashOutItems = differenceByTypeAndCashflowTypes.Where(element => element.CurrencyId == differenceCurrencyId && element.Type == CashType.Out).SelectMany(element => element.Items).Where(element => element.Unit != null && element.Unit.Id == divisionUnit.Id).ToList();

                                var nominal = cashInItems.Sum(element => element.Nominal) - cashOutItems.Sum(element => element.Nominal);
                                var currencyNominal = cashInItems.Sum(element => element.CurrencyNominal) - cashOutItems.Sum(element => element.CurrencyNominal);
                                var actual = cashInItems.Sum(element => element.Actual) - cashOutItems.Sum(element => element.Actual);

                                typeDifferenceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, divisionUnit, nominal, currencyNominal, actual));

                                divisionCurrencyNominal += currencyNominal;
                                divisionNominal += nominal;
                                divisionActual += actual;
                                divisionCurrencyNominalTotal += currencyNominal;
                                divisionNominalTotal += nominal;
                                divisionActualTotal += actual;
                            }

                            typeDifferenceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, divisionNominal, divisionCurrencyNominal, divisionActual));

                        }

                        typeDifferenceItem.SetRowSummary(divisionCurrencyNominalTotal, divisionNominalTotal, divisionActualTotal);

                        result.Items.Add(typeDifferenceItem);
                    }
                else
                {
                    var typeDifferenceItem = new BudgetCashflowDivisionItemDto(cashflowType, null, isShowDifferenceLabel);

                    typeDifferenceItem.InitializeItems();
                    var divisionCurrencyNominalTotal = 0.0;
                    var divisionNominalTotal = 0.0;
                    var divisionActualTotal = 0.0;
                    foreach (var division in divisions)
                    {
                        var divisionUnits = units.Where(unit => unit.DivisionId == division.Id);

                        var divisionCurrencyNominal = 0.0;
                        var divisionNominal = 0.0;
                        var divisionActual = 0.0;
                        foreach (var divisionUnit in divisionUnits)
                        {
                            typeDifferenceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, divisionUnit, 0, 0, 0));
                        }
                        typeDifferenceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, divisionNominal, divisionCurrencyNominal, divisionActual));
                    }

                    typeDifferenceItem.SetRowSummary(divisionCurrencyNominalTotal, divisionNominalTotal, divisionActualTotal);

                    result.Items.Add(typeDifferenceItem);
                }
            }

            var initialBalances = _dbContext.InitialCashBalances.Where(entity => unitIds.Contains(entity.UnitId) && entity.Month == date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Month && entity.Year == date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Year).ToList();
            var isShowGeneralSummaryLabel = true;
            if (initialBalances.Count > 0)
            {
                var currencyIds = initialBalances.Select(element => element.CurrencyId).Distinct().ToList();

                foreach (var currencyId in currencyIds)
                {

                    var currency = _currencies.FirstOrDefault(element => element.Id == currencyId);

                    var initialBalanceItem = new BudgetCashflowDivisionItemDto("Saldo Awal Kas", true, isShowGeneralSummaryLabel, currency);
                    isShowGeneralSummaryLabel = false;
                    var divisionNominalTotal = 0.0;
                    var divisionCurrencyNominalTotal = 0.0;
                    var divisionActualTotal = 0.0;
                    foreach (var division in divisions)
                    {
                        var divisionUnits = units.Where(element => element.DivisionId == division.Id);
                        var divisionNominal = 0.0;
                        var divisionCurrencyNominal = 0.0;
                        var divisionActual = 0.0;
                        foreach (var unit in divisionUnits)
                        {
                            var initialBalance = initialBalances.FirstOrDefault(element => element.CurrencyId == currencyId && element.UnitId == unit.Id);
                            var nominal = 0.0;
                            var currencyNominal = 0.0;
                            var actual = 0.0;

                            if (initialBalance != null)
                            {
                                nominal = initialBalance.Nominal;
                                currencyNominal = initialBalance.CurrencyNominal;
                                actual = initialBalance.Total;

                                divisionNominal += nominal;
                                divisionCurrencyNominal += currencyNominal;
                                divisionActual += actual;

                                divisionNominalTotal += nominal;
                                divisionCurrencyNominalTotal += currencyNominal;
                                divisionActualTotal += actual;

                            }
                            initialBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, unit, nominal, currencyNominal, actual));
                        }
                        initialBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, null, divisionNominal, divisionCurrencyNominal, divisionActual));
                    }

                    initialBalanceItem.SetRowSummary(divisionCurrencyNominalTotal, divisionNominalTotal, divisionActualTotal);
                    result.Items.Add(initialBalanceItem);
                }

            }
            else
            {
                var initialBalanceItem = new BudgetCashflowDivisionItemDto("Saldo Awal Kas", true, isShowGeneralSummaryLabel, null);

                foreach (var division in divisions)
                {
                    var divisionUnits = units.Where(element => element.DivisionId == division.Id).ToList();

                    foreach (var divisionUnit in divisionUnits)
                    {
                        initialBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto());
                    }
                    initialBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto());
                }

                result.Items.Add(initialBalanceItem);
            }

            // Total Surplus/Deficit Kas
            var differenceSummaries = result
                .Items
                .Where(element => element.IsDifference && element.Currency != null)
                .GroupBy(element => element.Currency.Id)
                .Select(element => new
                {
                    CurrencyId = element.Key,
                    Items = element.SelectMany(e => e.Items).ToList()
                })
                .ToList();
            isShowGeneralSummaryLabel = true;
            if (differenceSummaries.Count > 0)
            {
                var currencyIds = differenceSummaries.Select(element => element.CurrencyId).Distinct().ToList();

                foreach (var currencyId in currencyIds)
                {

                    var currency = _currencies.FirstOrDefault(element => element.Id == currencyId);

                    var differenceBalanceItem = new BudgetCashflowDivisionItemDto("TOTAL SURPLUS/DEFISIT KAS", true, isShowGeneralSummaryLabel, currency);
                    isShowGeneralSummaryLabel = false;
                    var divisionNominalTotal = 0.0;
                    var divisionCurrencyNominalTotal = 0.0;
                    var divisionActualTotal = 0.0;
                    foreach (var division in divisions)
                    {
                        var divisionUnits = units.Where(element => element.DivisionId == division.Id);
                        var divisionNominal = 0.0;
                        var divisionCurrencyNominal = 0.0;
                        var divisionActual = 0.0;
                        foreach (var unit in divisionUnits)
                        {
                            var differenceSummary = differenceSummaries.FirstOrDefault(element => element.CurrencyId == currencyId);
                            var nominal = 0.0;
                            var currencyNominal = 0.0;
                            var actual = 0.0;

                            if (differenceSummary != null)
                            {
                                nominal = differenceSummary.Items.Where(element => element.Unit != null && element.Unit.Id == unit.Id).Sum(sum => sum.Nominal);
                                currencyNominal = differenceSummary.Items.Where(element => element.Unit != null && element.Unit.Id == unit.Id).Sum(sum => sum.CurrencyNominal); ;
                                actual = differenceSummary.Items.Where(element => element.Unit != null && element.Unit.Id == unit.Id).Sum(sum => sum.Actual); ;

                                divisionNominal += nominal;
                                divisionCurrencyNominal += currencyNominal;
                                divisionActual += actual;

                                divisionNominalTotal += nominal;
                                divisionCurrencyNominalTotal += currencyNominal;
                                divisionActualTotal += actual;

                            }
                            differenceBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, unit, nominal, currencyNominal, actual));
                        }
                        differenceBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, null, divisionNominal, divisionCurrencyNominal, divisionActual));
                    }

                    differenceBalanceItem.SetRowSummary(divisionCurrencyNominalTotal, divisionNominalTotal, divisionActualTotal);
                    result.Items.Add(differenceBalanceItem);
                }

            }
            else
            {
                var differenceBalanceItem = new BudgetCashflowDivisionItemDto("TOTAL SURPLUS/DEFISIT KAS", true, isShowGeneralSummaryLabel, null);

                foreach (var division in divisions)
                {
                    var divisionUnits = units.Where(element => element.DivisionId == division.Id).ToList();

                    foreach (var divisionUnit in divisionUnits)
                    {
                        differenceBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto());
                    }
                    differenceBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto());
                }

                result.Items.Add(differenceBalanceItem);
            }

            var generalSummaryCurrencyIds = new List<int>();
            generalSummaryCurrencyIds.AddRange(initialBalances.Select(element => element.CurrencyId).Where(element => element > 0).Distinct().ToList());
            generalSummaryCurrencyIds.AddRange(differenceSummaries.Select(element => element.CurrencyId).Where(element => element > 0).Distinct().ToList());

            generalSummaryCurrencyIds = generalSummaryCurrencyIds.Distinct().ToList();
            isShowGeneralSummaryLabel = true;
            if (generalSummaryCurrencyIds.Count > 0)
                foreach (var currencyId in generalSummaryCurrencyIds)
                {
                    var currency = _currencies.FirstOrDefault(element => element.Id == currencyId);

                    var finalBalanceItem = new BudgetCashflowDivisionItemDto("Saldo Akhir Kas", true, isShowGeneralSummaryLabel, currency);
                    isShowGeneralSummaryLabel = false;
                    var divisionNominalTotal = 0.0;
                    var divisionCurrencyNominalTotal = 0.0;
                    var divisionActualTotal = 0.0;
                    foreach (var division in divisions)
                    {
                        var divisionUnits = units.Where(element => element.DivisionId == division.Id);
                        var divisionNominal = 0.0;
                        var divisionCurrencyNominal = 0.0;
                        var divisionActual = 0.0;
                        foreach (var unit in divisionUnits)
                        {
                            var selectedInitialBalance = initialBalances.FirstOrDefault(element => element.CurrencyId == currencyId && unit.Id == element.UnitId);
                            var selectedDifferenceSummary = differenceSummaries.Where(element => element.CurrencyId == currencyId).SelectMany(element => element.Items).Where(element => element.Unit != null && element.Unit.Id == unit.Id).ToList();
                            var nominal = 0.0;
                            var currencyNominal = 0.0;
                            var actual = 0.0;

                            if (selectedDifferenceSummary.Count > 0)
                            {
                                nominal += selectedDifferenceSummary.Sum(sum => sum.Nominal);
                                currencyNominal += selectedDifferenceSummary.Sum(sum => sum.CurrencyNominal); ;
                                actual += selectedDifferenceSummary.Sum(sum => sum.Actual);
                            }

                            if (selectedInitialBalance != null)
                            {
                                nominal += selectedInitialBalance.Nominal;
                                currencyNominal += selectedInitialBalance.CurrencyNominal;
                                actual += selectedInitialBalance.Total;
                            }


                            divisionNominal += nominal;
                            divisionCurrencyNominal += currencyNominal;
                            divisionActual += actual;

                            divisionNominalTotal += nominal;
                            divisionCurrencyNominalTotal += currencyNominal;
                            divisionActualTotal += actual;
                            finalBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, unit, nominal, currencyNominal, actual));
                        }
                        finalBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, null, divisionNominal, divisionCurrencyNominal, divisionActual));
                    }

                    finalBalanceItem.SetRowSummary(divisionCurrencyNominalTotal, divisionNominalTotal, divisionActualTotal);
                    result.Items.Add(finalBalanceItem);
                }
            else
            {
                var finalBalanceItem = new BudgetCashflowDivisionItemDto("Saldo Akhir Kas", true, isShowGeneralSummaryLabel, null);

                foreach (var division in divisions)
                {
                    var divisionUnits = units.Where(element => element.DivisionId == division.Id).ToList();

                    foreach (var divisionUnit in divisionUnits)
                    {
                        finalBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto());
                    }
                    finalBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto());
                }

                result.Items.Add(finalBalanceItem);
            }

            var realCashBalances = _dbContext.RealCashBalances.Where(entity => unitIds.Contains(entity.UnitId) && entity.Month == date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Month && entity.Year == date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Year).ToList();
            isShowGeneralSummaryLabel = true;
            if (realCashBalances.Count > 0)
            {
                var currencyIds = realCashBalances.Select(element => element.CurrencyId).Distinct().ToList();

                foreach (var currencyId in currencyIds)
                {

                    var currency = _currencies.FirstOrDefault(element => element.Id == currencyId);

                    var realCashBalanceItem = new BudgetCashflowDivisionItemDto("Saldo Real Kas", true, isShowGeneralSummaryLabel, currency);
                    isShowGeneralSummaryLabel = false;
                    var divisionNominalTotal = 0.0;
                    var divisionCurrencyNominalTotal = 0.0;
                    var divisionActualTotal = 0.0;
                    foreach (var division in divisions)
                    {
                        var divisionUnits = units.Where(element => element.DivisionId == division.Id);
                        var divisionNominal = 0.0;
                        var divisionCurrencyNominal = 0.0;
                        var divisionActual = 0.0;
                        foreach (var unit in divisionUnits)
                        {
                            var realCashBalance = realCashBalances.FirstOrDefault(element => element.CurrencyId == currencyId && element.UnitId == unit.Id);
                            var nominal = 0.0;
                            var currencyNominal = 0.0;
                            var actual = 0.0;

                            if (realCashBalance != null)
                            {
                                nominal = realCashBalance.Nominal;
                                currencyNominal = realCashBalance.CurrencyNominal;
                                actual = realCashBalance.Total;
                            }

                            divisionNominal += nominal;
                            divisionCurrencyNominal += currencyNominal;
                            divisionActual += actual;

                            divisionNominalTotal += nominal;
                            divisionCurrencyNominalTotal += currencyNominal;
                            divisionActualTotal += actual;

                            realCashBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, unit, nominal, currencyNominal, actual));
                        }
                        realCashBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, null, divisionNominal, divisionCurrencyNominal, divisionActual));
                    }

                    realCashBalanceItem.SetRowSummary(divisionCurrencyNominalTotal, divisionNominalTotal, divisionActualTotal);
                    result.Items.Add(realCashBalanceItem);
                }

            }
            else
            {
                var realCashBalanceItem = new BudgetCashflowDivisionItemDto("Saldo Real Kas", true, isShowGeneralSummaryLabel, null);

                foreach (var division in divisions)
                {
                    var divisionUnits = units.Where(element => element.DivisionId == division.Id).ToList();

                    foreach (var divisionUnit in divisionUnits)
                    {
                        realCashBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto());
                    }
                    realCashBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto());
                }

                result.Items.Add(realCashBalanceItem);
            }

            var isShowCurrencyLabel = true;
            if (generalSummaryCurrencyIds.Count > 0)
            {
                foreach (var currencyId in generalSummaryCurrencyIds)
                {
                    var currency = _currencies.FirstOrDefault(element => element.Id == currencyId);
                    result.Items.Add(new BudgetCashflowDivisionItemDto("Rate Kurs", currency, isShowCurrencyLabel));
                    isShowCurrencyLabel = false;
                }
            }
            else
            {
                result.Items.Add(new BudgetCashflowDivisionItemDto("Rate Kurs", null, isShowCurrencyLabel));
            }

            var endingBalances = result.Items.Where(element => element.GeneralSummaryLabel == "Saldo Akhir Kas").ToList();
            isShowGeneralSummaryLabel = true;
            var equivalent = 0.0;
            if (generalSummaryCurrencyIds.Count > 0)
                foreach (var currencyId in generalSummaryCurrencyIds)
                {
                    var currency = _currencies.FirstOrDefault(element => element.Id == currencyId);

                    var differenceGeneralSummaryItem = new BudgetCashflowDivisionItemDto("Selisih", true, isShowGeneralSummaryLabel, currency);
                    isShowGeneralSummaryLabel = false;
                    var divisionNominalTotal = 0.0;
                    var divisionCurrencyNominalTotal = 0.0;
                    var divisionActualTotal = 0.0;
                    foreach (var division in divisions)
                    {
                        var divisionUnits = units.Where(element => element.DivisionId == division.Id);
                        var divisionNominal = 0.0;
                        var divisionCurrencyNominal = 0.0;
                        var divisionActual = 0.0;
                        foreach (var unit in divisionUnits)
                        {
                            var selectedEndingBalances = endingBalances.Where(element => element.Currency != null && element.Currency.Id == currencyId).SelectMany(element => element.Items).Where(element => element.Unit != null && element.Unit.Id == unit.Id).ToList();
                            var selectedRealCashBalance = realCashBalances.FirstOrDefault(element => element.CurrencyId == currencyId && unit.Id == element.UnitId);
                            var nominal = 0.0;
                            var currencyNominal = 0.0;
                            var actual = 0.0;

                            if (selectedEndingBalances.Count > 0)
                            {
                                nominal += selectedEndingBalances.Sum(sum => sum.Nominal);
                                currencyNominal += selectedEndingBalances.Sum(sum => sum.CurrencyNominal); ;
                                actual += selectedEndingBalances.Sum(sum => sum.Actual);
                            }

                            if (selectedRealCashBalance != null)
                            {
                                nominal -= selectedRealCashBalance.Nominal;
                                currencyNominal -= selectedRealCashBalance.CurrencyNominal;
                                actual -= selectedRealCashBalance.Total;
                            }


                            divisionNominal += nominal;
                            divisionCurrencyNominal += currencyNominal;
                            divisionActual += actual;

                            divisionNominalTotal += nominal;
                            divisionCurrencyNominalTotal += currencyNominal;
                            divisionActualTotal += actual;
                            equivalent += actual;
                            differenceGeneralSummaryItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, unit, nominal, currencyNominal, actual));
                        }
                        differenceGeneralSummaryItem.Items.Add(new BudgetCashflowDivisionUnitItemDto(division, null, divisionNominal, divisionCurrencyNominal, divisionActual));
                    }

                    differenceGeneralSummaryItem.SetRowSummary(divisionCurrencyNominalTotal, divisionNominalTotal, divisionActualTotal);
                    result.Items.Add(differenceGeneralSummaryItem);
                }
            else
            {
                var finalBalanceItem = new BudgetCashflowDivisionItemDto("Saldo Akhir Kas", true, isShowGeneralSummaryLabel, null);

                foreach (var division in divisions)
                {
                    var divisionUnits = units.Where(element => element.DivisionId == division.Id).ToList();

                    foreach (var divisionUnit in divisionUnits)
                    {
                        finalBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto());
                    }
                    finalBalanceItem.Items.Add(new BudgetCashflowDivisionUnitItemDto());
                }

                result.Items.Add(finalBalanceItem);
            }


            result.Items.Add(new BudgetCashflowDivisionItemDto("Total Surplus (Defisit) Equivalent", equivalent));
            return result;
        }

        public List<BudgetCashflowUnitItemDto> GetBudgetCashflowUnit(int unitId, int subCategoryId, DateTimeOffset date)
        {
            return _dbContext.BudgetCashflowUnits
                .Where(entity => entity.UnitId == unitId && subCategoryId == entity.BudgetCashflowSubCategoryId && entity.Month == date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Month && entity.Year == date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Year)
                .ToList()
                .Select(entity =>
                {
                    return new BudgetCashflowUnitItemDto(entity, _currencies);
                })
                .ToList();
        }

        public int UpdateBudgetCashflowUnit(CashflowUnitFormDto form)
        {
            var existingModels = _dbContext.BudgetCashflowUnits.Where(entity => entity.BudgetCashflowSubCategoryId == form.CashflowSubCategoryId && entity.UnitId == form.UnitId && entity.DivisionId == form.DivisionId && entity.Month == form.Date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Month && entity.Year == form.Date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Year).ToList();
            foreach (var existingModel in existingModels)
            {
                EntityExtension.FlagForDelete(existingModel, _identityService.Username, UserAgent);
            }
            _dbContext.BudgetCashflowUnits.UpdateRange(existingModels);

            var models = new List<BudgetCashflowUnitModel>();
            foreach (var item in form.Items)
            {
                var model = new BudgetCashflowUnitModel(form.CashflowSubCategoryId, form.UnitId, form.DivisionId, form.Date.AddHours(_identityService.TimezoneOffset).AddMonths(1), item.CurrencyId, item.CurrencyNominal, item.Nominal, item.Total);
                EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
                models.Add(model);
            }
            _dbContext.BudgetCashflowUnits.AddRange(models);

            return _dbContext.SaveChanges();
        }

        #region Budget Cashflow Type CRUD
        public int CreateBudgetCashflowType(CashflowTypeFormDto form)
        {
            var model = new BudgetCashflowTypeModel(form.Name, form.LayoutOrder);
            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
            _dbContext.BudgetCashflowTypes.Add(model);
            return _dbContext.SaveChanges();
        }

        public ReadResponse<BudgetCashflowTypeModel> GetBudgetCashflowTypes(string keyword, int page, int size)
        {
            var query = _dbContext.BudgetCashflowTypes.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(entity => entity.Name.Contains(keyword));

            var data = query.OrderBy(entity => entity.LayoutOrder).ToList();
            return new ReadResponse<BudgetCashflowTypeModel>(data, query.Count(), new Dictionary<string, string>(), new List<string>());
        }

        public BudgetCashflowTypeModel GetBudgetCashflowTypeById(int id)
        {
            return _dbContext.BudgetCashflowTypes.FirstOrDefault(entity => entity.Id == id);
        }

        public int UpdateBudgetCashflowType(int id, CashflowTypeFormDto form)
        {
            var model = _dbContext.BudgetCashflowTypes.FirstOrDefault(entity => entity.Id == id);

            model.SetNewNameAndLayoutOrder(form.Name, form.LayoutOrder);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.BudgetCashflowTypes.Update(model);
            return _dbContext.SaveChanges();
        }

        public int DeleteBudgetCashflowType(int id)
        {
            var model = _dbContext.BudgetCashflowTypes.FirstOrDefault(entity => entity.Id == id);

            var cashflowCategories = _dbContext
                .BudgetCashflowCategories
                .Where(entity => entity.CashflowTypeId == id)
                .ToList()
                .Select(element =>
                {
                    EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);

                    return element;
                }).ToList();
            var categoryIds = cashflowCategories.Select(element => element.Id).ToList();

            var subCategories = _dbContext
                .BudgetCashflowSubCategories
                .Where(entity => categoryIds.Contains(entity.CashflowCategoryId))
                .ToList()
                .Select(element =>
                {
                    EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                    return element;
                })
                .ToList();

            EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent);
            _dbContext.BudgetCashflowTypes.Update(model);
            _dbContext.BudgetCashflowCategories.UpdateRange(cashflowCategories);
            _dbContext.BudgetCashflowSubCategories.UpdateRange(subCategories);
            return _dbContext.SaveChanges();
        }
        #endregion

        #region Budget Cashflow Category CRUD
        public int CreateBudgetCashflowCategory(CashflowCategoryFormDto form)
        {
            var model = new BudgetCashflowCategoryModel(form.Name, form.Type, form.CashflowTypeId, form.LayoutOrder, form.IsLabelOnly);
            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
            _dbContext.BudgetCashflowCategories.Add(model);
            return _dbContext.SaveChanges();
        }

        public ReadResponse<BudgetCashflowCategoryModel> GetBudgetCashflowCategories(string keyword, int cashflowTypeId, int page, int size)
        {
            var query = _dbContext.BudgetCashflowCategories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(entity => entity.Name.Contains(keyword));

            if (cashflowTypeId > 0)
                query = query.Where(entity => entity.CashflowTypeId == cashflowTypeId);

            var data = query.OrderBy(entity => entity.LayoutOrder).ToList();
            return new ReadResponse<BudgetCashflowCategoryModel>(data, query.Count(), new Dictionary<string, string>(), new List<string>());
        }

        public BudgetCashflowCategoryModel GetBudgetCashflowCategoryById(int id)
        {
            return _dbContext.BudgetCashflowCategories.FirstOrDefault(entity => entity.Id == id);
        }

        public int UpdateBudgetCashflowCategory(int id, CashflowCategoryFormDto form)
        {
            var model = _dbContext.BudgetCashflowCategories.FirstOrDefault(entity => entity.Id == id);

            model.SetNewValue(form.Name, form.Type, form.LayoutOrder, form.CashflowTypeId);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.BudgetCashflowCategories.Update(model);
            return _dbContext.SaveChanges();
        }

        public int DeleteBudgetCashflowCategories(int id)
        {
            var model = _dbContext.BudgetCashflowCategories.FirstOrDefault(entity => entity.Id == id);

            var subCategories = _dbContext
                .BudgetCashflowSubCategories
                .Where(entity => id == entity.CashflowCategoryId)
                .ToList()
                .Select(element =>
                {
                    EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                    return element;
                })
                .ToList();

            EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent);
            _dbContext.BudgetCashflowCategories.Update(model);
            _dbContext.BudgetCashflowSubCategories.UpdateRange(subCategories);
            return _dbContext.SaveChanges();
        }
        #endregion

        #region Budget Cashflow Sub Category CRUD
        public int CreateBudgetCashflowSubCategory(CashflowSubCategoryFormDto form)
        {
            var model = new BudgetCashflowSubCategoryModel(form.Name, form.CashflowCategoryId, form.LayoutOrder, form.PurchasingCategoryIds, form.IsReadOnly, form.ReportType, form.IsImport);
            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
            _dbContext.BudgetCashflowSubCategories.Add(model);
            return _dbContext.SaveChanges();
        }

        public ReadResponse<BudgetCashflowSubCategoryModel> GetBudgetCashflowSubCategories(string keyword, int subCategoryId, int page, int size)
        {
            var query = _dbContext.BudgetCashflowSubCategories.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
                query = query.Where(entity => entity.Name.Contains(keyword));

            if (subCategoryId > 0)
                query = query.Where(entity => entity.CashflowCategoryId == subCategoryId);

            var data = query.OrderBy(entity => entity.LayoutOrder).ToList();
            return new ReadResponse<BudgetCashflowSubCategoryModel>(data, query.Count(), new Dictionary<string, string>(), new List<string>());
        }

        public BudgetCashflowSubCategoryTypeDto GetBudgetCashflowSubCategoryById(int id)
        {
            var model = _dbContext.BudgetCashflowSubCategories.FirstOrDefault(entity => entity.Id == id);

            return new BudgetCashflowSubCategoryTypeDto(model);
        }

        public int UpdateBudgetCashflowSubCategory(int id, CashflowSubCategoryFormDto form)
        {
            var model = _dbContext.BudgetCashflowSubCategories.FirstOrDefault(entity => entity.Id == id);
            model.SetNewValue(form.CashflowCategoryId, form.IsReadOnly, form.LayoutOrder, form.Name, form.PurchasingCategoryIds, form.ReportType, form.IsImport);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.BudgetCashflowSubCategories.Update(model);
            return _dbContext.SaveChanges();
        }

        public int DeleteBudgetCashflowSubCategories(int id)
        {
            var model = _dbContext.BudgetCashflowSubCategories.FirstOrDefault(entity => entity.Id == id);
            EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent);
            _dbContext.BudgetCashflowSubCategories.Update(model);
            return _dbContext.SaveChanges();
        }
        #endregion

        public int CreateInitialCashBalance(CashBalanceFormDto form)
        {
            var models = new List<InitialCashBalanceModel>();

            foreach (var item in form.Items)
            {
                var model = new InitialCashBalanceModel(form.UnitId, form.DivisionId, item.CurrencyId, item.Nominal, item.CurrencyNominal, item.Total, form.Date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Month, form.Date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Year);
                EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
                models.Add(model);
            }

            _dbContext.InitialCashBalances.AddRange(models);
            return _dbContext.SaveChanges();
        }

        public int UpdateInitialCashBalance(CashBalanceFormDto form)
        {
            var existingModels = _dbContext.InitialCashBalances.Where(entity => entity.UnitId == form.UnitId && entity.DivisionId == form.DivisionId && entity.Month == form.Date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Month && entity.Year == form.Date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Year).ToList();
            foreach (var existingModel in existingModels)
            {
                EntityExtension.FlagForDelete(existingModel, _identityService.Username, UserAgent);
            }
            _dbContext.InitialCashBalances.UpdateRange(existingModels);

            var models = new List<InitialCashBalanceModel>();
            foreach (var item in form.Items)
            {
                var model = new InitialCashBalanceModel(form.UnitId, form.DivisionId, item.CurrencyId, item.Nominal, item.CurrencyNominal, item.Total, form.Date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Month, form.Date.AddMonths(1).AddHours(_identityService.TimezoneOffset).Year);
                EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
                models.Add(model);
            }
            _dbContext.InitialCashBalances.AddRange(models);

            return _dbContext.SaveChanges();
        }

        public List<BudgetCashflowUnitItemDto> GetInitialCashBalance(int unitId, DateTimeOffset date)
        {
            return _dbContext
                .InitialCashBalances
                .Where(entity => entity.UnitId == unitId && entity.Month == date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Month && entity.Year == date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Year)
                .ToList()
                .Select(entity =>
                {
                    return new BudgetCashflowUnitItemDto(entity, _currencies);
                }).ToList();
        }

        public int CreateRealCashBalance(CashBalanceFormDto form)
        {
            var models = new List<RealCashBalanceModel>();

            foreach (var item in form.Items)
            {
                var model = new RealCashBalanceModel(form.UnitId, form.DivisionId, item.CurrencyId, item.Nominal, item.CurrencyNominal, item.Total, form.Date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Month, form.Date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Year);
                EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
                models.Add(model);
            }

            _dbContext.RealCashBalances.AddRange(models);
            return _dbContext.SaveChanges();
        }

        public int UpdateRealCashBalance(CashBalanceFormDto form)
        {
            var existingModels = _dbContext.RealCashBalances.Where(entity => entity.UnitId == form.UnitId && entity.DivisionId == form.DivisionId && entity.Month == form.Date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Month && entity.Year == form.Date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Year).ToList();
            foreach (var existingModel in existingModels)
            {
                EntityExtension.FlagForDelete(existingModel, _identityService.Username, UserAgent);
            }
            _dbContext.RealCashBalances.UpdateRange(existingModels);

            var models = new List<RealCashBalanceModel>();
            foreach (var item in form.Items)
            {
                var model = new RealCashBalanceModel(form.UnitId, form.DivisionId, item.CurrencyId, item.Nominal, item.CurrencyNominal, item.Total, form.Date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Month, form.Date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Year);
                EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
                models.Add(model);
            }
            _dbContext.RealCashBalances.AddRange(models);

            return _dbContext.SaveChanges();
        }

        public List<BudgetCashflowUnitItemDto> GetRealCashBalance(int unitId, DateTimeOffset date)
        {
            return _dbContext
                .RealCashBalances
                .Where(entity => entity.UnitId == unitId && entity.Month == date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Month && entity.Year == date.AddHours(_identityService.TimezoneOffset).AddMonths(1).Year)
                .ToList()
                .Select(entity =>
                {
                    return new BudgetCashflowUnitItemDto(entity, _currencies);
                })
                .ToList();
        }
    }
}
