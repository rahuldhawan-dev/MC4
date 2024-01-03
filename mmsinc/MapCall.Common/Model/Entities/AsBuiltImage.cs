using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    public interface IAssetImage
    {
        int Id { get; }
        string FileName { get; }
        string Directory { get; set; }
        Town Town { get; }
        byte[] ImageData { get; }
    }

    [Serializable]
    public class AsBuiltImage
        : IEntityWithCreationTimeTracking,
            IEntityWithUpdateTracking<User>,
            IAssetImage,
            IThingWithCoordinate,
            IThingWithShadow,
            IThingWithTown,
            IThingWithTownSection,
            IThingWithOperatingCenter
    {
        #region Constants

        public const string STREET_FORMAT = "{0} {1} {2}";

        public struct StringLengths
        {
            public const int STREET_PREFIX = 50,
                             STREET = 50,
                             STREET_SUFFIX = 50,
                             XSTREET_PREFIX = 50,
                             CROSS_STREET = 50,
                             XSTREET_SUFFIX = 50,
                             PROJECT_NAME = 80,
                             MAP_PAGE = 50,
                             TASK_NUMBER = 50,
                             FILE_LIST = 255,
                             SPECIAL = 30,
                             FLD = 50,
                             COMMENTS = 150,
                             APARTMENT_NUMBER = 50;
        }

        public struct Display
        {
            public const string TASK_NUMBER = "WBS #",
                                APARTMENT_NUMBER = "Apartment Addtl";
        }

        #endregion

        #region Properties

        #region Table Columns

        // AsBuiltImageID
        public virtual int Id { get; set; }

        public virtual MapIcon Icon => Coordinate.Icon;

        [StringLength(StringLengths.COMMENTS)]
        public virtual string Comments { get; set; }

        [View("Date Added", FormatStyle.Date)]
        public virtual DateTime CreatedAt { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? DateInstalled { get; set; }

        [View("Physical In Service Date", FormatStyle.Date)]
        public virtual DateTime? PhysicalInService { get; set; }

        [StringLength(StringLengths.STREET_PREFIX)]
        public virtual string StreetPrefix { get; set; }

        [StringLength(StringLengths.STREET)]
        public virtual string Street { get; set; }

        [StringLength(StringLengths.STREET_SUFFIX)]
        public virtual string StreetSuffix { get; set; }

        [StringLength(StringLengths.XSTREET_PREFIX)]
        public virtual string CrossStreetPrefix { get; set; }

        [StringLength(StringLengths.CROSS_STREET)]
        public virtual string CrossStreet { get; set; }

        [StringLength(StringLengths.XSTREET_SUFFIX)]
        public virtual string CrossStreetSuffix { get; set; }

        [StringLength(StringLengths.PROJECT_NAME)]
        public virtual string ProjectName { get; set; }

        [StringLength(StringLengths.MAP_PAGE)]
        public virtual string MapPage { get; set; }

        [View(Display.TASK_NUMBER), StringLength(StringLengths.TASK_NUMBER)]
        public virtual string TaskNumber { get; set; }

        [StringLength(StringLengths.FILE_LIST)]
        public virtual string FileName { get; set; }

        [StringLength(StringLengths.FLD)]
        public virtual string Directory { get; set; }

        public virtual DateTime? CoordinatesModifiedOn { get; set; }

        public virtual Town Town { get; set; }
        public virtual TownSection TownSection { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual bool OfficeReviewRequired { get; set; }

        [View(Display.APARTMENT_NUMBER)]
        public virtual string ApartmentNumber { get; set; }

        #endregion

        #region IAssetImage

        /// <summary>
        /// This property's used by AsBuiltImageRepository for saving the
        /// image to disk. 
        /// </summary>
        public virtual byte[] ImageData { get; set; }

        /// <summary>
        /// The username of the user that just changed the coordinate. This
        /// is not stored in a column and is only here for sending out
        /// notification emails.
        /// </summary>
        public virtual string CoordinatesModifiedBy { get; set; }

        #endregion

        #region Logical Properties

        public virtual string FullStreet => string.Format(STREET_FORMAT, StreetPrefix, Street, StreetSuffix);

        public virtual string FullCrossStreet =>
            String.Format(STREET_FORMAT, CrossStreetPrefix, CrossStreet, CrossStreetSuffix);

        public virtual bool HasCoordinate => Coordinate != null;

        public virtual User UpdatedBy { get; set; }
        public virtual DateTime UpdatedAt { get; set; }

        #endregion

        #endregion
    }
}
