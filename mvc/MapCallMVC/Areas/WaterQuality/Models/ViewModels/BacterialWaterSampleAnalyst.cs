using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.WaterQuality.Models.ViewModels
{
    public class BacterialWaterSampleAnalystViewModel : ViewModel<BacterialWaterSampleAnalyst>
    {
        #region Properties

        public bool IsActive { get; set; }

        #endregion

        #region Constructors

        public BacterialWaterSampleAnalystViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateBacterialWaterSampleAnalyst : BacterialWaterSampleAnalystViewModel
    {
        #region Properties

        [Required, DropDown, EntityMap(MapDirections.None), EntityMustExist(typeof(OperatingCenter))] // This dropdown only exists for filtering and to add the first OperatingCenter associated with the Analyst.
        public int? OperatingCenter { get; set; }

        [DropDown("", "Employee", "ActiveEmployeesByOperatingCenterId", DependsOn = nameof(OperatingCenter))]
        [EntityMap, EntityMustExist(typeof(Employee))]
        public int? Employee { get; set; }

        #endregion

        #region Constructors

        public CreateBacterialWaterSampleAnalyst(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void SetDefaults()
        {
            base.SetDefaults();
            IsActive = true; // Convenience.
        }

        public override BacterialWaterSampleAnalyst MapToEntity(BacterialWaterSampleAnalyst entity)
        {
            base.MapToEntity(entity);

            // This is done for convenience. Rather than require the user to go through the operating centers tab
            // to add only one, use the selected OperatingCenter used for cascading and add that initially.
            var opc = _container.GetInstance<IOperatingCenterRepository>().Find(OperatingCenter.Value);
            entity.OperatingCenters.Add(opc);

            return entity;
        }

        #endregion

    }

    public class EditBacterialWaterSampleAnalyst : BacterialWaterSampleAnalystViewModel
    {
        #region Fields

        private BacterialWaterSampleAnalyst _original;

        #endregion

        #region Properties

        [DoesNotAutoMap("Display only")]
        public BacterialWaterSampleAnalyst Display
        {
            get
            {
                if (_original == null)
                {
                    _original = _container.GetInstance<IRepository<BacterialWaterSampleAnalyst>>().Find(Id);
                }

                return _original;
            }
        } 

        #endregion

        #region Constructors

        public EditBacterialWaterSampleAnalyst(IContainer container) : base(container) { }

        #endregion
    }

    public class AddBacterialWaterSampleAnalystOperatingCenter : ViewModel<BacterialWaterSampleAnalyst>
    {
        #region Properties

        [DropDown, EntityMap(MapDirections.None)] // None because it's manually mapped
        [Required, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        #endregion

        #region Constructors

        public AddBacterialWaterSampleAnalystOperatingCenter(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override BacterialWaterSampleAnalyst MapToEntity(BacterialWaterSampleAnalyst entity)
        {
            // Do not call base.MapToEntity.

            // We only want to add the Operating Cecnter if it doesn't already exist in the list. 
            // It's not worth throwing validation errors/regular errors for if it's already there. 
            // Just act like it worked correctly.
            if (!entity.OperatingCenters.Any(x => x.Id == OperatingCenter.Value))
            {
                var opc = _container.GetInstance<IOperatingCenterRepository>().Find(OperatingCenter.Value);
                entity.OperatingCenters.Add(opc);
            }

            return entity;
        }

        #endregion
    }

    public class RemoveBacterialWaterSampleAnalystOperatingCenter : ViewModel<BacterialWaterSampleAnalyst>
    {
        #region Properties

        [DoesNotAutoMap("Manually mapped")]
        [Required, EntityMustExist(typeof(OperatingCenter))]
        public virtual int? OperatingCenter { get; set; }

        #endregion

        #region Constructors

        public RemoveBacterialWaterSampleAnalystOperatingCenter(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override BacterialWaterSampleAnalyst MapToEntity(BacterialWaterSampleAnalyst entity)
        {
            // Do not call base.MapToEntity.

            // Not worth throwing any errors if the OperatingCenter doesn't exist in the list.
            var opc = entity.OperatingCenters.SingleOrDefault(x => x.Id == OperatingCenter.Value);
            if (opc != null)
            {
                entity.OperatingCenters.Remove(opc);
            }

            return entity;
        }

        #endregion
    }

}