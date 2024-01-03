namespace MMSINC.Utilities.Permissions
{
    public interface IRole
    {
        #region Properties

        int UserId { get; }

        string OperatingCenter { get; }
        int OperatingCenterId { get; }

        string Application { get; }
        int ApplicationId { get; }

        string Module { get; }
        int ModuleId { get; }

        string Action { get; }
        int ActionId { get; }

        #endregion
    }
}
