using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    public class GasMonitorCalibrationViewModel : ViewModel<GasMonitorCalibration>
    {
        #region Properties
       
        [Required]
        public DateTime? CalibrationDate { get; set; }
        [Required]
        public bool? CalibrationPassed { get; set; }

        [Multiline, RequiredWhen(nameof(CalibrationPassed), false)]
        public string CalibrationFailedNotes { get; set; }

        #endregion

        #region Constructor

        public GasMonitorCalibrationViewModel(IContainer container) : base(container)
        {
        }

        #endregion
    }

    public class EditGasMonitorCalibration : GasMonitorCalibrationViewModel
    {
        #region Constructor

        public EditGasMonitorCalibration(IContainer container) : base(container)
        {
        }

        #endregion
    }

    public class SearchGasMonitorCalibration : SearchSet<GasMonitorCalibration>
    {
        #region Properties

        [SearchAlias("GasMonitor", "G", "Id", Required = true)]
        [EntityMap, EntityMustExist(typeof(GasMonitor))]
        public int? GasMonitor { get; set; }

        [SearchAlias("E.Facility", "F", "OperatingCenter.Id", Required = true)]
        [DropDown, EntityMap, EntityMustExist(typeof(OperatingCenter))]
        public int? OperatingCenter { get; set; }

        [DropDown("", "Facility", "GetByOperatingCenterId", DependsOn = nameof(OperatingCenter))]
        [EntityMap, EntityMustExist(typeof(Facility))]
        [SearchAlias("G.Equipment", "E", "Facility.Id", Required = true)] // Required for OperatingCenter filtering
        public int? Facility { get; set; }

        [DropDown("", "User", "LockoutFormUsersByOperatingCenterId", DependsOn = nameof(OperatingCenter))]
        [EntityMap, EntityMustExist(typeof(User))]
        public int? CreatedBy { get; set; }

        public DateRange CalibrationDate { get; set; }

        #endregion
    }

    public class AddGasMonitorCalibration : ViewModel<GasMonitor>
    {
        #region Properties

        // Doing it this way so the validation is shared between create/edit 
        // models and also so we can use MapToEntity on the view model.
        [DoesNotAutoMap("Done manually in MapToEntity")]
        public GasMonitorCalibrationViewModel ViewModel { get; set; }

        #endregion

        #region Constructor

        public AddGasMonitorCalibration(IContainer container) : base(container)
        {
        }

        #endregion

        #region Public Methods

        public override GasMonitor MapToEntity(GasMonitor entity)
        {
            // don't call base.MapToEntity
            var calibration = _container.GetInstance<GasMonitorCalibration>();
            calibration.GasMonitor = entity;
            ViewModel.MapToEntity(calibration);
            entity.Calibrations.Add(calibration);

            return entity;
        }

        #endregion
    }

    public class RemoveGasMonitorCalibration : ViewModel<GasMonitor>
    {
        #region Properties

        [Required, DoesNotAutoMap, EntityMustExist(typeof(GasMonitorCalibration))]
        public int? GasMonitorCalibrationId { get; set; }

        #endregion

        #region Constructors

        public RemoveGasMonitorCalibration(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override GasMonitor MapToEntity(GasMonitor entity)
        {
            // Do not call base.MapToEntity.
            var toRemove = entity.Calibrations.Single(x => x.Id == GasMonitorCalibrationId.Value);
            entity.Calibrations.Remove(toRemove);
            return entity;
        }

        #endregion
    }
}