using System;
using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities;
using MMSINC.Validation;
using StructureMap;

namespace Contractors.Models.ViewModels
{
    public abstract class StreetOpeningPermitViewModelBase : ViewModel<StreetOpeningPermit>
    {
        #region Properties

        [Required, StringLength(25)]
        public string StreetOpeningPermitNumber { get; set; }

        [Required, View(FormatStyle.Date)]
        public DateTime? DateRequested { get; set; }

        [View(FormatStyle.Date)]
        public DateTime? ExpirationDate { get; set; }

        [View(FormatStyle.Date)]
        public DateTime? DateIssued { get; set; }

        [Multiline]
        public string Notes { get; set; }

        #endregion

        #region Constructor

        protected StreetOpeningPermitViewModelBase(IContainer container) : base(container)
        {
        }

        #endregion
    }

    public class StreetOpeningPermitNew : StreetOpeningPermitViewModelBase
    {
        #region Properties

        [Required, EntityMap, EntityMustExist(typeof(WorkOrder))]
        public int WorkOrder { get; set; }

        #endregion

        #region Constructor

        public StreetOpeningPermitNew(IContainer container) : base(container)
        {
        }

        #endregion
    }

    public class StreetOpeningPermitEdit : StreetOpeningPermitViewModelBase
    {
        #region Constructor

        public StreetOpeningPermitEdit(IContainer container) : base(container) { }

        #endregion
    }
}