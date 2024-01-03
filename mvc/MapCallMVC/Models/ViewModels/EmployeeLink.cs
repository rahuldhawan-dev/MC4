using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using MMSINC.Utilities.ObjectMapping;
using IContainer = StructureMap.IContainer;

namespace MapCallMVC.Models.ViewModels
{
    public class NewEmployeeLink : ViewModel<EmployeeLink>
    {
        [Required, Secured]
        public virtual int LinkedId { get; set; }

        [CheckBoxList]
        [DisplayName("Employees"), DoesNotAutoMap("This viewmodel is entirely manually mapped in EmployeeLinkController.")]
        public virtual int[] EmployeeIds { get; set; }
        [Required, Secured, DoesNotAutoMap("This viewmodel is entirely manually mapped in EmployeeLinkController.")]
        public virtual int DataTypeId { get; set; }
        [DoesNotAutoMap("This viewmodel is entirely manually mapped in EmployeeLinkController.")]
        public virtual string TableName { get; set; }
        [DoesNotAutoMap("This viewmodel is entirely manually mapped in EmployeeLinkController.")]
        public virtual string DataTypeName { get; set; }

        [DropDown, DoesNotAutoMap] // Needed for filtering the employee ids checkbox list.
        public int? OperatingCenter { get; set; }

        public NewEmployeeLink(IContainer container) : base(container) {}

    }

    public class DeleteEmployeeLink : ViewModel<IEmployeeLink>
    {
        #region Properties

        [Required, Secured, AutoMap]
        public override int Id { get; set; }

        #endregion

        #region Constructors

        public DeleteEmployeeLink(IContainer container) : base(container) {}

        #endregion
    }
}