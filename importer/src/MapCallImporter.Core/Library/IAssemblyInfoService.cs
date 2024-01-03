namespace MapCallImporter.Library
{
    public interface IAssemblyInfoService
    {
        #region Abstract Properties

        string Title { get; }
        string Version { get; }
        string Description { get; }
        string Product { get; }
        string Copyright { get; }
        string Company { get; }

        #endregion
    }
}