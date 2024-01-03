using System;
using System.ComponentModel;
using MapCall.Common.Model.Migrations._2019;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TapImage : IEntityWithCreationTimeTracking, IAssetImage
    {
        #region Consts

        public struct StringLengths
        {
            public const int CROSS_STREET = 50,
                             SERVICE_NUMBER = 50,
                             STREET_PREFIX = 50,
                             STREET_NUMBER = 50,
                             STREET = 50,
                             STREET_SUFFIX = 50,
                             APARTMENT_NUMBER = 50,
                             TOWN_SECTION = 50,
                             PREMISE_NUMBER = 50,
                             LOT = 50,
                             BLOCK = 50,
                             SERVICE_SIZE = 50,
                             SERVICE_MATERIAL = 255,
                             SERVICE_TYPE = 50,
                             LENGTH_OF_SERVICE = 50,
                             MAIN_SIZE = 50,
                             FILE_NAME = 255,
                             DIRECTORY = MC1400IncreaseFldLengthForTapImages.FLD;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        public virtual string FileName { get; set; }
        public virtual string Directory { get; set; }
        public virtual Town Town { get; set; }
        [View(FormatStyle.Date)]
        public virtual DateTime CreatedAt { get; set; }
        public virtual string CrossStreet { get; set; }
        public virtual string ServiceNumber { get; set; }
        public virtual string StreetPrefix { get; set; }
        public virtual string StreetNumber { get; set; }
        public virtual string Street { get; set; }
        public virtual string StreetSuffix { get; set; }

        [View("Apartment Addtl")]
        public virtual string ApartmentNumber { get; set; }

        public virtual string TownSection { get; set; }
        public virtual string PremiseNumber { get; set; }
        public virtual string Lot { get; set; }
        public virtual string Block { get; set; }
        public virtual string ServiceType { get; set; }
        public virtual string LengthOfService { get; set; }
        public virtual string MainSize { get; set; }
        public virtual bool OfficeReviewRequired { get; set; }

        public virtual bool ServiceMaterialIsNull { get; set; }

        [View("Service Asset")]
        public virtual Service Service { get; set; }

        public virtual ServiceMaterial CustomerSideMaterial { get; set; }
        public virtual ServiceSize CustomerSideSize { get; set; }

        [View("Is Default Image for Tap")]
        public virtual bool IsDefaultImageForService { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }
        [View(Service.DisplayNames.PREVIOUS_SERVICE_SIZE)]
        public virtual ServiceSize PreviousServiceSize { get; set; }
        [View(Service.DisplayNames.PREVIOUS_SERVICE_MATERIAL)]
        public virtual ServiceMaterial PreviousServiceMaterial { get; set; }
        [View(Service.DisplayNames.SERVICE_SIZE)]
        public virtual ServiceSize ServiceSize { get; set; }
        [View(Service.DisplayNames.SERVICE_MATERIAL)]
        public virtual ServiceMaterial ServiceMaterial { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? DateCompleted { get; set; }

        /// <summary>
        /// This is for use during uploads only.
        /// </summary>
        public virtual byte[] ImageData { get; set; }

        #region Logical Properties

        public virtual string FullStreetName => string.Format("{0} {1} {2}", StreetPrefix, Street, StreetSuffix).Trim();

        public virtual bool HasAsset { get; set; }
        public virtual bool HasValidPremiseNumber { get; set; }

        #endregion

        #endregion
    }

    #region Report Item Classes

    public class TapImageLinkReportItem : IEntity
    {
        #region Constants

        public const string
            TITLE = "Tap Image",
            URL_FORMAT_STRING = "https://mapcall.amwater.com/modules/mvc/FieldOperations/TapImage/Show/{0}.pdf";

        #endregion

        #region Properties

        [DisplayName("Premise Number")]
        public string PremiseNumber { get; set; }

        public string Title => TITLE;
        public string Address => String.Format(URL_FORMAT_STRING, Id);

        [DoesNotExport]
        public OperatingCenter OperatingCenter { get; set; }

        [DoesNotExport]
        public int Id { get; set; }

        #endregion
    }

    #endregion
}
