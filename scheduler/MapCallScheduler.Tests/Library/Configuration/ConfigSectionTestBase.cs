using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCallScheduler.Tests.Library.Configuration
{
    public abstract class ConfigSectionTestBase
    {
        protected TestConfiguration _target;

        [TestInitialize]
        public virtual void TestInitialize()
        {
            _target = new TestConfiguration();
        }
    }
}