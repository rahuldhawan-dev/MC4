using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MMSINC.Utilities;

namespace MMSINC.Testing.Utilities
{
    public interface IStaticPropertyReplacer : IDisposable
    {
        void Init();
    }

    /// <summary>
    /// Utility class for replacing a static class's field or property with an instance
    /// while retaining the previous value so it can be re-set afterwards. This is only
    /// really useful for when something is not mockable at all. 
    /// </summary>
    /// <typeparam name="TPropertyType"></typeparam>
    public class StaticPropertyReplacer<TPropertyType> : IStaticPropertyReplacer where TPropertyType : new()
    {
        #region Fields

        private TPropertyType _currentInstance;

        private readonly FieldInfo _fieldInfo;
        private readonly PropertyInfo _propInfo;
        private bool _isInit;

        #endregion

        #region Properties

        /// <summary>
        /// Returns the previous value set on the static class before this 
        /// instance was initialized.
        /// </summary>
        public TPropertyType PreviousInstance { get; private set; }

        /// <summary>
        /// Gets the replacement instance of TPropertyType that will be
        /// set on the static class when this instance is initialized.
        /// The constructor will set this to a new instance by default.
        /// </summary>
        public TPropertyType ReplacementInstance
        {
            get { return _currentInstance; }
            set
            {
                if (_isInit)
                {
                    throw new Exception(
                        "Can not change replacement instance after StaticPropertyReplacer instances are initialized.");
                }

                _currentInstance = value;
            }
        }

        #endregion

        #region Constructor

        public StaticPropertyReplacer(Type staticClassType, string fieldOrPropertyName)
        {
            const BindingFlags bindingFlags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
            ReplacementInstance = new TPropertyType();
            _fieldInfo = staticClassType.GetField(fieldOrPropertyName, bindingFlags);
            _propInfo = staticClassType.GetProperty(fieldOrPropertyName, bindingFlags);

            if (_fieldInfo == null && _propInfo == null)
            {
                throw ExceptionHelper.Format<ArgumentException>(
                    "Unable to find static field or property named '{0}' on type '{1}'.",
                    fieldOrPropertyName, staticClassType);
            }
        }

        #endregion

        public void Init()
        {
            if (!_isInit)
            {
                if (_fieldInfo != null)
                {
                    PreviousInstance = (TPropertyType)_fieldInfo.GetValue(null);
                    _fieldInfo.SetValue(null, ReplacementInstance);
                }
                else
                {
                    PreviousInstance = (TPropertyType)_propInfo.GetValue(null, new object[] { });
                    _propInfo.SetValue(null, ReplacementInstance, new object[] { });
                }

                _isInit = true;
            }
        }

        public void Dispose()
        {
            if (_isInit)
            {
                if (_fieldInfo != null)
                {
                    _fieldInfo.SetValue(null, PreviousInstance);
                }
                else
                {
                    _propInfo.SetValue(null, PreviousInstance, new object[] { });
                }

                PreviousInstance = default(TPropertyType);
                _isInit = false;
            }
        }
    }

    /// <summary>
    /// Utility class for replacing a static class's field or property with an instance
    /// while retaining the previous value so it can be re-set afterwards.
    /// </summary>
    public class StaticPropertyReplacer<TClass, TPropertyType> : StaticPropertyReplacer<TPropertyType>
        where TPropertyType : new()
    {
        public StaticPropertyReplacer(string fieldOrPropertyName) : base(typeof(TClass), fieldOrPropertyName) { }
    }
}
