using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class UnionViewModel : ViewModel<Union>
    {
        #region Properties

        [StringLength(Union.StringLengths.BARGAINING_UNIT), Required]
        public virtual string BargainingUnit { get; set; }

        #endregion

        #region Constructors

        public UnionViewModel(IContainer container) : base(container) {}

        #endregion
    }

    public class EditUnion : UnionViewModel
    {
        #region Constructors

        public EditUnion(IContainer container) : base(container) { }

        #endregion
    }

    public class CreateUnion : UnionViewModel
    {
        #region Constructors

        public CreateUnion(IContainer container) : base(container) {}

        #endregion
    }

    public class SearchUnion : SearchSet<Union>
    {
        #region Properties

        public virtual string BargainingUnit { get; set; }
        
        [View("Bargaining Unit"), DropDown]
        public virtual int? EntityId { get; set; }

        #endregion
    }
}