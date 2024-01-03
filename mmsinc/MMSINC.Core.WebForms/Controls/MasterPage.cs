using System;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using MMSINC.Common;
using MMSINC.Interface;

namespace MMSINC.Controls
{
    public class MasterPage : System.Web.UI.MasterPage, IPage
    {
        #region Private Members

        protected IClientScriptManager _iClientScript;
        protected IPage _iPage;
        protected IRequest _iRequest;
        protected IResponse _iResponse;
        protected IUser _iUser;

        #endregion

        #region Properties

        public virtual IClientScriptManager ClientScriptManager
        {
            get
            {
                // this was neutered on purpose.  that way tests of base classes
                // don't have to provide anything here if they don't need to.  if
                // they do, they probably need the script manager anyway, so they'd
                // mock it out.
                if (_iClientScript == null)
                    _iClientScript = new ClientScriptManagerWrapper(
                        ClientScript, this);
                return _iClientScript;
            }
        }

        public virtual IServer IServer
        {
            get { return IPage.IServer; }
        }

        public virtual IPage IPage
        {
            get
            {
                if (_iPage == null)
                    _iPage = new PageWrapper(Page);
                return _iPage;
            }
        }

        public virtual ISessionState ISession
        {
            get { throw new NotImplementedException(); }
        }

        public virtual ClientScriptManager ClientScript
        {
            get { return IPage.ClientScript; }
        }

        public virtual bool IsValid
        {
            get { throw new NotImplementedException(); }
        }

        public virtual EventHandler LoadComplete
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public virtual IPrincipal User
        {
            get { return IPage.User; }
        }

        public virtual IRequest IRequest
        {
            get { return _iRequest ?? IPage.IRequest; }
        }

        public virtual IResponse IResponse
        {
            get { return _iResponse ?? IPage.IResponse; }
        }

        public virtual IUser IUser
        {
            get { return _iUser ?? IPage.IUser; }
        }

        public virtual IMasterPage IMaster
        {
            get { throw new NotImplementedException(); }
        }

        public virtual ICache ICache
        {
            get { throw new NotImplementedException(); }
        }

        public virtual IHtmlHead IHeader
        {
            get { return IPage.IHeader; }
        }

        public virtual IRoles IRoles
        {
            get { return IPage.IRoles; }
        }

        public bool IsReusable
        {
            get { throw new NotImplementedException(); }
        }

        #endregion

        #region Exposed Methods

        public virtual void AddControl(Control control)
        {
            Controls.Add(control);
        }

        public virtual void AddHeaderControl(Control toAdd)
        {
            throw new NotImplementedException();
        }

        public void RegisterRequiresControlState(Control control)
        {
            throw new NotImplementedException();
        }

        public IHttpHandler ToHttpHandler()
        {
            throw new NotImplementedException();
        }

        public virtual TControl FindControl<TControl>(string id) where TControl : Control
        {
            throw new NotImplementedException();
        }

        public virtual TIControl FindIControl<TIControl>(string id) where TIControl : IControl
        {
            throw new NotImplementedException();
        }

        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }

        #endregion
    }

    public class RolesWrapper : IRoles
    {
        #region Exposed Methods

        public string[] GetRolesForUser(string name)
        {
            return Roles.GetRolesForUser(name);
        }

        #endregion
    }

    public interface IRoles
    {
        #region Methods

        string[] GetRolesForUser(string name);

        #endregion
    }
}
