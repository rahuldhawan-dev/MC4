using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class HelpTopic : IEntity, IValidatableObject, IThingWithDocuments, IThingWithNotes
    {
        #region Properties

        public virtual int Id { get; set; }

        public virtual IList<HelpTopicDocument> HelpTopicDocuments { get; set; }
        public virtual IList<HelpTopicNote> HelpTopicNotes { get; set; }

        public virtual HelpCategory Category { get; set; }
        public virtual HelpTopicSubjectMatter SubjectMatter { get; set; }

        [Required]
        [StringLength(255)]
        public virtual string Title { get; set; }

        public virtual string Description { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return HelpTopicNotes.Map(x => (INoteLink)x); }
        }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return HelpTopicDocuments.Map(x => (IDocumentLink)x); }
        }

        public virtual string TableName => nameof(HelpTopic) + "s";

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
