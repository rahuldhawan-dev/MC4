using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CovidIssue : IEntity, IThingWithNotes, IThingWithDocuments, IThingWithEmployee
    {
        #region Constants

        public struct StringLengths
        {
            #region Constants

            public const int SUPERVISORS_CELL = 22,
                             LOCAL_ERBP = 50,
                             LOCAL_ERBP_CELL = 22,
                             PERSONAL_EMAIL_ADDRESS = 255;

            #endregion
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual string State => Employee.OperatingCenter?.State?.Abbreviation;

        public virtual string OperatingCenter => Employee.OperatingCenter?.Description;

        public virtual Employee Employee { get; set; }
        public virtual string SupervisorsCell { get; set; }
        public virtual string LocalEmployeeRelationsBusinessPartner { get; set; }
        public virtual string LocalEmployeeRelationsBusinessPartnerCell { get; set; }

        /// <summary>
        /// The Employee's PersonnelArea at the time this record was created.
        /// </summary>
        public virtual PersonnelArea PersonnelArea { get; set; }

        public virtual ReleaseReason ReleaseReason { get; set; }
        public virtual CovidRequestType RequestType { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime SubmissionDate { get; set; }

        public virtual string QuestionFromEmail { get; set; }
        public virtual CovidSubmissionStatus SubmissionStatus { get; set; }
        public virtual string OutcomeDescription { get; set; }
        public virtual CovidOutcomeCategory OutcomeCategory { get; set; }
        public virtual CovidQuarantineStatus QuarantineStatus { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? StartDate { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? ReleaseDate { get; set; }

        [View(FormatStyle.Date)]
        public virtual DateTime? EstimatedReleaseDate { get; set; }

        public virtual string QuarantineReason { get; set; }
        public virtual CovidAnswerType WorkExposure { get; set; }
        public virtual CovidAnswerType AvoidableCloseContact { get; set; }
        public virtual CovidAnswerType FaceCoveringWorn { get; set; }
        public virtual string PersonalEmailAddress { get; set; }
        public virtual bool? HealthDepartmentNotification { get; set; }

        //ExportOnlyFields
        public virtual string PositionGroup => Employee?.PositionGroup?.Group;
        public virtual string PositionGroupDescription => Employee?.PositionGroup?.PositionDescription;
        public virtual string BusinessUnit => Employee?.PositionGroup?.BusinessUnit;
        public virtual string BusinesUnitDescription => Employee?.PositionGroup?.BusinessUnitDescription;

        /// <summary>
        /// The Employee's HumanResourcesManager at the time this record was created.
        /// </summary>
        public virtual Employee HumanResourcesManager { get; set; }

        #region Logical Properties

        // Doing these as logical makes exporting much simpler, as in no separate index code is needed
        public virtual string EmailAddress => Employee.EmailAddress;
        public virtual string EmployeeCell => Employee.PhoneCellular;
        public virtual Employee ReportsTo => Employee.ReportsTo;

        public virtual int? TotalDays
        {
            get
            {
                if (StartDate.HasValue && ReleaseDate.HasValue)
                {
                    return (ReleaseDate.Value - StartDate.Value).Days;
                }

                return null;
            }
        }

        #endregion

        #endregion

        #region Notes/Docs

        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        public virtual IList<Document<CovidIssue>> Documents { get; set; }

        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        public virtual IList<Note<CovidIssue>> Notes { get; set; }

        [DoesNotExport]
        public virtual string TableName => nameof(CovidIssue) + "s";

        #endregion

        #region Constructors

        public CovidIssue()
        {
            Documents = new List<Document<CovidIssue>>();
            Notes = new List<Note<CovidIssue>>();
        }

        #endregion
    }
}
