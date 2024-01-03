using System.Collections.Generic;
using System.IO;
using System.Linq;
using MapCallScheduler.Library.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using StructureMap;

namespace MapCallScheduler.Tests.Library.JobHelpers.Sap
{
    [DeploymentItem(@"TestData\", "TestData")]
    public abstract class SapFileParserTestBase<TTarget, TFileRecord>
        where TTarget : IFileParser<TFileRecord>
        where TFileRecord : new()
    {
        #region Private Members

        protected TTarget _target;
        protected IContainer _container;

        #endregion

        #region Abstract Properties

        protected abstract string SampleFile { get; }
        protected abstract IEnumerable<TFileRecord> SampleRecords { get; }

        #endregion

        #region Abstract Methods

        protected abstract void CompareResult(TFileRecord sampleRecord, TFileRecord resultRecord);

        #endregion

        #region Exposed Methods

        [TestInitialize]
        public virtual void TestInitialize()
        {
            _container = new Container();
            _target = _container.GetInstance<TTarget>();
        }

        [TestMethod]
        public void TestParserParsesFileCorrectly()
        {
            TFileRecord[] results;
            using (var infile = File.OpenRead(SampleFile))
            {
                results = _target.Parse(new FileData(SampleFile, infile.ReadString())).ToArray();
            }

            Assert.AreEqual(SampleRecords.Count(), results.Length);
            SampleRecords.EachWithIndex((r, i) => CompareResult(r, results[i]));
        }

        #endregion
    }
}
