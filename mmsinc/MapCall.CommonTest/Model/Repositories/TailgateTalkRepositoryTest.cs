using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Mappings;
using MapCall.Common.Model.Repositories;
using MapCall.Common.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NHibernate.Linq;
using StructureMap;
using System.Linq;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;

namespace MapCall.CommonTest.Model.Repositories
{
    [TestClass]
    public class TailgateTalkRepositoryTest : MapCallMvcInMemoryDatabaseTestBase<TailgateTalk, TailgateTalkRepository>
    {
        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
        }

        #endregion

        [TestMethod]
        public void TestDeleteCascadesDeletesForLinkedEmployees()
        {
            var dataType = GetEntityFactory<DataType>().Create(new {
                TableName = TailgateTalkMap.TABLE_NAME, Name = TailgateTalk.DATA_TYPE_NAME
            });
            var employee = GetEntityFactory<Employee>().Create();
            var tailgateTalk = GetEntityFactory<TailgateTalk>().Create();
            var employeeLink = GetEntityFactory<EmployeeLink>().Create(new {
                Employee = employee,
                LinkedId = tailgateTalk.Id,
                DataType = dataType
            });
            Session.Flush();
            Session.Clear();
            tailgateTalk = Session.Load<TailgateTalk>(tailgateTalk.Id);
            //Sanity Check. Make sure it's adding the employee record.
            Assert.AreEqual(tailgateTalk.LinkedEmployees.First().Employee.Id, employee.Id);

            Repository.Delete(tailgateTalk);

            employeeLink = Session.Query<EmployeeLink>().SingleOrDefault(x => x.Id == employeeLink.Id);

            Assert.IsNull(employeeLink);
        }
    }
}
