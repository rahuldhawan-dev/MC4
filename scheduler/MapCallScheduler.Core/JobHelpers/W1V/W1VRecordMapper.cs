using System;
using System.Globalization;
using System.Linq;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.W1V
{
    public class W1VRecordMapper : IW1VRecordMapper
    {
        private readonly IRepository<Premise> _premiseRepository;
        private readonly IRepository<ServiceMaterial> _serviceMaterialRepository;
        private readonly IRepository<SmallMeterLocation> _smallMeterLocationRepository;

        public W1VRecordMapper(
            IRepository<Premise> premiseRepository,
            IRepository<ServiceMaterial> serviceMaterialRepository,
            IRepository<SmallMeterLocation> smallMeterLocationRepository)
        {
            _premiseRepository = premiseRepository;
            _serviceMaterialRepository = serviceMaterialRepository;
            _smallMeterLocationRepository = smallMeterLocationRepository;
        }

        public void Map(
            ShortCycleCustomerMaterial entity,
            W1VFileParser.ParsedCustomerMaterial parsedRecord)
        {
            if (entity.Premise == null)
            {
                var premise = _premiseRepository.FindByPremiseNumber(parsedRecord.PremiseId)
                                                .SingleOrDefault(x =>
                                                     x.Installation == parsedRecord.Installation &&
                                                     x.DeviceLocation == parsedRecord.FunctionalLocation);
                entity.Premise = premise ?? _premiseRepository.Save(new Premise {
                    IsMajorAccount = false,
                    PremiseNumber = parsedRecord.PremiseId,
                    Installation = parsedRecord.Installation,
                    DeviceLocation = parsedRecord.FunctionalLocation
                });
            }

            entity.AssignmentStart = entity.AssignmentStart ?? parsedRecord.AssignmentStart;

            if (!string.IsNullOrWhiteSpace(parsedRecord.CustomerSideMaterial))
            {
                entity.CustomerSideMaterial = _serviceMaterialRepository
                                             .Linq.SingleOrDefault(x =>
                                                  x.Description == parsedRecord.CustomerSideMaterial);
            }

            if (!string.IsNullOrWhiteSpace(parsedRecord.MeterSize))
            {
                entity.ServiceLineSize = parsedRecord.MeterSize;
            }

            if (!string.IsNullOrWhiteSpace(parsedRecord.TechnicalInspectedOn))
            {
                entity.TechnicalInspectedOn =
                    DateTime.ParseExact(
                        parsedRecord.TechnicalInspectedOn,
                        "yyyyMMdd",
                        CultureInfo.CurrentCulture);
            }

            if (!string.IsNullOrWhiteSpace(parsedRecord.ReadingDevicePositionalLocation))
            {
                entity.ReadingDevicePositionalLocation =
                    _smallMeterLocationRepository
                       .Linq.SingleOrDefault(l =>
                            l.SAPCode == parsedRecord.ReadingDevicePositionalLocation);
            }

            if (entity.ShortCycleWorkOrderNumber <= default(int))
            {
                entity.ShortCycleWorkOrderNumber = parsedRecord.WorkOrderNumber;
            }
        }
    }

    public interface IW1VRecordMapper
    {
        void Map(ShortCycleCustomerMaterial entity, W1VFileParser.ParsedCustomerMaterial parsedRecord);
    }
}
