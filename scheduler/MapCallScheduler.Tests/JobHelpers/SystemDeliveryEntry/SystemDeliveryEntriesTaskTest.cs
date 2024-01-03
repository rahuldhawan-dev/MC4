using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCallScheduler.JobHelpers.SystemDeliveryEntry.DumpTasks;
using MapCallScheduler.Tests.Library.JobHelpers.FileDumps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Testing.Moq;
using Moq;
using Entry = MapCall.Common.Model.Entities.SystemDeliveryEntry;

namespace MapCallScheduler.Tests.JobHelpers.SystemDeliveryEntry
{
    [TestClass]
    public class SystemDeliveryEntriesTaskTest : FileDumpTaskTestBase<ISystemDeliveryEntryFileSerializer, ISystemDeliveryEntryFileUploader, SystemDeliveryEntryFileDumpViewModel, ISystemDeliveryEntryRepository, SystemDeliveryEntryFileDumpTask>
    {
        private readonly int[] _includedStates = {
            State.Indices.CA,
            State.Indices.HI,
            State.Indices.IA,
            State.Indices.IL,
            State.Indices.IN,
            State.Indices.KY,
            State.Indices.MD,
            State.Indices.MO,
            State.Indices.NJ,
            State.Indices.PA,
            State.Indices.TN,
            State.Indices.VA,
            State.Indices.WV
        };
        
        [TestMethod]
        public void TestRunDoesNotCreateEmptyFile()
        {
            var lastMonth = _now.AddMonths(-1).GetBeginningOfMonth();
            var coll = new List<SystemDeliveryEntryFileDumpViewModel>();
            _repository.Setup(x => x.GetDataForSystemDeliveryEntryFileDump(lastMonth, _includedStates))
                       .Returns(coll.AsQueryable());

            _target.Run();
        }

        [TestMethod]
        public void TestRunPassesSystemDeliveryEntriesFromRepositorySerializerAndSendsResultsToUploadService()
        {
            var lastMonth = _now.AddMonths(-1).GetBeginningOfMonth();
            var coll = new[] {
                new SystemDeliveryEntryFileDumpViewModel {Year = _now.Year.ToString()},
                new SystemDeliveryEntryFileDumpViewModel {Year = _now.Year.ToString()},
                new SystemDeliveryEntryFileDumpViewModel {Year = _now.Year.ToString()},
                new SystemDeliveryEntryFileDumpViewModel {Year = _now.Year.ToString()},
            }.AsQueryable();
            var str = "foo";

            _repository.Setup(x => x.GetDataForSystemDeliveryEntryFileDump(lastMonth, _includedStates)).Returns(coll);
            _serializer.Setup(x =>
                            x.Serialize(MyIt.ContainsAll<IQueryable<SystemDeliveryEntryFileDumpViewModel>, SystemDeliveryEntryFileDumpViewModel>(coll)))
                       .Returns(str);
            _uploadService.Setup(x => x.UploadSystemDeliveryEntries(str));

            _target.Run();
        }

        [TestMethod]
        public void TestRunUpdatesIsHyperionFileCreatedPropertyToTrue()
        {
            var lastMonth = _now.AddMonths(-1).GetBeginningOfMonth();
            var coll = new[] {
                new SystemDeliveryEntryFileDumpViewModel {
                    Year = _now.Year.ToString(), SystemDeliveryEntryId = 1
                }
            }.AsQueryable();
            var str = "foo";
            var entryIds = new[] { 1 }.AsQueryable();
            var systemDeliveryEntry = new Entry {
                Id = 1, WeekOf = _now, IsHyperionFileCreated = false
            };

            _repository.Setup(x => x.GetDataForSystemDeliveryEntryFileDump(lastMonth, _includedStates)).Returns(coll);
            _serializer.Setup(x => x.Serialize(MyIt
                           .ContainsAll<IQueryable<SystemDeliveryEntryFileDumpViewModel>,
                                SystemDeliveryEntryFileDumpViewModel>(coll)))
                       .Returns(str);
            _uploadService.Setup(x => x.UploadSystemDeliveryEntries(str));
            _repository.Setup(x => x.GetEntryIds(lastMonth, _includedStates)).Returns(entryIds);
            _repository.Setup(x => x.Find(systemDeliveryEntry.Id)).Returns(systemDeliveryEntry);
            _repository.Setup(x =>
                x.Save(It.Is<Entry>(e => e.IsHyperionFileCreated == true)));

            _target.Run();
        }
    }
}
