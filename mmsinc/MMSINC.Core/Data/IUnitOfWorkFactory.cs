namespace MMSINC.Data
{
    public interface IUnitOfWorkFactory
    {
        IUnitOfWork Build();
        IUnitOfWork BuildMemoized();
    }
}
