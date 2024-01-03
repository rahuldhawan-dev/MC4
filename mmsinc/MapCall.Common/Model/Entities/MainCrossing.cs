using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;
using StructureMap;
using StructureMap.Attributes;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class MainCrossing : IEntity, IThingWithDocuments, IThingWithNotes, IAsset, IRetirableWorkOrderAsset
    {
        #region Constants

        public struct StringLengths
        {
            public const int EMERGENCY_PHONE_NUMBER = 15,
                             RAILWAY_CROSSING_ID = 10;
        }

        #endregion

        #region Private Members

        private MainCrossingDisplayItem _display;

        [NonSerialized] private IIconSetRepository _iconSetRepository;

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [DoesNotExport]
        public virtual string TableName => nameof(MainCrossing) + "s";

        public virtual bool? IsCompanyOwned { get; set; }

        [DisplayName("Length of GIS Main Segment")]
        public virtual decimal? LengthOfSegment { get; set; }

        public virtual bool? IsCriticalAsset { get; set; }

        [DisplayName("Maximum Daily Flow(MGD)")]
        public virtual decimal? MaximumDailyFlow { get; set; }

        [DisplayName("Main in Casing?")]
        public virtual MainInCasingStatus MainInCasing { get; set; }

        [Multiline]
        public virtual string Comments { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Town Town { get; set; }
        public virtual Street Street { get; set; }
        public virtual Street ClosestCrossStreet { get; set; }

        [DisplayName("Location")]
        public virtual Coordinate Coordinate { get; set; }

        [DoesNotExport]
        public virtual MapIcon Icon
        {
            get
            {
                if (Coordinate == null)
                {
                    return null;
                }

                // TODO: I feel really gross about using a repository method inside a model.
                //       Also I just feel gross about this in general. - Ross 10/29/2014

                // This used to use IconSet.MainCrossings but bug-2339 asked that main crossings get mapped
                // with assets. -Ross 2/22/2017
                var iconSet = _iconSetRepository.Find(IconSets.Assets);
                if (RequiresInspection)
                {
                    return iconSet.Icons.Single(x => x.FileName == "MapIcons/maincrossing-red.png");
                }
                else
                {
                    return iconSet.Icons.Single(x => x.FileName == "MapIcons/maincrossing-green.png");
                }
            }
        }

        public virtual BodyOfWater BodyOfWater { get; set; }
        public virtual MainCrossingConsequenceOfFailureType ConsequenceOfFailure { get; set; }
        public virtual CrossingCategory CrossingCategory { get; set; }
        public virtual PressureZone PressureZone { get; set; }

        [DisplayName("Number of Customers")]
        public virtual CustomerRange CustomerRange { get; set; }

        public virtual PublicWaterSupply PWSID { get; set; }
        public virtual PipeMaterial MainMaterial { get; set; }
        public virtual PipeDiameter MainDiameter { get; set; }
        public virtual SupportStructure SupportStructure { get; set; }
        public virtual ConstructionType ConstructionType { get; set; }
        public virtual CrossingType CrossingType { get; set; }
        public virtual int InspectionFrequency { get; set; }
        public virtual RecurringFrequencyUnit InspectionFrequencyUnit { get; set; }
        public virtual MainCrossingStatus MainCrossingStatus { get; set; }
        public virtual DateTime? DateRetired { get; set; }
        public virtual MainCrossingInspectionType InspectionType { get; set; }
        public virtual IList<MainCrossingInspection> Inspections { get; set; }
        public virtual RailwayOwnerType RailwayOwnerType { get; set; }
        public virtual IList<WorkOrder> WorkOrders { get; set; }
        public virtual IList<Valve> Valves { get; set; }
        public virtual IList<MainCrossingImpactToType> ImpactTo { get; set; }

        [DisplayName("Railway Crossing ID")]
        public virtual string RailwayCrossingId { get; set; }

        public virtual string EmergencyPhoneNumber { get; set; }

        [BoolFormat("Yes", "No", "Operations update required")]
        public virtual bool? IsolationValves { get; set; }

        public virtual AssetCategory AssetCategory { get; set; }
        public virtual PressureSurgePotentialType PressureSurgePotentialType { get; set; }
        public virtual TypicalOperatingPressureRange TypicalOperatingPressureRange { get; set; }

        #region Logical Properties

        #region Notes/Documents

        public virtual IList<MainCrossingNote> MainCrossingNotes { get; set; }
        public virtual IList<MainCrossingDocument> MainCrossingDocuments { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return MainCrossingNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return MainCrossingNotes.Map(n => n.Note); }
        }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return MainCrossingDocuments.Map(ld => (IDocumentLink)ld); }
        }

        public virtual IList<Document> Documents
        {
            get { return MainCrossingDocuments.Map(d => d.Document); }
        }

        #endregion

        public virtual decimal? Latitude => (Coordinate != null) ? Coordinate.Latitude : (decimal?)null;

        public virtual decimal? Longitude => (Coordinate != null) ? Coordinate.Longitude : (decimal?)null;

        [DisplayName("Inspection Frequency")]
        public virtual string InspectionFrequencyDisplay
        {
            get
            {
                if (InspectionFrequencyUnit == null)
                {
                    return null;
                }

                return string.Format("{0} {1}", InspectionFrequency, InspectionFrequencyUnit.Description);
            }
        }

        // Done with a formula
        public virtual bool RequiresInspection { get; protected set; }
        public virtual bool HasWorkOrder { get; set; }

        // Done with a formula
        [DisplayFormat(DataFormatString = CommonStringFormats.DATE)]
        public virtual DateTime? LastInspectedOn { get; protected set; }

        [DoesNotExport]
        public virtual string Identifier => $"CR{Id}";

        public virtual string Description => (_display ?? (_display = new MainCrossingDisplayItem {
            Id = Id,
            OperatingCenter = OperatingCenter?.OperatingCenterCode,
            Town = Town?.ShortName,
            BodyOfWater = BodyOfWater?.Name,
            Street = Street?.FullStName,
            ClosestCrossStreet = ClosestCrossStreet?.FullStName,
            MainCrossingStatus = MainCrossingStatus?.Description
        })).Display;

        #endregion

        #region Injected Properties

        [SetterProperty]
        public virtual IIconSetRepository IconSetRepository
        {
            set => _iconSetRepository = value;
        }

        #endregion

        #endregion

        #region Constructors

        public MainCrossing()
        {
            MainCrossingDocuments = new List<MainCrossingDocument>();
            MainCrossingNotes = new List<MainCrossingNote>();
            Inspections = new List<MainCrossingInspection>();
            WorkOrders = new List<WorkOrder>();
            Valves = new List<Valve>();
            ImpactTo = new List<MainCrossingImpactToType>();
        }

        #endregion

        #region Exposed Methods

        public virtual MainCrossingAssetCoordinate ToAssetCoordinate()
        {
            // This smells and is a hack just to get icon logic in one spot. 
            var hac = new MainCrossingAssetCoordinate(_iconSetRepository) {
                IsActive = true,
                IsPublic = true,
                Id = Id,
                RequiresInspection = RequiresInspection,
                OutOfService = false
            };

            if (Coordinate != null)
            {
                hac.Latitude = Coordinate.Latitude;
                hac.Longitude = Coordinate.Longitude;
            }

            return hac;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        //TODO: This is duplicated in WorkOrders.Model.MainCrossing
        // TODO: This is also duplicated in MainCrossingDropDownItem
        public virtual string Display => (_display ?? (_display = new MainCrossingDisplayItem {
            BodyOfWater = BodyOfWater?.Name,
            ClosestCrossStreet = ClosestCrossStreet?.FullStName,
            Id = Id,
            OperatingCenter = OperatingCenter?.OperatingCenterCode,
            Street = Street?.FullStName,
            Town = Town?.ShortName
        })).Display;

        public override string ToString()
        {
            return Display;
        }

        #endregion
    }

    [Serializable]
    public class MainInCasingStatus : EntityLookup { }

    [Serializable]
    public class MainCrossingStatus : EntityLookup { }

    [Serializable]
    public class MainCrossingDisplayItem : DisplayItem<MainCrossing>
    {
        #region Fields

        private string _display;

        #endregion

        #region Properties

        [SelectDynamic("OperatingCenterCode")]
        public string OperatingCenter { get; set; }

        [SelectDynamic("ShortName")]
        public string Town { get; set; }

        [SelectDynamic("Name")]
        public string BodyOfWater { get; set; }

        [SelectDynamic("FullStName")]
        public string Street { get; set; }

        [SelectDynamic("FullStName")]
        public string ClosestCrossStreet { get; set; }

        [SelectDynamic("Description")]
        public string MainCrossingStatus { get; set; }

        public override string Display
        {
            get
            {
                // cache the display so we're not creating a lot of extra useless strings for null checks
                // done elsewhere in MVC. 

                if (_display == null)
                {
                    var display = new StringBuilder($"CR{Id} - {OperatingCenter} - {Town}");

                    void AddString(StringBuilder sb, string s)
                    {
                        sb.Append(s != null ? " - " + s : string.Empty);
                    }

                    AddString(display, BodyOfWater);
                    AddString(display, Street);
                    AddString(display, ClosestCrossStreet);
                    AddString(display, MainCrossingStatus);
                    return display.ToString();
                }

                return _display;
            }
        }

        #endregion
    }

    [Serializable]
    public class MainCrossingConsequenceOfFailureType : EntityLookup { }

    [Serializable]
    public class MainCrossingImpactToType : EntityLookup
    {
        public virtual IList<MainCrossing> MainCrossings { get; set; }

        public MainCrossingImpactToType()
        {
            MainCrossings = new List<MainCrossing>();
        }
    }
}
