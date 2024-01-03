using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.ClassExtensions.ReflectionExtensions;
using MMSINC.ClassExtensions.TypeExtensions;
using MMSINC.Data;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class AllEntitiesTest
    {
        [TestMethod]
        public void TestAllEntityClassesImplementIEntity()
        {
            var classNameRegex = new Regex("^[a-zA-Z]+$");
            var entities = typeof(Town).Assembly.GetClassesByCondition(t =>
                                            !t.IsAbstract &&
                                            t.Namespace == "MapCall.Common.Model.Entities" &&
                                            !t.IsSubclassOfRawGeneric(typeof(DisplayItem<>)) &&
                                            t.HasPropertyNamed("Id") &&
                                            classNameRegex.IsMatch(t.Name))
                                       .OrderBy(t => t.Name);

            var badClasses = new List<string>();

            foreach (var cls in entities)
            {
                if (!cls.Implements<IEntity>())
                {
                    badClasses.Add(cls.Name);
                }
            }

            if (badClasses.Any())
            {
                Assert.Fail(
                    $"All entity classes must implement IEntity.  The following {badClasses.Count} do not:{Environment.NewLine}{string.Join(Environment.NewLine, badClasses)}");
            }
        }
    }
}
