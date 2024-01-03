using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.ViewModels;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class HelpTopicViewModel : ViewModel<HelpTopic>
    {
        #region Properties

        [Required, DropDown, EntityMap]
        public int? Category { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required, DropDown, EntityMap, EntityMustExist(typeof(HelpTopicSubjectMatter))]
        public int? SubjectMatter { get; set; }
        #endregion

        #region Constructors

        public HelpTopicViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateHelpTopic : HelpTopicViewModel
    {
        #region Constructors

        public CreateHelpTopic(IContainer container) : base(container) {}

        #endregion
    }

    public class EditHelpTopic : HelpTopicViewModel
    {
        #region Constructors

        public EditHelpTopic(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchHelpTopic : SearchSet<HelpTopic>, ISearchHelpTopicWithDocument
    {
        #region Properties

        public int? Id { get; set; }
        [DropDown]
        public int? Category { get; set; }
        [DisplayName("Topic Title")]
        public string Title { get; set; }
        [DisplayName("Topic Description")]
        public string Description { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(HelpTopicSubjectMatter))]
        public int? SubjectMatter { get; set; }
        
        public DateRange DocumentUpdated { get; set; }

        public SearchString DocumentTitle { get; set; }

        [MultiSelect]
        public int[] DocumentType { get; set; }

        [DropDown, EntityMap, EntityMustExist(typeof(DocumentStatus))]
        public int? Active { get; set; } = DocumentStatus.Indices.ACTIVE; // set default

        public DateRange DocumentNextReviewDate { get; set; }
        #endregion
    }
}