using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using MapCall.Common.Utility.Notifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.Common.Testing.Utilities
{
    public abstract class NotificationConfigurationTestBase
    {
        #region Private Members

        private Assembly _assembly;
        private SqlConnection _conn;

        #endregion

        #region Abstract Properties

        protected abstract string ConnectionString { get; }

        #endregion

        #region Properties

        protected virtual Dictionary<string, string> EmbeddedExceptions => new Dictionary<string, string>();
        protected virtual Dictionary<string, string> DatabaseExceptions => new Dictionary<string, string>();

        #endregion

        #region Private Methods

        private IEnumerable<Notification> GetAllEmbeddedNotifcations()
        {
            var regex = new Regex(@"\.([^\.]+)\.([^\.]+)\.cshtml$");

            return _assembly
                  .GetManifestResourceNames().Where(n => n.EndsWith(".cshtml"))
                  .Select(name => regex.Match(name))
                  .Select(match => new Notification(match.Groups[1].Value, match.Groups[2].Value));
        }

        private IEnumerable<Notification> GetAllNotificationsInDatabase()
        {
            var reader =
                new SqlCommand(
                        "SELECT replace(m.Name, ' ', '') as Module, replace(p.Purpose, ' ', '') FROM NotificationPurposes p INNER JOIN Modules m on p.ModuleId = m.ModuleId",
                        _conn)
                   .ExecuteReader();

            while (reader.Read())
            {
                yield return new Notification(reader.GetString(0), reader.GetString(1));
            }

            reader.Close();
        }

        #endregion

        #region Exposed Methods

        public virtual void TestEmbeddedResourcesMatchDatabaseConfiguration()
        {
            var allDatabase = GetAllNotificationsInDatabase().ToList();
            var allEmbedded = GetAllEmbeddedNotifcations().ToList();

            var unmatchedEmbedded = allEmbedded.Where(n =>
                                                    !allDatabase.Contains(n) && !EmbeddedExceptions.Any(e =>
                                                        e.Key == n.Module && e.Value == n.Purpose))
                                               .ToList();
            var unmatchedDatabase = allDatabase.Where(n =>
                                                    !allEmbedded.Contains(n) && !DatabaseExceptions.Any(e =>
                                                        e.Key == n.Module && e.Value == n.Purpose))
                                               .ToList();

            var sb =
                new StringBuilder(unmatchedEmbedded.Any()
                    ? Environment.NewLine + "UNMATCHED EMBEDDED NOTIFICATIONS:" + Environment.NewLine
                    : string.Empty);

            foreach (var notification in unmatchedEmbedded)
            {
                sb.AppendLine($"Unmatched Embedded Notification: {notification}");
            }

            sb.AppendLine(unmatchedDatabase.Any()
                ? Environment.NewLine + "UNMATCHED DATABASE NOTIFICATIONS:"
                : string.Empty);

            foreach (var notification in unmatchedDatabase)
            {
                sb.AppendLine($"Unmatched Database Notification: {notification}");
            }

            var result = sb.ToString();

            if (!string.IsNullOrWhiteSpace(result))
            {
                Assert.Fail(result);
            }
        }

        #endregion

        #region Nested Type: Notification

        private class Notification
        {
            #region Properties

            public string Module { get; }
            public string Purpose { get; }

            #endregion

            #region Constructors

            public Notification(string module, string purpose)
            {
                Module = module;
                Purpose = purpose;
            }

            #endregion

            #region Private Methods

            private bool Equals(Notification other)
            {
                return string.Equals(ToString(), other.ToString());
            }

            #endregion

            #region Exposed Methods

            public override string ToString()
            {
                return $"Module {Module}, Purpose {Purpose}";
            }

            public override bool Equals(object obj)
            {
                var other = obj as Notification;

                return other != null && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ToString().GetHashCode();
                }
            }

            #endregion
        }

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void TestInitialize()
        {
            _assembly = typeof(RazorNotifier).Assembly;
            _conn = new SqlConnection(ConnectionString);
            _conn.Open();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _conn.Close();
        }

        #endregion
    }
}
