using System.Diagnostics.CodeAnalysis;
using StructureMap;

namespace MapCallImporter.Library.TypeRegistration
{
    [ExcludeFromCodeCoverage]
    public class DependencyRegistrar
    {
        #region Exposed Methods

        public IContainer Initialize()
        {
            return new Container(new StructureMapRegistry());
        }

        #endregion
    }
}
