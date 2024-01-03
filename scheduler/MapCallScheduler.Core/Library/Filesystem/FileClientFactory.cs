namespace MapCallScheduler.Library.Filesystem
{
    public class FileClientFactory : IFileClientFactory
    {
        public IFileClient Build()
        {
            return new FileClient();
        }
    }

    public interface IFileClientFactory
    {
        IFileClient Build();
    }
}