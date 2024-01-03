using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class EditWorkDescription : ViewModel<WorkDescription>
    {
        #region Properties

        [DoesNotAutoMap("Display only")]
        public WorkDescription Display => _container.GetInstance<IRepository<WorkDescription>>().Find(Id);

        [Required]
        public int? FirstRestorationCostBreakdown { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(RestorationAccountingCode))]
        public int? FirstRestorationAccountingCode { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(RestorationProductCode))]
        public int? FirstRestorationProductCode { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RestorationAccountingCode))]
        public int? SecondRestorationAccountingCode { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(RestorationProductCode))]
        public int? SecondRestorationProductCode { get; set; }
        public int? SecondRestorationCostBreakdown { get; set; }

        [Required]
        public decimal? TimeToComplete { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(AccountingType))]
        public int? AccountingType { get; set; }

        [Required]
        public bool? ShowBusinessUnit { get; set; }

        [Required]
        public bool? ShowApprovalAccounting { get; set; }
      
        [StringLength(WorkDescription.StringLengths.MAINT_ACT_TYPE)]
        public string MaintenanceActivityType { get; set; }

        [Required, DropDown, EntityMap, EntityMustExist(typeof(PlantMaintenanceActivityType))]
        public int? PlantMaintenanceActivityType { get; set; }
        
        [Secured(AppliesToAdmins = false)]
        public virtual bool MarkoutRequired { get; set; }
        [Secured(AppliesToAdmins = false)]
        public virtual bool MaterialsRequired { get; set; }
        [Secured(AppliesToAdmins = false)]
        public virtual bool JobSiteCheckListRequired { get; set; }
        
        [Secured(AppliesToAdmins = false)]
        public virtual bool DigitalAsBuiltRequired { get; set; } 

        #endregion

        #region Constructors

        public EditWorkDescription(IContainer container) : base(container) { }

        #endregion
    }
}
