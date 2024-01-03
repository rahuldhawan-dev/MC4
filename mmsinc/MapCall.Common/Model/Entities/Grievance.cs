using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Migrations;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Grievance : IEntity, IValidatableObject, IThingWithDocuments, IThingWithNotes, IThingWithEmployees,
        IThingWithOperatingCenter
    {
        #region Constants

        public const string DATA_TYPE_NAME = "Grievance";

        public const string DATA_TYPE_AND_TABLE_NAME =
            DATA_TYPE_NAME + AddFieldsToTrainingRecordsAndSuchForBug1738.DELIMITER + GrievanceMap.TABLE_NAME;
        
        public struct DisplayNames
        {
            public const string GRIEVANCE_ID = "Grievance Id",
                                GRIEVANCE_NUM = "Grievance #",
                                OPERATING_CENTER = "Operating Center",
                                CONTRACT_ID = "Contract ID",
                                GRIEVANCE_CATEGORIZATION = "Grievance SubCategory",
                                GRIEVANCE_STATUS = "GrievanceStatus",
                                GRIEVANCE_STEP_1 = "Step 1 Grievance Description",
                                NOTES = "Notes",
                                DOCUMENTS = "Documents";
        }

        #endregion

        #region Properties

        [View(Grievance.DisplayNames.GRIEVANCE_ID)]
        public virtual int Id { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime DateReceived { get; set; }

        public virtual decimal? EstimatedImpactValue { get; set; }

        [View(Grievance.DisplayNames.GRIEVANCE_NUM)]
        [StringLength(GrievanceMap.NUMBER_LENGTH)]
        public virtual string Number { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? IncidentDate { get; set; }

        [View(Grievance.DisplayNames.GRIEVANCE_STEP_1)]
        public virtual string Description { get; set; }

        public virtual string DescriptionOfOutcome { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? UnionDueDate { get; set; }

        [DisplayFormat(DataFormatString = CommonStringFormats.DATE, ApplyFormatInEditMode = true)]
        public virtual DateTime? ManagementDueDate { get; set; }

        public virtual UnionContract Contract { get; set; }

        [View(Grievance.DisplayNames.GRIEVANCE_CATEGORIZATION)]
        public virtual GrievanceCategorization Categorization { get; set; }
        public virtual GrievanceStatus Status { get; set; }
        [View(Grievance.DisplayNames.OPERATING_CENTER)]
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual IList<Note<Grievance>> Notes { get; set; }
        public virtual IList<Document<Grievance>> Documents { get; set; }
        public virtual IList<GrievanceEmployee> GrievanceEmployees { get; set; }

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

        [DoesNotExport]
        public virtual string TableName => GrievanceMap.TABLE_NAME;

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<IEmployeeLink> LinkedEmployees
        {
            get { return GrievanceEmployees.Map(x => (IEmployeeLink)x); }
        }

        [View(Grievance.DisplayNames.DOCUMENTS)]
        public virtual int DocumentCount => Documents.Count;

        [View(Grievance.DisplayNames.NOTES)]
        public virtual int NoteCount => Notes.Count;

        public virtual GrievanceCategory GrievanceCategory { get; set; }

        public virtual Employee LaborRelationsBusinessPartner { get; set; }

        #endregion

        #region Constructors

        public Grievance()
        {
            Notes = new List<Note<Grievance>>();
            Documents = new List<Document<Grievance>>();
            GrievanceEmployees = new List<GrievanceEmployee>();
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
