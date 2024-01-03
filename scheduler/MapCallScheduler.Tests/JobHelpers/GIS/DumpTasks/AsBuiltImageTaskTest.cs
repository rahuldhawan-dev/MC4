using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.GIS;
using MapCallScheduler.JobHelpers.GIS.DumpTasks;
using MapCallScheduler.Tests.Library.JobHelpers.FileDumps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MapCallScheduler.Tests.JobHelpers.GIS.DumpTasks
{
    [TestClass]
    public class AsBuiltImageTaskTest : FileDumpTaskTestBase<IGISFileSerializer, IGISFileUploader, AsBuiltImage, IRepository<AsBuiltImage>, AsBuiltImageTask>
    {
        [TestMethod]
        public void TestProcessDoesNotCreateEmptyFile()
        {
            var coll = new List<AsBuiltImage>();
            _repository.Setup(x => x.Linq).Returns(coll.AsQueryable());

            _target.Run();
        }

        [TestMethod]
        public void TestProcessPassesServicesFromRepositorySerializerAndSendsResultsToUploadService()
        {
            var yesterday = _now.AddDays(-1);
            var coll = new[] {
                new AsBuiltImage {UpdatedAt = yesterday},
                new AsBuiltImage {UpdatedAt = yesterday},
                new AsBuiltImage {UpdatedAt = yesterday},
                new AsBuiltImage {UpdatedAt = yesterday},
            }.AsQueryable();
            var str = "foo";

            _repository.Setup(x => x.Linq).Returns(coll);
            _serializer.Setup(x =>
                            x.Serialize(MyIt.ContainsAll<IQueryable<AsBuiltImage>, AsBuiltImage>(coll), Formatting.None))
                       .Returns(str);
            _uploadService.Setup(x => x.UploadAsBuiltImages(str));

            _target.Run();
        }

        [TestMethod]
        public void TestProcessOnlyProcessesServicesUpdatedInTheLastDay()
        {
            var yesterday = _now.AddDays(-1);
            var coll = new[] {
                new AsBuiltImage {UpdatedAt = yesterday},
                new AsBuiltImage {UpdatedAt = yesterday.AddDays(-1)},
                new AsBuiltImage {UpdatedAt = yesterday.AddDays(1)},
            }.AsQueryable();
            var str = "foo";

            _repository.Setup(x => x.Linq).Returns(coll);
            _serializer.Setup(x =>
                            x.Serialize(MyIt.ContainsTheseButNotThose<IQueryable<AsBuiltImage>, AsBuiltImage>(
                                new[] { coll.First() },
                                new[] { coll.ElementAt(1), coll.Last() }), Formatting.None))
                       .Returns(str);
            _uploadService.Setup(x => x.UploadAsBuiltImages(str));

            _target.Run();
        }
    }
}
