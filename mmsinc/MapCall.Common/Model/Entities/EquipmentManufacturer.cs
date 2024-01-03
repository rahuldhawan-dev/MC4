using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EquipmentManufacturer : ReadOnlyEntityLookup
    {
        #region Constants

        // There's a hundred of these, that's why this isn't being stored as an indice.
        public const string MAPCALLDESCRIPTION_OTHER = "OTHER";

        #endregion

        public virtual EquipmentType EquipmentType { get; set; }

        [StringLength(StringLengths.DESCRIPTION)]
        public virtual string MapCallDescription { get; set; }

        public virtual string Display => new EquipmentManufacturerDisplayItem {
            Description = Description,
            MapCallDescription = MapCallDescription
        }.Display;

        public override string ToString()
        {
            return Display;
        }
    }

    public class EquipmentManufacturerDisplayItem : DisplayItem<EquipmentManufacturer>
    {
        public string Description { get; set; }
        public string MapCallDescription { get; set; }

        public override string Display =>
            (!string.IsNullOrWhiteSpace(MapCallDescription) ? MapCallDescription : Description);
    }
}
