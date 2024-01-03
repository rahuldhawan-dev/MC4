using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MMSINC.Utilities
{
    /// <summary>
    /// Thread-safe class for retrieving types via a cached key value. This is meant to be used
    /// for passing type information to the client-side without having to reveal the full
    /// Type.AssemblyQualifiedName.
    /// </summary>
    /// <remarks>
    /// This is needed for dealing with saving DataTables. I don't want the full type/assembly
    /// information available to the client
    /// </remarks>
    public static class TypeCache
    {
        #region Fields

        private static readonly ConcurrentDictionary<Guid, Type> _typesByGuid = new ConcurrentDictionary<Guid, Type>();

        #endregion

        #region Private Static Methods

        private static Guid HashType(Type type)
        {
            using (var md5Hash = MD5.Create())
            {
                var hash = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(type.AssemblyQualifiedName));
                return new Guid(hash);
            }
        }

        #endregion

        #region Public Static Methods

        /// <summary>
        /// This clears the entire cache. THIS IS ONLY FOR TESTING PURPOSES!
        /// </summary>
        public static void Clear()
        {
            _typesByGuid.Clear();
        }

        /// <summary>
        /// Returns a guid that can be used to look up the given type instance. 
        /// 
        /// NOTE: This guid is only guaranteed to stay the same for the lifetime of the application.
        /// Renaming the type, moving the type to a different namespace, recompiling, or something else
        /// could potentially cause this value to change.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Guid RegisterType(Type type)
        {
            var guid = HashType(type);
            _typesByGuid.GetOrAdd(guid, type);
            return guid;
        }

        /// <summary>
        /// Tries to return the type based on the guid key. 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool TryRetrieveType(Guid key, out Type type)
        {
            return _typesByGuid.TryGetValue(key, out type);
        }

        #endregion
    }
}
