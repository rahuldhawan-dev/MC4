using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class OpeningConditionRepositoryTest
        : InMemoryDatabaseTest<OpeningCondition, OpeningConditionRepository>
    {
        #region Private Members

        private OpeningCondition _active, _inactive;
        private IOpeningConditionRepository _target;
        
        #endregion
        
        #region Init/Cleanup

        [TestInitialize]
        public void TestInitialize()
        {

            _active = GetEntityFactory<OpeningCondition>().Create(new { IsActive = true });
            _inactive = GetEntityFactory<OpeningCondition>().Create(new { IsActive = false });
            _target = _container.GetInstance<OpeningConditionRepository>();
        }

        #endregion

        [TestMethod]
        public void Test_Linq_FiltersOutInactiveConditions()
        {
            var result = _target.Linq.ToList();
            
            CollectionAssert.Contains(result, _active);
            CollectionAssert.DoesNotContain(result, _inactive);
        }

        [TestMethod]
        public void Test_Criteria_FiltersOutInactiveConditions()
        {
            var result = _target.Criteria.List();
            
            CollectionAssert.Contains(result, _active);
            CollectionAssert.DoesNotContain(result, _inactive);
        }
    }
}
