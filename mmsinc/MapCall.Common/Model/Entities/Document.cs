using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;
using MMSINC.Metadata;
using MMSINC.Utilities;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Document : IEntityWithChangeTracking<User>, IValidatableObject
    {
        #region Constants

        public struct StringLengths
        {
            public const int CREATED_BY = 50,
                             FILE_NAME = 255;

            public const int MODIFIED_BY = CREATED_BY;
        }

        public readonly struct DisplayNames
        {
            public const string
                CREATED_ON_EST = "Created On (EST)";
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }

        [StringLength(StringLengths.CREATED_BY),
         Obsolete(
             "Should be using the user object instead. Except Contractors don't have Users, they have ContractorUsers.")]
        public virtual string CreatedByStr { get; set; }

        [View(DisplayNames.CREATED_ON_EST, FormatStyle.DateTimeWithSecondsWithEstTimezone)]
        public virtual DateTime CreatedAt { get; set; }

        [StringLength(StringLengths.MODIFIED_BY),
         Obsolete(
             "Should be using the user object instead. Except Contractors don't have Users, they have ContractorUsers.")]
        public virtual string ModifiedByStr { get; set; }

        public virtual DateTime UpdatedAt { get; set; }

        [StringLength(StringLengths.FILE_NAME)]
        public virtual string FileName { get; set; }

        public virtual DocumentData DocumentData { get; set; }

        public virtual User CreatedBy { get; set; }
        public virtual User UpdatedBy { get; set; }

        public virtual DocumentType DocumentType { get; set; }
        public virtual IList<WorkOrder> WorkOrders { get; set; }

        #endregion

        #region Constructors

        public Document()
        {
            WorkOrders = new List<WorkOrder>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }

    public interface IThingWithDocuments
    {
        int Id { get; }

        IList<IDocumentLink> LinkedDocuments { get; }

        // used so we can get a list of DocumentTypes
        // for creating/linking documents
        string TableName { get; }
    }
}
