using System.Collections.Generic;
using System.Linq;
using log4net;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MapCallScheduler.Library.JobHelpers.Sap;
using MMSINC.Data.NHibernate;

namespace MapCallScheduler.JobHelpers.SapChemical
{
    public class SapChemicalUpdaterService : SapEntityUpdaterServiceBase<SapChemicalFileRecord, ISapChemicalFileParser, Chemical, IRepository<Chemical>>, ISapChemicalUpdaterService
    {

        #region Constructors

        public SapChemicalUpdaterService(ISapChemicalFileParser parser, IRepository<Chemical> repository, ILog log) : base(parser, repository, log) { }

        #endregion

        #region Private Methods

        protected override void MapRecord(Chemical chemical, SapChemicalFileRecord record, int currentLine)
        {
            MapField(chemical, record, m => m.Name);
        }

        protected override void SecondPass(List<Chemical> mappedEntities)
        {
            var copy = mappedEntities.ToArray();

            foreach (var chemical in copy)
            {
                var dups = mappedEntities.Where(m => m.PartNumber == chemical.PartNumber);
                if (dups.Count() > 1)
                {
                    var first = dups.First();

                    foreach (var dup in dups.Skip(1).ToArray())
                    {
                        mappedEntities.Remove(dup);
                    }
                }
            }
        }

        protected override Chemical FindOrCreateEntity(SapChemicalFileRecord record, int currentLine)
        {
            return _repository.GetByPartNumber(record.PartNumber) ??
                   new Chemical {PartNumber = record.PartNumber};
        }

        protected override void LogRecord(SapChemicalFileRecord record)
        {
            _log.Info(
                $"Updating Chemical with part number '{record.PartNumber}' on planning plant '{record.Plant}' (deletion flag {(record.DeletionFlag ? "" : "un")}set)...");
        }

        #endregion
    }
}
