namespace MMSINC.Utilities.ActiveMQ
{
    public interface IActiveMQConfiguration
    {
        #region Abstract Properties

        string Scheme { get; }
        string Host { get; }
        int Port { get; }
        string User { get; }
        string Password { get; }
        string Url { get; }

        #endregion
    }
}
