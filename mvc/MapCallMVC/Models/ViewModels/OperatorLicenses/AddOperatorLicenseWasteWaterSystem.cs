using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels.OperatorLicenses
{
    public class AddOperatorLicenseWasteWaterSystem : ViewModel<OperatorLicense>
    {
        #region Properties
        
        [DoesNotAutoMap("Manually mapped.")]
        [Required, EntityMustExist(typeof(WasteWaterSystem)), DropDown]
        public int? WasteWaterSystem { get; set; }
        
        #endregion
        
        #region Constructors
        
        public AddOperatorLicenseWasteWaterSystem(IContainer container) : base(container) { }
        
        #endregion
        
        #region Exposed Methods

        public override OperatorLicense MapToEntity(OperatorLicense entity)
        {
            var system = _container.GetInstance<IRepository<WasteWaterSystem>>().Find(WasteWaterSystem.Value);
            
            if (!entity.WasteWaterSystems.Contains(system))
            {
                entity.WasteWaterSystems.Add(system);
            }

            return entity;
        }
        
        #endregion
    }
}
