using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class EditGeneralWorkOrder : ViewModel<WorkOrder>
    {
        #region Properties

        [DoesNotAutoMap("Not an actual view property, set by MapToEntity and used by controller. It smells yes. -Ross 2/3/2015")]
        public bool SAPWorkOrdersEnabled { get; set; }

        //[RequiredWhen("SAPWorkOrdersEnabled", ComparisonType.EqualTo, true)]
        public long? SAPNotificationNumber { get; set; }
        
        //[RequiredWhen("SAPWorkOrdersEnabled", ComparisonType.EqualTo, true)]
        public long? SAPWorkOrderNumber { get; set; }

        [StringLength(WorkOrder.StringLengths.PREMISE_NUMBER)]
        public string PremiseNumber { get; set; }

        [StringLength(WorkOrder.StringLengths.ACCOUNT_CHARGED)]
        [View(DisplayName = "WBS Charged")]
        public string AccountCharged { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(WorkDescription))]
        public int? WorkDescription { get; set; }

        public string SAPErrorCode { get; set; }

        public DateTime? MaterialsApprovedOn { get; set; }
        public DateTime? MaterialPostingDate { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(User))]
        public int? MaterialsApprovedBy { get; set; }

        public DateTime? ApprovedOn { get; set; }
        [DropDown, EntityMap, EntityMustExist(typeof(User))]
        public int? ApprovedBy { get; set; }

        [StringLength(WorkOrder.StringLengths.MATERIALS_DOC_ID)]
        public string MaterialsDocID { get; set; }

        public bool DigitalAsBuiltRequired { get; set; }

        #endregion

        #region Constructors

        public EditGeneralWorkOrder(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override void Map(WorkOrder entity)
        {
            base.Map(entity);
            SAPWorkOrdersEnabled = WorkOrderViewModel.ShouldSendToSAP(entity.OperatingCenter);
        }

        public override WorkOrder MapToEntity(WorkOrder entity)
        {
            entity = base.MapToEntity(entity);

            WorkOrderViewModel
               .MaybeMapDigitalAsBuiltRequired(entity, _container, WorkDescription, DigitalAsBuiltRequired);
            
            WorkOrderViewModel
               .SetPremiseIfAvailable(entity, _container);
                
            return entity;
        }

        #endregion
    }
}