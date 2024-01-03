using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Migrations;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data;
using MMSINC.Data.ChangeTracking;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class BappTeamIdea
        : IEntityWithCreationTimeTracking, IValidatableObject, IThingWithNotes, IThingWithDocuments
    {
        #region Properties

        #region Table Properties

        public virtual int Id { get; set; }

        [Required]
        public virtual BappTeam BappTeam { get; set; }

        [Required]
        public virtual Employee Contact { get; set; }

        [Required]
        public virtual SafetyImplementationCategory SafetyImplementationCategory { get; set; }

        public virtual string Description { get; set; }

        [Required]
        public virtual DateTime CreatedAt { get; set; }

        #endregion

        #region Logical Properties

        #region Documents

        public virtual IList<BappTeamIdeaDocument> BappTeamIdeaDocuments { get; set; }

        public virtual IList<IDocumentLink> LinkedDocuments
        {
            get { return BappTeamIdeaDocuments.Map(epd => (IDocumentLink)epd); }
        }

        public virtual IList<Document> Documents
        {
            get { return BappTeamIdeaDocuments.Map(epd => epd.Document); }
        }

        #endregion

        #region Notes

        public virtual IList<BappTeamIdeaNote> BappTeamIdeaNotes { get; set; }

        public virtual IList<INoteLink> LinkedNotes
        {
            get { return BappTeamIdeaNotes.Map(n => (INoteLink)n); }
        }

        public virtual IList<Note> Notes
        {
            get { return BappTeamIdeaNotes.Map(n => n.Note); }
        }

        #endregion

        public virtual string TableName => CreateBAPPTeamIdeasTableForBug1999.TableNames.BAPP_TEAM_IDEAS;

        #endregion

        #endregion

        #region Exposed Methods

        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            return Enumerable.Empty<ValidationResult>();
        }

        #endregion

        #region Constructors

        public BappTeamIdea()
        {
            BappTeamIdeaDocuments = new List<BappTeamIdeaDocument>();
            BappTeamIdeaNotes = new List<BappTeamIdeaNote>();
        }

        #endregion
    }
}
