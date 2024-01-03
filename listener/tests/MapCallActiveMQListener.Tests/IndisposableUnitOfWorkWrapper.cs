using System;
using MMSINC.Data.V2;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;
using IUnitOfWork = MMSINC.Data.IUnitOfWork;

namespace MapCallActiveMQListener.Tests
{
    public class IndisposableUnitOfWorkWrapper : IUnitOfWork
    {
        #region Private Members

        private readonly IUnitOfWork _unitOfWork;

        #endregion

        #region Properties

        public IContainer Container => _unitOfWork.Container;

        #endregion

        #region Constructors

        public IndisposableUnitOfWorkWrapper(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion

        #region Exposed Methods

        public void Dispose() {}

        public MMSINC.Data.NHibernate.IRepository<T> GetRepository<T>()
        {
            return _unitOfWork.GetRepository<T>();
        }

        public TRepository GetRepository<T, TRepository>() where TRepository : MMSINC.Data.NHibernate.IRepository<T>
        {
            return _unitOfWork.GetRepository<T, TRepository>();
        }

        public ISqlQuery SqlQuery(string query)
        {
            throw new NotImplementedException();
        }

        public void Commit()
        {
            _unitOfWork.Commit();
        }

        public void Rollback()
        {
            _unitOfWork.Rollback();
        }

        public IObjectMapper GetMapper(Type getType, Type type)
        {
            throw new NotImplementedException();
        }

        public void Flush()
        {
            _unitOfWork.Flush();
        }

        #endregion
    }
}