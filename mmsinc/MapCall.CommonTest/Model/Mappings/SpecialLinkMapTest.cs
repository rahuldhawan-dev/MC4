using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using MapCall.Common.Testing.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.ClassExtensions.TypeExtensions;
using MMSINC.Testing.MSTest.TestExtensions;
using MMSINC.Testing.NHibernate;
using MMSINC.Utilities.Documents;
using NHibernate.Exceptions;
using NHibernate.Persister.Entity;
using StructureMap;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using MapCall.Common.Model.Repositories;
using MMSINC.Authentication;
using MMSINC.Testing.ClassExtensions;
using MMSINC.Testing.Utilities;
using MMSINC.Utilities;
using MMSINC.Utilities.Pdf;
using NHibernate;

namespace MapCall.CommonTest.Model.Mappings
{
    [TestClass]
    public class SpecialLinkMapTest : InMemoryDatabaseTest<User>
    {
        private Assembly _entityAssembly;

        #region Init/Cleanup

        protected override void InitializeObjectFactory(ConfigurationExpression e)
        {
            base.InitializeObjectFactory(e);
            e.For<IDocumentService>().Singleton().Use<InMemoryDocumentService>();
            e.For<IIconSetRepository>().Use<IconSetRepository>();
            e.For<IDateTimeProvider>().Use<TestDateTimeProvider>();
            e.For<IServiceRepository>().Use<ServiceRepository>();
            e.For<IAuthenticationService<User>>().Mock();
            e.For<ITapImageRepository>().Use<TapImageRepository>();
            e.For<IImageToPdfConverter>().Mock();
        }

        [TestInitialize]
        public void TestInitialize()
        {
            _container.GetInstance<ISession>();
        }

        protected Assembly EntityAssembly => _entityAssembly ?? (_entityAssembly = typeof(Town).Assembly);

        protected override ITestDataFactoryService CreateFactoryService()
        {
            return new TestDataFactoryService(_container, typeof(TownFactory).Assembly);
        }

        #endregion

        public Type[] SkipDocumentClasses
        {
            get
            {
                return new[] {
                    // relies on a discriminator value that's difficult to setup for
                    typeof(JobSiteSafetyAnalysis), 
                    // This errors when the test tries to delete a User because User.AggregateRoles
                    // is a view. User records can't be deleted in practice, anyway.
                    typeof(User) 
                };
            }
        }

        public Type[] SkipNoteClasses
        {
            get { return SkipDocumentClasses; }
        }

        [TestMethod]
        public void TestDocumentLinkedThing()
        {
            foreach (var type in EntityAssembly.GetTypes()
                                               .Where(t => t.Implements<IThingWithDocuments>() &&
                                                           !SkipDocumentClasses.Contains(t)))
            {
                dynamic entityFactory = GetEntityFactory(type);
                var tableName =
                    ((ILockable)
                        Session.GetSessionImplementation().GetEntityPersister(null, Activator.CreateInstance(type)))
                   .RootTableName;
                var document = GetEntityFactory<Document>().Create();
                var dataType = GetEntityFactory<DataType>().Create(new {
                    TableName = tableName
                });
                var documentType = GetEntityFactory<DocumentType>().Create(new {
                    DataType = dataType,
                    Name = tableName + " Document"
                });
                dynamic entity = entityFactory.Create();
                Session.Clear();
                GetEntityFactory<DocumentLink>().Create(new {
                    LinkedId = entity.Id,
                    Document = document,
                    DataType = dataType,
                    DocumentType = documentType
                });

                Session.Clear();

                entity = Session.Load(type, entity.Id);

                MyAssert.Contains(((IList<IDocumentLink>)entity.LinkedDocuments).Map(l => l.Document.Id).ToArray(),
                    document.Id,
                    String.Format(
                        "Error linking document to entity of type {0}. Did you remember to do the HasMany in the map?",
                        type));

                MyAssert.DoesNotThrow<GenericADOException>(() => {
                        Session.Delete(entity);
                        Session.Flush();
                    },
                    "Exception was thrown when deleting entity of type {0} with linked document. Please adjust the mapping.",
                    type);
            }
        }

        [TestMethod]
        public void TestNoteLinkedThing()
        {
            foreach (var type in EntityAssembly.GetTypes()
                                               .Where(t => t.Implements<IThingWithNotes>() &&
                                                           !SkipNoteClasses.Contains(t)))
            {
                dynamic entityFactory = GetEntityFactory(type);
                var tableName =
                    ((ILockable)
                        Session.GetSessionImplementation().GetEntityPersister(null, Activator.CreateInstance(type)))
                   .RootTableName;
                dynamic entity = entityFactory.Create();
                Session.Clear();
                var dataType = GetEntityFactory<DataType>().Create(new {
                    TableName = tableName
                });
                var note = GetEntityFactory<Note>().Create(new {
                    DataType = dataType,
                    LinkedId = entity.Id
                });

                Session.Clear();

                entity = Session.Load(type, entity.Id);

                MyAssert.Contains(((IList<INoteLink>)entity.LinkedNotes).Map(l => l.Note.Id), note.Id,
                    String.Format(
                        "Error linking note to entity of type {0}. Did you remember to do the HasMany in the map?",
                        type));

                MyAssert.DoesNotThrow<GenericADOException>(() => {
                        Session.Delete(entity);
                        Session.Flush();
                    },
                    "Exception was thrown when deleting entity of type {0} with linked note. Please adjust the mapping.",
                    type);
            }
        }

        [TestMethod]
        public void TestEmployeeLinkedThing()
        {
            foreach (var type in EntityAssembly.GetTypes().Where(t => t.Implements<IThingWithEmployees>()))
            {
                // TODO: fix this for classes that don't have DATA_TYPE_NAME as a constant
                var dataTypeNameField = type.GetField("DATA_TYPE_NAME");
                if (dataTypeNameField == null)
                {
                    continue;
                }

                var dataTypeName = dataTypeNameField.GetValue(type);
                dynamic entityFactory = GetEntityFactory(type);
                var tableName =
                    ((ILockable)
                        Session.GetSessionImplementation().GetEntityPersister(null, Activator.CreateInstance(type)))
                   .RootTableName;
                dynamic entity = entityFactory.Create();
                var dataType = GetEntityFactory<DataType>().Create(new {
                    TableName = tableName, Name = dataTypeName
                });
                var employee = GetEntityFactory<Employee>().Create();
                GetEntityFactory<EmployeeLink>().Create(new {
                    LinkedId = entity.Id,
                    DataType = dataType,
                    Employee = employee
                });

                Session.Flush();
                Session.Clear();

                entity = Session.Load(type, entity.Id);

                MyAssert.Contains(((IList<IEmployeeLink>)entity.LinkedEmployees).Map(l => l.Employee.Id).ToArray(),
                    employee.Id,
                    String.Format("Error linking employee to entity of type {0}.", type));

                MyAssert.DoesNotThrow<GenericADOException>(() => {
                        Session.Delete(entity);
                        Session.Flush();
                    },
                    "Exception was thrown when deleting entity of type {0} with linked employee. Please adjust the mapping.",
                    type);
            }
        }
    }
}
