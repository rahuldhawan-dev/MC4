using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINCTestImplementation.Model;

namespace MMSINCTestImplementation.Presenters
{
    public class ContactDetailPresenter : DetailPresenter<Contact>
    {
        #region Constructors

        public ContactDetailPresenter(IDetailView<Contact> view)
            : base(view) { }

        #endregion
    }

    public class ContactResourcePresenter : ResourcePresenter<Contact>
    {
        #region Constructors

        public ContactResourcePresenter(IResourceView view, IRepository<Contact> repository)
            : base(view, repository) { }

        #endregion
    }

    public class ContactsListPresenter : ListPresenter<Contact>
    {
        #region Constructors

        public ContactsListPresenter(IListView<Contact> view)
            : base(view) { }

        #endregion
    }
}
