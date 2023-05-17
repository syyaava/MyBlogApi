using BlogCore.Validators;

namespace BlogCore.Exceptions
{
    public class ValidatorNotFoundException : NotFoundException<Type>
    {
        public ValidatorNotFoundException() : base() { }
        public ValidatorNotFoundException(string message) : base(message) { }
        public ValidatorNotFoundException(Type type) : base($"Validator for type \"{type}\" not found.") { }
        public ValidatorNotFoundException(string message, Type typeValidator) : base(message, typeValidator) { }
    }
}
