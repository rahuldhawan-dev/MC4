using System;
using System.Data;
using System.Data.SqlClient;
using System.Web;

namespace MMSINC
{
    public enum AuditCategory
    {
        None,
        System,
        DataInsert,
        DataUpdate,
        DataDelete,
        DataView
    }

    /// <summary>
    /// Represents the ability to add items to the Audit log.
    /// 
    /// TODO: Should this be a static class using the Factory pattern?
    /// </summary>
    public class Audit
    {
        #region Properites

        public int Category { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string Details { get; set; }

        #endregion

        #region Exposed Static Methods

        /// <summary>
        /// This overload assumes that the audit is created by HttpContext.Current.User, which is the same as passing in Page.User. 
        /// </summary>
        public static void Insert(AuditCategory category, string details, string sqlConnectionString)
        {
            var cont = HttpContext.Current;
            if (cont == null)
            {
                throw new NullReferenceException("This Audit.Insert overload can't be used when HttpContext.Current is null.");
            }

            Insert((int)category, cont.User.Identity.Name, details, sqlConnectionString);
        }

        public static void Insert(AuditCategory category, string createdBy, string details, string sqlConnectionString)
        {
            Insert((int)category, createdBy, details, sqlConnectionString);
        }

        public static void Insert(int category, string createdBy, string details, string sqlConnectionString)
        {
            using (var conn = new SqlConnection(sqlConnectionString))
            {
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    conn.Open();

                    cmd.CommandText = "insert into [Audit] (AuditCategoryID, CreatedBy, Details) VALUES (@Category, @CreatedBy, @Details)";

                    cmd.Parameters.Add(new SqlParameter("Category", category));
                    cmd.Parameters.Add(new SqlParameter("CreatedBy", createdBy));
                    cmd.Parameters.Add(new SqlParameter("Details", details));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion
    }
}
