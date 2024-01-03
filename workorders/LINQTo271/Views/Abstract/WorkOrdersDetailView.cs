using System.Web.UI.WebControls;
using MMSINC.Controls;
using MMSINC.Interface;
using MMSINC.View;
using WorkOrders;

namespace LINQTo271.Views.Abstract
{
    public abstract class WorkOrdersDetailView<TEntity> : DetailView<TEntity>
        where TEntity : class
    {
        #region Control Declarations

        protected Button btnEdit, btnSave, btnCancel, btnDelete;

        #endregion

        #region Abstract Properties

        public virtual Button EditButton => btnEdit;
        public virtual Button SaveButton => btnSave;
        public virtual Button CancelButton => btnCancel;
        public virtual Button DeleteButton => btnDelete;

        public abstract IObjectContainerDataSource DataSource { get; }

        public TEntity Entity { get; protected set; }

        #endregion

        #region Exposed Methods

        public override void SetViewMode(DetailViewMode newMode)
        {
            var readOnly = (newMode == DetailViewMode.ReadOnly);
            DetailControl.ChangeMvpMode(newMode);

            if (EditButton != null) 
                EditButton.Visible = readOnly;

            if (SaveButton != null)
                SaveButton.Visible = !readOnly;
            if (CancelButton != null) 
                CancelButton.Visible = !readOnly;
        }

        public override void SetViewControlsVisible(bool visible)
        {
            if (EditButton != null)
                EditButton.Visible = visible;
            if (SaveButton != null)
                SaveButton.Visible = visible;
            if (CancelButton != null)
                CancelButton.Visible = visible;
        }

        public override void ShowEntity(TEntity instance)
        {
            DataSource.DataSource = Entity = instance;
        }

        #endregion
    }
}
