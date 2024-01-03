using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class FacilityProcessViewModel : ViewModel<FacilityProcess>
    {
        #region Constructors

        public FacilityProcessViewModel(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [DropDown, Required, DoesNotAutoMap("Cascading only")] // Needed for cascades only
        public int? OperatingCenter { get; set; }

        [Required, EntityMustExist(typeof(Facility)), EntityMap]
        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn = "OperatingCenter",
            PromptText = "Select an operating center above")]
        public int? Facility { get; set; }

        [Required, DoesNotAutoMap("Needed for cascades only")]
        [DropDown]
        public int? ProcessStage { get; set; }

        [Required, EntityMap, EntityMustExist(typeof(Process))]
        [DropDown("Facilities", "Process", "ByProcessStage", DependsOn = "ProcessStage",
            PromptText = "Select a process stage above")]
        public int? Process { get; set; }

        public string FacilityProcessDescription { get; set; }

        #endregion

        #region Public Methods

        public override void Map(FacilityProcess entity)
        {
            base.Map(entity);

            OperatingCenter = entity.Facility.OperatingCenter.Id;
            ProcessStage = entity.Process.ProcessStage.Id;
        }

        #endregion
    }

    public class AddFacilityProcessForFacilityController : ViewModel<Facility>
    {
        #region Constructors

        public AddFacilityProcessForFacilityController(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [Secured]
        [Required, EntityMustExist(typeof(Facility)), EntityMap(MapDirections.None)]
        public int? Facility { get; set; }

        [Required, DoesNotAutoMap]
        [DropDown] // Needed for cascades only
        public int? ProcessStage { get; set; }

        [Required, EntityMap(MapDirections.None), EntityMustExist(typeof(Process))]
        [DropDown("Facilities", "Process", "ByProcessStage", DependsOn = "ProcessStage", PromptText = "Select a process stage above")]
        public int? Process { get; set; }

        #endregion

        #region Public Methods

        public override void Map(Facility entity)
        {
            base.Map(entity);
            Facility = entity.Id;
        }

        public override Facility MapToEntity(Facility entity)
        {
           // base.MapToEntity(entity);
            var fp = new FacilityProcess();
            fp.Facility = _container.GetInstance<IFacilityRepository>().Find(Facility.Value);
            fp.Process = _container.GetInstance<IProcessRepository>().Find(Process.Value);

            entity.FacilityProcesses.Add(fp);

            return entity;
        }

        #endregion
    }

    public class RemoveFacilityProcessForFacilityController : ViewModel<Facility>
    {
        [Required, DoesNotAutoMap]
        public int? FacilityProcessId { get; set; }

        public RemoveFacilityProcessForFacilityController(IContainer container) : base(container) { }

        public override Facility MapToEntity(Facility entity)
        {
            // NOTE: Don't call base method.
            var fp = entity.FacilityProcesses.SingleOrDefault(x => x.Id == FacilityProcessId.Value);
            entity.FacilityProcesses.Remove(fp);
            return entity;
        }
    }

    public class RemoveFacilityProcessStep : ViewModel<FacilityProcess>
    {
        [DoesNotAutoMap]
        [Required, EntityMustExist(typeof(FacilityProcessStep))]
        public int? FacilityProcessStepId { get; set; }

        public RemoveFacilityProcessStep(IContainer container) : base(container) { }

        public override FacilityProcess MapToEntity(FacilityProcess entity)
        {
            // NOTE: Don't call base method.
            var fp = entity.FacilityProcessSteps.SingleOrDefault(x => x.Id == FacilityProcessStepId.Value);
            entity.FacilityProcessSteps.Remove(fp);
            return entity;
        }
    }

    public class SearchFacilityProcess : SearchSet<FacilityProcess>
    {
        #region Properties

        [DropDown, Search(CanMap=false)] // Needed for cascades only
        public int? OperatingCenter { get; set; }

        [DropDown("", "Facility", "ByOperatingCenterId", DependsOn="OperatingCenter", PromptText="Select an operating center above")]
        public int? Facility { get; set; }

        [DropDown, Search(CanMap=false)] // Needed for cascades only
        public int? ProcessStage { get; set; }

        [DropDown("Facilities", "Process", "ByProcessStage", DependsOn="ProcessStage", PromptText="Select a process stage above")]
        public int? Process { get; set; }

        #endregion
    }
}