using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.SpaceTimeInsight;
using MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks;
using MapCallScheduler.Tests.Library.JobHelpers.FileDumps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data.NHibernate;
using Moq;
using Newtonsoft.Json;

namespace MapCallScheduler.Tests.JobHelpers.SpaceTimeInsight.Tasks
{
    [TestClass]
    public class HydrantInspectionTaskTest : FileDumpTaskTestBase<ISpaceTimeInsightJsonFileSerializer, ISpaceTimeInsightFileUploadService, HydrantInspection, IRepository<HydrantInspection>, HydrantInspectionTask>
    {
        [TestMethod]
        public void TestProcessDoesNotCreateEmptyFile()
        {
            var coll = new List<HydrantInspection>();
            var str = "foo";
            _repository.Setup(r => r.Where(It.IsAny<Expression<Func<HydrantInspection, bool>>>())).Returns(coll.AsQueryable());

            _target.Run();
        }

        [TestMethod]
        public void TestProcessPassesHydrantInspectionsWithLostWaterFromRepositoryToJsonSerializerAndSendsResultsToUploadService()
        {
            var coll = new List<HydrantInspection> {new HydrantInspection()};
            var str = "foo";
            _repository.Setup(r => r.Where(It.IsAny<Expression<Func<HydrantInspection, bool>>>())).Returns(coll.AsQueryable());
            _serializer.Setup(u => u.SerializeHydrantInspections(coll, Formatting.None)).Returns(str);
            _uploadService.Setup(u => u.UploadHydrantInspections(str));

            _target.Run();
        }
    }
}