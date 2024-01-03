using System;
using System.Data;
using System.Diagnostics;
using System.Text;
using MapCallImporter.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using MMSINC.Data;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Utilities.ValueRanges;

namespace MapCallImporter.Library.Testing
{
    public abstract class ExcelRecordTestBase<TEntity, TViewModel, TExcelRecord> : MapCallImporterInMemoryDatabaseTestBase
        where TEntity : class, new()
        where TViewModel : ViewModel<TEntity>
        where TExcelRecord : ExcelRecordBase<TEntity, TViewModel, TExcelRecord>
    {
        #region Private Members

        protected ExcelRecordItemHelperBase<TEntity> _mappingHelper;
        protected TExcelRecord _target;

        #endregion

        #region Properties

        public ExcelRecordItemHelperBase<TEntity> MappingHelper => _mappingHelper;

        #endregion

        #region Private Methods

        [TestInitialize]
        public void ExcelRecordTestBaseTestInitialize()
        {
            ImportTestData();

            _mappingHelper = Container.GetInstance<TestExcelRecordItemValidationHelper<TEntity, TViewModel, TExcelRecord>>();
            _target = CreateTarget();
        }

        protected virtual string GenerateIntString(int length)
        {
            var sb = new StringBuilder(length);
            new IntValueRange(0, length - 1).Each(i => sb.Append((i >= 10 ? i % 10 : i).ToString()));
            return sb.ToString();
        }

        protected virtual void ImportTestData() {}

        #endregion

        #region Abstract Methods

        protected abstract TExcelRecord CreateTarget();

        protected abstract void InnerTestMappings(
            ExcelRecordMappingTester<TEntity, TViewModel, TExcelRecord> test);

        #endregion

        #region Exposed Methods

        [TestMethod]
        public virtual void TestMappings()
        {
            WithMappingTester(t => {
                InnerTestMappings(t);
                t.AssertCountsMatch();
            });
        }

        [DebuggerStepThrough]
        public void ExpectMappingFailure(Action fn, string message = null)
        {
            MyAssert.Throws<DataException>(fn, message);
        }

        [DebuggerStepThrough]
        public void ExpectNoMappingFailure(Action fn, string message = null)
        {
            MyAssert.DoesNotThrow(fn, message);
        }

        [DebuggerStepThrough]
        public void WithMappingTester(Action<ExcelRecordMappingTester<TEntity, TViewModel, TExcelRecord>> fn)
        {
            fn(new ExcelRecordMappingTester<TEntity, TViewModel, TExcelRecord>(this, _target));
        }

        #endregion
    }
}