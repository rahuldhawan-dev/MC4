using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.GIS;
using MapCallScheduler.JobHelpers.GIS.DumpTasks;
using MapCallScheduler.Tests.Library.JobHelpers.FileDumps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.Moq;
using Newtonsoft.Json;

namespace MapCallScheduler.Tests.JobHelpers.GIS.DumpTasks
{
    [TestClass]
    public class ServiceTaskTest
        : FileDumpTaskTestBase<
            IGISFileSerializer,
            IGISFileUploader,
            MostRecentlyInstalledService,
            IRepository<MostRecentlyInstalledService>,
            ServiceTask>
    {
        [TestMethod]
        public void TestProcessDoesNotCreateEmptyFile()
        {
            var coll = new List<MostRecentlyInstalledService>();
            _repository.Setup(x => x.Linq).Returns(coll.AsQueryable());

            _target.Run();
        }

        [TestMethod]
        public void TestProcessPassesServicesFromRepositorySerializerAndSendsResultsToUploadService()
        {
            var twentyfourhoursago = _now.AddDays(-1);
            var coll = new[] {
                new MostRecentlyInstalledService { UpdatedAt = twentyfourhoursago, Service = new Service() },
                new MostRecentlyInstalledService { UpdatedAt = twentyfourhoursago, Service = new Service() },
                new MostRecentlyInstalledService { UpdatedAt = twentyfourhoursago, Service = new Service() },
                new MostRecentlyInstalledService { UpdatedAt = twentyfourhoursago, Service = new Service() },
                new MostRecentlyInstalledService { UpdatedAt = twentyfourhoursago.AddSeconds(-1), Service = new Service() }
            }.AsQueryable();
            // expected results should not include anything prior to 24 hours ago
            var expectedResults = coll.Where(x => x.UpdatedAt >= twentyfourhoursago);
            var str = "foo";

            _repository.Setup(x => x.Linq).Returns(coll);
            _serializer.Setup(x => x.Serialize(MyIt.ContainsAll<IQueryable<MostRecentlyInstalledService>, MostRecentlyInstalledService>(expectedResults), Formatting.None))
                       .Returns(str);
            _uploadService.Setup(x => x.UploadServices(str));

            _target.Run();
        }

        [TestMethod]
        public void TestProcessOnlyProcessesServicesUpdatedInTheLastDay()
        {
            var yesterday = _now.AddDays(-1);
            var coll = new[] {
                new MostRecentlyInstalledService { UpdatedAt = yesterday, Service = new Service() },
                new MostRecentlyInstalledService { UpdatedAt = yesterday.AddDays(-1), Service = new Service() },
                new MostRecentlyInstalledService { UpdatedAt = yesterday.AddDays(1), Service = new Service() },
            }.AsQueryable();
            var str = "foo";

            _repository.Setup(x => x.Linq).Returns(coll);
            _serializer.Setup(x =>
                           x.Serialize(MyIt.ContainsTheseButNotThose<IQueryable<MostRecentlyInstalledService>, MostRecentlyInstalledService>(
                               new[] { coll.First() },
                               new[] { coll.ElementAt(1), coll.Last() }), Formatting.None))
                       .Returns(str);
            _uploadService.Setup(x => x.UploadServices(str));

            _target.Run();
        }
    }
}
