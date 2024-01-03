using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public interface ISensorMeasurementTypeRepository : IRepository<SensorMeasurementType>
    {
        SensorMeasurementType GetByDescription(string description);
    }

    public class SensorMeasurementTypeRepository : RepositoryBase<SensorMeasurementType>,
        ISensorMeasurementTypeRepository
    {
        #region Constructor

        public SensorMeasurementTypeRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region Public Methods

        public SensorMeasurementType GetByDescription(string description)
        {
            var entity = Linq.SingleOrDefault(x => x.Description == description);

            if (entity == null)
            {
                throw new InvalidOperationException("Unable to find a sensor measurement type with description \"" +
                                                    description + "\"");
            }

            return entity;
        }

        #endregion
    }
}
