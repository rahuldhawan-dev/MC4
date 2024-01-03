using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MMSINC.ClassExtensions
{
    public static class NHibernateConfigurationExtensions
    {
        #region Fields

        private static readonly Type _abstractAuxiliaryDatabaseObjectType =
            typeof(NHibernate.Mapping.AbstractAuxiliaryDatabaseObject);

        #endregion

        #region Extension Methods

        /// <summary>
        /// Creates and adds a new instance of every type that inherits from NHibernate.Mapping.AbstractAuxiliaryDatabaseObject
        /// in an assembly to the NHibernate Configuration.
        /// </summary>
        /// <typeparam name="TAssemblyOf"></typeparam>
        /// <param name="config"></param>
        public static void AddAuxiliaryDatabaseObjectsInAssemblyOf<TAssemblyOf>(
            this NHibernate.Cfg.Configuration config)
        {
            foreach (var type in typeof(TAssemblyOf).Assembly.GetTypes())
            {
                if (_abstractAuxiliaryDatabaseObjectType.IsAssignableFrom(type))
                {
                    var instance = (NHibernate.Mapping.AbstractAuxiliaryDatabaseObject)Activator.CreateInstance(type);
                    config.AddAuxiliaryDatabaseObject(instance);
                }
            }
        }

        #endregion
    }
}
