using System;
using System.Reflection;
using MapCall.Common.Model.Entities.Users;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing;

namespace MapCall.CommonTest
{
    [TestClass]
    public class AssemblyTest
    {
        public struct Assemblies
        {
            public static readonly Assembly MAPCALL_COMMON = Assembly.GetAssembly(typeof(User));
        }

        [TestMethod]
        public void TestAllNHibernateMapsHaveNullableSettingsConsistentWithTheMappedPropertyType()
        {
            TestLibrary.TestAllNHibernateMapsHaveNullableSettingsConsistentWithTheMappedPropertyType(
                Assemblies.MAPCALL_COMMON);
        }
    }
}
