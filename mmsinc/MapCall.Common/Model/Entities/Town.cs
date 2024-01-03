using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.ClassExtensions.IListExtensions;
using MapCall.Common.Model.Mappings;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Town : IEntity, IValidatableObject, IThingWithDocuments, IThingWithNotes
    {
        #region Constants

        public struct StringLengths
        {
            public const int ADDRESS = 50,
                             CONTACT_NAME = 100,
                             COUNTY = 50,
                             EMERGENCY_CONTACT = 25,
                             EMERGENCY_FAX = 12,
                             EMERGENCY_PHONE = 12,
                             FAX = 12,
                             FD1_CONTACT = 25,
                             FD1_FAX = 12,
                             FD1_PHONE = 12,
                             PHONE = 50,
                             STATE = 2,
                             TOWN = 50,
                             TOWN_NAME = 50,
                             ZIP = 10,
                             LINK = 1,
                             LAT = 50,
                             OPERATING_SECTION = 255,
                             DBA = 52,
                             CRITICAL_MAIN_BREAK_NOTES = 255;

            public const int LON = LAT;
        }

        #endregion

        #region Properties

        #region Table Column Properties

        public virtual int Id { get; set; }

        [StringLength(StringLengths.ADDRESS)]
        public virtual string Address { get; set; }

        [StringLength(StringLengths.CONTACT_NAME)]
        public virtual string ContactName { get; set; }

        [StringLength(StringLengths.COUNTY), Obsolete("You should be using the mapped County property instead")]
        public virtual string CountyName { get; set; }

        // TODO: Seriously, why is this a float? -Ross 9/18/2014
        public virtual float? DistrictId { get; set; }

        [StringLength(StringLengths.EMERGENCY_CONTACT)]
        public virtual string EmergencyContact { get; set; }

        [StringLength(StringLengths.EMERGENCY_FAX)]
        public virtual string EmergencyFax { get; set; }

        [StringLength(StringLengths.EMERGENCY_PHONE)]
        public virtual string EmergencyPhone { get; set; }

        [StringLength(StringLengths.FAX)]
        public virtual string Fax { get; set; }

        [StringLength(StringLengths.FD1_CONTACT), Display(Name = "FD Emergency Contact")]
        public virtual string FD1Contact { get; set; }

        [StringLength(StringLengths.FD1_FAX), Display(Name = "FD Emergency Fax")]
        public virtual string FD1Fax { get; set; }

        [StringLength(StringLengths.FD1_PHONE), Display(Name = "FD Emergency Phone")]
        public virtual string FD1Phone { get; set; }

        [StringLength(StringLengths.PHONE)]
        public virtual string Phone { get; set; }

        [StringLength(StringLengths.STATE), Obsolete("You should be using the mapped State property instead")]
        public virtual string StateAbbreviation { get; set; }

        [StringLength(StringLengths.TOWN)]
        public virtual string ShortName { get; set; }

        [StringLength(StringLengths.TOWN_NAME)]
        public virtual string FullName { get; set; }

        [StringLength(StringLengths.ZIP)]
        public virtual string Zip { get; set; }

        [StringLength(StringLengths.LINK)]
        public virtual string Link { get; set; }

        [StringLength(StringLengths.LAT)]
        public virtual string Lat { get; set; }

        [StringLength(StringLengths.LON)]
        public virtual string Lon { get; set; }

        [StringLength(StringLengths.OPERATING_SECTION)]
        public virtual string OperatingSection { get; set; }

        public virtual bool? SharedOpCenter { get; set; }

        [StringLength(StringLengths.DBA)]
        public virtual string DBA { get; set; }

        [StringLength(StringLengths.CRITICAL_MAIN_BREAK_NOTES)]
        public virtual string CriticalMainBreakNotes { get; set; }

        #endregion

        #region References

        public virtual AbbreviationType AbbreviationType { get; set; }

        //        public virtual ZipCode ZipCode { get; set; }
        //        public virtual Division Division { get; set; }
        public virtual County County { get; set; }
        public virtual State State { get; set; }
        public virtual Gradient Gradient { get; set; }

        public virtual IList<OperatingCenter> OperatingCenters
        {
            get { return OperatingCentersTowns.Select(x => x.OperatingCenter).ToList(); }
        }

        public virtual IList<Gradient> Gradients { get; set; }
        public virtual IList<AsBuiltImage> AsBuiltImages { get; set; }
        public virtual IList<TownDocument> TownDocuments { get; set; }
        public virtual IList<TownNote> TownNotes { get; set; }
        public virtual IList<Facility> Facilities { get; set; }
        public virtual IList<PublicWaterSupply> PublicWaterSupplies { get; set; }
        public virtual IList<TownSection> TownSections { get; set; }
        public virtual IList<OperatingCenterTown> OperatingCentersTowns { get; set; }
        public virtual IList<WasteWaterSystem> WasteWaterSystems { get; set; }
        public virtual IList<Street> Streets { get; set; }
        public virtual IList<FunctionalLocation> FunctionalLocations { get; set; }

        #endregion

        #region Logical Properties

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return TownDocuments.Map(td => (IDocumentLink)td); }
        }

        public virtual IList<Document> Documents
        {
            get { return TownDocuments.Map(td => td.Document); }
        }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return TownNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return TownNotes.Map(n => n.Note); }
        }

        public virtual string TableName => nameof(Town) + "s";

        public virtual IList<FireDistrictTown> TownFireDistricts { get; set; }

        public virtual IList<TownContact> TownContacts { get; set; }

        public virtual string NameWithCounty => $"{ShortName}, {County.Name} COUNTY";

        #endregion

        #endregion

        #region Constructors

        public Town()
        {
            Gradients = new List<Gradient>();
            OperatingCentersTowns = new List<OperatingCenterTown>();
            AsBuiltImages = new List<AsBuiltImage>();
            TownDocuments = new List<TownDocument>();
            TownFireDistricts = new List<FireDistrictTown>();
            TownContacts = new List<TownContact>();
            PublicWaterSupplies = new List<PublicWaterSupply>();
            WasteWaterSystems = new List<WasteWaterSystem>();
            TownSections = new List<TownSection>();
            Streets = new List<Street>();
            FunctionalLocations = new List<FunctionalLocation>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public override string ToString()
        {
            return ShortName;
        }

        #endregion
    }
}
