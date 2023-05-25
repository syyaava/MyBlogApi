namespace BlogCore.Validators
{
    public interface IMainDataValidatior
    {        
        public bool ValidateObject<T>(T value);
    }
}
