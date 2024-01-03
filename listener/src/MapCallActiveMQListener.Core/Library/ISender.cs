namespace MapCallActiveMQListener.Library
{
    public interface ISender
    {
        #region Abstract Methods

        void Send(string topic, string message);

        #endregion
    }
}