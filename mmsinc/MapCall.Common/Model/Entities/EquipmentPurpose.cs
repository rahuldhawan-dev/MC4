using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Migrations;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EquipmentPurpose : ReadOnlyEntityLookup
    {
        #region Constants

        public new struct StringLengths
        {
            public const int ABBREVIATION = CreateEquipmentEquipmentTypeForBug1442.StringLengths.ABBREVIATION,
                             DESCRIPTION = CreateEquipmentEquipmentTypeForBug1442.StringLengths.DESCRIPTION;
        }

        public struct Indices
        {
            public const int PERSONAL_GAS_DETECTOR = 203;
        }

        // This is a bit of a smell because this entity is erroneously setup as a ReadOnlyEntityLookup
        // but we have controllers that allow users to add/edit any of these. Unit/functional tests
        // can't have factories to create these by Id since we don't generate the database Ids for this.
        // Also, there are multiple EquipmentPurpose with the same EquipmentTypeId, so it can't be done that
        // way, etiher.
        // Therefore, this abbreviation exists for being able to run GasMonitor/Equipment queries.
        public const string PERSONAL_GAS_DETECTOR_ABBREVIATION = "PGDR";
        public const string POINT_OF_ENTRY_ABBREVIATION = "POE";

        #endregion

        #region Private Members

        private EquipmentPurposeDisplayItem _display;

        #endregion

        #region Properties

        public virtual EquipmentCategory EquipmentCategory { get; set; }
        public virtual EquipmentLifespan EquipmentLifespan { get; set; }
        public virtual EquipmentSubCategory EquipmentSubCategory { get; set; }
        public virtual EquipmentType EquipmentType { get; set; }

        [Required]
        public virtual string Abbreviation { get; set; }

        public virtual IList<Equipment> Equipment { get; set; }
        public virtual IList<TaskGroup> TaskGroups { get; set; }

        public virtual bool HasNoEquipmentType { get; set; }

        #endregion

        #region Constructors

        public EquipmentPurpose()
        {
            Equipment = new List<Equipment>();
        }

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return (_display ?? (_display = new EquipmentPurposeDisplayItem {
                Id = Id,
                Abbreviation = Abbreviation,
                Description = Description,
                EquipmentCategory = EquipmentCategory?.Description,
                EquipmentSubCategory = EquipmentSubCategory?.Description
            })).Display;
        }

        #endregion
    }

    [Serializable]
    public class EquipmentPurposeDisplayItem : DisplayItem<EquipmentPurpose>
    {
        public string Abbreviation { get; set; }
        public string Description { get; set; }

        [SelectDynamic("Description")]
        public string EquipmentCategory { get; set; }

        [SelectDynamic("Description")]
        public string EquipmentSubCategory { get; set; }

        public override string Display =>
            $"{Abbreviation} - {Id} : - {Description} - {EquipmentCategory} - {EquipmentSubCategory}";
    }
}
