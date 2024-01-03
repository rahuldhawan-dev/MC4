namespace MapCall.Common.Utility
{
    public interface IUrlHelper
    {
        #region Abstract Methods

        string Action(string action, string controller, object routeValues);

        #endregion
    }
}
