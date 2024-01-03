using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Migrations;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class TailgateTalk : IEntity, IThingWithEmployees, IThingWithDocuments, IThingWithNotes, IValidatableObject,
        IThingWithVideos
    {
        #region Constants

        public const string DATA_TYPE_NAME = "Tailgate Talks";

        public const string DATA_TYPE_AND_TABLE_NAME =
            DATA_TYPE_NAME + AddFieldsToTrainingRecordsAndSuchForBug1738.DELIMITER + TailgateTalkMap.TABLE_NAME;

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [DisplayFormat(DataFormatString = "{0:d}"), Required]
        public virtual DateTime? HeldOn { get; set; }

        [Required]
        public virtual decimal TrainingTimeHours { get; set; }

        public virtual TailgateTalkTopic Topic { get; set; }

        [Display(Name = "Presenter"), Required]
        public virtual Employee PresentedBy { get; set; }

        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual TailgateTopicCategory Category { get; set; }

        public virtual IList<TailgateTalkEmployee> TailgateTalkEmployees { get; set; }
        public virtual IList<TailgateTalkDocument> TailgateTalkDocuments { get; set; }
        public virtual IList<TailgateTalkNote> TailgateTalkNotes { get; set; }

        [Display(Name = "ORM Reference Number")]
        public virtual string OrmReferenceNumber => Topic != null ? Topic.OrmReferenceNumber : null;

        [DoesNotExport]
        public virtual bool AllowNotifications => true;

        [DoesNotExport]
        public virtual string RoleModule => RoleModules.OperationsHealthAndSafety.ToString();

        [DoesNotExport]
        public virtual string NotificationPurpose => "Tailgate Talk";

        [DoesNotExport]
        public virtual string EntityType => typeof(TailgateTalk).AssemblyQualifiedName;

        [DoesNotExport]
        public virtual int OperatingCenterId => (OperatingCenter != null) ? OperatingCenter.Id : 0;

        [DoesNotExport]
        public virtual string DataTypeName => null;

        [DoesNotExport]
        public virtual string TableName => TailgateTalkMap.TABLE_NAME;

        public virtual IList<IEmployeeLink> LinkedEmployees
        {
            get { return TailgateTalkEmployees.Map(x => (IEmployeeLink)x); }
        }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return TailgateTalkDocuments.Map(x => (IDocumentLink)x); }
        }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return TailgateTalkNotes.Map(x => (INoteLink)x); }
        }

        public virtual IList<TailgateTalkVideo> Videos { get; set; }

        public virtual IEnumerable<IVideoLink> LinkedVideos => Videos;

        #endregion

        #region Constructors

        public TailgateTalk()
        {
            TailgateTalkEmployees = new List<TailgateTalkEmployee>();
            TailgateTalkDocuments = new List<TailgateTalkDocument>();
            TailgateTalkNotes = new List<TailgateTalkNote>();
            Videos = new List<TailgateTalkVideo>();
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
