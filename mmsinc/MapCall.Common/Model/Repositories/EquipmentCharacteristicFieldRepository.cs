using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class EquipmentCharacteristicFieldRepository : RepositoryBase<EquipmentCharacteristicField>,
        IEquipmentCharacteristicFieldRepository
    {
        public EquipmentCharacteristicFieldRepository(ISession session, IContainer container) :
            base(session, container) { }
    }

    public static class EquipmentCharacteristicFieldRepositoryExtensions
    {
        public static EquipmentCharacteristicField GetByEquipmentTypeAndName(
            this IRepository<EquipmentCharacteristicField> that, int equipmentPurposeId, string name)
        {
            var field = that.Where(x => x.EquipmentType != null &&
                                        x.EquipmentType.Id == equipmentPurposeId &&
                                        x.FieldName == name).SingleOrDefault();
            if (field == null)
            {
                throw new ArgumentException(
                    $"Could not find field with equipment purpose {equipmentPurposeId} and name '{name}'.");
            }

            return field;
        }
    }

    public interface IEquipmentCharacteristicFieldRepository : IRepository<EquipmentCharacteristicField> { }
}
