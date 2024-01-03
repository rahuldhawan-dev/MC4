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
    public class SewerOpeningTaskTest : FileDumpTaskTestBase<IGISFileSerializer, IGISFileUploader, SewerOpening, IRepository<SewerOpening>, SewerOpeningTask>
    {
        [TestMethod]
        public void TestProcessDoesNotCreateEmptyFile()
        {
            var coll = new List<SewerOpening>();
            _repository.Setup(x => x.Linq).Returns(coll.AsQueryable());

            _target.Run();
        }

        [TestMethod]
        public void TestProcessPassesSewerOpeningsFromRepositorySerializerAndSendsResultsToUploadService()
        {
            var yesterday = _now.AddDays(-1);
            var coll = new[] {
                new SewerOpening {UpdatedAt = yesterday},
                new SewerOpening {UpdatedAt = yesterday},
                new SewerOpening {UpdatedAt = yesterday},
                new SewerOpening {UpdatedAt = yesterday},
            }.AsQueryable();
            var str = "foo";

            _repository.Setup(x => x.Linq).Returns(coll);
            _serializer.Setup(x =>
                            x.Serialize(MyIt.ContainsAll<IQueryable<SewerOpening>, SewerOpening>(coll), Formatting.None))
                       .Returns(str);
            _uploadService.Setup(x => x.UploadSewerOpenings(str));

            _target.Run();
        }

        [TestMethod]
        public void TestProcessOnlyProcessesSewerOpeningsUpdatedInTheLastDay()
        {
            var yesterday = _now.AddDays(-1);
            var coll = new[] {
                new SewerOpening {UpdatedAt = yesterday},
                new SewerOpening {UpdatedAt = yesterday.AddDays(-1)},
                new SewerOpening {UpdatedAt = yesterday.AddDays(1)},
            }.AsQueryable();
            var str = "foo";

            _repository.Setup(x => x.Linq).Returns(coll);
            _serializer.Setup(x =>
                            x.Serialize(MyIt.ContainsTheseButNotThose<IQueryable<SewerOpening>, SewerOpening>(
                                new[] { coll.First() },
                                new[] { coll.ElementAt(1), coll.Last() }), Formatting.None))
                       .Returns(str);
            _uploadService.Setup(x => x.UploadSewerOpenings(str));

            _target.Run();
        }
    }
}
