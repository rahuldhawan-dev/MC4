using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using NHibernate.Spatial.Type;
using StructureMap.Attributes;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class BelowGroundHazard : IEntity, IThingWithNotes, IThingWithDocuments, IThingWithCoordinate
    {
        #region Consts

        public struct StringLengths
        {
            public const int HAZARD_DESCRIPTION_MAX = 255,
                             HAZARD_DESCRIPTION_MIN = 5;
        }

        public struct Indices
        {
            public const int WORKORDER = 1;
        }

        public struct Ranges
        {
            public const int AREA_LOWER = 1,
                             AREA_UPPER = 500,
                             DEPTH_LOWER = 0,
                             DEPTH_UPPER = 144;
        }

        public struct Strings
        {
            public const string STREET = "Street",
                                TOWN = "Town",
                                HAZARD_AREA = "Hazard Area (Feet)",
                                DEPTH_OF_HAZARD = "Depth Of Hazard (Inches)",
                                STATUS = "Status",
                                PROXIMITY_TO_ASSET = "Proximity to American Water Asset (Feet)",
                                APPROACH_RECOMMENDED = "Approach Recommended",
                                COORDINATES = "Coordinates";
        }

        #endregion

        #region Fields

        [NonSerialized] private IIconSetRepository _iconSetRepository;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual MapIcon Icon => Coordinate != null ? Coordinate.Icon : null;
        public virtual WorkOrder WorkOrder { get; set; }
        [View(Strings.COORDINATES)]
        public virtual Coordinate Coordinate { get; set; }
        public virtual State State => Town?.State;
        public virtual Town Town { get; set; }
        public virtual TownSection TownSection { get; set; }
        public virtual int? StreetNumber { get; set; }
        public virtual Street Street { get; set; }
        public virtual Street CrossStreet { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        [View(Strings.HAZARD_AREA)]
        public virtual int HazardArea { get; set; }
        public virtual HazardType HazardType { get; set; }
        [View(Strings.DEPTH_OF_HAZARD)]
        public virtual int? DepthOfHazard { get; set; }
        [View(Strings.STATUS)]
        public virtual AssetStatus AssetStatus { get; set; }
        [Multiline]
        public virtual string HazardDescription { get; set; }
        [View(Strings.PROXIMITY_TO_ASSET)]
        public virtual int? ProximityToAmWaterAsset { get; set; }
        [View(Strings.APPROACH_RECOMMENDED)]
        public virtual HazardApproachRecommendedType HazardApproachRecommendedType { get; set; }
        public virtual IList<BelowGroundHazardDocument> BelowGroundHazardDocuments { get; set; }
        public virtual IList<BelowGroundHazardNote> BelowGroundHazardNotes { get; set; }
        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return BelowGroundHazardDocuments.Cast<IDocumentLink>().ToList(); }
        }
        public virtual IList<INoteLink> LinkedNotes
        {
            get { return BelowGroundHazardNotes.Cast<INoteLink>().ToList(); }
        }
        [DoesNotExport]
        public virtual string TableName => BelowGroundHazardMap.TABLE_NAME;

        [SetterProperty]
        public virtual IIconSetRepository IconSetRepository
        {
            set => _iconSetRepository = value;
        }

        #endregion

        #region Constructor

        public BelowGroundHazard()
        {
            BelowGroundHazardNotes = new List<BelowGroundHazardNote>();
            BelowGroundHazardDocuments = new List<BelowGroundHazardDocument>();
        }

        #endregion

        #region Exposed Methods

        public virtual BelowGroundHazardAssetCoordinate ToAssetCoordinate()
        { 
            var ac = new BelowGroundHazardAssetCoordinate(_iconSetRepository) {
                IsActive = AssetStatus.ACTIVE_STATUSES.Contains(AssetStatus.Id),
                IsPublic = true,
                Id = Id,
                Latitude = Coordinate?.Latitude,
                Longitude = Coordinate?.Longitude
            };

            return ac;
        }

        #endregion
    }
}