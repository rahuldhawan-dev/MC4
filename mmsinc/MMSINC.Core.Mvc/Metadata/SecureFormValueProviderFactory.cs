using System;
using System.Linq;
using System.Security;
using System.Web.Mvc;
using MMSINC.Authentication;
using MMSINC.Helpers;
using MMSINC.Utilities;
using StructureMap;

namespace MMSINC.Metadata
{
    /// <summary>
    /// ValueProviderFactory needed to create SecureFormValueProviders in MVC.
    /// </summary>
    public class SecureFormValueProviderFactory<TToken, TValues> : ValueProviderFactory
        where TToken : class, ISecureFormToken<TToken, TValues>, new()
        where TValues : ISecureFormDynamicValue<TValues, TToken>
    {
        #region Fields

        private IContainer _container;

        #endregion

        #region Constructor

        public SecureFormValueProviderFactory(IContainer container)
        {
            _container = container;
        }

        #endregion

        #region Public Methods

        public override IValueProvider GetValueProvider(ControllerContext controllerContext)
        {
            // NOTE: The TokenFilter will do the validation on whether the token has all the correct values and junk.
            if (!FormBuilder.RequiresSecureForm(new RouteContext(controllerContext)))
            {
                return null;
            }

            var token = SecureTokenFilter<TToken, TValues>.TryFindToken(controllerContext,
                _container.GetInstance<ITokenRepository<TToken, TValues>>());

            if (token == null)
            {
                // We can return null here. The value providers won't be used when
                // a SecureForm is required and the SecureFormTokenFilter can't
                // find a token(it forwards to an error page, so no model binding
                // will take place).
                return null;
            }

            return new SecureFormValueProvider<TToken, TValues>(token);
        }

        #endregion
    }
}
