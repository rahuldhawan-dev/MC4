using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;

namespace MMSINC.DataPages
{
    // TODO: Add testing. 

    /// <summary>
    /// Helper class for storing values to/from ControlState(as opposed to ViewState). 
    /// </summary>
    public class ControlState : IControlState
    {
        #region Fields

        private readonly Dictionary<int, object> _stateDict = new Dictionary<int, object>();

        #endregion

        #region Constructors

        public ControlState()
        {
            // Empty constructor
        }

        /// <summary>
        /// Creates a new ControlState instance with the values from a previously serialized instance.
        /// </summary>
        /// <param name="previousState"></param>
        public ControlState(object previousState)
            : this()
        {
            LoadControlState(previousState);
        }

        #endregion

        #region Private Methods

        private static object PrepareValue(object value)
        {
            if (value != null)
            {
                // Preparing values should go from quickest to parse/serialize
                // to slowest. 

                var t = value.GetType();

                if (t.IsEnum)
                {
                    // Convert enums to ints to prevent the ASP
                    // serializer from storing the fully qualified
                    // type/assembly name for each instance.
                    return (int)value;
                }

                if (value is Guid)
                {
                    // Otherwise it serializes Guid's full type info. 
                    return value.ToString();
                }
            }

            // Return the original value if it's null or has otherwise
            // not been converted to something else.
            return value;
        }

        #endregion

        #region Exposed Methods

        /// <summary>
        /// Adds a property and value to this ControlState instance. 
        /// If the value parameter is equal to the defaultValue parameter,
        /// then it will not be added to the internal dictionary. This reduces
        /// serialization overhead. 
        /// </summary>
        /// <remarks>
        /// This has a generic constraint to enforce that the value and defaultValues
        /// must be of the same type. Otherwise the Get method will cast incorrectly if
        /// it doesn't know which type to expect. 
        /// </remarks>
        public void Add<T>(string key, T value, T defaultValue)
        {
            // Null is bad, but empty strings are ok.
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            if (!Equals(value, defaultValue))
            {
                // Store the key's hash code. This will serialize smaller than
                // the string itself will. A string's hash code is guaranteed
                // to always be equal for the same value during runtime. 
                _stateDict.Add(key.GetHashCode(), value);
            }
        }

        /// <summary>
        /// Gets a value from this ControlState. 
        /// </summary>
        public T Get<T>(string key)
        {
            object value;

            if (_stateDict.TryGetValue(key.GetHashCode(), out value))
            {
                // TODO: Add testing for this. 
                if (typeof(T) == typeof(Guid))
                {
                    value = new Guid((string)value);
                }

                return (T)value;
            }

            return default(T);
        }

        /// <summary>
        /// Returns the actual ControlState object that can be serialized. 
        /// </summary>
        public object GetControlStateObject()
        {
            if (_stateDict.Any())
            {
                var pairs = new List<Pair>();

                foreach (var kvPair in _stateDict)
                {
                    pairs.Add(new Pair(kvPair.Key, PrepareValue(kvPair.Value)));
                }

                return pairs.ToArray();
            }

            // Return null if there are no entries. This prevents needless serialization
            // to the client. 
            return null;
        }

        /// <summary>
        /// Loads values from a previous ControlState instance into this instance. 
        /// </summary>
        /// <param name="state"></param>
        /// <remarks>
        /// 
        /// Don't throw for nulls, just don't add any values to the internal dictionary.
        /// GetControlStateObject returns null if there aren't any values to return.
        /// 
        /// </remarks>
        public void LoadControlState(object state)
        {
            // There's no merging of ControlStates here. Loading
            // should set up this instance as a copy of the previous
            // state. So clear the dictionary if this is used anytime
            // after values may have been added to this instance.
            _stateDict.Clear();

            var st = (state as IEnumerable<Pair>);

            if (st != null)
            {
                foreach (var pair in st)
                {
                    _stateDict.Add((int)pair.First, pair.Second);
                }
            }
        }

        #endregion
    }
}
