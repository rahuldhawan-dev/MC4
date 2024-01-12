using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using System.ComponentModel.DataAnnotations;

namespace MapCallMVC.Areas.Reports.Models
{
    public class SearchIncompleteWorkOrders : SearchSet<WorkOrder>, ISearchIncompleteWorkOrder
    {
        [DropDown, Required, EntityMap, EntityMustExist(typeof(WorkDescription))]
        public virtual int? WorkDescription { get; set; }
    }
}