using MMSINC.Metadata;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels.Requisitions
{
    public abstract class WorkOrderRequisitionViewModelBase : ViewModel<Requisition>
    {
        #region Properties

        [Required]
        [StringLength(Requisition.StringLengths.SAP_REQUISITION_NUMBER_MAX_LENGTH)]
        public string SAPRequisitionNumber { get; set; }

        [Required]
        [DropDown, EntityMap, EntityMustExist(typeof(RequisitionType))]
        public int? RequisitionType { get; set; }
        
        #endregion

        #region Constructor

        public WorkOrderRequisitionViewModelBase(IContainer container) : base(container) { }

        #endregion
    }
}