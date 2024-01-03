using MapCall.Common.Model.Entities;
using MMSINC.Data;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class SearchCorrectiveOrderProblemCode : SearchSet<CorrectiveOrderProblemCode>
    {
        #region Properties

        public string Code { get; set; }

        public string Description { get; set; }

        #endregion
    }
}