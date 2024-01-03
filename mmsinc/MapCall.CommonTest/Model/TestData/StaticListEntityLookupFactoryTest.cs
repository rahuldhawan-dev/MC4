using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Humanizer;
using MapCall.Common.Model.Entities.Users;
using MapCall.Common.Testing.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.Testing.NHibernate;

namespace MapCall.CommonTest.Model.TestData
{
    [TestClass]
    public class StaticListEntityLookupFactoryTest : InMemoryDatabaseTest<User>
    {
        private IEnumerable<Type> _assemblyTypes;

        private IEnumerable<Type> GetAssemblyTypes()
        {
            return _assemblyTypes ?? (_assemblyTypes = typeof(AssetStatusFactory).Assembly.GetTypes());
        }

        private IEnumerable<Type> GetParentTypes()
        {
            return GetAssemblyTypes()
               .Where(t => t.IsClass &&
                           t.IsSubclassOfRawGeneric(typeof(StaticListEntityLookupFactory<,>)) &&
                           t.BaseType.GetGenericArguments().Any(g => g == t));
        }

        private IEnumerable<Type> GetChildTypes(Type parent)
        {
            return GetAssemblyTypes()
               .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(parent));
        }

        [TestMethod]
        public void TestStaticEntityLookupStuff()
        {
            /*
             * Hello troubleshooter - are you here because you can't figure out why this test is failing with the output similar to
             * "Output from {SomeReadOnlyEntityLookupFactory} had wrong Id, expected {SomeValue}, actual {NotSameValueButProbablyAOneOrTwo}"
             *
             * If so - go to your entity's map file and make sure the id property is being assigned... similar to this:
             *      Id(x => x.Id).GeneratedBy.Assigned()
             */

            var issues = new List<string>();
            var childrenNotTested = new List<Type>();
            var parents = GetParentTypes();

            foreach (var parent in parents)
            {
                var children = GetChildTypes(parent);
                var entityType = parent.BaseType.GetGenericArguments()[0];
                var indices = entityType.GetNestedTypes().SingleOrDefault(t => t.Name == "Indices" && t.IsValueType);

                if (indices == null)
                {
                    issues.Add($"Entity class {entityType} has no Indices struct");
                }

                var constants = indices.GetConstantValues();
                var childrenTested = new List<Type>();

                foreach (var constant in constants)
                {
                    var childName = constant.Name.ToLower().Pascalize() + parent.Name;
                    var childFactory = children.SingleOrDefault(t =>
                        t.Name == childName);

                    if (childFactory == null)
                    {
                        continue;
                    }

                    childrenTested.Add(childFactory);

                    dynamic factory = GetType().GetMethod("GetFactory").MakeGenericMethod(childFactory)
                                               .Invoke(this, null);
                    dynamic entity = factory.Create();

                    if (entity.Id != constant.Value)
                    {
                        issues.Add(
                            $"Output from {childName} had wrong Id, expected {constant.Value}, actual {entity.Id}. Make sure your TEntityMap is auto assigning the Id property in it's constructor.");
                    }
                }

                childrenNotTested.AddRange(children.Where(t => !childrenTested.Contains(t)));

                // TOO MANY EXCEPTIONS:
                //foreach (var child in children.Where(t => !childrenTested.Contains(t)))
                //{
                //    issues.Add($"{parent.Name} subclass {child.Name} does not appear to have a matching constant in {entityType.Name}.Indices ({string.Join(", ", constants.Select(c => c.Name))})");
                //}
            }

            if (issues.Any())
            {
                Assert.Fail(
                    $"The following issues were encountered:{Environment.NewLine}{string.Join(Environment.NewLine, issues)}");
            }
        }
    }
}
