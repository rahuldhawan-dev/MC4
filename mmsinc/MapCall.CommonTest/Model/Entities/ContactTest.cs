using MapCall.Common.Model.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MapCall.CommonTest.Model.Entities
{
    [TestClass]
    public class ContactTest
    {
        #region Tests

        [TestMethod]
        public void TestContactNameReturnsFormattedName()
        {
            var target = new Contact();
            target.FirstName = "Billy";
            target.LastName = "Thornton";

            Assert.AreEqual("Thornton, Billy", target.ContactName);

            target.MiddleInitial = string.Empty;
            Assert.AreEqual("Thornton, Billy", target.ContactName);

            target.MiddleInitial = "   ";
            Assert.AreEqual("Thornton, Billy", target.ContactName);

            target.MiddleInitial = "B";
            Assert.AreEqual("Thornton, Billy B.", target.ContactName);

            // Trimming too

            target.FirstName = "   Billy  ";
            target.LastName = "   Thornton ";
            target.MiddleInitial = "  B ";
            Assert.AreEqual("Thornton, Billy B.", target.ContactName);

            target.MiddleInitial = null;
            Assert.AreEqual("Thornton, Billy", target.ContactName);
        }

        [TestMethod]
        public void TestFullNameReturnsProperFullName()
        {
            var target = new Contact {FirstName = "Billy ", LastName = "Thorton ", MiddleInitial = "Bob "};

            Assert.AreEqual("Billy Bob Thorton", target.FullName);

            target.MiddleInitial = null;

            Assert.AreEqual("Billy Thorton", target.FullName);
        }

        #endregion
    }
}
