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
    public class ValveTaskTest : FileDumpTaskTestBase<IGISFileSerializer, IGISFileUploader, Valve, IRepository<Valve>, ValveTask>
    {
        [TestMethod]
        public void TestProcessDoesNotCreateEmptyFile()
        {
            var coll = new List<Valve>();
            _repository.Setup(x => x.Linq).Returns(coll.AsQueryable());

            _target.Run();
        }

        [TestMethod]
        public void TestProcessPassesValvesFromRepositorySerializerAndSendsResultsToUploadService()
        {
            var yesterday = _now.AddDays(-1);
            var coll = new[] {
                new Valve {UpdatedAt = yesterday},
                new Valve {UpdatedAt = yesterday},
                new Valve {UpdatedAt = yesterday},
                new Valve {UpdatedAt = yesterday},
            }.AsQueryable();
            var str = "foo";

            _repository.Setup(x => x.Linq).Returns(coll);
            _serializer.Setup(x =>
                            x.Serialize(MyIt.ContainsAll<IQueryable<Valve>, Valve>(coll), Formatting.None))
                       .Returns(str);
            _uploadService.Setup(x => x.UploadValves(str));

            _target.Run();
        }

        [TestMethod]
        public void TestProcessOnlyProcessesValvesUpdatedInTheLastDay()
        {
            var yesterday = _now.AddDays(-1);
            var coll = new[] {
                new Valve {UpdatedAt = yesterday},
                new Valve {UpdatedAt = yesterday.AddDays(-1)},
                new Valve {UpdatedAt = yesterday.AddDays(1)},
            }.AsQueryable();
            var str = "foo";

            _repository.Setup(x => x.Linq).Returns(coll);
            _serializer.Setup(x =>
                            x.Serialize(MyIt.ContainsTheseButNotThose<IQueryable<Valve>, Valve>(
                                new[] { coll.First() },
                                new[] { coll.ElementAt(1), coll.Last() }), Formatting.None))
                       .Returns(str);
            _uploadService.Setup(x => x.UploadValves(str));

            _target.Run();
        }
    }
}
