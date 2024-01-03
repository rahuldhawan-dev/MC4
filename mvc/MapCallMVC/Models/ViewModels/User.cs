using MapCall.Common.Model.Entities.Users;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class EditUser : ViewModel<User>
    {
        #region Constructors

        public EditUser(IContainer container) : base(container) { }

        #endregion
    }
}