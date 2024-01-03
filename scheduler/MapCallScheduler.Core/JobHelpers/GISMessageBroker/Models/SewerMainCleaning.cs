using MapCall.Common.Model.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace MapCallScheduler.JobHelpers.GISMessageBroker.Models
{
    public class SewerMainCleaning
    {
        #region Properties

        [Required]
        public int Id { get; set; }
        public DateTime? NextCleanLineDate { get; set; }
        public DateTime? CompletedDate { get; set; }
        public SewerOpening Opening1 { get; set; }
        public SewerOpening Opening2 { get; set; }
        public State State { get; set; }

        #endregion

        #region Exposed Methods

        public static SewerMainCleaning FromDbRecord(MapCall.Common.Model.Entities.SewerMainCleaning sewerMainCleaning)
        {
            var record = new SewerMainCleaning {
                Id = sewerMainCleaning.Id,
                CompletedDate = sewerMainCleaning.InspectedDate,
                Opening1 = sewerMainCleaning.Opening1 == null ? null : new SewerOpening {
                    Id = sewerMainCleaning.Opening1.Id,
                    OpeningNumber = sewerMainCleaning.Opening1.OpeningNumber
                },
                Opening2 = sewerMainCleaning.Opening2 == null ? null : new SewerOpening {
                    Id = sewerMainCleaning.Opening2.Id,
                    OpeningNumber = sewerMainCleaning.Opening2.OpeningNumber
                },
                State = sewerMainCleaning.OperatingCenter?.State == null ? null : new State {
                    Id = sewerMainCleaning.OperatingCenter.State.Id, 
                    Abbreviation = sewerMainCleaning.OperatingCenter.State.Abbreviation
                }
            };

            if (sewerMainCleaning.Opening1 != null && sewerMainCleaning.Opening2 != null)
            {
                var result = sewerMainCleaning.Opening1.SewerOpeningConnections
                    .SingleOrDefault(x => 
                    (x.UpstreamOpening == sewerMainCleaning.Opening1 && x.DownstreamOpening == sewerMainCleaning.Opening2) || 
                    (x.UpstreamOpening == sewerMainCleaning.Opening2 && x.DownstreamOpening == sewerMainCleaning.Opening1));

                if (result != null && result.InspectionFrequencyUnit != null && result.InspectionFrequency != null)
                {
                    switch (result.InspectionFrequencyUnit.Id)
                    {
                        case RecurringFrequencyUnit.Indices.DAY:
                            record.NextCleanLineDate = sewerMainCleaning.Date?.AddDays(result.InspectionFrequency.Value);
                            break;
                        case RecurringFrequencyUnit.Indices.WEEK:
                            record.NextCleanLineDate = sewerMainCleaning.Date?.AddDays(result.InspectionFrequency.Value * 7);
                            break;
                        case RecurringFrequencyUnit.Indices.MONTH:
                            record.NextCleanLineDate = sewerMainCleaning.Date?.AddMonths(result.InspectionFrequency.Value);
                            break;
                        case RecurringFrequencyUnit.Indices.YEAR:
                            record.NextCleanLineDate = sewerMainCleaning.Date?.AddYears(result.InspectionFrequency.Value);
                            break;
                    }
                }
            }

            return record;
        }

        #endregion
    }
}
