using System.Data;
using Humanizer;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using NHibernate;

namespace MapCallScheduler.JobHelpers.ServicePremiseLink
{
    // A Note to Future Implementors of Things:
    //
    // This is usually not the preferred way of performing MapCall Scheduler jobs; all of MapCall's application
    // components are designed to use the central NHibernate model from the MapCall.Common library so that constraints
    // and other business logic can be preserved.  HOWEVER, the links being performed by this service in particular have
    // no need for tracking or validation, thus it was much faster to employ the most efficient possible direct queries
    // rather than pulling all manner of data into memory within the Scheduler application and looping on it.
    public class ServicePremiseLinkService : IServicePremiseLinkService
    {
        #region Constants

        public struct Queries
        {
            #region Table/Column Names
            
            private const string SERVICES =
                                     nameof(MapCall.Common.Model.Entities.Service) + "s",
                                 SERVICE_CATEGORIES = ServiceCategoryMap.TABLE_NAME,
                                 PREMISES =
                                     nameof(MapCall.Common.Model.Entities.Premise) + "s";

            private struct Service
            {
                public const string SERVICE_CATEGORY_ID =
                                        nameof(MapCall.Common.Model.Entities.Service.ServiceCategory) + "Id",
                                    INSTALLATION =
                                        nameof(MapCall.Common.Model.Entities.Service.Installation),
                                    PREMISE_NUMBER =
                                        nameof(MapCall.Common.Model.Entities.Service.PremiseNumber),
                                    PREMISE_ID =
                                        nameof(MapCall.Common.Model.Entities.Service.Premise) + "Id";
            }

            private struct ServiceCategory
            {
                public const string ID = ServiceCategoryMap.ID_COLUMN_NAME,
                                    SERVICE_UTILITY_TYPE_ID =
                                        nameof(MapCall.Common.Model.Entities.ServiceCategory.ServiceUtilityType) + "Id";
            }

            private struct Premise
            {
                public const string INSTALLATION =
                                        nameof(MapCall.Common.Model.Entities.Premise.Installation),
                                    PREMISE_NUMBER =
                                        nameof(MapCall.Common.Model.Entities.Premise.PremiseNumber),
                                    SERVICE_UTILITY_TYPE_ID =
                                        nameof(MapCall.Common.Model.Entities.Premise.ServiceUtilityType) + "Id",
                                    ID = "Id";
            }
            
            #endregion
            
            private const string UNLINKED_SERVICE_QUERY_BASE =
                "FROM " + SERVICES + " s " +
                "INNER JOIN " + SERVICE_CATEGORIES + " scat " +
                "ON s." + Service.SERVICE_CATEGORY_ID + " = scat." + ServiceCategory.ID + " " +
                "INNER JOIN " + PREMISES + " p " +
                "ON s." + Service.INSTALLATION + " = p." + Premise.INSTALLATION + " " +
                "AND s." + Service.PREMISE_NUMBER + " = p." + Premise.PREMISE_NUMBER + " " +
                "AND scat." + ServiceCategory.SERVICE_UTILITY_TYPE_ID + " = p." + Premise.SERVICE_UTILITY_TYPE_ID + " " +
                "WHERE s." + Service.PREMISE_ID + " IS NULL";

            public const string COUNT_UNLINKED_SERVICES =
                "SELECT COUNT(1) " + UNLINKED_SERVICE_QUERY_BASE;

            public const string LINK_UNLINKED_SERVICES =
                "UPDATE s " +
                "SET s." + Service.PREMISE_ID + " = p." + Premise.ID + " " +
                UNLINKED_SERVICE_QUERY_BASE;
        }

        #endregion
         
        #region Private Members

        private readonly ILog _log;
        private readonly IDbConnection _connection;

        #endregion

        #region Constructors

        public ServicePremiseLinkService(ILog log, ISession session)
        {
            _log = log;
            _connection = session.Connection;
        }

        #endregion

        #region Public Methods

        public void Process()
        {
            LinkThingToThing(
                nameof(Service).Titleize().ToLower(),
                nameof(Premise).Titleize().ToLower(),
                Queries.COUNT_UNLINKED_SERVICES,
                Queries.LINK_UNLINKED_SERVICES);
        }

        private int RunCountQuery(string query)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = query;

                return (int)cmd.ExecuteScalar();
            }
        }

        private int RunLinkQuery(string query)
        {
            using (var cmd = _connection.CreateCommand())
            {
                cmd.CommandText = query;

                return cmd.ExecuteNonQuery();
            }
        }

        private void LinkThingToThing(string firstThing, string secondThing, string countQuery, string linkQuery)
        {
            var firstThings = firstThing.Pluralize();
            var secondThings = secondThing.Pluralize();
            
            _log.Info($"Linking {firstThings} to {secondThings}...");

            var thingsToLinkCount = RunCountQuery(countQuery);
            _log.Info(
                $"Found {thingsToLinkCount} {firstThing} records which need linking to {secondThings}.");

            var thingsLinkedCount = RunLinkQuery(linkQuery);

            _log.Info($"Successfully linked {thingsLinkedCount} {firstThings} to {secondThings}.");

            if (thingsLinkedCount < thingsToLinkCount)
            {
                _log.Warn(
                    $"{thingsToLinkCount - thingsLinkedCount} fewer {firstThings} were linked than expected");
            }
        }

        #endregion
    }
}
