using MapCall.Common.Model.Entities.Users;
using MMSINC.Data.NHibernate;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Models.ViewModels.Users
{
    public class EditUser : BaseUserViewModel
    {
        #region Properties

        [DoesNotAutoMap("Only for display purposes.")]
        public string UserName => _container.GetInstance<IRepository<User>>().Find(Id)?.UserName;

        #endregion

        #region Constructor

        public EditUser(IContainer container) : base(container) { }

        #endregion

        #region Public Methods

        public override void SetDefaults()
        {
            base.SetDefaults();

            // They usually want the new user to have access, so this saves them a click.
            HasAccess = true;
        }

        public override void Map(User entity)
        {
            base.Map(entity);
            // Not all users are employees!
            EmployeeNumber = entity.Employee?.EmployeeId;
        }

        #endregion
    }
}