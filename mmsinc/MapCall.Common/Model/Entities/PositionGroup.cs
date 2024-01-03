using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Migrations;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class PositionGroup : IEntityLookup, IThingWithDocuments, IThingWithNotes
    {
        #region Consts

        public struct StringLengths
        {
            #region Constants

            public const int GROUP = CreatePositionGroupsTable.MAX_GROUP_LENGTH,
                             POSITION_DESCRIPTION =
                                 100, // 09/18/2020 Updated from 50 to 100 since SAP doesn't have a max they send on this. Also if your coming here to see the length, Don't edit the sync file in Excel. Its only leads to bad news bears.
                             BUSINESS_UNIT = 256, // ditto, 6 to 256
                             BUSINESS_UNIT_DESCRIPTION = 50,
                             SAP_POSITION_GROUP_KEY = 8;

            #endregion
        }

        public const string TO_STRING_FORMAT = "{0} - {1} - {2} - {3} - {4} - {5}";

        #endregion

        #region Private Members

        private PositionGroupDisplayItem _display;

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual string TableName => nameof(PositionGroup) + "s";

        public virtual string Description => (_display ?? (_display = new PositionGroupDisplayItem {
            Id = Id,
            Group = Group,
            PositionDescription = PositionDescription,
            BusinessUnit = BusinessUnit,
            BusinessUnitDescription = BusinessUnitDescription,
            SAPCompanyCode = SAPCompanyCode.Description,
            SAPPositionGroupKey = SAPPositionGroupKey
        })).Display;

        public virtual string Group { get; set; }
        public virtual string PositionDescription { get; set; }
        public virtual PositionGroupCommonName CommonName { get; set; }

        // This is not a reference at the moment because they're going to import
        // a bunch of records with BusinessUnits that aren't actually in the 
        // database yet. hurrah. -Ross 8/27/2014
        public virtual string BusinessUnit { get; set; }
        public virtual string BusinessUnitDescription { get; set; }
        public virtual State State { get; set; }
        public virtual SAPCompanyCode SAPCompanyCode { get; set; }

        /// <summary>
        /// The unique key from SAP. You might look at the data and ask yourself "Why isn't
        /// this an int?". It's because there are a couplpe values that are non-numeric.
        /// </summary>
        public virtual string SAPPositionGroupKey { get; set; }

        public virtual IList<Document<PositionGroup>> Documents { get; set; }
        public virtual IList<Note<PositionGroup>> Notes { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        #endregion

        #region Constructors

        public PositionGroup()
        {
            Documents = new List<Document<PositionGroup>>();
            Notes = new List<Note<PositionGroup>>();
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return Description;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    [Serializable]
    public class PositionGroupDisplayItem : DisplayItem<PositionGroup>
    {
        public string Group { get; set; }
        public string PositionDescription { get; set; }
        public string BusinessUnit { get; set; }
        public string BusinessUnitDescription { get; set; }
        [SelectDynamic("Description")]
        public string SAPCompanyCode { get; set; }
        public string SAPPositionGroupKey { get; set; }
        public override string Display =>
            $"{Group} - {PositionDescription} - {BusinessUnit} - {BusinessUnitDescription} - {SAPCompanyCode} - {SAPPositionGroupKey}";
    }
}
