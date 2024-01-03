using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels.OperatorLicenses
{
    public class RemoveOperatorLicenseWasteWaterSystem : ViewModel<OperatorLicense>
    {
        #region Properties
        
        [DoesNotAutoMap("Manually mapped.")]
        [Required, EntityMustExist(typeof(WasteWaterSystem))]
        public int? WasteWaterSystem { get; set; }
        
        #endregion
        
        #region Constructors
        
        public RemoveOperatorLicenseWasteWaterSystem(IContainer container) : base(container) { }
        
        #endregion
        
        #region Exposed Methods

        public override OperatorLicense MapToEntity(OperatorLicense entity)
        {
            var system = entity.WasteWaterSystems.SingleOrDefault(x => x.Id == WasteWaterSystem.Value);
            entity.WasteWaterSystems.Remove(system);
            return entity;
        }

        #endregion
    }
}
