using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.ProjectManagement.Models.ViewModels
{
    public class RecurringProjectMainViewModel : ViewModel<RecurringProjectMain>
    {
        #region Properties

        [Required]
        public string Layer { get; set; }
        [Required]
        public string Guid { get; set; }
        //[Required]
        public decimal? TotalInfoMasterScore { get; set; }
        [Required]
        public decimal? Length { get; set; }

        public DateTime? DateInstalled { get; set; }
        public decimal? Diameter { get; set; }
        public string Material { get; set; }

        #endregion

        #region Constructors

        public RecurringProjectMainViewModel(IContainer container) : base(container) { }

        #endregion
    }

    public class EditRecurringProjectMain : RecurringProjectMainViewModel
    {
        #region Properties
        
        [EntityMap, EntityMustExist(typeof(RecurringProject))]
        [Required]
        public int RecurringProject { get; set; }
        
        #endregion

        #region Constructors

        public EditRecurringProjectMain(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateRecurringProjectMain : RecurringProjectMainViewModel
    {
        #region Constructors

        public CreateRecurringProjectMain(IContainer container) : base(container) {}

        #endregion
    }
}