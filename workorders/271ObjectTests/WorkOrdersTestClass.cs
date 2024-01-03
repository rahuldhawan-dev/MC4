using System.Web.Mvc;
using MMSINC.Interface;
using MMSINC.Testing.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Utilities.StructureMap;
using StructureMap;
using WorkOrders.Model;

namespace _271ObjectTests
{
    [TestClass]
    public abstract class WorkOrdersTestClass<TUnitType> : LinqUnitTestClass<TUnitType>
        where TUnitType : class, new()
    {
        private IContainer _container;

        [TestInitialize]
        public override void WorkOrdersModelTestInitialize()
        {
            base.WorkOrdersModelTestInitialize();
            _container = new Container();
            _container.Inject<IDataContext>(new WorkOrdersDataContext());
            DependencyResolver.SetResolver(
                new StructureMapDependencyResolver(_container));
        }
    }

    public interface IWorkOrderDependentObjectTest
    {
        #region Methods

        void TestCannotSaveWithoutWorkOrder();

        void TestCannotChangeWorkOrderAfterSave();

        #endregion
    }
}
