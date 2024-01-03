using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Migrations;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class JobObservation
        : IEntityWithCreationUserTracking<User>,
            IValidatableObject,
            IThingWithCoordinate,
            IThingWithNotes,
            IThingWithDocuments,
            IThingWithEmployees
    {
        #region Constants

        public struct StringLengths
        {
            #region Constants

            public const int LOCATION = 50,
                             ADDRESS = 100,
                             DESCRIPTION = 100,
                             SUGGESTIONS_TO_EMPLOYEES = 255,
                             DEFICIENCIES = 255,
                             COMMENTS = 255;

            #endregion
        }

        public struct Display
        {
            public const string WorkOrder = "T&D WorkOrder";
        }

        public const string DATA_TYPE_NAME = "Job Observations";

        public const string DATA_TYPE_AND_TABLE_NAME =
            DATA_TYPE_NAME + AddFieldsToTrainingRecordsAndSuchForBug1738.DELIMITER + JobObservationMap.TABLE_NAME;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual JobCategory Department { get; set; }
        public virtual Coordinate Coordinate { get; set; }
        public virtual MapIcon Icon => Coordinate != null ? Coordinate.Icon : null;
        public virtual OverallSafetyRating OverallSafetyRating { get; set; }
        public virtual OverallQualityRating OverallQualityRating { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Employee JobObservedBy { get; set; }

        [DisplayName("Created By")]
        public virtual User CreatedBy { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? ObservationDate { get; set; }

        public virtual string Address { get; set; }
        public virtual string TaskObserved { get; set; }

        [View(Display.WorkOrder)]
        public virtual WorkOrder WorkOrder { get; set; }
        public virtual ProductionWorkOrder ProductionWorkOrder { get; set; }

        [View("Why was the task safe or at risk?")]
        public virtual string WhyWasTaskSafeOrAtRisk { get; set; }

        [View("Deficiencies or Additional Comments")]
        public virtual string Deficiencies { get; set; }

        public virtual string RecommendSolutions { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? EqTruckForkliftsHoistsLadders { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? EqFrontEndLoaderOrBackhoe { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? EqOther { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? CsPreEntryChecklistOrEntryPermit { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? CsAtmosphereContinuouslyMonitored { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? CsRetrievalEquipmentTripodHarnessWinch { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? CsVentilationEquipment { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? PpeHardHat { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? PpeReflectiveVest { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? PpeEyeProtection { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? PpeEarProtection { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? PpeFootProtection { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? PpeGloves { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? TcBarricadesConesBarrels { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? TcAdvancedWarningSigns { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? TcLightsArrowBoard { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? TcPoliceFlagman { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? TcWorkZoneInCompliance { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? PsWalkwaysClear { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? PsMaterialStockpile { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? ExMarkoutRequestedForWorkSite { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? ExWorkSiteSafetyCheckListUtilized { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? ExUtilitiesSupportedProtected { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? ExAtmosphereTestingPerformed { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? ExSpoilPile2FeetFromEdgeOfExcavation { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? ExLadderUsedIfGreaterThan4FeetDeep { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? ExShoringNecessaryOver5FeetDeep { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? ExProtectiveSystemInUseOver5Feet { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? ExWaterControlSystemInUse { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? ErChecklistUtilized { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? ErErgonomicFactorsProhibitingGoodBodyMechanics { get; set; }

        [BoolFormat("Yes", "No", "N/A")]
        public virtual bool? ErToolsEquipmentUsedCorrectly { get; set; }

        /// <summary>
        /// A terrible terrible hack property for passing the url
        /// for this record to a notification template.
        /// </summary>
        [DoesNotExport]
        public virtual string RecordUrl { get; set; }

        [DoesNotExport]
        public virtual bool AllowNotifications => false;

        [DoesNotExport]
        public virtual string RoleModule => null;

        [DoesNotExport]
        public virtual string NotificationPurpose => null;

        [DoesNotExport]
        public virtual string EntityType => null;

        [DoesNotExport]
        public virtual int OperatingCenterId => 0;

        [DoesNotExport]
        public virtual string DataTypeName => null;

        public virtual IList<JobObservationEmployee> JobObservationEmployees { get; set; }

        public virtual IList<IEmployeeLink> LinkedEmployees
        {
            get { return JobObservationEmployees.Map(x => (IEmployeeLink)x); }
        }

        public virtual IList<Document<JobObservation>> JobObservationDocuments { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return JobObservationDocuments.Map(epd => (IDocumentLink)epd); }
        }

        public virtual IList<Document> Documents
        {
            get { return JobObservationDocuments.Map(epd => epd.Document); }
        }

        public virtual IList<Note<JobObservation>> JobObservationNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return JobObservationNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return JobObservationNotes.Map(n => n.Note); }
        }

        public virtual string TableName => JobObservationMap.TABLE_NAME;

        #endregion

        #region Constructors

        public JobObservation()
        {
            JobObservationNotes = new List<Note<JobObservation>>();
            JobObservationDocuments = new List<Document<JobObservation>>();
            JobObservationEmployees = new List<JobObservationEmployee>();
        }

        #endregion

        #region Exposed Methods

        public virtual bool AllowMoreEmployeesFor(string dataTypeName)
        {
            return true;
        }

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        public virtual IList<IEmployeeLink> GetLinkedEmployees(string dataTypeName)
        {
            // change when there's more than one type of linked employee
            return LinkedEmployees;
        }

        #endregion
    }
}
