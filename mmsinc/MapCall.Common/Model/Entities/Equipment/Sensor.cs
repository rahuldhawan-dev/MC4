using System;
using System.Collections.Generic;
using MMSINC.ClassExtensions.IQueryableExtensions;
using MMSINC.ClassExtensions.StringExtensions;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class Sensor : IEntity
    {
        #region Consts

        public const int MAX_NAME_LENGTH = 25,
                         MAX_DESCRIPTION_LENGTH = 32,
                         MAX_LOCATION_LENGTH = 25,
                         MAX_MEASUREMENT_UNITS_LENGTH = 20;

        #endregion

        #region Private Members

        private SensorDisplayItem _display;

        #endregion

        #region Properties

        public virtual int Id { get; set; }
        public virtual Board Board { get; set; }
        public virtual IList<Reading> Readings { get; set; }
        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual EquipmentSensor Equipment { get; set; }
        public virtual string Location { get; set; }
        public virtual SensorMeasurementType MeasurementType { get; set; }

        public virtual string ShortDisplayDescription
        {
            get
            {
                // Need to trim Name/Description because they're char fields and Nhibernate doesn't trim the whitespace off.
                var name = Name.CoalesceAndTrim();
                var desc = Description.CoalesceAndTrim();
                var loc = Location.CoalesceAndTrim();
                return $"{name} - {desc} - {loc}";
            }
        }

        #endregion

        #region Constructor

        public Sensor()
        {
            Readings = new List<Reading>();
        }

        #endregion

        #region Public Methods

        public override string ToString()
        {
            return (_display ?? (_display = new SensorDisplayItem {
                Project = Board.Site.Project.ToString(),
                Site = Board.Site.ToString(),
                Board = Board.ToString(),
                Name = Name,
                Description = Description,
                Location = Location
            })).Display;
        }

        #endregion
    }

    [Serializable]
    public class SensorDisplayItem : DisplayItem<Sensor>
    {
        [SelectDynamic("Site.Project.Name", Field = "Board")]
        public string Project { get; set; }

        [SelectDynamic("Site.Name", Field = "Board")]
        public string Site { get; set; }

        [SelectDynamic("Name")]
        public string Board { get; set; }

        public string Name { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }

        public override string Display =>
            $"{Project.CoalesceAndTrim()} - {Site.CoalesceAndTrim()} - {Board.CoalesceAndTrim()} - {Name.CoalesceAndTrim()} - {Description.CoalesceAndTrim()} - {Location.CoalesceAndTrim()}";
    }
}
