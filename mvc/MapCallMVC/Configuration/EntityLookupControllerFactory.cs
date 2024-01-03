using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Routing;
using MapCallMVC.Controllers;
using MMSINC.ControllerFactories;
using MMSINC.Data;
using StructureMap;

namespace MapCallMVC.Configuration
{
    public class EntityLookupControllerFactory : DefaultCompositableControllerFactory
    {
        #region Fields

        private readonly Lazy<Dictionary<string, EntityControllerInfo>> _controllerInfoByName;

        #endregion

        #region Properties

        /// <summary>
        /// Collection of Assemblies that contain classes that inherit EntityLookup
        /// that should be supported by this factory.
        /// </summary>
        public IList<Assembly> Assemblies { get; private set; }

        #endregion

        #region Constructor

        public EntityLookupControllerFactory(IContainer container) : base(container)
        {
            Assemblies = new List<Assembly>();
            Assemblies.Add(typeof(MapCall.Common.Utility.MapCallUrlHelper).Assembly);

            _controllerInfoByName = new Lazy<Dictionary<string, EntityControllerInfo>>(() =>
            {
                var dict = new Dictionary<string, EntityControllerInfo>(StringComparer.InvariantCultureIgnoreCase);
                var entityLookupBaseType = typeof(EntityLookup);

                foreach (var ass in Assemblies)
                {
                    var types = ass.GetTypes()
                                       .Where(x => entityLookupBaseType.IsAssignableFrom(x) && x != entityLookupBaseType);
                    foreach (var t in types)
                    {
                        dict.Add(t.Name, new EntityControllerInfo(t));
                    }
                }
                return dict;
            }, isThreadSafe: true);
        }

        #endregion

        #region Private Methods

        protected override Type GetControllerType(RequestContext requestContext, string controllerName)
        {
            // We don't wanna change anything in regards to how explicit controllers
            // are handled. So if there's already a match, just return it and we can
            // be on our way. This will also allow for custom controllers to inherit
            // from the generic EntityLookupController and have it act like normal.
            //var baseResult = base.GetControllerType(requestContext, controllerName);
            //if (baseResult != null)
            //{
            //    return baseResult;
            //}


            EntityControllerInfo controllerInfo;
            return _controllerInfoByName.Value.TryGetValue(controllerName, out controllerInfo) ? controllerInfo.ControllerType : null;
        }

        #endregion

        #region Implementation detail fun class

        private sealed class EntityControllerInfo
        {
            public Type EntityType { get; private set; }
            public Type ControllerType { get; private set; }

            public EntityControllerInfo(Type entityType)
            {
                EntityType = entityType;
                var repositoryType = typeof(MMSINC.Data.NHibernate.IRepository<>).MakeGenericType(EntityType);
                ControllerType = typeof(EntityLookupController<,>).MakeGenericType(repositoryType, EntityType);
                // TODO: make generic controller type.
            }
        }

        #endregion
    }
}