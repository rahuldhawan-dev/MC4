using System;
using System.ComponentModel.DataAnnotations;
using MMSINC.Data;

namespace MapCall.Common.Model.Entities
{
    [Serializable]
    public class SmartCoverAlertAlarm : IEntity
    {
        #region

        public struct StringLengths
        {
            public const int ALARM_TYPE = 50;
        }

        #endregion

        #region Table Properties

        public virtual int Id { get; set; }

        public virtual int AlarmId { get; set; }
        
        public virtual SmartCoverAlertAlarmType AlarmType { get; set; }

        public virtual decimal Value { get; set; }

        public virtual DateTime AlarmDate { get; set; }

        public virtual decimal Level { get; set; }

        #region References

        public virtual SmartCoverAlert SmartCoverAlert { get; set; }

        #endregion

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return $"{AlarmType} - {Level} - {AlarmDate}";
        }

        #endregion
    }
}
