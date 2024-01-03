using System.Linq;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Data;
using MMSINC.Data.NHibernate;
using MMSINC.Testing.NHibernate;

namespace MapCall.Common.Testing
{
    public static class ActionItemLinkedThingRepositoryTestExtensions
    {
        public static void TestActionItemIsDeletedWithThing<TThing, TRepository>(
            this InMemoryDatabaseTest<TThing, TRepository> that, string tableName)
            where TThing : class, IEntity, IThingWithActionItems, new()
            where TRepository : class, IRepository<TThing>
        {
            var dataType = that.GetEntityFactory<DataType>().Create(new {TableName = tableName});

            var thing = that.GetEntityFactory<TThing>().Create();
            var actionItem = that.GetEntityFactory<ActionItem>().Create(new {DataType = dataType, LinkedId = thing.Id});

            that.Session.Flush();
            that.Session.Clear();

            //Reload so you have it and its action item:
            thing = that.Session.Load<TThing>(thing.Id);

            Assert.AreEqual(1, thing.LinkedActionItems.Count);
            Assert.AreEqual(actionItem.Id, thing.LinkedActionItems.Single().ActionItem.Id);

            that.Repository.Delete(thing);

            Assert.IsNull(that.Session.Get<ActionItem>(actionItem.Id));
        }
    }
}
