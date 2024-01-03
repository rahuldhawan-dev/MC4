namespace MapCallScheduler.JobHelpers.GIS.Models
{
    public class User
    {
        #region Properties

        public int Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }

        #endregion

        public static User FromDbRecord(MapCall.Common.Model.Entities.Users.User user)
        {
            return user == null
                ? null
                : new User {
                    Id = user.Id,
                    FullName = user.FullName, 
                    UserName = user.UserName
                };
        }
    }
}