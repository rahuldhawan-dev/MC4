using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.ObjectMapping;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class GeneratorViewModel : ViewModel<Generator>
    {
        #region Properties

        [AutoMap(MapDirections.ToViewModel)]
        public override int Id { get; set; }

        [Required]
        [DropDown]
        [DisplayName("Equipment")]
        [EntityMustExist(typeof(Equipment))]
        //[EntityMap("Equipment")]
        public virtual int? Equipment { get; set; }
        [DropDown]
        [DisplayName("Emergency Power Type")]
        [EntityMustExist(typeof(EmergencyPowerType))]
        //[EntityMap("EmergencyPowerType")]
        public virtual int? EmergencyPowerType { get; set; }
        [DropDown]
        [DisplayName("Engine Manufacturer")]
        [EntityMap, EntityMustExist(typeof(EquipmentManufacturer))]
        public virtual int? EngineManufacturer { get; set; }

        [DropDown("EquipmentModel", "ByManufacturerID", DependsOn = "EngineManufacturer", PromptText = "Please select the engine manufacturer")]
        [DisplayName("Engine Model")]
        [EntityMustExist(typeof(EquipmentModel))]
        public virtual int? EngineModel { get; set; }
        
        [DropDown]
        [DisplayName("Generator Manufacturer")]
        [EntityMap, EntityMustExist(typeof(EquipmentManufacturer))]
        public virtual int? GeneratorManufacturer { get; set; }
        
        [DropDown("EquipmentModel", "ByManufacturerID", DependsOn = "GeneratorManufacturer", PromptText = "Please select the generator manufacturer")]
        [DisplayName("Generator Model")]
        [EntityMustExist(typeof(EquipmentModel))]
        //[EntityMap("GeneratorModel")]
        public virtual int? GeneratorModel { get; set; }
        
        [DropDown]
        [DisplayName("Fuel Type")]
        [EntityMustExist(typeof(FuelType))]
        //[EntityMap("FuelType")]
        public virtual int? FuelType { get; set; }

        public virtual string EngineSerialNumber { get; set; }
        public virtual string GeneratorSerialNumber { get; set; }
        public virtual decimal? OutputVoltage { get; set; }
        public virtual decimal? OutputKW { get; set; }
        public virtual decimal? LoadCapacity { get; set; }
        public virtual bool HasParallelElectricOperation { get; set; }
        public virtual bool HasAutomaticStart { get; set; }
        public virtual bool HasAutomaticPowerTransfer { get; set; }
        public virtual bool IsPortable { get; set; }
        public virtual bool SCADA { get; set; }
        public virtual string TrailerVIN { get; set; }
        public virtual string GVWR { get; set; }
        public virtual decimal? FuelGPH { get; set; }
        public virtual int? BTU { get; set; }
        public virtual int? HP { get; set; }
        public virtual string AQPermitNumber { get; set; }

        #endregion
        
        #region Constructors

        public GeneratorViewModel(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override void Map(Generator entity)
        {
            base.Map(entity);

            if (entity.Equipment != null)
                Equipment = entity.Equipment.Id;
            if (entity.EmergencyPowerType != null)
                EmergencyPowerType = entity.EmergencyPowerType.Id;
            if (entity.EngineManufacturer != null)
                EngineManufacturer = entity.EngineManufacturer.Id;
            if (entity.EngineModel != null)
                EngineModel = entity.EngineModel.Id;
            if (entity.GeneratorManufacturer != null)
                GeneratorManufacturer = entity.GeneratorManufacturer.Id;
            if (entity.GeneratorModel != null)
                GeneratorModel = entity.GeneratorModel.Id;
            if (entity.FuelType != null)
                FuelType = entity.FuelType.Id;
        }

        public override Generator MapToEntity(Generator entity)
        {
            entity = base.MapToEntity(entity);
            entity.Equipment = (Equipment != null) ?
                _container.GetInstance<RepositoryBase<Equipment>>().Find((int)Equipment) : null;
            entity.EmergencyPowerType = (EmergencyPowerType != null) ?
                _container.GetInstance<MMSINC.Data.NHibernate.IRepository<EmergencyPowerType>>().Find((int)EmergencyPowerType) : null;
            entity.EngineManufacturer = (EngineManufacturer != null) ?
                _container.GetInstance<IEquipmentManufacturerRepository>().Find((int)EngineManufacturer) : null;
            entity.EngineModel = (EngineModel != null) ?
                _container.GetInstance<IEquipmentModelRepository>().Find((int)EngineModel) : null;
            entity.GeneratorManufacturer = (GeneratorManufacturer != null) ?
                _container.GetInstance<IEquipmentManufacturerRepository>().Find((int)GeneratorManufacturer) : null;
            entity.GeneratorModel = (GeneratorModel != null) ?
                _container.GetInstance<IEquipmentModelRepository>().Find((int)GeneratorModel) : null;
            entity.FuelType = (FuelType != null) ?
                _container.GetInstance<MMSINC.Data.NHibernate.IRepository<FuelType>>().Find((int)FuelType) : null;

            return entity;
        }

        #endregion
    }

    public class CreateGenerator : GeneratorViewModel
    {
        public CreateGenerator(IContainer container) : base(container) {}
    }

    public class EditGenerator : GeneratorViewModel
    {
        public EditGenerator(IContainer container) : base(container) {}
    }
}