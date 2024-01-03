using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.UI;
using MMSINC.Common;
using MMSINC.Interface;
using Microsoft.Practices.Web.UI.WebControls;
using StructureMap;
using StructureMap.Pipeline;

namespace MMSINC.View
{
    public abstract class DetailView<TEntity> : MvpUserControl, IDetailView<TEntity>
        where TEntity : class
    {
        #region Private Members

        protected IDetailPresenter<TEntity> _presenter;

        #endregion

        #region Properties

        public abstract IDetailControl DetailControl { get; }

        public virtual IDetailPresenter<TEntity> Presenter
        {
            get { return _presenter; }
        }

        public virtual DetailViewMode CurrentMode
        {
            get { return DetailControl.CurrentMvpMode; }
        }

        public IEnumerable<IChildResourceView> ChildResourceViews
        {
            get { return GetAllChildResourceViews(Controls); }
        }

        public virtual object CurrentDataKey
        {
            get { return null; }
        }

        #endregion

        #region Private Methods

        private static List<IChildResourceView> GetAllChildResourceViews(ControlCollection ctrls)
        {
            return GetAllChildResourceViews(ctrls,
                new List<IChildResourceView>());
        }

        private static List<IChildResourceView> GetAllChildResourceViews(ControlCollection ctrls,
            List<IChildResourceView> ret)
        {
            ret = ret ?? new List<IChildResourceView>();
            foreach (Control ctrl in ctrls)
            {
                var castCtrl = ctrl as IChildResourceView;
                if (castCtrl != null)
                    ret.Add(castCtrl);
                GetAllChildResourceViews(ctrl.Controls, ret);
            }

            return ret;
        }

        #endregion

        #region Events

        public event EventHandler EditClicked,
                                  DiscardChangesClicked,
                                  UserControlLoaded,
                                  EntityLoaded;

        public event EventHandler<EntityEventArgs<TEntity>> Inserting,
                                                            Updating,
                                                            DeleteClicked;

        #endregion

        #region Event Handlers

        /// <summary>
        /// TODO: transform this to use a protected virtual PageInitPresenter Method
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected virtual void Page_Load(object sender, EventArgs e)
        {
            _presenter =
                DependencyResolver.Current.GetService<IContainer>().GetInstance<IDetailPresenter<TEntity>>(
                    new ExplicitArguments(new Dictionary<string, object> {{"view", this}}));

            if (!IsMvpPostBack)
                Presenter.OnViewInitialized();

            // Fire that the UserControl was loaded.
            // This is needed if other views are listening for the event.
            // In this case the ListView is listening to this event 
            OnUserControlLoaded();

            //notify the presenter that the view was loaded.
            //the presenter can then add the event handlers to the view
            Presenter.OnViewLoaded();
        }

        protected virtual void btnEdit_Click(object sender, EventArgs e)
        {
            OnEditClicked(e);
        }

        protected virtual void btnCancel_Click(object sender, EventArgs e)
        {
            OnDiscardChangesClicked(e);
        }

        protected virtual void ods_Updated(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            OnUpdating(new EntityEventArgs<TEntity>((TEntity)e.Instance));
        }

        protected virtual void ods_Inserted(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            OnInserting(new EntityEventArgs<TEntity>((TEntity)e.Instance));
        }

        protected virtual void ods_Deleted(object sender, ObjectContainerDataSourceStatusEventArgs e)
        {
            OnDeleteClicked(new EntityEventArgs<TEntity>((TEntity)e.Instance));
        }

        protected virtual void btnSave_Click(object sender, EventArgs e)
        {
            switch (DetailControl.CurrentMvpMode)
            {
                case DetailViewMode.Edit:
                    DetailControl.UpdateItem(true);
                    break;
                case DetailViewMode.Insert:
                    DetailControl.InsertItem(true);
                    break;
                default:
                    throw new InvalidOperationException(
                        "Cannot save an item when in ReadOnly mode.");
            }
        }

        protected virtual void btnDelete_Click(object sender, EventArgs e)
        {
            DetailControl.DeleteItem();
        }

        #endregion

        #region Event Passthroughs

        // Views do not contain code to handle user interface events; instead, views notify 
        // their presenters through events or direct method calls to the presenter. 
        // In this case, the view notify their presenters through events

        // Fires the EditClicked event
        protected virtual void OnEditClicked(EventArgs e)
        {
            if (EditClicked != null)
                EditClicked(this, e);
        }

        protected virtual void OnInserting(EntityEventArgs<TEntity> e)
        {
            if (Inserting != null)
                Inserting(this, e);
        }

        protected virtual void OnUpdating(EntityEventArgs<TEntity> e)
        {
            if (Updating != null)
                Updating(this, e);
        }

        protected virtual void OnDiscardChangesClicked(EventArgs e)
        {
            if (DiscardChangesClicked != null)
                DiscardChangesClicked(this, e);
        }

        protected virtual void OnUserControlLoaded()
        {
            if (UserControlLoaded != null)
                UserControlLoaded(this, EventArgs.Empty);
        }

        protected virtual void OnDeleteClicked(EntityEventArgs<TEntity> e)
        {
            if (DeleteClicked != null)
                DeleteClicked(this, e);
        }

        protected virtual void OnEntityLoaded()
        {
            if (EntityLoaded != null)
                EntityLoaded(this, EventArgs.Empty);
        }

        #endregion

        #region Abstract Methods

        public abstract void SetViewControlsVisible(bool visible);
        public abstract void SetViewMode(DetailViewMode newMode);
        public abstract void ShowEntity(TEntity instance);

        #endregion
    }
}
