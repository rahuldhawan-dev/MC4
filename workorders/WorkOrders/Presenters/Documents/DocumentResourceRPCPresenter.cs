using MMSINC.Data.Linq;
using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.Documents
{
    public class DocumentResourceRPCPresenter : ResourceRPCPresenter<Document>
    {
        #region Constructors

        public DocumentResourceRPCPresenter(IResourceRPCView<Document> view, IRepository<Document> repository)
            : base(view, repository)
        {
        }

        #endregion

        #region Private Methods

        protected override void ProcessCommandAndArgument()
        {
            var document = Repository.Get(RPCView.Argument);
            RPCView.ShowEntityOnDetailView(document);
        }

        #endregion
    }
}
