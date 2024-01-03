namespace Permits.Data.Client.Entities
{
    public class FieldPermitValue : IFieldPermitValue<Field>
    {
        public virtual int Id { get; set; }
        public virtual Field Field { get; set; }
        public virtual string PermitValue { get; set; }
    }

    public interface IFieldPermitValue
    {
        int Id { get; set; }
        string PermitValue { get; set; }
    }

    public interface IFieldPermitValue<TFieldType> : IFieldPermitValue
        where TFieldType : IField
    {
        TFieldType Field { get; set; }
    }
}
