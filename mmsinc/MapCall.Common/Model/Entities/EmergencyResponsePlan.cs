using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.Excel;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class EmergencyResponsePlan : IEntity, IValidatableObject, IThingWithDocuments, IThingWithNotes
    {
        #region Constants

        public struct StringLengths
        {
            public const int
                TITLE = 255;
        }

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual State State { get; set; }
        public virtual OperatingCenter OperatingCenter { get; set; }
        public virtual Facility Facility { get; set; }

        [View("Plan Category")]
        public virtual EmergencyPlanCategory EmergencyPlanCategory { get; set; }

        public virtual string Title { get; set; }
        public virtual string Description { get; set; }
        public virtual ReviewFrequency ReviewFrequency { get; set; }
        public virtual IList<PlanReview> Reviews { get; set; }

        #region Notes/Documents

        [DoesNotExport]
        public virtual string TableName => nameof(EmergencyResponsePlan) + "s";

        public virtual IList<EmergencyResponsePlanNote> EmergencyResponsePlanNotes { get; set; }
        public virtual IList<EmergencyResponsePlanDocument> EmergencyResponsePlanDocuments { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return EmergencyResponsePlanNotes.Map(x => (INoteLink)x); }
        }

        public virtual IList<Note> Notes
        {
            get { return EmergencyResponsePlanNotes.Map(x => x.Note); }
        }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return EmergencyResponsePlanDocuments.Map(x => (IDocumentLink)x); }
        }

        public virtual IList<Document> Documents
        {
            get { return EmergencyResponsePlanDocuments.Map(d => d.Document); }
        }

        #endregion

        #endregion

        #region Constructors

        public EmergencyResponsePlan()
        {
            Reviews = new List<PlanReview>();
        }

        #endregion

        #region Exposed Methods

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion

        #endregion
    }

    [Serializable]
    public class EmergencyPlanCategory : EntityLookup { }

    [Serializable]
    public class ReviewFrequency : EntityLookup { }
}
