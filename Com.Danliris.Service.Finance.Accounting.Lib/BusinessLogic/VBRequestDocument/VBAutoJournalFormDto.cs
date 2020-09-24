using Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.Services.ClearaceVB;
using System;
using System.Collections.Generic;

namespace Com.Danliris.Service.Finance.Accounting.Lib.BusinessLogic.VBRequestDocument
{
    public class VBAutoJournalFormDto
    {
        public DateTimeOffset? Date { get; set; }
        public string DocumentNo { get; set; }
        public List<long> EPOIds { get; set; }
        public List<UPOAndAmountDto> UPOIds { get; set; }
    }
}