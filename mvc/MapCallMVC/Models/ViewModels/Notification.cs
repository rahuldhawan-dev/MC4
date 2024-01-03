using System.ComponentModel.DataAnnotations;
using MapCall.Common.Model.Entities;
using MMSINC.Data;
using MMSINC.Metadata;
using StructureMap;

namespace MapCallMVC.Models.ViewModels
{
    public class CreateNotification : ViewModel<Notification>
    {
        public CreateNotification(IContainer container) : base(container) {}

        [Required, Secured]
        public override int Id { get; set; }
        [Required, Secured]
        public RoleModules RoleModule { get; set; }
        [Required, Secured]
        public string NotificationPurpose { get; set; }
        [Required, Secured]
        public int OperatingCenterId { get; set; }
        [Required, Secured]
        public string EntityType { get; set; }
    }
}