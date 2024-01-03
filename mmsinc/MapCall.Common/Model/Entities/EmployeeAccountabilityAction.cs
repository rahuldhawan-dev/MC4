using MMSINC.Data;
using MMSINC.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Utilities;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EmployeeAccountabilityAction : IEntity, IThingWithNotes, IThingWithDocuments, IThingWithOperatingCenter
    {
        #region Consts

        public struct StringLengths
        {
            public const int ACCOUNTABILITY_ACTION_TAKEN_DESCRIPTION = 255;
        }

        public struct DataTypeTableName
        {
            public const string DATA_TYPE_AND_TABLE_NAME = DATA_TYPE + "|" + TABLE_NAME,
                                DATA_TYPE = "Employee Accountability Action",
                                TABLE_NAME = "EmployeeAccountabilityActions";
        }
        
        #endregion

        #region Properties

        public virtual int Id { get; set; }

        #region Original
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Employee Employee { get; set; }
        public virtual Employee DisciplineAdministeredBy { get; set; }
        public virtual AccountabilityActionTakenType AccountabilityActionTakenType { get; set; }
        public virtual string AccountabilityActionTakenDescription { get; set; }
        [View(FormatStyle.Date)]
        public virtual DateTime DateAdministered { get; set; }
        [View(FormatStyle.Date)]
        public virtual DateTime? StartDate { get; set; }
        [View(FormatStyle.Date)]
        public virtual DateTime? EndDate { get; set; }
        public virtual int? NumberOfWorkDays { get; set; }
        [DoesNotExport]
        public virtual Incident Incident { get; set; }

        [DoesNotExport]
        public virtual Grievance Grievance { get; set; }

        public virtual string DescriptionOfIncident
        {
            get { return Incident != null ? $"{Incident.Id} - {Incident.OperatingCenter}  " : null; }
        }

        public virtual string DescriptionOfGrievance
        {
            get { return Grievance != null ? $"{Grievance.Id} - {Grievance.OperatingCenter}  " : null; }
        }

        #endregion

        #region Modified
        public virtual bool? HasModifiedDiscipline { get; set; }
        public virtual Employee ModifiedDisciplineAdministeredBy { get; set; }
        public virtual AccountabilityActionTakenType ModifiedAccountabilityActionTakenType { get; set; }
        public virtual string ModifiedAccountabilityActionTakenDescription { get; set; }
        // This is NOT our change tracking "UpdatedAt" field
        [View(FormatStyle.Date)]
        public virtual DateTime? DateModified { get; set; }
        [View(FormatStyle.Date)]
        public virtual DateTime? ModifiedStartDate { get; set; }
        [View(FormatStyle.Date)]
        public virtual DateTime? ModifiedEndDate { get; set; }
        public virtual int? ModifiedNumberOfWorkDays { get; set; }
        public virtual bool? BackPayRequired { get; set; }

        #endregion

        #region Documents

        [DoesNotExport]
        public virtual string TableName => EmployeeAccountabilityAction.DataTypeTableName.TABLE_NAME;

        [DoesNotExport]
        public virtual string DataTypeName
        {
            get { return null; }
        }
        [DoesNotExport]
        public virtual bool AllowNotifications
        {
            get { return true; }
        }
        [DoesNotExport]
        public virtual string RoleModule
        {
            get { return RoleModules.HumanResourcesAccountabilityAction.ToString(); }
        }
        [DoesNotExport]
        public virtual string NotificationPurpose
        {
            get { return "Employee Accountability Action"; }
        }
        [DoesNotExport]
        public virtual string EntityType
        {
            get { return typeof(EmployeeAccountabilityAction).AssemblyQualifiedName; }
        }
        [DoesNotExport]
        public virtual int OperatingCenterId
        {
            get { return (OperatingCenter != null) ? OperatingCenter.Id : 0; }
        }
        public virtual bool AllowMoreEmployeesFor(string dataTypeName)
        {
            return true;
        }

        #region Documents

        public virtual IList<Document<EmployeeAccountabilityAction>> Documents { get; set; }
        public virtual IList<IDocumentLink> LinkedDocuments => Documents.Cast<IDocumentLink>().ToList();

        #endregion

        #endregion

        #region Notes

        public virtual IList<Note<EmployeeAccountabilityAction>> Notes { get; set; }
        public virtual IList<INoteLink> LinkedNotes => Notes.Cast<INoteLink>().ToList();

        #endregion

        #region EmployeeLink

        public virtual IList<EmployeeAccountabilityActionEmployee> EmployeeAccountabilityActionEmployee { get; set; }

        #endregion

        #endregion

        #region Constructor

        public EmployeeAccountabilityAction()
        {
            Documents = new List<Document<EmployeeAccountabilityAction>>();
            Notes = new List<Note<EmployeeAccountabilityAction>>();
            EmployeeAccountabilityActionEmployee = new List<EmployeeAccountabilityActionEmployee>();
        }

        #endregion
    }
}