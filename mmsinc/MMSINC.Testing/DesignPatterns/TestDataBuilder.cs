using System;
using System.Reflection;
using MMSINC.DesignPatterns;

namespace MMSINC.Testing.DesignPatterns
{
    public abstract class TestDataBuilder<TTarget> : Builder<TTarget>
        where TTarget : class
    {
        #region Private Static Members

        private static readonly Type _entityType = typeof(TTarget);

        #endregion

        #region Private Methods

        protected static void SetFieldValue(TTarget entity, string fieldName, object newValue)
        {
            _entityType.GetField(fieldName,
                BindingFlags.Instance | BindingFlags.NonPublic).SetValue(
                entity, newValue);
        }

        #endregion
    }
}
