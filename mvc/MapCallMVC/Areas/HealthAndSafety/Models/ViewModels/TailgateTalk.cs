using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    public class TailgateTalkViewModel : ViewModel<TailgateTalk>
    {
        #region Properties

        [Required, EntityMap, ComboBox, EntityMustExist(typeof(TailgateTalkTopic))]
        public int? Topic { get; set; }
        [Required]
        public DateTime? HeldOn { get; set; }

        [Required, EntityMap, ComboBox, EntityMustExist(typeof(Employee))]
        public int? PresentedBy { get; set; }
        [Required]
        public decimal? TrainingTimeHours { get; set; }

        #endregion

        #region Constructors

        public TailgateTalkViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreateTailgateTalk : TailgateTalkViewModel
    {
        #region Constructors

        public CreateTailgateTalk(IContainer container) : base(container) {}

        #endregion
    }

    public class EditTailgateTalk : TailgateTalkViewModel
    {
        #region Constructors

        public EditTailgateTalk(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchTailgateTalk : SearchSet<TailgateTalk>
    {
        #region Properties

        public DateRange HeldOn { get; set; }

        [DropDown]
        public int? OperatingCenter { get; set; }

        [DropDown]
        public int? PresentedBy { get; set; }

        [DropDown]
        [SearchAlias("Topic", "TTC", "Category.Id")]
        public int? TailgateTopicCategory { get; set; }

        [DropDown("HealthAndSafety", "TailgateTalkTopic", "ByCategoryId", DependsOn = "TailgateTopicCategory")]
        public int? Topic { get; set; }

        [SearchAlias("Topic", "TTC", "Id"), DisplayName("TopicID")]
        public int? TopicId { get; set; }

        [SearchAlias("Topic", "TTC", "Topic")]
        public string TopicName { get; set; }

        public NumericRange TrainingTimeHours { get; set; }

        [SearchAlias("Topic", "TTC", "OrmReferenceNumber")]
        public string OrmReferenceNumber { get; set; }

        #endregion
    }
}