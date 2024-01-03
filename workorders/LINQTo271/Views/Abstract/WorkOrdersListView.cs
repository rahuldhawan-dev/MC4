using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using MMSINC.View;
using WorkOrders;

namespace LINQTo271.Views.Abstract
{
    public abstract class WorkOrdersListView<TEntity> : ListView<TEntity>
        where TEntity : class
    {
        #region Properties

        public Label lblCount = new Label();

        #if DEBUG

        public override bool IsMvpPostBack
        {
            get
            {
                return (_isMvpPostBack == null) ?
                    IsPostBack : _isMvpPostBack.Value;
            }
        }

        #endif

        protected override void GetAndDisplayCount(IEnumerable<TEntity> data)
        {
            lblCount.Text = string.Format("{0} Result(s)", data.Count());
            base.GetAndDisplayCount(data);
        }

        #endregion
    }
}
