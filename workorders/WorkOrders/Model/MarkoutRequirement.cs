using System;
using System.ComponentModel;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using MMSINC.Exceptions;
using MapCall.Common.Utility;

namespace WorkOrders.Model
{
    [Table(Name = "dbo.MarkoutRequirements")]
    public class MarkoutRequirement : INotifyPropertyChanging, INotifyPropertyChanged
    {
        #region Constants

        private const short MAX_DESCRIPTION_LENGTH = 10;

        #endregion

        #region Private Static Members

        private static readonly PropertyChangingEventArgs emptyChangingEventArgs = new PropertyChangingEventArgs(String.Empty);

        #endregion

        #region Private Members

        private int _markoutRequirementID;

        private string _description;

        private MarkoutRequirementEnum? _requirementEnum;

        private readonly EntitySet<WorkOrder> _workOrders;

        #endregion

        #region Properties

        [Column(Storage = "_markoutRequirementID", AutoSync = AutoSync.OnInsert, DbType = "Int NOT NULL IDENTITY", IsPrimaryKey = true, IsDbGenerated = true)]
        public int MarkoutRequirementID
        {
            get { return _markoutRequirementID; }
            set
            {
                if (_markoutRequirementID != value)
                {
                    SendPropertyChanging();
                    _markoutRequirementID = value;
                    SendPropertyChanged("MarkoutRequirementID");
                }
            }
        }

        [Column(Storage = "_description", DbType = "VarChar(10) NOT NULL")]
        public string Description
        {
            get { return _description; }
            set
            {
                if (value != null && value.Length > MAX_DESCRIPTION_LENGTH)
                    throw new StringTooLongException("Description", MAX_DESCRIPTION_LENGTH);
                if (_description != value)
                {
                    SendPropertyChanging();
                    _description = value;
                    SendPropertyChanged("Description");
                }
            }
        }

        /// <summary>
        /// MarkoutRequirementEnum value indicating the level of requirement.
        /// </summary>
        public MarkoutRequirementEnum RequirementEnum
        {
            get
            {
                if (_requirementEnum == null)
                    _requirementEnum =
                        MarkoutRequirementRepository.GetEnumerationValue(this);
                return _requirementEnum.Value;
            }
        }

        /// <summary>
        /// Boolean value indicating whether a Markout is required at all.
        /// </summary>
        public bool IsRequired
        {
            get { return RequirementEnum != MarkoutRequirementEnum.None; }
        }

        [Association(Name = "MarkoutRequirement_WorkOrder", Storage = "_workOrders", OtherKey = "MarkoutRequirementID")]
        public EntitySet<WorkOrder> WorkOrders
        {
            get { return _workOrders; }
            set { _workOrders.Assign(value); }
        }

        #endregion

        #region Constructors

        public MarkoutRequirement()
        {
            _workOrders = new EntitySet<WorkOrder>(attach_WorkOrders, detach_WorkOrders);
        }
        
        #endregion

        #region Private Methods

        // ReSharper disable UnusedPrivateMember
        private void OnValidate(ChangeAction action)
        {
            switch (action)
            {
                case ChangeAction.Insert:
                case ChangeAction.Update:
                    if (String.IsNullOrEmpty(Description))
                        throw new DomainLogicException("Description cannot be null");
                    break;
            }
        }
        // ReSharper restore UnusedPrivateMember

        private void attach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.MarkoutRequirement = this;
        }

        private void detach_WorkOrders(WorkOrder entity)
        {
            SendPropertyChanging();
            entity.MarkoutRequirement = null;
        }

        private void SendPropertyChanging()
        {
            if (PropertyChanging != null)
                PropertyChanging(this, emptyChangingEventArgs);
        }

        private void SendPropertyChanged(String propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Events

        public event PropertyChangingEventHandler PropertyChanging;

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Exposed Methods

        public override string ToString()
        {
            return Description;
        }

        #endregion
    }
}
