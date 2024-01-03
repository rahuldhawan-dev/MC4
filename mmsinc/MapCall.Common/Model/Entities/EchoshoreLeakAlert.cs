using System;
using System.Collections.Generic;
using MMSINC.Data;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    /// <summary>
    ///
    /// This data is populated via the scheduler from a csv file provided by Echoshore.
    /// These are the details for that original file.
    /// PCN - Persistent Correlated Noise
    ///
    /// PCN ID, DatePCNCreated, POIStatusID, POIStatus, Latitude, Longitude, SocketID1, SocketID2, DistanceFrom1, DistanceFrom2, Note, SiteName
    ///
    /// PCN ID: unique numeric ID of the PCN within the site
    /// DateCreated: date of creation of the PCN in ISO8601 format, UTC time
    /// POIStatusID: numeric ID of the status of the PCN. Since we are exporting only PCNs in the state “Field Investigation Recommended”, this will always be 3.
    /// POIStatus: Human-readable status of the PCN. Since we are exporting only PCNs in the state “Field Investigation Recommended”, this will always be the value.
    /// Latitude: The latitude in degrees of the PCN
    /// Longitude: The longitude in degrees of the PCN
    /// SocketID1: The hydrant ID of the first leak detection node used to find the PCN
    /// SocketID2: The hydrant ID of the second leak detection node used to find the PCN
    /// DistanceFrom1: The distance in feet from the first hydrant
    /// DistanceFrom2: The distance in feet from the second hydrant
    /// Note: Human-entered notes attached to the PCN
    /// SiteName: unique string ID for the site, such as hillside, littlefallsnj, etc.
    ///
    /// </summary>
    [Serializable]
    public class EchoshoreLeakAlert : IEntity, IThingWithCoordinate
    {
        #region Constants

        public struct StringLengths
        {
            #region Constants

            public const int SOCKET_ID_TEXT = 20;

            #endregion
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [DoesNotExport]
        public virtual MapIcon Icon => Coordinate.Icon;

        // This is the Echologics PrimaryKey
        public virtual int PersistedCorrelatedNoiseId { get; set; }
        public virtual DateTime DatePCNCreated { get; set; }
        public virtual DateTime? FieldInvestigationRecommendedOn { get; set; }
        public virtual PointOfInterestStatus PointOfInterestStatus { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual Hydrant Hydrant1 { get; set; }
        public virtual Hydrant Hydrant2 { get; set; }
        public virtual string Hydrant1Text { get; set; }
        public virtual string Hydrant2Text { get; set; }
        public virtual decimal DistanceFromHydrant1 { get; set; }
        public virtual decimal DistanceFromHydrant2 { get; set; }
        public virtual string Note { get; set; }
        public virtual EchoshoreSite EchoshoreSite { get; set; }
        public virtual IList<WorkOrder> WorkOrders { get; set; }

        #endregion

        #region Constructors

        public EchoshoreLeakAlert()
        {
            WorkOrders = new List<WorkOrder>();
        }

        #endregion
    }
}
