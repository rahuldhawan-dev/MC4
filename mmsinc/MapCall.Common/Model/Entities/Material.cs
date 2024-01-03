using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Material : ReadOnlyEntityLookup, IValidatableObject
    {
        #region Constants

        public const int THE_STRING_LENGTH = 15;

        #endregion

        #region Private Members

        private MaterialDisplayItem _display;

        #endregion

        #region Properties

        #region Table Columns

        [StringLength(THE_STRING_LENGTH)]
        public virtual string OldPartNumber { get; set; }

        [StringLength(THE_STRING_LENGTH)]
        public virtual string Size { get; set; }

        [Required]
        [StringLength(THE_STRING_LENGTH)]
        public virtual string PartNumber { get; set; }

        [Required]
        public virtual bool IsActive { get; set; }

        public virtual string UnitOfMeasure { get; set; }

        [Required]
        public virtual bool DoNotOrder { get; set; }

        #endregion

        #region References

        public virtual IList<OperatingCenterStockedMaterial> OperatingCenterStockedMaterials { get; set; }
        public virtual IList<OperatingCenter> OperatingCenters { get; set; }

        #endregion

        #region Logical Properties

        public virtual string FullDescription => (_display ?? (_display = new MaterialDisplayItem {
            Description = Description,
            PartNumber = PartNumber
        })).Display;

        #endregion

        #endregion

        #region Constructors

        public Material()
        {
            OperatingCenters = new List<OperatingCenter>();
            OperatingCenterStockedMaterials = new List<OperatingCenterStockedMaterial>();
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return FullDescription;
        }

        #endregion
    }

    [Serializable]
    public class MaterialDisplayItem : DisplayItem<Material>
    {
        public string Description { get; set; }
        public string PartNumber { get; set; }

        public override string Display =>
            string.IsNullOrWhiteSpace(Description) ? PartNumber : $"{PartNumber} - {Description}";
    }

    [Serializable]
    public class MaterialUsed : IEntity
    {
        #region Properties

        public virtual int Id { get; set; }
        public virtual WorkOrder WorkOrder { get; set; }
        public virtual Material Material { get; set; }
        public virtual int Quantity { get; set; }
        public virtual string NonStockDescription { get; set; }
        public virtual StockLocation StockLocation { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public virtual decimal? Cost { get; set; }

        [DisplayFormat(DataFormatString = "{0:c}")]
        public virtual decimal? Price => Cost * 1.25m;

        #endregion
    }
}
