using System.ComponentModel.DataAnnotations;
using MapCall.Common.Metadata;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Validation;
using StructureMap;

namespace MapCallMVC.Areas.Environmental.Models.ViewModels
{
    public class ChemicalVendorViewModel : ViewModel<ChemicalVendor>
    {
        #region Properties

        public virtual string JdeVendorId { get; set; }
        [Required]
        public virtual string Vendor { get; set; }
        [Coordinate, EntityMap, EntityMustExist(typeof(Coordinate))]
        public int? Coordinate { get; set; }
        public virtual string OrderContact { get; set; }
        public virtual string PhoneOffice { get; set; }
        public virtual string PhoneCell { get; set; }
        public virtual string Fax { get; set; }
        public virtual string Email { get; set; }
        public virtual string Address { get; set; }
        public virtual string City { get; set; }
        public virtual string State { get; set; }
        public virtual string Zip { get; set; }

        #endregion

        #region Constructors

        public ChemicalVendorViewModel(IContainer container) : base(container) {}

        #endregion
	}

    public class CreateChemicalVendor : ChemicalVendorViewModel
    {
        #region Constructors

        public CreateChemicalVendor(IContainer container) : base(container) {}

        #endregion
	}

    public class EditChemicalVendor : ChemicalVendorViewModel
    {
        #region Constructors

        public EditChemicalVendor(IContainer container) : base(container) {}

        #endregion
	}

    public class SearchChemicalVendor : SearchSet<ChemicalVendor>
    {
        #region Properties

        public SearchString Vendor { get; set; }

        #endregion
	}
}