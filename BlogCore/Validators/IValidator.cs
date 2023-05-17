namespace BlogCore.Validators
{
    public interface IValidator
    {
        public Type TypeForValidating { get; }
        public bool IsValid(object value);
    }
}
