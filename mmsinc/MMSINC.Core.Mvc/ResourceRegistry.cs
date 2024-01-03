namespace MMSINC
{
    /// <summary>
    /// Represents resources that are needed by a partial view that a view or masterview should render.
    /// </summary>
    public class ResourceRegistry
    {
        #region Fields

        private ResourceRegistryDictionary _scriptResources;
        private ResourceRegistryDictionary _styleSheets;

        #endregion

        #region Properties

        public ResourceRegistryDictionary Scripts
        {
            get { return _scriptResources ?? (_scriptResources = new ResourceRegistryDictionary()); }
        }

        public ResourceRegistryDictionary StyleSheets
        {
            get { return _styleSheets ?? (_styleSheets = new ResourceRegistryDictionary()); }
        }

        #endregion
    }
}
