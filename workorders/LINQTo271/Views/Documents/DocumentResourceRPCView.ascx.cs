using LINQTo271.Views.Abstract;
using MMSINC.Interface;
using WorkOrders.Model;

namespace LINQTo271.Views.Documents
{
    public partial class DocumentResourceRPCView : WorkOrdersResourceRPCView<Document>
    {
        #region Private Members

        protected IDetailView<Document> ddvDocument;

        #endregion

        #region Properties

        public override IListView<Document> ListView
        {
            get { return null; }
        }

        public override IDetailView<Document> DetailView
        {
            get { return ddvDocument; }
        }

        public override ISearchView<Document> SearchView
        {
            get { return null; }
        }

        #endregion

        #region Event Hanlders

        

        #endregion
    }
}