using System.Collections.Generic;
using System.Linq;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Model.ViewModels;
using MapCallScheduler.JobHelpers.NonRevenueWater;
using MapCallScheduler.Tests.Library.JobHelpers.FileDumps;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.DateTimeExtensions;
using MMSINC.Testing.Moq;

namespace MapCallScheduler.Tests.JobHelpers.NonRevenueWaters
{
    [TestClass]
    public class NonRevenueWaterEntriesTaskTest : FileDumpTaskTestBase<INonRevenueWaterEntryFileSerializer,
        INonRevenueWaterEntryFileUploader, NonRevenueWaterEntryFileDumpViewModel, INonRevenueWaterEntryRepository,
        NonRevenueWaterEntryFileDumpTask>
    {
        [TestMethod]
        public void Test_Run_DoesNotCreateEmptyFile()
        {
            var lastMonth = _now.AddMonths(-1).GetBeginningOfMonth();
            var nonRevenueWaterEntryFileDumpViewModels = new List<NonRevenueWaterEntryFileDumpViewModel>();
            _repository.Setup(x => x.GetDataForNonRevenueWaterEntryFileDump(lastMonth))
                       .Returns(nonRevenueWaterEntryFileDumpViewModels.AsQueryable());

            _target.Run();
        }

        [TestMethod]
        public void Test_Run_SerializesNonRevenueWaterEntriesAndSendsToUploadService()
        {
            var lastMonth = _now.AddMonths(-1).GetBeginningOfMonth();
            var nonRevenueWaterEntryFileDumpViewModels = new[] {
                new NonRevenueWaterEntryFileDumpViewModel {
                    Year = _now.Year.ToString(), 
                    Month = _now.Month.ToString()
                },
                new NonRevenueWaterEntryFileDumpViewModel {
                    Year = _now.Year.ToString(), 
                    Month = _now.Month.ToString()
                },
                new NonRevenueWaterEntryFileDumpViewModel {
                    Year = _now.Year.ToString(), 
                    Month = _now.Month.ToString()
                },
                new NonRevenueWaterEntryFileDumpViewModel {
                    Year = _now.Year.ToString(), 
                    Month = _now.Month.ToString()
                }
            }.AsQueryable();
            var str = "foo";

            _repository.Setup(x => 
                x.GetDataForNonRevenueWaterEntryFileDump(lastMonth)).Returns(nonRevenueWaterEntryFileDumpViewModels);
            
            _serializer.Setup(x =>
                            x.Serialize(MyIt
                               .ContainsAll<IQueryable<NonRevenueWaterEntryFileDumpViewModel>,
                                    NonRevenueWaterEntryFileDumpViewModel>(nonRevenueWaterEntryFileDumpViewModels)))
                       .Returns(str);
            _uploadService.Setup(x => x.UploadNonRevenueWaterEntries(str));

            _target.Run();
        }
    }
}
