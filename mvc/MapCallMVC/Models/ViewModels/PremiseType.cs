using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using MapCall.Common.Model.Entities;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class PremiseTypeViewModel : ViewModel<PremiseType>
    {
        #region Properties

        [Required]
        public virtual string Description { get; set; }
        [Required]
        public virtual string Abbreviation { get; set; }

        #endregion

        #region Constructor

        public PremiseTypeViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class CreatePremiseType : PremiseTypeViewModel
    {
        public CreatePremiseType(IContainer container) : base(container) {}
    }

    public class EditPremiseType : PremiseTypeViewModel
    {
        public EditPremiseType(IContainer container) : base(container) {}
    }

    // What is this class for? There aren't any properties
    public class SearchPremiseType : SearchSet<PremiseType>
    {
       
    }
}