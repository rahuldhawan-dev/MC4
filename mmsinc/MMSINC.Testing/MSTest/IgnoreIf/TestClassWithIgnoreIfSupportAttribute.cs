// stolen from https://matt.kotsenas.com/posts/ignoreif-mstest
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MMSINC.Testing.MSTest.IgnoreIf
{
    /// <summary>
    /// An extension of the [TestClass] attribute. If applied to a class, any [TestMethod] attributes
    /// are automatically upgraded to [TestMethodWithIgnoreIfSupport].
    /// </summary>
    public class TestClassWithIgnoreIfSupportAttribute : TestClassAttribute
    {
        public override TestMethodAttribute GetTestMethodAttribute(TestMethodAttribute testMethodAttribute)
        {
            return testMethodAttribute is TestMethodWithIgnoreIfSupportAttribute
                ? testMethodAttribute
                : new TestMethodWithIgnoreIfSupportAttribute();
        }
    }
}
