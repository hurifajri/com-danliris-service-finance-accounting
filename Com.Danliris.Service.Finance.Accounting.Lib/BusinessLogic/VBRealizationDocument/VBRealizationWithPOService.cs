using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Helpers;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.HttpClientService;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocument
{
    public class VBRealizationWithPOService : IVBRealizationWithPOService
    {
        private const string UserAgent = "finance-service";
        public readonly FinanceDbContext _dbContext;

        private readonly IIdentityService _identityService;
        private readonly IServiceProvider _serviceProvider;

        public VBRealizationWithPOService(FinanceDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _identityService = serviceProvider.GetService<IIdentityService>();
            _serviceProvider = serviceProvider;
        }

        private Tuple<string, int> GetDocumentNo(FormDto form, VBRealizationDocumentModel existingData)
        {
            var now = form.Date.GetValueOrDefault().AddHours(_identityService.TimezoneOffset);
            var year = now.ToString("yy");
            var month = now.ToString("MM");

            var division = "";

            if (form.Type == "Tanpa Nomor VB")
                division = form.SuppliantUnit.Division.Name;
            else
            {
                var vbRequest = _dbContext.VBRequestDocuments.FirstOrDefault(entity => entity.Id == form.VBRequestDocument.Id.GetValueOrDefault());
                division = vbRequest.SuppliantDivisionName;
            }

            var unitCode = "T";
            if (division == "GARMENT")
                unitCode = "G";


            var documentNo = $"R-{unitCode}-{month}{year}-";

            var index = 1;

            if (existingData != null)
            {
                index = existingData.Index + 1;
            }

            documentNo += string.Format("{0:000}", index);


            return new Tuple<string, int>(documentNo, index);
        }

        public int Create(FormDto form)
        {
            var model = new VBRealizationDocumentModel();

            var existingData = _dbContext.VBRealizationDocuments.Where(a => a.Date.AddHours(_identityService.TimezoneOffset).Month == form.Date.GetValueOrDefault().AddHours(_identityService.TimezoneOffset).Month).OrderByDescending(s => s.Index).FirstOrDefault();
            var documentNo = GetDocumentNo(form, existingData);

            var amount = form.Items.Sum(element => 
            {
                var nominal = element.UnitPaymentOrder.Amount.GetValueOrDefault();
                if (element.UnitPaymentOrder.UseVat.GetValueOrDefault())
                    nominal += element.UnitPaymentOrder.Amount.GetValueOrDefault() * (decimal)0.1;


                if (element.UnitPaymentOrder.UseIncomeTax.GetValueOrDefault() && element.UnitPaymentOrder.IncomeTaxBy.ToUpper() == "SUPPLIER")
                    nominal -= element.UnitPaymentOrder.Amount.GetValueOrDefault() * (decimal)element.UnitPaymentOrder.IncomeTax.Rate.GetValueOrDefault();


                return nominal;
            });

            if (form.Type == "Tanpa Nomor VB")
                model = new VBRealizationDocumentModel(form.Currency, form.Date, form.SuppliantUnit, documentNo, (decimal)amount);
            else
            {
                var vbRequest = _dbContext.VBRequestDocuments.FirstOrDefault(entity => entity.Id == form.VBRequestDocument.Id.GetValueOrDefault());

                if (vbRequest != null)
                {
                    vbRequest.SetIsRealized(true, _identityService.Username, UserAgent);
                    _dbContext.VBRequestDocuments.Update(vbRequest);
                }

                model = new VBRealizationDocumentModel(form.Date, vbRequest, documentNo, (decimal)amount);
            }

            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
            _dbContext.VBRealizationDocuments.Add(model);
            _dbContext.SaveChanges();

            AddItems(model.Id, form.Items, model.SuppliantDivisionName);

            AddUnitCosts(model.Id, form.Items.SelectMany(element => element.UnitPaymentOrder.UnitCosts).ToList());

            _dbContext.SaveChanges();
            return model.Id;
        }

        private void AddUnitCosts(int id, List<UnitCostDto> unitCosts)
        {
            var models = unitCosts.Select(element =>
            {
                var result = new VBRealizationDocumentUnitCostsItemModel(id, element);
                EntityExtension.FlagForCreate(result, _identityService.Username, UserAgent);

                return result;
            }).ToList();

            _dbContext.VBRealizationDocumentUnitCostsItems.AddRange(models);
            _dbContext.SaveChanges();
        }

        private void AddItems(int id, List<FormItemDto> items, string suppliantDivisionName)
        {
            //var models = items.Select(element =>
            //{
            //    var result = new VBRealizationDocumentExpenditureItemModel(headerId: id, element);
            //    EntityExtension.FlagForCreate(result, _identityService.Username, UserAgent);
            //    return result;
            //}).ToList();
            var httpClientService = _serviceProvider.GetService<IHttpClientService>();


            foreach (var item in items)
            {
                var model = new VBRealizationDocumentExpenditureItemModel(id, item);
                EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);
                _dbContext.VBRealizationDocumentExpenditureItems.Add(model);
                _dbContext.SaveChanges();
                var result = httpClientService.PutAsync($"{APIEndpoint.Purchasing}vb-request-po-external/spb/{item.UnitPaymentOrder.Id.GetValueOrDefault()}?division={suppliantDivisionName}", new StringContent("{}", Encoding.UTF8, General.JsonMediaType)).Result;
            }

        }

        private void AddDetails(int itemId, List<UnitPaymentOrderItemDto> items)
        {
            var models = items.Select(element =>
            {
                var result = new VBRealizationDocumentUnitCostsItemModel(itemId, element);
                EntityExtension.FlagForCreate(result, _identityService.Username, UserAgent);
                return result;
            }).ToList();

            _dbContext.VBRealizationDocumentUnitCostsItems.AddRange(models);
            _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var model = _dbContext.VBRealizationDocuments.FirstOrDefault(entity => entity.Id == id);
            EntityExtension.FlagForDelete(model, _identityService.Username, UserAgent);
            _dbContext.VBRealizationDocuments.Update(model);

            var httpClientService = _serviceProvider.GetService<IHttpClientService>();

            var items = _dbContext.VBRealizationDocumentExpenditureItems.Where(entity => entity.VBRealizationDocumentId == id).ToList();
            items = items.Select(element =>
            {
                element.FlagForDelete(_identityService.Username, UserAgent);
                var result = httpClientService.PutAsync($"{APIEndpoint.Purchasing}vb-request-po-external/spb/{element.UnitPaymentOrderId}?division={model.SuppliantDivisionName}", new StringContent("{}", Encoding.UTF8, General.JsonMediaType)).Result;
                return element;
            }).ToList();
            _dbContext.VBRealizationDocumentExpenditureItems.UpdateRange(items);

            var vbRequest = _dbContext.VBRequestDocuments.FirstOrDefault(entity => entity.Id == model.VBRequestDocumentId);
            if (vbRequest != null)
            {
                vbRequest.SetIsRealized(false, _identityService.Username, UserAgent);
                _dbContext.VBRequestDocuments.Update(vbRequest);
            }

            _dbContext.SaveChanges();
            return id;
        }

        public ReadResponse<VBRealizationDocumentModel> Read(int page, int size, string order, List<string> select, string keyword, string filter)
        {
            var query = _dbContext.Set<VBRealizationDocumentModel>().AsQueryable(); ;

            List<string> searchAttributes = new List<string>()
            {
                "DocumentNo", "SuppliantUnitName"
            };

            query = QueryHelper<VBRealizationDocumentModel>.Search(query, searchAttributes, keyword);

            var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            query = QueryHelper<VBRealizationDocumentModel>.Filter(query, filterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<VBRealizationDocumentModel>.Order(query, orderDictionary);

            var pageable = new Pageable<VBRealizationDocumentModel>(query, page - 1, size);
            var data = pageable.Data.ToList();

            int TotalData = pageable.TotalCount;

            return new ReadResponse<VBRealizationDocumentModel>(data, TotalData, orderDictionary, new List<string>());
        }

        public VBRealizationWithPODto ReadById(int id)
        {
            var model = _dbContext.VBRealizationDocuments.FirstOrDefault(entity => entity.Id == id);
            var result = (VBRealizationWithPODto)null;

            if (model != null)
            {
                var items = _dbContext.VBRealizationDocumentExpenditureItems.Where(entity => entity.VBRealizationDocumentId == model.Id).ToList();


                result = new VBRealizationWithPODto()
                {
                    Id = model.Id,
                    Position = model.Position,
                    Date = model.Date,
                    Type = model.DocumentType == RealizationDocumentType.WithVB ? "Dengan Nomor VB" : "Tanpa Nomor VB",
                    SuppliantUnit = new UnitDto()
                    {
                        Code = model.SuppliantUnitCode,
                        Division = new DivisionDto()
                        {
                            Code = model.SuppliantDivisionCode,
                            Id = model.SuppliantDivisionId,
                            Name = model.SuppliantDivisionName
                        },
                        Id = model.SuppliantUnitId,
                        Name = model.SuppliantUnitName
                    },
                    Currency = new CurrencyDto()
                    {
                        Code = model.CurrencyCode,
                        Description = model.CurrencyDescription,
                        Id = model.CurrencyId,
                        Rate = model.CurrencyRate,
                        Symbol = model.CurrencySymbol
                    },
                    Items = items.Select(item =>
                    {
                        var unitCostItems = _dbContext.VBRealizationDocumentUnitCostsItems.Where(entity => entity.VBRealizationDocumentExpenditureItemId == item.Id).ToList();

                        var itemResult = new VBRealizationWithPOItemDto()
                        {
                            Id = item.Id,
                            UnitPaymentOrder = new UnitPaymentOrderDto()
                            {
                                Id = item.UnitPaymentOrderId,
                                No = item.UnitPaymentOrderNo,
                                Amount = item.Amount,
                                Date = item.Date,
                                IncomeTax = new IncomeTaxDto()
                                {
                                    Id = item.IncomeTaxId,
                                    Name = item.IncomeTaxName,
                                    Rate = item.IncomeTaxRate
                                },
                                IncomeTaxBy = item.IncomeTaxBy,
                                UseIncomeTax = item.UseIncomeTax,
                                UseVat = item.UseVat
                            }
                        };
                        return itemResult;
                    }).ToList()
                };

                if (model.VBRequestDocumentId > 0)
                    result.VBRequestDocument = _dbContext.VBRequestDocuments.FirstOrDefault(entity => entity.Id == model.VBRequestDocumentId);

            }
            return result;
        }

        public int Update(int id, FormDto form)
        {
            var model = _dbContext.VBRealizationDocuments.FirstOrDefault(entity => entity.Id == id);
            model.UpdatePosition(VBRealizationPosition.Purchasing, _identityService.Username, UserAgent);

            if (form.VBRequestDocument != null && form.VBRequestDocument.Id.GetValueOrDefault() > 0)
            {
                var vbRequest = _dbContext.VBRequestDocuments.FirstOrDefault(entity => entity.Id == form.VBRequestDocument.Id.GetValueOrDefault());
                model.Update(vbRequest);
                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            }

            var httpClientService = _serviceProvider.GetService<IHttpClientService>();

            var items = _dbContext.VBRealizationDocumentExpenditureItems.Where(entity => entity.VBRealizationDocumentId == id).ToList();
            items = items.Select(element =>
            {
                EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                var result = httpClientService.PutAsync($"{APIEndpoint.Purchasing}vb-request-po-external/spb/{element.UnitPaymentOrderId}?division={model.SuppliantDivisionName}", new StringContent("{}", Encoding.UTF8, General.JsonMediaType)).Result;
                return element;
            }).ToList();
            _dbContext.VBRealizationDocumentExpenditureItems.UpdateRange(items);

            var details = _dbContext.VBRealizationDocumentUnitCostsItems.Where(entity => entity.VBRealizationDocumentId == id).ToList();
            details = details.Select(element =>
            {
                EntityExtension.FlagForDelete(element, _identityService.Username, UserAgent);
                return element;
            }).ToList();
            _dbContext.VBRealizationDocumentUnitCostsItems.UpdateRange(details);

            AddItems(id, form.Items, form.SuppliantUnit.Division.Name);
            AddUnitCosts(model.Id, form.Items.SelectMany(element => element.UnitPaymentOrder.UnitCosts).ToList());

            return id;
        }

        public VBRealizationPdfDto ReadModelById(int id)
        {
            var model = _dbContext.VBRealizationDocuments.FirstOrDefault(entity => entity.Id == id);

            if (model == null)
                return null;

            var items = _dbContext.VBRealizationDocumentExpenditureItems.Where(s => s.VBRealizationDocumentId == id).ToList();

            var unitCosts = _dbContext.VBRealizationDocumentUnitCostsItems.Where(s => s.VBRealizationDocumentId == id).ToList();

            return new VBRealizationPdfDto()
            {
                Header = model,
                Items = items,
                UnitCosts = unitCosts
            };
        }
    }
}
