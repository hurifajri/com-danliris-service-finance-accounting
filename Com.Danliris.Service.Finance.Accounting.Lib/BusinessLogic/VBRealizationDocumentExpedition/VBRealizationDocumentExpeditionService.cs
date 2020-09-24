using Com.Danliris.Service.Finance.Accounting.Lib.Enums.Expedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocument;
using Com.Danliris.Service.Finance.Accounting.Lib.Models.VBRealizationDocumentExpedition;
using Com.Danliris.Service.Finance.Accounting.Lib.Services.IdentityService;
using Com.Danliris.Service.Finance.Accounting.Lib.Utilities;
using Com.Moonlay.Models;
using Com.Moonlay.NetCore.Lib;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Org.BouncyCastle.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRealizationDocumentExpedition
{
    public class VBRealizationDocumentExpeditionService : IVBRealizationDocumentExpeditionService
    {
        private const string UserAgent = "finance-accounting-service";
        private readonly FinanceDbContext _dbContext;
        private readonly IIdentityService _identityService;

        public VBRealizationDocumentExpeditionService(FinanceDbContext dbContext, IServiceProvider serviceProvider)
        {
            _dbContext = dbContext;
            _identityService = serviceProvider.GetService<IIdentityService>();
        }

        public Task<int> CashierReceipt(List<int> vbRealizationIds)
        {
            var models = _dbContext.VBRealizationDocumentExpeditions.Where(entity => vbRealizationIds.Contains(entity.VBRealizationId) && entity.Position == VBRealizationPosition.VerifiedToCashier).ToList();

            models.ForEach(model =>
            {
                model.CashierVerification(_identityService.Username);
                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            });

            _dbContext.VBRealizationDocumentExpeditions.UpdateRange(models);
            UpdateVBRealizationPosition(vbRealizationIds, VBRealizationPosition.Cashier, "");

            return _dbContext.SaveChangesAsync();
        }

        private void UpdateVBRealizationPosition(int vbRealizationId, VBRealizationPosition position, string reason)
        {
            var model = _dbContext.VBRealizationDocuments.FirstOrDefault(entity => entity.Id == vbRealizationId);
            model.UpdatePosition(position, _identityService.Username, UserAgent);

            if (position == VBRealizationPosition.VerifiedToCashier || position == VBRealizationPosition.NotVerified)
                model.UpdateVerified(position, reason, _identityService.Username, UserAgent);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.VBRealizationDocuments.Update(model);
        }

        private void UpdateVBRealizationPosition(List<int> vbRealizationIds, VBRealizationPosition position, string reason)
        {
            var models = _dbContext.VBRealizationDocuments.Where(entity => vbRealizationIds.Contains(entity.Id)).ToList();
            models.ForEach(model =>
            {
                model.UpdatePosition(position, _identityService.Username, UserAgent);
                if (position == VBRealizationPosition.VerifiedToCashier || position == VBRealizationPosition.NotVerified)
                    model.UpdateVerified(position, reason, _identityService.Username, UserAgent);
                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            });
            _dbContext.VBRealizationDocuments.UpdateRange(models);
        }

        public async Task<VBRealizationDocumentExpeditionReportDto> GetReports(int vbId, int vbRealizationId, string vbRequestName, int unitId, int divisionId, DateTimeOffset dateStart, DateTimeOffset dateEnd, string status, int page = 1, int size = 25)
        {
            var vbRealizationQuery = _dbContext.VBRealizationDocuments.AsQueryable();
            var expeditionQuery = _dbContext.VBRealizationDocumentExpeditions.AsQueryable();

            var query = from realization in vbRealizationQuery
                        join expedition in expeditionQuery on realization.Id equals expedition.VBRealizationId into realizationExpeditions

                        from realizationExpedition in realizationExpeditions.DefaultIfEmpty()

                        select new ReportDto()
                        {
                            CashierReceiptBy = realizationExpedition != null ? realizationExpedition.CashierReceiptBy : null,
                            CashierReceiptDate = realizationExpedition != null ? realizationExpedition.CashierReceiptDate : null,
                            CurrencyCode = realization.CurrencyCode,
                            CurrencyRate = realization.CurrencyRate,
                            DivisionId = realization.SuppliantDivisionId,
                            DivisionName = realization.SuppliantDivisionName,
                            NotVerifiedBy = realizationExpedition != null ? realizationExpedition.NotVerifiedBy : null,
                            NotVerifiedDate = realizationExpedition != null ? realizationExpedition.NotVerifiedDate : null,
                            NotVerifiedReason = realizationExpedition != null ? realizationExpedition.NotVerifiedReason : null,
                            Position = realizationExpedition != null ? realizationExpedition.Position : VBRealizationPosition.Purchasing,
                            SendToVerificationBy = realizationExpedition != null ? realizationExpedition.SendToVerificationBy : null,
                            SendToVerificationDate = realizationExpedition != null ? realizationExpedition.SendToVerificationDate : null,
                            UnitId = realization.SuppliantUnitId,
                            UnitName = realization.SuppliantUnitName,
                            VBAmount = realization.VBRequestDocumentAmount,
                            VBId = realization.VBRequestDocumentId,
                            VBNo = realization.VBRequestDocumentNo,
                            VBRealizationAmount = realization.Amount,
                            VBRealizationDate = realization.Date,
                            VBRealizationId = realization.Id,
                            VBRealizationNo = realization.DocumentNo,
                            VBRequestName = realization.VBRequestDocumentCreatedBy,
                            VBType = realization.Type,
                            VerificationReceiptBy = realizationExpedition != null ? realizationExpedition.VerificationReceiptBy : null,
                            VerificationReceiptDate = realizationExpedition != null ? realizationExpedition.VerificationReceiptDate : null,
                            VerifiedToCashierBy = realizationExpedition != null ? realizationExpedition.VerifiedToCashierBy : null,
                            VerifiedToCashierDate = realizationExpedition != null ? realizationExpedition.VerifiedToCashierDate : null,
                            Purpose = realization.VBRequestDocumentPurpose
                        };
            query = query.Where(entity => entity.VBRealizationDate >= dateStart && entity.VBRealizationDate <= dateEnd);

            if (vbId > 0)
                query = query.Where(entity => entity.VBId == vbId);

            if (vbRealizationId > 0)
                query = query.Where(entity => entity.VBRealizationId == vbRealizationId);

            if (!string.IsNullOrWhiteSpace(vbRequestName))
                query = query.Where(entity => entity.VBRequestName == vbRequestName);

            if (unitId > 0)
                query = query.Where(entity => entity.UnitId == unitId);

            if (!string.IsNullOrWhiteSpace(status) && status.ToUpper() == "UNIT")
                query = query.Where(entity => entity.Position <= VBRealizationPosition.PurchasingToVerification);
            else if (!string.IsNullOrWhiteSpace(status) && status.ToUpper() == "VERIFIKASI")
                query = query.Where(entity => entity.Position >= VBRealizationPosition.Verification && entity.Position <= VBRealizationPosition.VerifiedToCashier);
            else if (!string.IsNullOrWhiteSpace(status) && status.ToUpper() == "KASIR")
                query = query.Where(entity => entity.Position == VBRealizationPosition.Cashier);
            else if (!string.IsNullOrWhiteSpace(status) && status.ToUpper() == "RETUR")
                query = query.Where(entity => entity.Position == VBRealizationPosition.NotVerified);

            if (divisionId > 0)
                query = query.Where(entity => entity.DivisionId == divisionId);

            var result = query.Skip((page - 1) * size).Take(size).ToList();
            var total = await query.CountAsync();
            return new VBRealizationDocumentExpeditionReportDto(result, total, size, page);
        }

        public Task<int> InitializeExpedition(int vbRealizationId)
        {
            var realizationVB = _dbContext.RealizationVbs.FirstOrDefault(entity => entity.Id == vbRealizationId);

            var model = new VBRealizationDocumentExpeditionModel(
                //realizationVB.Id,
                //realizationVB.VBId,
                //realizationVB.VBNo,
                //realizationVB.VBNoRealize,
                //realizationVB.Date,
                //realizationVB.RequestVbName,
                //realizationVB.UnitId,
                //realizationVB.UnitName,
                //realizationVB.DivisionId,
                //realizationVB.DivisionName,
                //realizationVB.Amount_VB,
                //realizationVB.Amount,
                //realizationVB.CurrencyCode,
                //(double)realizationVB.CurrencyRate,
                //realizationVB.VBRealizeCategory
                );

            EntityExtension.FlagForCreate(model, _identityService.Username, UserAgent);

            _dbContext.Add(model);
            UpdateVBRealizationPosition(vbRealizationId, VBRealizationPosition.Purchasing, "");

            return _dbContext.SaveChangesAsync();
        }

        public ReadResponse<VBRealizationDocumentExpeditionModel> Read(int page, int size, string order, string keyword, VBRealizationPosition position, int vbId, int vbRealizationId, DateTimeOffset? realizationDate, string vbRealizationRequestPerson, int unitId)
        {
            var query = _dbContext.VBRealizationDocumentExpeditions.AsQueryable();

            var vbRequestNotCompletedIds = _dbContext.VBRequestDocuments.Where(entity => !entity.IsCompleted).Select(entity => entity.Id).ToList();
            var vbRealizationNotCompletedIds = _dbContext.VBRealizationDocuments.Where(entity =>  vbRequestNotCompletedIds.Contains(entity.VBRequestDocumentId) || !entity.IsCompleted).Select(entity => entity.Id).ToList();

            query = query.Where(entity => vbRealizationNotCompletedIds.Contains(entity.VBRealizationId));

            if (position > 0)
                query = query.Where(entity => entity.Position == position);

            if (vbId > 0)
                query = query.Where(entity => entity.VBId == vbId);

            if (vbRealizationId > 0)
                query = query.Where(entity => entity.VBRealizationId == vbRealizationId);

            if (realizationDate.HasValue)
            {
                var date = realizationDate.GetValueOrDefault().AddHours(_identityService.TimezoneOffset * -1);
                query = query.Where(entity => entity.VBRealizationDate.Date == date.Date);
            }

            if (!string.IsNullOrWhiteSpace(vbRealizationRequestPerson))
                query = query.Where(entity => entity.VBRequestName == vbRealizationRequestPerson);

            if (unitId > 0)
                query = query.Where(entity => entity.UnitId == unitId);

            //query = query
            //    .Select(entity => new VBRealizationDocumentExpeditionIndexDto
            //    {
            //        Id = entity.Id,
            //        LastModifiedUtc = entity.LastModifiedUtc,
            //        VBNo = entity.VBNo,
            //        VBRealizationNo = entity.VBRealizationNo,
            //        VBRealizationDate = entity.VBRealizationDate,
            //        VBName = entity.VBRequestName,
            //        UnitId = entity.UnitId,
            //        UnitName = entity.UnitName,
            //        DivisionId = entity.DivisionId,
            //        DivisionName = entity.DivisionName,
            //        Currency

            //    });

            List<string> searchAttributes = new List<string>()
            {
                "VBRealizationNo", "VBRequestName", "UnitName"
            };

            query = QueryHelper<VBRealizationDocumentExpeditionModel>.Search(query, searchAttributes, keyword);

            //var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            //Query = QueryHelper<JournalTransactionModel>.Filter(Query, FilterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<VBRealizationDocumentExpeditionModel>.Order(query, orderDictionary);

            var pageable = new Pageable<VBRealizationDocumentExpeditionModel>(query, page - 1, size);
            var data = pageable.Data.ToList();

            var totalData = pageable.TotalCount;

            return new ReadResponse<VBRealizationDocumentExpeditionModel>(data, totalData, orderDictionary, new List<string>());
        }

        public Task<int> Reject(int vbRealizationId, string reason)
        {
            var vbRealizationExpedition = _dbContext.VBRealizationDocumentExpeditions.OrderByDescending(x => x.Id).FirstOrDefault(entity => entity.VBRealizationId == vbRealizationId && (entity.Position == VBRealizationPosition.Verification || entity.Position == VBRealizationPosition.VerifiedToCashier));

            vbRealizationExpedition.VerificationRejected(_identityService.Username, reason);
            EntityExtension.FlagForUpdate(vbRealizationExpedition, _identityService.Username, UserAgent);

            _dbContext.VBRealizationDocumentExpeditions.Update(vbRealizationExpedition);
            UpdateVBRealizationPosition(vbRealizationId, VBRealizationPosition.NotVerified, reason);

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> SubmitToVerification(List<int> vbRealizationIds)
        {
            var vbRealizationDocuments = _dbContext.VBRealizationDocuments.Where(entity => vbRealizationIds.Contains(entity.Id) && (entity.Position == VBRealizationPosition.Purchasing || entity.Position == VBRealizationPosition.NotVerified)).ToList();

            var models = vbRealizationDocuments.Select(element =>
            {
                var result = new VBRealizationDocumentExpeditionModel(
                   element.Id,
                   element.VBRequestDocumentId,
                   element.VBRequestDocumentNo,
                   element.DocumentNo,
                   element.Date,
                   element.VBRequestDocumentCreatedBy,
                   element.SuppliantUnitId,
                   element.SuppliantUnitName,
                   element.SuppliantDivisionId,
                   element.SuppliantDivisionName,
                   element.VBRequestDocumentAmount,
                   element.Amount,
                   element.CurrencyCode,
                   element.CurrencyRate,
                   element.VBRequestDocumentPurpose,
                   element.Type);
                result.SubmitToVerification(_identityService.Username);
                EntityExtension.FlagForCreate(result, _identityService.Username, UserAgent);

                return result;
            }).ToList();

            vbRealizationDocuments = vbRealizationDocuments.Select(element =>
            {
                element.UpdatePosition(VBRealizationPosition.PurchasingToVerification, _identityService.Username, UserAgent);

                return element;
            }).ToList();

            _dbContext.VBRealizationDocuments.UpdateRange(vbRealizationDocuments);
            _dbContext.VBRealizationDocumentExpeditions.AddRange(models);

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> VerifiedToCashier(List<int> vbRealizationIds)
        {
            var models = _dbContext.VBRealizationDocumentExpeditions.Where(entity => vbRealizationIds.Contains(entity.VBRealizationId) || entity.Position == VBRealizationPosition.Verification).ToList();

            models.ForEach(model =>
            {
                model.SendToCashier(_identityService.Username);
                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            });

            _dbContext.VBRealizationDocumentExpeditions.UpdateRange(models);
            UpdateVBRealizationPosition(vbRealizationIds, VBRealizationPosition.VerifiedToCashier, null);

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> VerificationDocumentReceipt(List<int> vbRealizationIds)
        {
            var models = _dbContext.VBRealizationDocumentExpeditions.Where(entity => vbRealizationIds.Contains(entity.VBRealizationId) && entity.Position == VBRealizationPosition.PurchasingToVerification).ToList();

            models.ForEach(model =>
            {
                model.VerificationReceipt(_identityService.Username);
                EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            });

            _dbContext.VBRealizationDocumentExpeditions.UpdateRange(models);
            UpdateVBRealizationPosition(vbRealizationIds, VBRealizationPosition.Verification, null);

            return _dbContext.SaveChangesAsync();
        }

        public ReadResponse<VBRealizationDocumentModel> ReadRealizationToVerification(int vbId, int vbRealizationId, DateTimeOffset? realizationDate, string vbRealizationRequestPerson, int unitId)
        {
            var query = _dbContext.Set<VBRealizationDocumentModel>().AsQueryable();
            query = query.Where(entity => entity.Position == VBRealizationPosition.Purchasing || entity.Position == VBRealizationPosition.NotVerified);

            if (vbId > 0)
                query = query.Where(entity => entity.VBRequestDocumentId == vbId);

            if (vbRealizationId > 0)
                query = query.Where(entity => entity.Id == vbRealizationId);

            if (realizationDate.HasValue)
            {
                var date = realizationDate.GetValueOrDefault().AddHours(_identityService.TimezoneOffset * -1);
                query = query.Where(entity => entity.Date.Date == date.Date);
            }

            if (!string.IsNullOrWhiteSpace(vbRealizationRequestPerson))
                query = query.Where(entity => entity.VBRequestDocumentCreatedBy == vbRealizationRequestPerson);

            if (unitId > 0)
                query = query.Where(entity => entity.SuppliantUnitId == unitId);

            var result = query.ToList();
            return new ReadResponse<VBRealizationDocumentModel>(result, result.Count, new Dictionary<string, string>(), new List<string>());
        }

        public Task<int> VerifiedToCashier(int vbRealizationId)
        {
            var vbRealizationExpedition = _dbContext.VBRealizationDocumentExpeditions.OrderByDescending(x => x.Id).FirstOrDefault(entity => entity.VBRealizationId == vbRealizationId && (entity.Position == VBRealizationPosition.Verification || entity.Position == VBRealizationPosition.NotVerified));

            vbRealizationExpedition.SendToCashier(_identityService.Username);
            EntityExtension.FlagForUpdate(vbRealizationExpedition, _identityService.Username, UserAgent);

            _dbContext.VBRealizationDocumentExpeditions.Update(vbRealizationExpedition);

            UpdateVBRealizationPosition(vbRealizationId, VBRealizationPosition.VerifiedToCashier, null);

            return _dbContext.SaveChangesAsync();
        }

        public Task<int> UpdateExpeditionByRealizationId(int vbRealizationId)
        {
            var realizationVB = _dbContext.RealizationVbs.FirstOrDefault(entity => entity.Id == vbRealizationId);

            var model = _dbContext.VBRealizationDocumentExpeditions.FirstOrDefault(entity => entity.VBRealizationId == vbRealizationId);

            model.UpdateVBRealizationInfo(realizationVB);
            EntityExtension.FlagForUpdate(model, _identityService.Username, UserAgent);
            _dbContext.VBRealizationDocumentExpeditions.Update(model);
            return _dbContext.SaveChangesAsync();
        }

        public ReadResponse<VBRealizationDocumentExpeditionModel> ReadVerification(int page, int size, string order, string keyword, VBRealizationPosition position, int vbId, int vbRealizationId, DateTimeOffset? realizationDate, string vbRealizationRequestPerson, int unitId)
        {
            var query = _dbContext.Set<VBRealizationDocumentExpeditionModel>().AsQueryable();

            var idQuery = query;
            var selectData = idQuery.GroupBy(entity => entity.VBRealizationId)
                .Select(e => e.OrderByDescending(x => x.Id)
                .FirstOrDefault()).ToList();
            var ids = selectData.Select(element => element.Id).ToList();

            query = query.Where(entity => ids.Contains(entity.Id) && (entity.Position == VBRealizationPosition.VerifiedToCashier || entity.Position == VBRealizationPosition.NotVerified));

            if (vbId > 0)
                query = query.Where(entity => entity.VBId == vbId);

            if (vbRealizationId > 0)
                query = query.Where(entity => entity.VBRealizationId == vbRealizationId);

            if (realizationDate.HasValue)
            {
                var date = realizationDate.GetValueOrDefault().AddHours(_identityService.TimezoneOffset * -1);
                query = query.Where(entity => entity.VBRealizationDate.Date == date.Date);
            }

            if (!string.IsNullOrWhiteSpace(vbRealizationRequestPerson))
                query = query.Where(entity => entity.VBRequestName == vbRealizationRequestPerson);

            if (unitId > 0)
                query = query.Where(entity => entity.UnitId == unitId);

            //query = query
            //    .Select(entity => new VBRealizationDocumentExpeditionIndexDto
            //    {
            //        Id = entity.Id,
            //        LastModifiedUtc = entity.LastModifiedUtc,
            //        VBNo = entity.VBNo,
            //        VBRealizationNo = entity.VBRealizationNo,
            //        VBRealizationDate = entity.VBRealizationDate,
            //        VBName = entity.VBRequestName,
            //        UnitId = entity.UnitId,
            //        UnitName = entity.UnitName,
            //        DivisionId = entity.DivisionId,
            //        DivisionName = entity.DivisionName,
            //        Currency

            //    });

            List<string> searchAttributes = new List<string>()
            {
                "VBRealizationNo", "VBRequestName", "UnitName"
            };

            query = QueryHelper<VBRealizationDocumentExpeditionModel>.Search(query, searchAttributes, keyword);

            //var filterDictionary = JsonConvert.DeserializeObject<Dictionary<string, object>>(filter);
            //Query = QueryHelper<JournalTransactionModel>.Filter(Query, FilterDictionary);

            var orderDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(order);
            query = QueryHelper<VBRealizationDocumentExpeditionModel>.Order(query, orderDictionary);

            var pageable = new Pageable<VBRealizationDocumentExpeditionModel>(query, page - 1, size);
            var data = pageable.Data.ToList();

            var totalData = pageable.TotalCount;

            return new ReadResponse<VBRealizationDocumentExpeditionModel>(data, totalData, orderDictionary, new List<string>());
        }
    }
}
