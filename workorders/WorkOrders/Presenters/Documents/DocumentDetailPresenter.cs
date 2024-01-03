using MMSINC.Interface;
using MMSINC.Presenter;
using WorkOrders.Model;

namespace WorkOrders.Presenters.Documents
{
    public class DocumentDetailPresenter : DetailPresenter<Document>
    {
        #region Constructors

        public DocumentDetailPresenter(IDetailView<Document> view)
            : base(view)
        {
        }

        #endregion
    }
}
