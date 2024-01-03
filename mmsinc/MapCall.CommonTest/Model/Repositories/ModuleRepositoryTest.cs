using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using NHibernate;
using NHibernate.Criterion;
using StructureMap;

namespace MapCall.CommonTest.Model.Repositories
{
    /// <summary>
    /// Summary description for ModuleRepositoryTest
    /// </summary>
    [TestClass]
    public class ModuleRepositoryTest
    {
        #region Private Members

        private Mock<ISession> _sessionMock;
        private Mock<ICriteria> _criteriaMock;
        private TestModuleRepository _target;

        #endregion

        #region Setup/Teardown

        [TestInitialize]
        public void NotificationConfigurationTestInitialize()
        {
            _sessionMock = new Mock<ISession>();
            _criteriaMock = new Mock<ICriteria>();
            _target =
                new TestModuleRepository(
                    _sessionMock.Object, null, _criteriaMock.Object);
        }

        #endregion

        [TestMethod]
        public void TestFindByApplicationAndModuleNameDoesWhatItSaysOnTheTin()
        {
            var applicationCriteriaMock = new Mock<ICriteria>();
            var expected = new Module();

            _target.NameMatchesCriterion =
                new Mock<AbstractCriterion>().Object;
            _target.ApplicationNameMatchesCriterion =
                new Mock<AbstractCriterion>().Object;

            _criteriaMock
               .Setup(x => x.Add(_target.NameMatchesCriterion))
               .Returns(_criteriaMock.Object);
            _criteriaMock
               .Setup(x => x.CreateCriteria("Application"))
               .Returns(applicationCriteriaMock.Object);
            applicationCriteriaMock
               .Setup(x => x.Add(_target.ApplicationNameMatchesCriterion))
               .Returns(applicationCriteriaMock.Object);
            applicationCriteriaMock
               .Setup(x => x.UniqueResult<Module>())
               .Returns(expected);

            var actual = _target.FindByApplicationAndModuleName(null, null);

            _criteriaMock.VerifyAll();
            applicationCriteriaMock.VerifyAll();

            Assert.AreSame(expected, actual);
        }
    }

    public class TestModuleRepository : ModuleRepository
    {
        #region Private Members

        private readonly ICriteria _criteria;

        #endregion

        #region Properties

        public override ICriteria Criteria => _criteria ?? base.Criteria;

        public AbstractCriterion NameMatchesCriterion;
        public AbstractCriterion ApplicationNameMatchesCriterion;

        #endregion

        #region Constructors

        public TestModuleRepository(ISession session, IContainer container, ICriteria criteria) : base(session,
            container)
        {
            _criteria = criteria;
        }

        #endregion

        #region Exposed Methods

        public override AbstractCriterion GetApplicationNameMatchesCriterion(string applicationName)
        {
            return ApplicationNameMatchesCriterion ??
                   base.GetApplicationNameMatchesCriterion(applicationName);
        }

        public override AbstractCriterion GetNameMatchesCriterion(string moduleName)
        {
            return NameMatchesCriterion ??
                   base.GetNameMatchesCriterion(moduleName);
        }

        #endregion
    }
}
