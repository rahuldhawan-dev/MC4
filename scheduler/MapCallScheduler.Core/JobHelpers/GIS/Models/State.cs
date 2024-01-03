using MapCall.Common.Model.Entities;

namespace MapCallScheduler.JobHelpers.GIS.Models
{
    public class State
    {
        #region Properties

        public int Id { get; set; }
        public string Abbreviation { get; set; }

        #endregion

        #region Exposed Methods

        public static State FromDbRecord(MapCall.Common.Model.Entities.State state)
        {
            return state == null ? null : new State {Id = state.Id, Abbreviation = state.Abbreviation};
        }

        public static State FromDbRecord(IThingWithTownAndOperatingCenter thing)
        {
            return thing.Town != null
                ? FromDbRecord(thing.Town.State)
                : FromDbRecord(thing.OperatingCenter.State);
        }

        #endregion
    }
}
