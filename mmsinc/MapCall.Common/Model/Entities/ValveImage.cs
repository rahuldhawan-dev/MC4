using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class ValveImage : IEntityWithCreationTimeTracking, IAssetImage
    {
        #region Consts

        public struct StringLengths
        {
            public const int APARTMENT_NUMBER = 50,
                             CROSS_STREET = 50,
                             CROSS_STREET_PREFIX = 50,
                             CROSS_STREET_SUFFIX = 50,
                             DATE_COMPLETED = 50,
                             DIRECTORY = 50,
                             FILENAME = 400, // Yes that's 400 for some reason
                             LOCATION = 4000,
                             NUMBER_OF_TURNS = 50,
                             STREET = 50,
                             STREET_NUMBER = 50,
                             STREET_PREFIX = 50,
                             STREET_SUFFIX = 50,
                             TOWNSECTION = 50,
                             VALVE_NUMBER = 50,
                             VALVE_SIZE = 50;
        }

        public struct Display
        {
            public const string APARTMENT_NUMBER = "Apartment Addtl";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string CrossStreet { get; set; }
        public virtual string CrossStreetPrefix { get; set; }
        public virtual string CrossStreetSuffix { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}")]
        public virtual DateTime CreatedAt { get; set; }

        public virtual string DateCompleted { get; set; }
        public virtual ValveOpenDirection OpenDirection { get; set; }
        public virtual string Directory { get; set; }
        public virtual string FileName { get; set; }

        [DisplayName("Is Default Image for Valve")]
        public virtual bool IsDefaultImageForValve { get; set; }

        [DataType(System.ComponentModel.DataAnnotations.DataType.MultilineText)]
        public virtual string Location { get; set; }

        public virtual string ValveSize { get; set; }
        public virtual ValveNormalPosition NormalPosition { get; set; }
        public virtual string NumberOfTurns { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual string Street { get; set; }
        public virtual string StreetNumber { get; set; }
        public virtual string StreetPrefix { get; set; }
        public virtual string StreetSuffix { get; set; }
        public virtual Town Town { get; set; }
        public virtual string TownSection { get; set; }
        public virtual Valve Valve { get; set; }
        public virtual string ValveNumber { get; set; }
        public virtual bool OfficeReviewRequired { get; set; }

        [View(Display.APARTMENT_NUMBER)]
        public virtual string ApartmentNumber { get; set; }

        /// <summary>
        /// This is only used when saving an uploaded image to disk.
        /// </summary>
        public virtual byte[] ImageData { get; set; }

        #region Logical Properties

        public virtual string FullStreetName
        {
            get
            {
                const string format = "{0} {1} {2} {3}";
                return string.Format(format, StreetNumber, StreetPrefix, Street, StreetSuffix);
            }
        }

        public virtual string FullCrossStreetName
        {
            get
            {
                const string format = "{0} {1} {2}";
                return string.Format(format, CrossStreetPrefix, CrossStreet, CrossStreetSuffix);
            }
        }

        public virtual bool HasAsset { get; set; }
        public virtual bool HasValidValveNumber { get; set; }

        #endregion

        #endregion
    }

    #region Search Classes

    public class ValveImageLinkReportItem
    {
        #region Constants

        public const string
            TITLE = "Valve Image",
            URL_FORMAT_STRING = "https://mapcall.amwater.com/modules/mvc/FieldOperations/ValveImage/Show/{0}.pdf";

        #endregion

        #region Properties

        [DisplayName("SAP Equipment Number")]
        public int SAPEquipmentId { get; set; }

        public string Title => TITLE;
        public string Address => String.Format(URL_FORMAT_STRING, ValveImageId);

        [DoesNotExport]
        public OperatingCenter OperatingCenter { get; set; }

        [DoesNotExport]
        public Valve Valve { get; set; }

        [DoesNotExport]
        public int ValveImageId { get; set; }

        #endregion
    }

    #endregion
}
