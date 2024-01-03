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
    public class HydrantTaskTest : FileDumpTaskTestBase<IGISFileSerializer, IGISFileUploader, Hydrant, IRepository<Hydrant>, HydrantTask>
    {
        [TestMethod]
        public void TestRunDoesNotCreateEmptyFile()
        {
            var coll = new List<Hydrant>();
            _repository.Setup(x => x.Linq).Returns(coll.AsQueryable());

            _target.Run();
        }

        [TestMethod]
        public void TestRunPassesHydrantsFromRepositorySerializerAndSendsResultsToUploadService()
        {
            var yesterday = _now.AddDays(-1);
            var coll = new[] {
                new Hydrant {UpdatedAt = yesterday},
                new Hydrant {UpdatedAt = yesterday},
                new Hydrant {UpdatedAt = yesterday},
                new Hydrant {UpdatedAt = yesterday},
            }.AsQueryable();
            var str = "foo";

            _repository.Setup(x => x.Linq).Returns(coll);
            _serializer.Setup(x =>
                            x.Serialize(MyIt.ContainsAll<IQueryable<Hydrant>, Hydrant>(coll), Formatting.None))
                       .Returns(str);
            _uploadService.Setup(x => x.UploadHydrants(str));

            _target.Run();
        }

        [TestMethod]
        public void TestRunOnlyProcessesHydrantsUpdatedInTheLastDay()
        {
            var yesterday = _now.AddDays(-1);
            var coll = new[] {
                new Hydrant {UpdatedAt = yesterday},
                new Hydrant {UpdatedAt = yesterday.AddDays(-1)},
                new Hydrant {UpdatedAt = yesterday.AddDays(1)},
            }.AsQueryable();
            var str = "foo";

            _repository.Setup(x => x.Linq).Returns(coll);
            _serializer.Setup(x =>
                            x.Serialize(MyIt.ContainsTheseButNotThose<IQueryable<Hydrant>, Hydrant>(
                                new[] {coll.First()},
                                new [] {coll.ElementAt(1), coll.Last()}), Formatting.None))
                       .Returns(str);
            _uploadService.Setup(x => x.UploadHydrants(str));

            _target.Run();
        }
    }
}
