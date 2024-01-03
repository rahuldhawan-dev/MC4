using System;
using System.Collections.Generic;
using Historian.Data.Client.Entities;
using Historian.Data.Client.Repositories;
using MapCallScheduler.JobHelpers.SpaceTimeInsight;
using MapCallScheduler.JobHelpers.SpaceTimeInsight.Tasks;
using MapCallScheduler.Tests.Library.JobHelpers.FileDumps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using Moq;
using Newtonsoft.Json;
using NHibernate.Util;
using StructureMap;

namespace MapCallScheduler.Tests.JobHelpers.SpaceTimeInsight.Tasks
{
    [TestClass]
    public class InterconnectTaskTest : FileDumpTaskTestBase<ISpaceTimeInsightJsonFileSerializer, ISpaceTimeInsightFileUploadService, RawData, IRawDataRepository, InterconnectTask>
    {
        [TestMethod]
        public void TestProcessDoesNotCreateEmptyFile()
        {
            var coll = new List<RawData>();
            var str = "foo";
            var yesterday = _now.AddDays(-1).Date;

            foreach (var tagName in InterconnectTask.TAG_NAMES)
            {
                _repository
                    .Setup(r => r.FindByTagName(tagName, false, yesterday, yesterday.EndOfDay()))
                    .Returns(coll);
            }

            _target.Run();
        }

        [TestMethod]
        public void TestProcessPassesRawDatumsFromRepositoryToJsonSerializerAndSendsResultsToUploadService()
        {
            var coll = new[] {
                new List<RawData> {new RawData {TagName = InterconnectTask.TAG_NAMES[0], TimeStamp = DateTime.Now } },
                new List<RawData> {new RawData {TagName = InterconnectTask.TAG_NAMES[1], TimeStamp = DateTime.Now.AddHours(1) } },
                new List<RawData> {new RawData {TagName = InterconnectTask.TAG_NAMES[2], TimeStamp = DateTime.Now.AddHours(2) } },
                new List<RawData> {new RawData {TagName = InterconnectTask.TAG_NAMES[3], TimeStamp = DateTime.Now.AddHours(3) } }
            };
            var str = "foo";
            var yesterday = _now.AddDays(-1).Date;
            _target.ExpectedCount = 4;

            InterconnectTask.TAG_NAMES.EachWithIndex((tagName, idx) =>
            {
                _repository
                    .Setup(r => r.FindByTagName(tagName, false, yesterday, yesterday.EndOfDay()))
                    .Returns(coll[idx]);
            });
            _serializer.Setup(u => u.SerializeInterconnectData(It.IsAny<IEnumerable<RawData>>(), Formatting.None)).Returns<IEnumerable<RawData>, Formatting>(
                (c, formatting) =>
                {
                    for (var i = 0; i < coll.Length; ++i)
                    {
                        MyAssert.Contains(c, coll[i].First());
                    }
                    return str;
                });
            _uploadService.Setup(u => u.UploadInterconnectData(str));

            _target.Run();
        }

        [TestMethod]
        public void TestProcessPassesRawDatumsFromRepositoryToJsonSerializerAndDoesNotSendResultsToUploadServiceWhenCountIsIncorrect()
        {
            var coll = new[] {
                new List<RawData> {new RawData {TagName = InterconnectTask.TAG_NAMES[0], TimeStamp = DateTime.Now } },
                new List<RawData> {new RawData {TagName = InterconnectTask.TAG_NAMES[1], TimeStamp = DateTime.Now.AddHours(1) } },
                new List<RawData> {new RawData {TagName = InterconnectTask.TAG_NAMES[2], TimeStamp = DateTime.Now.AddHours(2) } },
                new List<RawData> {new RawData {TagName = InterconnectTask.TAG_NAMES[3], TimeStamp = DateTime.Now.AddHours(3) } }
            };
            var str = "foo";
            var yesterday = _now.AddDays(-1).Date;

            InterconnectTask.TAG_NAMES.EachWithIndex((tagName, idx) =>
            {
                _repository
                    .Setup(r => r.FindByTagName(tagName, false, yesterday, yesterday.EndOfDay()))
                    .Returns(coll[idx]);
            });

            _target.Run();

            _serializer.Verify(u => u.SerializeInterconnectData(It.IsAny<IEnumerable<RawData>>(), Formatting.None), Times.Never);
            _uploadService.Verify(u => u.UploadInterconnectData(str), Times.Never);
        }
    }
}