using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace Contractors.Models.ViewModels
{
    public abstract class MainBreakViewModel : ViewModel<MainBreak>
    {
        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(WorkOrder))]
        public virtual int? WorkOrder { get; set; }

        [View(MainBreak.MATERIAL), Required, DropDown]
        [EntityMap, EntityMustExist(typeof(MainBreakMaterial))]
        public virtual int MainBreakMaterial { get; set; }

        [View(MainBreak.MAIN_CONDITION), Required, DropDown]
        [EntityMap, EntityMustExist(typeof(MainCondition))]
        public virtual int MainCondition { get; set; }

        [View(MainBreak.FAILURE_TYPE), Required, DropDown]
        [EntityMap, EntityMustExist(typeof(MainFailureType))]
        public virtual int? MainFailureType { get; set; }

        [Required]
        public virtual decimal Depth { get; set; }

        [View(MainBreak.SOIL_CONDITION), Required, DropDown]
        [EntityMap, EntityMustExist(typeof(MainBreakSoilCondition))]
        public virtual int MainBreakSoilCondition { get; set; }

        [Required]
        public virtual int CustomersAffected { get; set; }

        [Required]
        public virtual decimal ShutdownTime { get; set; }

        [View(MainBreak.DISINFECTION_METHOD), Required, DropDown]
        [EntityMap, EntityMustExist(typeof(MainBreakDisinfectionMethod))]
        public virtual int MainBreakDisinfectionMethod { get; set; }

        [View(MainBreak.FLUSH_METHOD), Required, DropDown]
        [EntityMap, EntityMustExist(typeof(MainBreakFlushMethod))]
        public virtual int MainBreakFlushMethod { get; set; }

        [RegularExpression(@"(^[0-2]$)|(^[0-2]?\.[0-9]$)", ErrorMessage="Must be in steps of .1")]
        public virtual decimal? ChlorineResidual { get; set; }

        [Required]
        public virtual bool BoilAlertIssued { get; set; }

        [View(MainBreak.SIZE), Required, DropDown]
        [EntityMap, EntityMustExist(typeof(ServiceSize))]
        public virtual int ServiceSize { get; set; }

        [RequiredWhen(nameof(DisplayFootageReplaced), ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        public virtual int? FootageReplaced { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(MainBreakMaterial)),
         RequiredWhen(nameof(DisplayFootageReplaced), ComparisonType.EqualTo, true, FieldOnlyVisibleWhenRequired = true)]
        public virtual int? ReplacedWith { get; set; }

        [DoesNotAutoMap]
        public bool DisplayFootageReplaced => DisplayFootageReplacedAndWithReplaced();

        #endregion

        #region Private Method

        public bool DisplayFootageReplacedAndWithReplaced()
        {
            var workOrder = _container.GetInstance<IRepository<WorkOrder>>().Find(WorkOrder.Value);
            return workOrder != null && workOrder.WorkDescription?.Id ==
                (int)WorkDescription.Indices.WATER_MAIN_BREAK_REPLACE;
        }

        #endregion

        #region Constructors

        protected MainBreakViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class EditMainBreak : MainBreakViewModel
    {
        #region Constructors

        public EditMainBreak(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateMainBreak : MainBreakViewModel
    {
        #region Constructors

        public CreateMainBreak(IContainer container) : base(container) {}

        #endregion
    }
}