using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class SkillSetViewModel : ViewModel<SkillSet>
    {
        #region Constructors
        
        public SkillSetViewModel(IContainer container) : base(container) {}

        #endregion

        #region Properties

        [Required, StringLength(SkillSet.StringLengths.NAME)] 
        public virtual string Name { get; set; }

        [Required, StringLength(SkillSet.StringLengths.ABBREVIATION)]
        public virtual string Abbreviation { get; set; }

        [Required]
        public virtual bool? IsActive { get; set; }

        [StringLength(SkillSet.StringLengths.DESCRIPTION)]
        public virtual string Description { get; set; }

        #endregion
    }
}