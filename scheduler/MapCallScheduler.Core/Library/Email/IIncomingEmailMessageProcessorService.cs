namespace MapCallScheduler.Library.Email
{
    public interface IIncomingEmailMessageProcessorService
    {
        #region Abstract Methods

        void Process(string mailbox, uint uid);

        #endregion
    }
}