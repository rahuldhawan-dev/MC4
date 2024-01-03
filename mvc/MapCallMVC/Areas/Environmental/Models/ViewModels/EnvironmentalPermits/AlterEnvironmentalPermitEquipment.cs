using MapCall.Common.Model.Entities;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels.EnvironmentalPermits
{
    public class AlterEnvironmentalPermitEquipment : EnvironmentalPermitViewModel
    {
        #region Properties

        // This hugely smells. Don't do this.
        // public virtual IEnumerable<Equipment> Equipment { get; set; }

        [EntityMustExist(typeof(Equipment))]
        [DoesNotAutoMap("Manually used in MapToEntity")]
        public virtual int EquipmentId { get; set; }

        #endregion

        #region Constructors

        public AlterEnvironmentalPermitEquipment(IContainer container) : base(container) { }

        #endregion
    }
}