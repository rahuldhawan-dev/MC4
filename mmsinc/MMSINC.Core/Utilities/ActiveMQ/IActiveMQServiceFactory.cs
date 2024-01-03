namespace MMSINC.Utilities.ActiveMQ
{
    public interface IActiveMQServiceFactory
    {
        IActiveMQService Build(IActiveMQConfiguration config);
    }
}
