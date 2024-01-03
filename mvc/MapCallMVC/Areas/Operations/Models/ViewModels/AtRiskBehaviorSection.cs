using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.ObjectMapping;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Operations.Models.ViewModels
{
    public class AtRiskBehaviorSectionViewModel : ViewModel<AtRiskBehaviorSection>
    {
        #region Properties

        [Required]
        [StringLength(AtRiskBehaviorSection.StringLengths.DESCRIPTION)]
        public string Description { get; set; }

        [Required]
        public int? SectionNumber { get; set; }

        #endregion

        #region Constructors

        public AtRiskBehaviorSectionViewModel(IContainer container) : base(container) {}
        
        #endregion
    }

    public class AddAtRiskBehaviorSubSectionToSection : ViewModel<AtRiskBehaviorSection>
    {
        #region Constructors

        public AddAtRiskBehaviorSubSectionToSection(IContainer container) : base(container) { }

        #endregion

        #region Properties

        [DoesNotAutoMap("Mapped manually")]
        [Required, EntityMustExist(typeof(AtRiskBehaviorSection))]
        public int? Section { get; set; }

        [DoesNotAutoMap("Mapped manually. Also it would overwrite the AtRiskBehaviorSection.Description property if someone called base.MapToEntity.")]
        [Required, StringLength(AtRiskBehaviorSubSection.StringLengths.DESCRIPTION)]
        public string Description { get; set; }

        [DoesNotAutoMap("Mapped manually")]
        [Required]
        public decimal? SubSectionNumber { get; set; }

        #endregion

        #region Public Methods

        public override void Map(AtRiskBehaviorSection entity)
        {
            base.Map(entity);
            Section = entity.Id;
        }

        public override AtRiskBehaviorSection MapToEntity(AtRiskBehaviorSection entity)
        {
            // Don't call base.MapToEntity, it will cause the entity's Description to
            // be overwritten with whatever the sub section's Description is.
            var sub = new AtRiskBehaviorSubSection();
            sub.Description = Description;
            sub.Section = entity;
            sub.SubSectionNumber = SubSectionNumber.Value;
            entity.SubSections.Add(sub);

            return entity;
        }

        #endregion
    }

    public class RemoveAtRiskBehaviorSubSectionFromSection : ViewModel<AtRiskBehaviorSection>
    {
        [DoesNotAutoMap("Mapped manually")]
        [Required, EntityMustExist(typeof(AtRiskBehaviorSubSection))]
        public int? AtRiskBehaviorSubSectionId { get; set; }

        public RemoveAtRiskBehaviorSubSectionFromSection(IContainer container) : base(container) { }

        public override AtRiskBehaviorSection MapToEntity(AtRiskBehaviorSection entity)
        {
            // NOTE: Don't call base method.
            var fp = entity.SubSections.SingleOrDefault(x => x.Id == AtRiskBehaviorSubSectionId.Value);
            entity.SubSections.Remove(fp);
            return entity;
        }
    }
}