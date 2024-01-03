namespace MMSINC.Utilities.Permissions
{
    public interface IRoleModule
    {
        #region Properties

        int ApplicationId { get; }
        int ModuleId { get; }
        string Name { get; }

        #endregion
    }
}
