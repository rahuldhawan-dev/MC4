using System;
using System.Collections.Generic;
using System.Web.Mvc;
using MMSINC.Interface;
using StructureMap;
using StructureMap.Pipeline;

namespace MMSINC.View
{
    public abstract class ChildResourceView<TEntity> : ResourceView<TEntity>, IChildResourceView<TEntity>
        where TEntity : class
    {
        #region Private Members

        #endregion

        #region Properties

        public virtual IChildResourcePresenter<TEntity> ChildPresenter
        {
            get { return (IChildResourcePresenter<TEntity>)Presenter; }
        }

        public override IResourcePresenter<TEntity> Presenter
        {
            get
            {
                if (_presenter == null)
                {
                    _presenter =
                        DependencyResolver.Current.GetService<IContainer>()
                                          .GetInstance<IChildResourcePresenter<TEntity>>(
                                               new ExplicitArguments(new Dictionary<string, object> {
                                                   {"view", this},
                                                   {"repository", Repository}
                                               }));
                }

                return _presenter;
            }
        }

        IChildResourcePresenter IChildResourceView.Presenter
        {
            get { return (IChildResourcePresenter)Presenter; }
        }

        #endregion

        #region Events

        public event EventHandler ChildEvent;

        #endregion

        #region Event Handlers

        ///// <summary>
        ///// We need to override page load and get the ChildResourcePresenter, to 
        ///// add stuff via OnPageLoaded.
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //protected override void Page_Load(object sender, EventArgs e)
        //{
        //    base.Page_Load(sender, e);

        //    // Not all ChildResourceView Presenters are of the correct type.
        //    if (Presenter is IChildResourcePresenter<TEntity>)
        //    {
        //        if (!IsMvpPostBack)
        //            ChildPresenter.OnViewInitialized();

        //        ChildPresenter.OnViewLoaded();
        //    }
        //}

        #endregion

        #region Event Passthroughs

        public virtual void OnChildEvent(EventArgs e)
        {
            if (ChildEvent != null)
                ChildEvent(this, e);
        }

        #endregion

        #region Exposed Methods

        public virtual void ShowEntityOnDetailView(TEntity entity) { }

        #endregion
    }
}
