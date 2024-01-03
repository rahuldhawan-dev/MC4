using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MapCall.Common.Model.Entities;
using MapCallScheduler.JobHelpers.GIS;
using MapCallScheduler.JobHelpers.GIS.ImportTasks;
using MapCallScheduler.Library.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.JobHelpers.GIS.ImportTasks
{
    [TestClass]
    public class ValveTaskTest : GISFileImportTaskTestBase<Valve, ValveTask>
    {
        #region Properties

        protected override Expression<Func<IGISFileDownloadService, IEnumerable<FileData>>> DownloadMethod
            => x => x.DownloadValveFiles();

        #endregion

        #region Private Methods

        protected override Expression<Func<IGISFileParser, IEnumerable<GISFileParser.ParsedRecord>>> SetupParseMethod(string fileContent)
        {
            return x => x.ParseValves(fileContent);
        }

        #endregion
    }
}
