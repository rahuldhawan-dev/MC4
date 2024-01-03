using MMSINC.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace MMSINC.Core.MvcTest.Data
{
    /// <summary>
    /// ?????
    /// </summary>
    [TestClass]
    public class ViewModelSetOfTEntityTest { }

    #region Test Classes

    public class TestViewModelSet : ViewModel<Entity>
    {
        public TestViewModelSet(IContainer container, Entity entity) : base(container)
        {
            if (entity != null)
            {
                Map(entity);
            }
        }
    }

    #endregion
}
