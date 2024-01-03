using System;
using System.Linq;
using MapCallScheduler.Library.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IEnumerableExtensions;
using StructureMap;

namespace MapCallScheduler.Tests.Library.Common
{
    public abstract class TaskServiceTestBase<TTaskService, TTask>
        where TTaskService : TaskServiceBase<TTask>
    {
        #region Private Members

        protected TTaskService _target;
        protected IContainer _container;

        #endregion

        #region Abstract Properties

        protected abstract Type[] ExpectedTaskTypes { get; }

        #endregion

        #region Private Methods

        protected virtual TTaskService InitializeTarget()
        {
            return _container.GetInstance<TTaskService>();
        }

        #endregion

        #region Abstract Methods

        protected abstract void InitializeContainer(ConfigurationExpression e);

        #endregion

        #region Exposed Methods

        [TestInitialize]
        public void TestInitialize()
        {
            _container = new Container(InitializeContainer);

            _target = InitializeTarget();
        }

        [TestMethod]
        public void TestGetAllTasksReturnsAppropriateTasks()
        {
            var result = _target.GetAllTasks().ToArray();
            var expectedTypes = ExpectedTaskTypes;

            Assert.AreEqual(expectedTypes.Length, result.Length);

            expectedTypes.Each(t => Assert.IsTrue(result.Any(r => r.GetType() == t)));
        }

        #endregion
    }
}
