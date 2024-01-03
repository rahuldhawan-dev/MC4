namespace Permits.Data.Client.Entities
{
    public class Field : IField
    {
        public virtual int Id { get; set; }
    }

    public interface IField
    {
        int Id { get; }
    }
}
