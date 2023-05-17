using BlogCore.Db;
using BlogCore.Exceptions;
using BlogCore.Validators;

namespace BlogCore.Blog
{
    public class Blog //TODO: Реализовать валидацию данных
    {
        public readonly Guid Id;

        private readonly User user;
        //TODO: Добавить кэш сообщений.
        private IBlogDbProvider dbProvider;
        private IEnumerable<IValidator> validators;

        public Blog(User user, IBlogDbProvider dbProvider, IEnumerable<IValidator> validators)
        {
            this.user = user;
            this.dbProvider = dbProvider;
            Id = Guid.NewGuid();
            this.validators = validators;
        }

        public ActionResult<BlogMessage> GetMessage(string id)
        {
            try
            {
                //if (!ValidateValue(id))
                //    throw new NotValidValueException();

                var message = dbProvider.GetMessage(id);
                return new ActionResult<BlogMessage>(message, StatusCode.Success);
            }
            catch (NotFoundInDbException<string> ex)
            {
                return new ActionResult<BlogMessage>(null, StatusCode.NotFound);
            }//TODO: обработчики для исключений в валидаторе
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public ActionResult<IEnumerable<BlogMessage>> GetAllUserMessages()
        {
            try
            {
                var messages = dbProvider.GetAllUserMessages(user.Id);
                return new ActionResult<IEnumerable<BlogMessage>>(messages, StatusCode.Success);
            }
            catch (NotFoundInDbException<IEnumerable<BlogMessage>> ex)
            {
                return new ActionResult<IEnumerable<BlogMessage>>(Enumerable.Empty<BlogMessage>(), StatusCode.NotFound);
            }
            catch (Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public ActionResult<IEnumerable<BlogMessage>> GetLastUserMessages(int count = 10)
        {
            try
            {
                var messages = dbProvider.GetLastUserMessages(user.Id, count);
                return new ActionResult<IEnumerable<BlogMessage>>(messages, StatusCode.Success);
            }
            catch(NotFoundInDbException<IEnumerable<BlogMessage>> ex)
            {
                return new ActionResult<IEnumerable<BlogMessage>>(Enumerable.Empty<BlogMessage>(), StatusCode.NotFound);
            }
            catch(ArgumentOutOfRangeException ex)
            {
                throw new NotImplementedException();
            }
            catch(Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public ActionResult<IEnumerable<BlogMessage>> AddMessages(params BlogMessage[] messages)
        {
            try
            {
                var addedMessages = dbProvider.AddMessages(messages);
                return new ActionResult<IEnumerable<BlogMessage>>(addedMessages, StatusCode.Success);
            }
            catch(AlreadyExistException ex)
            {
                return new ActionResult<IEnumerable<BlogMessage>>(new List<BlogMessage>() { (BlogMessage)ex.Object! }, StatusCode.Error, ex);
            }
            catch(Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public ActionResult<BlogMessage> RemoveMessage(string id)
        {
            try
            {
                var removedMessage = dbProvider.RemoveMessage(id);
                return new ActionResult<BlogMessage>(removedMessage, StatusCode.Success);
            }
            catch(NotFoundInDbException<string> ex)
            {
                return new ActionResult<BlogMessage>(null, StatusCode.NotFound);
            }
            catch(Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        public ActionResult<BlogMessage> UpdateMessage(string id, BlogMessage newMessage)
        {
            try
            {
                var updatedMessage = dbProvider.UpdateMessage(id, newMessage);
                return new ActionResult<BlogMessage>(updatedMessage, StatusCode.Success);
            }
            catch(NotFoundInDbException<string> ex)
            {
                return new ActionResult<BlogMessage>(null, StatusCode.NotFound);
            }
            catch(Exception ex)
            {
                throw new NotImplementedException();
            }
        }

        private bool ValidateValue<T>(T obj)
        {
            if (obj == null)
                throw new ArgumentNullException($"Validating object cannot be null. Object type: {typeof(T)}");

            IValidator validator = FindValidator(obj);

            return validator.IsValid(obj);
        }

        private IValidator FindValidator<T>(T obj)
        {
            var validator = validators.FirstOrDefault(v => v.TypeForValidating == obj.GetType());
            return validator == null ? throw new ValidatorNotFoundException(obj.GetType()) : validator;
        }
    }
}
