using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Bond : IEntity, IThingWithDocuments, IThingWithNotes
    {
        #region Constants

        public struct StringLengths
        {
            public const int BOND_NUMBER = 12,
                             PRINCIPAL = 50,
                             OBLIGEE = 50,
                             BONDING_AGENCY = 50;
        }

        #endregion

        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }
        public virtual State State { get; set; }
        public virtual County County { get; set; }
        public virtual Town Town { get; set; }
        public virtual BondType BondType { get; set; }
        public virtual BondPurpose BondPurpose { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual string BondNumber { get; set; }
        public virtual string Principal { get; set; }
        public virtual string Obligee { get; set; }
        public virtual bool? RecurringBond { get; set; }
        public virtual string BondingAgency { get; set; }
        public virtual decimal? BondValue { get; set; }
        public virtual decimal? AnnualPremium { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? StartDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? EndDate { get; set; }

        public virtual int PermitsBondId { get; set; }
        public virtual bool? BondOpen { get; set; }

        #endregion

        #region Logical Properties

        #region Documents

        public virtual IList<BondDocument> BondDocuments { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return BondDocuments.Map(epd => (IDocumentLink)epd); }
        }

        public virtual IList<Document> Documents
        {
            get { return BondDocuments.Map(epd => epd.Document); }
        }

        #endregion

        #region Notes

        public virtual IList<BondNote> BondNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return BondNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return BondNotes.Map(n => n.Note); }
        }

        #endregion

        [DoesNotExport]
        public virtual string TableName => nameof(Bond) + "s";

        #endregion

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return String.Format("{0} - {1}", BondNumber, Obligee);
        }

        #endregion
    }

    [Serializable]
    public class BondType : EntityLookup { }

    [Serializable]
    public class BondPurpose : ReadOnlyEntityLookup
    {
        #region Constants

        public struct Indices
        {
            public const int MAINTENANCE_BOND = 1,
                             PERFORMANCE_BOND = 2,
                             ROAD_OPENING_PERMIT = 3,
                             SUBDIVISION_BOND = 4,
                             MORATORIUM = 5,
                             TRAFFIC_CONTROL = 6,
                             INSPECTION = 7;
        }

        #endregion

        #region Properties

        //public virtual int Id { get; protected set; }
        //public virtual string Description { get; set; }

        #endregion
    }
}
