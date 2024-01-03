namespace MMSINC.Data.V2
{
    public interface IUnitOfWorkFactory
    {
        #region Abstract Methods

        IUnitOfWork Build();
        IUnitOfWork BuildMemoized();

        #endregion
    }
}
