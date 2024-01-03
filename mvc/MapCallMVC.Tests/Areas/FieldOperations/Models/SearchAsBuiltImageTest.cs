using MapCall.Common.Model.Entities;
using MapCallMVC.Areas.FieldOperations.Models.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MMSINC.Testing.NHibernate;

namespace MapCallMVC.Tests.Areas.FieldOperations.Models
{
    [TestClass]
    public class SearchAsBuiltImageTest : InMemoryDatabaseTest<AsBuiltImage>
    {
        #region Fields

        private SearchAsBuiltImage _target;

        #endregion

        #region Init/Cleanup

        [TestInitialize]
        public void InitializeTest()
        {
            _target = new SearchAsBuiltImage();
        }

        #endregion

        #region Tests

        #endregion
    }
}
