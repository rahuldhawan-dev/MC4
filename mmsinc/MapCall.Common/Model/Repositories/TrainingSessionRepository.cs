using System;
using System.Linq;
using MapCall.Common.Model.Entities;
using MMSINC.Data.NHibernate;
using NHibernate;
using StructureMap;

namespace MapCall.Common.Model.Repositories
{
    public class TrainingSessionRepository : RepositoryBase<TrainingSession>, ITrainingSessionRepository
    {
        #region Constructors

        public TrainingSessionRepository(ISession session, IContainer container) : base(session, container) { }

        #endregion

        #region ExposedMethods

        public bool InstructorBooked(int instructorId, DateTime startDateTime, DateTime endDateTime)
        {
            return Linq.Any(x =>
                (
                    (x.TrainingRecord.Instructor != null && x.TrainingRecord.Instructor.Id == instructorId)
                    ||
                    (x.TrainingRecord.SecondInstructor != null && x.TrainingRecord.SecondInstructor.Id == instructorId)
                )
                &&
                (startDateTime < x.EndDateTime && endDateTime > x.StartDateTime)
            );
        }

        #endregion
    }

    public interface ITrainingSessionRepository : IRepository<TrainingSession>
    {
        bool InstructorBooked(int instructorId, DateTime startDateTime, DateTime endDateTime);
    }
}
