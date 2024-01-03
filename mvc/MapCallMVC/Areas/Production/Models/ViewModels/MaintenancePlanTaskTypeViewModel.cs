using MMSINC.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MapCall.Common.Model.Entities;
using StructureMap;

namespace MapCallMVC.Areas.Production.Models.ViewModels
{
    public class MaintenancePlanTaskTypeViewModel : ViewModel<MaintenancePlanTaskType>
    {
        #region Properties

        [Required, StringLength(MaintenancePlanTaskType.StringLengths.DESCRIPTION)]
        public string Description { get; set; }

        [Required, StringLength(MaintenancePlanTaskType.StringLengths.ABBREVIATION)]
        public string Abbreviation { get; set; }

        [Required]
        public bool? IsActive { get; set; }
        
        #endregion

        #region Constructors

        public MaintenancePlanTaskTypeViewModel(IContainer container) : base(container) { }

        #endregion
    }
}