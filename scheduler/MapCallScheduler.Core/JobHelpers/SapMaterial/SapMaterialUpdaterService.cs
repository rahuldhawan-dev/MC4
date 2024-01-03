using System;
using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallScheduler.Library.JobHelpers.Sap;
using MMSINC.ClassExtensions.IListExtensions;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.SapMaterial
{
    public class SapMaterialUpdaterService : SapEntityUpdaterServiceBase<SapMaterialFileRecord, ISapMaterialFileParser, Material, IRepository<Material>>, ISapMaterialUpdaterService
    {
        #region Private Members

        private readonly IPlanningPlantRepository _planningPlantRepository;

        #endregion

        #region Constructors

        public SapMaterialUpdaterService(ISapMaterialFileParser parser, IRepository<Material> repository, IPlanningPlantRepository planningPlantRepository, ILog log) : base(parser, repository, log)
        {
            _planningPlantRepository = planningPlantRepository;
        }

        #endregion

        #region Private Methods

        protected override void MapRecord(Material material, SapMaterialFileRecord record, int currentLine)
        {
            MapField(material, record, m => m.Description);
            MapField(material, record, m => m.UnitOfMeasure);

            if (!record.DeletionFlag)
            {
                MaybeMapField(material, "IsActive", material.IsActive, !record.DeletionFlag);
            }

            if (!string.IsNullOrWhiteSpace(record.Plant))
            {
                var plant = _planningPlantRepository.GetByCode(record.Plant);

                if (plant == null)
                {
                    throw new ArgumentException($"Could not find planning plant with code '{record.Plant}'.");
                }

                if (plant.OperatingCenter == null)
                {
                    throw new ArgumentException($"Planning plant {plant.Id} with code '{record.Plant}' does not have a linked operating center.");
                }

                if (material.OperatingCenterStockedMaterials.Select(x => x.OperatingCenter).Contains(plant.OperatingCenter))
                {
                    if (record.DeletionFlag) //delete the material
                    {
                        _log.Info($"Removing material '{material.PartNumber}' from operating center '{plant.OperatingCenter.OperatingCenterCode}'...");
                        material.OperatingCenters.Remove(plant.OperatingCenter);
                    }
                    else // update the material cost
                    {
                        _log.Info($"Setting cost for material '{material.PartNumber}' for operating center '{plant.OperatingCenter.OperatingCenterCode}' to {record.Cost}...");
                        foreach (var sm in material.OperatingCenterStockedMaterials)
                        {
                            if (sm.OperatingCenter == plant.OperatingCenter)
                                sm.Cost = record.Cost;
                        }
                    }
                }
                else if(!record.DeletionFlag) // add the missing material to the operating center
                {
                    _log.Info(
                        $"Adding material '{material.PartNumber}' to operating center '{plant.OperatingCenter.OperatingCenterCode}' with cost {record.Cost}...");
                    material.OperatingCenterStockedMaterials.Add(new OperatingCenterStockedMaterial
                    {
                        OperatingCenter = plant.OperatingCenter,
                        Cost = record.Cost,
                        Material = material
                    });
                }
            }
        }

        protected override void SecondPass(List<Material> mappedEntities)
        {
            var copy = mappedEntities.ToArray();

            foreach (var material in copy)
            {
                var dups = mappedEntities.Where(m => m.PartNumber == material.PartNumber);
                if (dups.Count() > 1)
                {
                    var first = dups.First();

                    foreach (var dup in dups.Skip(1).ToArray())
                    {
                        foreach (var oc in dup.OperatingCenterStockedMaterials.Map(ocsm => ocsm.OperatingCenter).Where(x => !first.OperatingCenterStockedMaterials.Map(ocsm => ocsm.OperatingCenter).Contains(x)))
                        {
                            first.OperatingCenterStockedMaterials.Add(new OperatingCenterStockedMaterial {
                                OperatingCenter = oc
                            });
                        }

                        mappedEntities.Remove(dup);
                    }
                }
            }
        }

        protected override Material FindOrCreateEntity(SapMaterialFileRecord record, int currentLine)
        {
            return _repository.GetByPartNumber(record.PartNumber) ??
                   new Material {PartNumber = record.PartNumber};
        }

        protected override void LogRecord(SapMaterialFileRecord record)
        {
            _log.Info(
                $"Updating material with part number '{record.PartNumber}' on planning plant '{record.Plant}' (deletion flag {(record.DeletionFlag ? "" : "un")}set)...");
        }

        #endregion
    }
}