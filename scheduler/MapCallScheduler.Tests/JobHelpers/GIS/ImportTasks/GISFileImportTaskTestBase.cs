using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.GIS;
using MapCallScheduler.JobHelpers.GIS.ImportTasks;
using MapCallScheduler.Library.Common;
using MapCallScheduler.Tests.Library.JobHelpers.FileImports;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.MSTest.TestExtensions;
using Moq;

namespace MapCallScheduler.Tests.JobHelpers.GIS.ImportTasks
{
    public abstract class GISFileImportTaskTestBase<TEntity, TTask>
        : FileImportTaskTestBase<
            IGISFileParser,
            IGISFileDownloadService,
            TEntity,
            TTask>
        where TEntity : class, IThingWithCoordinate, new()
        where TTask : IDailyGISFileImportTask
    {
        #region Abstract Methods

        protected abstract Expression<Func<IGISFileParser, IEnumerable<GISFileParser.ParsedRecord>>>
            SetupParseMethod(string fileContent);

        #endregion

        #region Exposed Methods

        [TestMethod]
        public void TestRunDownloadsParsesMapsAndSavesEntityRecordsAndThenDeletesFiles()
        {
            var fileName = "fileName";
            var fileContent = "fileContent";

            _downloadService.Setup(DownloadMethod).Returns(new List<FileData> {
                new FileData(fileName, fileContent)
            });
            _parser.Setup(SetupParseMethod(fileContent)).Returns(new List<GISFileParser.ParsedRecord> {
                new GISFileParser.ParsedRecord {Id = 1, Latitude = 2m, Longitude = 3m},
                new GISFileParser.ParsedRecord {Id = 2, Latitude = 3m, Longitude = 4m},
                new GISFileParser.ParsedRecord {Id = 3, Latitude = 4m, Longitude = 5m},
            });
            for (var i = 1; i < 4; ++i)
            {
                var cur = i;
                _repository.Setup(x => x.Find(cur)).Returns(new TEntity { Id = cur });
                _repository.Setup(x => x.Update(It.Is<TEntity>(h =>
                    h.Id == cur &&
                    h.Coordinate.Latitude == (cur + 1) &&
                    h.Coordinate.Longitude == (cur + 2))));
            }
            _downloadService.Setup(x => x.DeleteFile(fileName));
            
            _target.Run();
        }

        [TestMethod]
        public void TestRunThrowsWhenEntityCannotBeFoundById()
        {
            var fileContent = "fileContent";

            _downloadService.Setup(DownloadMethod).Returns(new List<FileData> {
                new FileData("foo", fileContent)
            });
            _parser.Setup(SetupParseMethod(fileContent)).Returns(new List<GISFileParser.ParsedRecord> {
                new GISFileParser.ParsedRecord {Id = 1, Latitude = 2m, Longitude = 3m},
            });
            _repository.Setup(x => x.Find(1)).Returns(default(TEntity));

            MyAssert.Throws<InvalidOperationException>(() => _target.Run());
        }

        [TestMethod]
        public void TestRunDoesNotGenerateExceptionWhenFileDownloadServiceReturnsNull()
        {
            _downloadService.Setup(DownloadMethod).Returns((IEnumerable<FileData>)null);

            MyAssert.DoesNotThrow<NullReferenceException>(() => _target.Run());
        }

        #endregion
    }
}
