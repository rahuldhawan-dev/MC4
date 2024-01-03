using System;
using System.Data;
using System.Data.SqlClient;

namespace MMSINC.Utilities.Auditing
{
    public class Auditor : IAuditor
    {
        #region Properties

        public string SqlConnectionString { get; set; }

        #endregion

        #region Exposed Methods

        public void Insert(AuditCategory category, string createdBy, string details)
        {
            if (string.IsNullOrEmpty(createdBy))
            {
                throw new ArgumentNullException("createdBy");
            }

            if (string.IsNullOrEmpty(details))
            {
                throw new ArgumentNullException("details");
            }

            using (var conn = CreateConnection())
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText =
                        "insert into [Audit] (AuditCategoryID, CreatedBy, Details) VALUES (@Category, @CreatedBy, @Details)";

                    var parm = cmd.Parameters;
                    // Not sure why category needs to be an int, just doing it for backwards compatibility.
                    parm.Add(new SqlParameter("Category", (int)category));
                    parm.Add(new SqlParameter("CreatedBy", createdBy));
                    parm.Add(new SqlParameter("Details", details));
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        protected virtual IDbConnection CreateConnection()
        {
            if (string.IsNullOrEmpty(SqlConnectionString))
            {
                throw new NullReferenceException("SqlConnectionString can not be null or empty.");
            }

            return new SqlConnection(SqlConnectionString);
        }

        #endregion
    }
}
