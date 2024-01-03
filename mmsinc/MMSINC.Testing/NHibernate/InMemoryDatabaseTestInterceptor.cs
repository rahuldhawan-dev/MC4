using System.Collections.Generic;
using MMSINC.Utilities.StructureMap;
using NHibernate;
using NHibernate.SqlCommand;
using StructureMap;

namespace MMSINC.Testing.NHibernate
{
    /// <remarks>
    /// 
    /// Usage:
    ///     1. Call Init() after everything is flushed to the database and any
    ///        additional database setup is completed.
    ///     2. Call Session.Clear() to ensure that anything you query for is
    ///        explicitly accessed from the database instead of NHibernate's 
    ///        session cache.
    ///     3. Do whatever it is you need to test.
    ///     4. Call Reset() to disable it. Or don't, it's reset during TestInitialize anyway.
    ///        But definitely make sure it resets somewhere. This is a singleton and could
    ///        quickly lead to memory issues if it keeps storing thousands of sql commands.
    /// </remarks>
    public class InMemoryDatabaseTestInterceptor : StructureMapInterceptor, IInMemoryDatabaseTestInterceptor
    {
        #region Private Members

        #region Fields

        private bool _enabled;

        #endregion

        #endregion

        #region Properties

        /// <summary>
        /// Returns all the prepared statements created since Init was called.
        /// </summary>
        public List<SqlString> PreparedStatements { get; set; }

        #endregion

        #region Constructors

        public InMemoryDatabaseTestInterceptor(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        #region Interceptor Methods

        public override SqlString OnPrepareStatement(SqlString sql)
        {
            if (_enabled)
            {
                PreparedStatements.Add(sql);
            }

            return base.OnPrepareStatement(sql);
        }

        #endregion

        #endregion

        #region Public Methods

        /// <summary>
        /// Enables the interceptor to start logging things and doing whatever.
        /// </summary>
        public void Init()
        {
            // Call a Reset to clear stuff in case someone forgets to reset.
            // This is a singleton instance so it needs to be reset all the time for performance reasons(probably).
            Reset();
            _enabled = true;
        }

        /// <summary>
        /// Disables the interceptor and clears out any logs.
        /// </summary>
        public void Reset()
        {
            _enabled = false;
            PreparedStatements = new List<SqlString>();
        }

        #endregion
    }

    public interface IInMemoryDatabaseTestInterceptor : IInterceptor
    {
        void Init();
        void Reset();
        List<SqlString> PreparedStatements { get; }
    }
}
