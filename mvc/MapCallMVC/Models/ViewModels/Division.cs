using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class DivisionViewModel : EntityLookupViewModel<Division>
    {
        #region Constructors

        public DivisionViewModel(IContainer container) : base(container) { }
        
        #endregion

        #region Properties

        [Required, DropDown, EntityMap]
        public virtual int State { get; set; }

        #endregion
    }

    public class CreateDivision : DivisionViewModel
    {
        public CreateDivision(IContainer container) : base(container) { }
    }

    public class EditDivision : DivisionViewModel
    {
        public EditDivision(IContainer container) : base(container) { }
    }

    public class SearchDivision : SearchSet<Division>
    {
        #region Properties

        public string Description { get; set; }

        #endregion
    }
}