using System;
using JetBrains.Annotations;
using MMSINC.ClassExtensions.BooleanExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rhino.Mocks;

namespace MMSINC.CoreTest.ClassExtensions
{
    /// <summary>
    /// Summary description for BooleanExtensionsTest
    /// </summary>
    [TestClass]
    public class BooleanExtensionsTest
    {
        #region Private Members

        private ICustomFormatter _customFormatter;
        private IFormatProvider _formatProvider;
        private MockRepository _mocks;
        private bool _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void BooleanExtensionsTestInitialize()
        {
            _mocks = new MockRepository();
            _mocks
               .DynamicMock(out _formatProvider)
               .DynamicMock(out _customFormatter);

            _target = true;
        }

        [TestCleanup]
        public void BooleanExtensionsTestCleanup()
        {
            _mocks.VerifyAll();
        }

        #endregion

        [TestMethod]
        public void TestDefaultFormatProviderIsABooleanFormatProvider()
        {
            _mocks.ReplayAll();

            Assert.IsInstanceOfType(BooleanExtensions.FormatProviderFactory(),
                typeof(BooleanFormatProvider));
        }

        [TestMethod]
        public void TestResetFormatProviderFactoryResetsFormatProviderFactoryLambdaToReturnBooleanFormatProvider()
        {
            _mocks.ReplayAll();

            BooleanExtensions.SetFormatProviderFactory(() => null);
            BooleanExtensions.ResetFormatProviderFactory();

            Assert.IsInstanceOfType(BooleanExtensions.FormatProviderFactory(),
                typeof(BooleanFormatProvider));
        }

        [TestMethod]
        public void TestSetFormatProviderFactoryInjectsMethodToReturnIFormattable()
        {
            _mocks.ReplayAll();

            BooleanExtensions.SetFormatProviderFactory(() => _formatProvider);

            Assert.AreSame(_formatProvider, BooleanExtensions.FormatProviderFactory());

            BooleanExtensions.ResetFormatProviderFactory();
        }

        [TestMethod]
        public void TestToStringUsesFormatProviderFactoryToFormatAsStringWhenFormatStringArgumentMatches()
        {
            _mocks.CreateMock(out _formatProvider);
            BooleanExtensions.SetFormatProviderFactory(() => _formatProvider);

            using (_mocks.Record())
            {
                SetupResult.For(_formatProvider.GetFormat(null))
                           .Return(_customFormatter);
                LastCall.IgnoreArguments();
            }

            using (_mocks.Playback())
            {
                _target.ToString("yn");
            }

            BooleanExtensions.ResetFormatProviderFactory();
        }

        [TestMethod]
        public void TestToStringDoesNotUseFormatProviderFactoryToFormatAsStrignWhenFormatStringArgumentDoesNotMatch()
        {
            _mocks.ReplayAll();

            Assert.AreEqual(true.ToString(), true.ToString(String.Empty));
            Assert.AreEqual(false.ToString(), false.ToString(String.Empty));
        }

        [TestMethod]
        public void TestIsNullOrFalseWorksAsExpected()
        {
            _mocks.ReplayAll();

            Assert.IsTrue(((bool?)null).IsNullOrFalse());
            Assert.IsTrue(((bool?)false).IsNullOrFalse());
            Assert.IsFalse(((bool?)true).IsNullOrFalse());
        }
    }

    [TestClass]
    public class BooleanFormatProviderTest
    {
        #region Private Members

        private BooleanFormatProvider _target;

        #endregion

        #region Additional Test Attributes

        [TestInitialize]
        public void BooleanFormatProviderTestInitialize()
        {
            _target = new BooleanFormatProvider();
        }

        #endregion

        [TestMethod]
        public void TestGetFormatReturnsSelfWhenArgumentIsTypeOfICustomFormatter()
        {
            Assert.AreSame(_target, _target.GetFormat(typeof(ICustomFormatter)));
        }

        [TestMethod]
        public void TestGetFormatReturnsNullWhenArgumentIsNotTypeOfICustomFormatter()
        {
            Assert.IsNull(_target.GetFormat(null));
            Assert.IsNull(_target.GetFormat(typeof(IFormatProvider)));
        }

        [TestMethod]
        public void TestFormatReturnsYesWhenFormatIsYNAndArgIsTrue()
        {
            Assert.AreEqual(BooleanFormatProvider.FormatStrings.YES,
                _target.Format(BooleanFormatProvider.FormatStrings.YES_OR_NO,
                    true, null));
        }

        [TestMethod]
        public void TestFormatReturnsNoWhenFormatIsYNAndArgIsFalse()
        {
            Assert.AreEqual(BooleanFormatProvider.FormatStrings.NO,
                _target.Format(BooleanFormatProvider.FormatStrings.YES_OR_NO,
                    false, null));
        }
    }
}
