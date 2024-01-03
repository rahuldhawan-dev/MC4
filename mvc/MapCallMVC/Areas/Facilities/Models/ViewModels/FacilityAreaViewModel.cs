using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Areas.Facilities.Models.ViewModels
{
    public class FacilityAreaViewModel : ViewModel<FacilityArea>
    {
        #region Constructor

        public FacilityAreaViewModel(IContainer container) : base(container) {}

        #endregion

        #region Properties

        [Required, StringLength(50)]
        public string Description { get; set; }

        #endregion
    }

    public class CreateFacilityArea : FacilityAreaViewModel
    {
        public CreateFacilityArea(IContainer container) : base(container) {}
    }

    public class EditFacilityArea : FacilityAreaViewModel
    {
        public EditFacilityArea(IContainer container) : base(container) {}
    }

    public class SearchFacilityArea : SearchSet<FacilityArea> { }
}