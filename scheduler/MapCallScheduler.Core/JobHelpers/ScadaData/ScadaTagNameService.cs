using System.Linq;
using Historian.Data.Client.Repositories;
using log4net;
using MapCall.Common.Model.Entities;
using MapCallScheduler.Library;
using MapCallScheduler.Library.Common;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.ScadaData
{
    public class ScadaTagNameService : IScadaTagNameService
    {
        #region Private Members

        private readonly ILog _log;
        private readonly ITagNameRepository _remoteRepo;
        private readonly IRepository<ScadaTagName> _localRepo;

        #endregion

        #region Constructors

        public ScadaTagNameService(ILog log, ITagNameRepository remoteRepo, IRepository<ScadaTagName> localRepo)
        {
            _log = log;
            _remoteRepo = remoteRepo;
            _localRepo = localRepo;
        }

        #endregion

        #region Exposed Methods

        public void Process()
        {
            var allRemoteTags = _remoteRepo.GetAll();
            var allLocalTags = _localRepo.GetAll();

            foreach (var tag in allRemoteTags)
            {
                var existing = allLocalTags.SingleOrDefault(t => t.TagName == tag.Name);

                if (existing == null)
                {
                    _log.Info($"Creating ScadaTagName '{tag.Name}'...");
                }
                else
                {
                    _log.Info($"Updating ScadaTagName '{tag.Name}' ({existing.Id})...");

                    existing.Description = tag.Description;
                    existing.Units = tag.Units;
                    existing.Inactive = false;
                }

                _localRepo.Save(existing ?? new ScadaTagName {
                    TagName = tag.Name,
                    Description = tag.Description,
                    Units = tag.Units
                });
            }

            foreach (var tag in allLocalTags)
            {
                if (!allRemoteTags.Any(t => t.Name == tag.TagName))
                {
                    _log.Info($"Setting ScadaTagName '{tag.TagName}' to inactive...");
                    tag.Inactive = true;
                    _localRepo.Save(tag);
                }
            }
        }

        #endregion
    }

    public interface IScadaTagNameService : IProcessableService { }
}
