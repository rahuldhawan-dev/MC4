using System.Web.UI.WebControls;
using LINQTo271.Views.Abstract;
using MMSINC.Controls;
using WorkOrders.Model;

namespace LINQTo271.Views.Materials
{
    public partial class MaterialListView : WorkOrdersListView<Material>
    {
        #region Control Declarations

        protected IListControl gvMaterials;

        #endregion

        #region Properties

        public override IListControl ListControl
        {
            get { return gvMaterials; }
        }
        
        #endregion

        #region Private Methods

        private void SetGridViewIndex(int index)
        {
            ((GridView)gvMaterials).EditIndex = index;
        }

        #endregion

        #region Event Handlers

        protected void ListControl_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            // TODO: if this needs to change at all, it needs to be moved up into the concerning presenter and fully tested first
            var material = MaterialRepository.GetEntity(e.Keys["MaterialID"]);
            material.PartNumber = e.NewValues["PartNumber"].ToString();
            material.IsActive = (bool)e.NewValues["IsActive"];
            material.DoNotOrder = (bool)e.NewValues["DoNotOrder"];
           
            MaterialRepository.Update(material);

            SetGridViewIndex(-1);
        }

        protected void ListControl_RowEditing(object sender, GridViewEditEventArgs e)
        {
            SetGridViewIndex(e.NewEditIndex);
        }

        protected void ListControl_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            SetGridViewIndex(-1);
        }

        #endregion

        #region Exposed Methods

        public override void SetViewControlsVisible(bool visible)
        {
            //noop
        }

        #endregion
    }
}
