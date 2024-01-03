using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MMSINC.Authentication;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.HealthAndSafety.Models.ViewModels
{
    public class TailgateTalkTopicViewModel : ViewModel<TailgateTalkTopic>
    {
        #region Properties

        [Required]
        public virtual bool? IsActive { get; set; }
        [Required]
        public virtual string Topic { get; set; }
        [Required]
        [DropDown, EntityMustExist(typeof(TailgateTopicCategory)), EntityMap]
        public virtual int? Category { get; set; }
        [Required]
        [DropDown, EntityMustExist(typeof(TailgateTopicMonth)), EntityMap]
        public virtual int? Month { get; set; }
        [DropDown, Required]
        public virtual int? TopicLevel { get; set; }
        [Required]
        public virtual DateTime? TargetDeliveryDate { get; set; }
        [View("ORM Reference Number"), RequiredWhen(nameof(TopicLevel), (int)TopicLevels.State, ErrorMessage = "ORM Reference Number is required when Topic Level is 'State'.")]
        public virtual string OrmReferenceNumber { get; set; }

        #endregion

        #region Constructors

        public TailgateTalkTopicViewModel(IContainer container) : base(container) {}

        #endregion

        #region Exposed Methods

        public override TailgateTalkTopic MapToEntity(TailgateTalkTopic entity)
        {
            if (TopicLevel.HasValue)
            {
                entity.TopicLevel = (TopicLevels)TopicLevel;
            }

            return base.MapToEntity(entity);
        }

        public override void Map(TailgateTalkTopic entity)
        {
            base.Map(entity);

            if (entity.TopicLevel.HasValue)
            {
                TopicLevel = (int)entity.TopicLevel;
            }
        }

        #endregion
    }

    public class CreateTailgateTalkTopic : TailgateTalkTopicViewModel
    {
        #region Properties

        [EntityMap]
        public virtual int? CreatedBy { get; set; }

        #endregion

        #region Constructors

        public CreateTailgateTalkTopic(IContainer container) : base(container) {}

        #endregion
    }

    public class EditTailgateTalkTopic : TailgateTalkTopicViewModel
    {
        #region Constructors

        public EditTailgateTalkTopic(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchTailgateTalkTopic : SearchSet<TailgateTalkTopic>
    {
        #region Properties

        public virtual bool? IsActive { get; set; }
        public virtual string Topic { get; set; }
        [DropDown, EntityMustExist(typeof(TailgateTopicCategory))]
        public virtual int? Category { get; set; }
        [DropDown, EntityMustExist(typeof(TailgateTopicMonth))]
        public virtual int? Month { get; set; }
        public virtual DateRange TargetDeliveryDate { get; set; }
        [View("ORM Reference Number")]
        public virtual string OrmReferenceNumber { get; set; }

        #endregion
    }
}