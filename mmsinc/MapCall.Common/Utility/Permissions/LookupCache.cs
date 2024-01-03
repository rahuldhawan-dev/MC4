using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using MMSINC.Utilities.Permissions;

namespace MapCall.Common.Utility.Permissions
{
    public class LookupCache : ILookupCache
    {
        #region Fields

        private readonly object _lockObj = new object();
        private readonly string _connectionString;
        private bool _isInitialized;
        private IDictionary<int, IRoleAction> _actions;
        private IDictionary<int, IRoleApplication> _applications;
        private IDictionary<string, IRoleApplication> _applicationsByName;
        private IDictionary<int, IRoleModule> _modules;
        private IDictionary<int, IOperatingCenter> _operatingCenters;
        private IDictionary<string, IOperatingCenter> _operatingCentersByName;

        #endregion

        #region Properties

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        public IDictionary<int, IRoleAction> Actions
        {
            get
            {
                Initialize();
                return _actions;
            }
            private set { _actions = value; }
        }

        public IDictionary<int, IRoleApplication> Applications
        {
            get
            {
                Initialize();
                return _applications;
            }
            private set { _applications = value; }
        }

        public IDictionary<string, IRoleApplication> ApplicationsByName
        {
            get
            {
                Initialize();
                return _applicationsByName;
            }
            private set { _applicationsByName = value; }
        }

        public IDictionary<int, IRoleModule> Modules
        {
            get
            {
                Initialize();
                return _modules;
            }
            private set { _modules = value; }
        }

        public IDictionary<int, IOperatingCenter> OperatingCenters
        {
            get
            {
                Initialize();
                return _operatingCenters;
            }
            private set { _operatingCenters = value; }
        }

        public IDictionary<string, IOperatingCenter> OperatingCentersByName
        {
            get
            {
                Initialize();
                return _operatingCentersByName;
            }
            private set { _operatingCentersByName = value; }
        }

        #endregion

        #region Constructors

        public LookupCache(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentNullException("connectionString");
            }

            _connectionString = connectionString;
        }

        #endregion

        #region Private Methods

        protected virtual IDbConnection GetConnection()
        {
            return new SqlConnection(_connectionString);
        }

        protected virtual void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            using (var conn = GetConnection())
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = " select ActionID, Name from [Actions]" +
                                  " select OperatingCenterID, OperatingCenterCode from [OperatingCenters]" +
                                  " select ApplicationID, ModuleID, Name from [Modules]" +
                                  " select ApplicationID, Name from [Applications]";
                conn.Open();

                using (var r = cmd.ExecuteReader())
                {
                    ReadAndSetActions(r);
                    r.NextResult();
                    ReadAndSetOperatingCenters(r);
                    r.NextResult();
                    ReadAndSetModules(r);
                    r.NextResult();
                    ReadAndSetApplications(r);
                }
            }

            _isInitialized = true;
        }

        protected virtual void ReadAndSetActions(IDataReader reader)
        {
            var acts = new List<IRoleAction>();

            var actionIdOrd = reader.GetOrdinal("ActionID");
            var nameOrd = reader.GetOrdinal("Name");

            while (reader.Read())
            {
                acts.Add(new RoleAction {
                    ActionId = reader.GetInt32(actionIdOrd),
                    Name = reader.GetString(nameOrd),
                });
            }

            Actions = acts.ToDictionary(a => a.ActionId);
        }

        protected virtual void ReadAndSetApplications(IDataReader reader)
        {
            var apps = new List<IRoleApplication>();

            var appIdOrd = reader.GetOrdinal("ApplicationID");
            var nameOrd = reader.GetOrdinal("Name");

            while (reader.Read())
            {
                apps.Add(new RoleApplication {
                    ApplicationId = reader.GetInt32(appIdOrd),
                    Name = reader.GetString(nameOrd)
                });
            }

            Applications = apps.ToDictionary(a => a.ApplicationId);
            ApplicationsByName = apps.ToDictionary(a => a.Name,
                StringComparer.InvariantCultureIgnoreCase);
        }

        protected virtual void ReadAndSetModules(IDataReader reader)
        {
            var mods = new List<IRoleModule>();
            var appIdOrd = reader.GetOrdinal("ApplicationId");
            var modIdOrd = reader.GetOrdinal("ModuleId");
            var nameOrd = reader.GetOrdinal("Name");

            while (reader.Read())
            {
                mods.Add(new RoleModule {
                    ApplicationId = reader.GetInt32(appIdOrd),
                    ModuleId = reader.GetInt32(modIdOrd),
                    Name = reader.GetString(nameOrd)
                });
            }

            Modules = mods.ToDictionary(m => m.ModuleId);
        }

        protected virtual void ReadAndSetOperatingCenters(IDataReader reader)
        {
            var ops = new List<IOperatingCenter>();

            var opCenterIdOrd = reader.GetOrdinal("OperatingCenterId");
            var nameOrd = reader.GetOrdinal("OperatingCenterCode");

            while (reader.Read())
            {
                ops.Add(new OperatingCenter {
                    OperatingCenterId = reader.GetInt32(opCenterIdOrd),
                    OperatingCenterCode = reader.GetString(nameOrd),
                });
            }

            OperatingCenters = ops.ToDictionary(o => o.OperatingCenterId);
            OperatingCentersByName = ops.ToDictionary(
                o => o.OperatingCenterCode,
                StringComparer.InvariantCultureIgnoreCase);
        }

        #endregion

        #region Public Methods

        public void Reinitialize()
        {
            _isInitialized = false;
            lock (_lockObj)
            {
                Initialize();
            }
        }

        #endregion
    }
}
