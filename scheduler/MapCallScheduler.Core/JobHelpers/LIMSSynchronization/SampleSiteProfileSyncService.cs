using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.LIMS.Client;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.LIMSSynchronization
{
    /// <summary>
    /// A service that performs the work necessary to sync data between LIMS and MapCall.
    /// </summary>
    public class SampleSiteProfileSyncService : ISampleSiteProfileSyncService
    {
        #region Constants

        public static readonly int[] SYNCABLE_PUBLIC_WATER_SUPPLY_STATUSES = new[] {
            PublicWaterSupplyStatus.Indices.ACTIVE,
            PublicWaterSupplyStatus.Indices.PENDING,
            PublicWaterSupplyStatus.Indices.PENDING_MERGER
        };

        #endregion  

        #region Private Members

        private readonly ILog _log;
        private readonly ILIMSApiClient _limsApiClient;
        private readonly IRepository<SampleSiteProfile> _sampleSiteProfileRepository;
        private readonly IRepository<PublicWaterSupply> _publicWaterSupplyRepository;
        private readonly IRepository<SampleSiteProfileAnalysisType> _sampleSiteProfileAnalysisTypeRepository;

        #endregion  

        #region Constructors

        public SampleSiteProfileSyncService(
            ILIMSApiClient limsApiClient,
            IRepository<SampleSiteProfile> sampleSiteRepository,
            IRepository<PublicWaterSupply> publicWaterSupplyRepository,
            IRepository<SampleSiteProfileAnalysisType> sampleSiteProfileAnalysisTypeRepository,
            ILog log)
        {
            _limsApiClient = limsApiClient;
            _sampleSiteProfileRepository = sampleSiteRepository;
            _publicWaterSupplyRepository = publicWaterSupplyRepository;
            _sampleSiteProfileAnalysisTypeRepository = sampleSiteProfileAnalysisTypeRepository;
            _log = log;
        }

        #endregion

        #region Exposed Methods

        public void Process()
        {
            /* 
             * Grab the latest profiles from external lims 
             */
            var limsProfiles = _limsApiClient.GetProfiles()
                                             .Where(x => !string.IsNullOrEmpty(x.PublicWaterSupplyIdentifier))
                                             .ToList();

            /*
             *  Grab the latest profiles from internal mapcall
             */
            var sampleSiteProfilesMap = _sampleSiteProfileRepository
                                       .GetAll()
                                       .ToList()
                                       .MapToDictionary(
                                            x => x.Number,
                                            number => _log.Info($"Unexpected duplicate profile number found: {number}"));

            /*
             * Grab the latest profile analysis types from internal mapcall
             */
            var sampleSiteProfileAnalysisTypesMap = _sampleSiteProfileAnalysisTypeRepository
                                                   .GetAll()
                                                   .ToList()
                                                   .MapToDictionary(
                                                        x => x.Description,
                                                        description =>
                                                            _log.Info($"Unexpected duplicate sample site profile analysis type found: {description}"));

            /*
             * Grab the active, pending and pending-merger public water supplies from internal mapcall
             */
            var publicWaterSuppliesMap = _publicWaterSupplyRepository.Where(x => x.Identifier != null &&
                                                                                 SYNCABLE_PUBLIC_WATER_SUPPLY_STATUSES.Contains(x.Status.Id))
                                                                     .ToList()
                                                                     .MapToDictionary(
                                                                          x => x.Identifier,
                                                                          identifier => _log.Info($"Unexpected duplicate public water supply found: {identifier}"));

            _log.Info($"Total external Lims Profiles..........: {limsProfiles.Count}");
            _log.Info($"Total internal Sample Site Profiles...: {sampleSiteProfilesMap.Keys.Count()}");

            var sampleSiteProfilesToSync = new List<SampleSiteProfile>();

            /*
             * For each external lims profile...
             */
            foreach (var limsProfile in limsProfiles)
            {
                if (!sampleSiteProfileAnalysisTypesMap.TryGetValue(limsProfile.AnalysisType, out var analysisType))
                {
                    /*
                     * Ignore any external lims profiles that have an analysis type that we are 
                     * not aware of, but we don't want to crash ourselves, so carry on and continue
                     */
                    _log.Info($"Unexpected analysis type encountered: {limsProfile.AnalysisType}");
                    continue;
                }

                if (!publicWaterSuppliesMap.TryGetValue(limsProfile.PublicWaterSupplyIdentifier, out var publicWaterSupply))
                {
                    /*
                     * There are a lot of profile numbers in the LIMS system that are not owned by American Water, and the API sends us 
                     * all of it's profile numbers, therefore, some (a greater percentage) will not match. We can ignore these.
                     */
                    continue;
                }

                /*
                 * the external lims profile has a known analysis type, and a known public water supply
                 * let's see if this is a known internal profile...
                 */
                var sampleSiteProfile = sampleSiteProfilesMap.ContainsKey(limsProfile.Number)
                    ? sampleSiteProfilesMap[limsProfile.Number]
                    : new SampleSiteProfile {
                        Number = limsProfile.Number,
                        Name = limsProfile.Name
                    };

                /*
                 * if this is an unknown sample site, or if it's known but changed, sync it
                 */
                if (sampleSiteProfile.Id == 0 || 
                    sampleSiteProfile.Number != limsProfile.Number ||
                    sampleSiteProfile.Name != limsProfile.Name ||
                    sampleSiteProfile.SampleSiteProfileAnalysisType != analysisType ||
                    sampleSiteProfile.PublicWaterSupply != publicWaterSupply)
                {
                    sampleSiteProfile.Number = limsProfile.Number;
                    sampleSiteProfile.Name = limsProfile.Name;
                    sampleSiteProfile.SampleSiteProfileAnalysisType = analysisType;
                    sampleSiteProfile.PublicWaterSupply = publicWaterSupply;
                    sampleSiteProfilesToSync.Add(sampleSiteProfile);
                }
            }

            _sampleSiteProfileRepository.Save(sampleSiteProfilesToSync);
        }

        #endregion
    }
}
