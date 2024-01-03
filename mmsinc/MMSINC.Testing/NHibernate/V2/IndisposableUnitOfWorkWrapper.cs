using System;
using MMSINC.Data.V2;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MMSINC.Testing.NHibernate.V2
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

        public void Dispose() { }

        public MMSINC.Data.V2.IRepository<T> GetRepository<T>()
        {
            return _unitOfWork.GetRepository<T>();
        }

        public TRepository GetRepository<T, TRepository>() where TRepository : MMSINC.Data.V2.IRepository<T>
        {
            return _unitOfWork.GetRepository<T, TRepository>();
        }

        public ISqlQuery SqlQuery(string query)
        {
            return _unitOfWork.SqlQuery(query);
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
            return _unitOfWork.GetMapper(getType, type);
        }

        public void Flush()
        {
            _unitOfWork.Flush();
        }

        public void Clear()
        {
            _unitOfWork.Clear();
        }

        public T GetInstance<T>()
        {
            return Container.GetInstance<T>();
        }

        #endregion
    }
}
