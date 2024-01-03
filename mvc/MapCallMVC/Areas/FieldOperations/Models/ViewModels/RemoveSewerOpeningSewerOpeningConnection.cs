using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MapCall.Common.Model.Repositories;
using MMSINC.Data;
using MMSINC.Utilities.ObjectMapping;
using StructureMap;

namespace MapCallMVC.Areas.FieldOperations.Models.ViewModels
{
    public class RemoveSewerOpeningSewerOpeningConnection : ViewModel<SewerOpening>
    {
        #region Properties

        [Required, DoesNotAutoMap]
        public virtual int SewerOpeningSewerOpeningConnectionId { get; set; }

        #endregion

        #region Constructors

        public RemoveSewerOpeningSewerOpeningConnection(IContainer container) : base(container) { }

        #endregion

        #region Exposed Methods

        public override SewerOpening MapToEntity(SewerOpening entity)
        {
            var connection = _container.GetInstance<ISewerOpeningConnectionRepository>().Find(SewerOpeningSewerOpeningConnectionId);
            
            if (connection.UpstreamOpening.Id == entity.Id)
                entity.DownstreamSewerOpeningConnections.Remove(connection);
            else
                entity.UpstreamSewerOpeningConnections.Remove(connection);

            return base.MapToEntity(entity);
        }

        #endregion
    }
}
