using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using MMSINCTestImplementation.Model;

namespace MMSINCTestImplementation.Presenters
{
    public class AddressDetailPresenter : DetailPresenter<Address>
    {
        #region Constructors

        public AddressDetailPresenter(IDetailView<Address> view)
            : base(view) { }

        #endregion
    }

    public class AddressResourcePresenter : ResourcePresenter<Address>
    {
        #region Constructors

        public AddressResourcePresenter(IResourceView view, IRepository<Address> repository)
            : base(view, repository) { }

        #endregion
    }

    public class AddresssListPresenter : ListPresenter<Address>
    {
        #region Constructors

        public AddresssListPresenter(IListView<Address> view)
            : base(view) { }

        #endregion
    }
}
