using BlogCore.Exceptions;

namespace BlogCore.Validators
{
    public class MainDataValidator : IMainDataValidatior
    {
        private readonly IValidator[] validators;

        public MainDataValidator(params IValidator[] validators)
        {
            if(validators == null || validators.Length == 0)
                throw new ArgumentNullException(nameof(validators), "Passed Validators collection is null or empty.");

            this.validators = new IValidator[validators.Length];
            validators.CopyTo(this.validators, 0);
        }

        public bool ValidateObject<T>(T value)
        {
            if (value == null)
                throw new ArgumentNullException($"Validating object cannot be null. Object type: {typeof(T)}");

            IValidator validator = FindValidator(value.GetType());

            return validator.IsValid(value);
        }

        private IValidator FindValidator(Type type)
        {
            var validator = validators.FirstOrDefault(v => v.TypeForValidating == type);
            return validator ?? throw new ValidatorNotFoundException(type);
        }
    }
}
