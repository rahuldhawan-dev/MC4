using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using MMSINC.ClassExtensions.IListExtensions;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Migrations;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class CutoffSawQuestionnaire
        : IEntityWithCreationTimeTracking, IValidatableObject, IThingWithNotes, IThingWithDocuments
    {
        #region Constants

        public struct StringLengths
        {
            public const int WORK_ORDER_SAP = AddCutoffSawQuestions.StringLengths.WORK_ORDER_SAP,
                             CREATED_BY = AddCutoffSawQuestions.StringLengths.CREATED_BY;
        }

        public const string ADDRESS_FORMAT = "{0} {1}";

        #endregion

        #region Properties

        #region Table Column Properties

        public virtual int Id { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return CutoffSawQuestionnaireDocuments.Map(td => (IDocumentLink)td); }
        }

        public virtual IList<Document> Documents
        {
            get { return CutoffSawQuestionnaireDocuments.Map(td => td.Document); }
        }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return CutoffSawQuestionnaireNotes.Map(n => (INoteLink)n); }
        }

        public virtual string TableName => nameof(CutoffSawQuestionnaire) + "s";
        public virtual WorkOrder WorkOrder { get; set; }

        [StringLength(StringLengths.WORK_ORDER_SAP)]
        public virtual string WorkOrderSAP { get; set; }

        [Required]
        public virtual Employee LeadPerson { get; set; }

        [Required]
        public virtual Employee SawOperator { get; set; }

        [Required]
        public virtual DateTime OperatedOn { get; set; }

        public virtual string Comments { get; set; }

        [StringLength(StringLengths.CREATED_BY)]
        public virtual string CreatedBy { get; set; }

        [Required]
        public virtual DateTime CreatedAt { get; set; }

        public virtual IList<CutoffSawQuestionnaireDocument> CutoffSawQuestionnaireDocuments { get; set; }
        public virtual IList<CutoffSawQuestionnaireNote> CutoffSawQuestionnaireNotes { get; set; }

        #endregion

        #region Associations

        public virtual PipeMaterial PipeMaterial { get; set; }
        public virtual PipeDiameter PipeDiameter { get; set; }

        public virtual IList<CutoffSawQuestion> CutoffSawQuestions { get; set; }

        #endregion

        #region Logical

        public virtual string WorkOrderLocation
        {
            get
            {
                if (WorkOrder == null)
                    return String.Empty;
                return String.Format("{0} {1}", WorkOrder.StreetAddress, WorkOrder.TownAddress);
            }
        }

        #endregion

        #endregion

        #region Constructors

        public CutoffSawQuestionnaire()
        {
            CutoffSawQuestions = new List<CutoffSawQuestion>();
            CutoffSawQuestionnaireDocuments = new List<CutoffSawQuestionnaireDocument>();
            CutoffSawQuestionnaireNotes = new List<CutoffSawQuestionnaireNote>();
        }

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion
    }
}
