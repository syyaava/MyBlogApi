using BlogCore.Db;
using BlogCore.Exceptions;
using BlogCore.Validators;
using System.ComponentModel.DataAnnotations;

namespace BlogCore.Blog
{
    public class Blog //TODO: Реализовать валидацию данных
    { 
        public readonly Guid Id;

        private readonly User user;
        //TODO: Добавить кэш сообщений.
        private readonly IBlogDbProvider dbProvider;
        private readonly IMainDataValidatior validator;

        public Blog(User user, IBlogDbProvider dbProvider, IMainDataValidatior validator)
        {            
            this.user = user;
            this.dbProvider = dbProvider;
            Id = Guid.NewGuid();
            this.validator = validator;

            validator.ValidateObject(this.user);
        }

        public ActionResult<BlogMessage> GetMessage(string id)
        {
            try
            {
                ValidateId(id);

                var message = dbProvider.GetMessage(id);
                return new ActionResult<BlogMessage>(message, StatusCode.Success);
            }
            catch (NotFoundInDbException<string>)
            {
                return new ActionResult<BlogMessage>(null, StatusCode.NotFound);
            }
            catch (ValidatorNotFoundException)
            {
                return new ActionResult<BlogMessage>(null, StatusCode.Error, new Exception("Internal server error."));
            }
            catch (Exception ex)
            {
                return new ActionResult<BlogMessage>(null, StatusCode.Error, ex);
            }
        }

        private void ValidateId(string id)
        {
            var blogMessageForIdValidation = new BlogMessage(id, user.Id, "Message", DateTime.Now, DateTime.Now);
            if (!validator.ValidateObject(blogMessageForIdValidation))
                throw new ValidationException();
        }

        public ActionResult<IEnumerable<BlogMessage>> GetAllUserMessages()
        {
            try
            {
                var messages = dbProvider.GetAllUserMessages(user.Id);
                return new ActionResult<IEnumerable<BlogMessage>>(messages, StatusCode.Success);
            }
            catch (NotFoundInDbException<string>)
            {
                return new ActionResult<IEnumerable<BlogMessage>>(null, StatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return new ActionResult<IEnumerable<BlogMessage>>(null, StatusCode.Error, ex);
            }
        }

        public ActionResult<IEnumerable<BlogMessage>> GetLastUserMessages(int count = 10)
        {
            try
            {
                if(count <= 0) throw new ArgumentOutOfRangeException($"Count out of range. Count value: {count}");
                var messages = dbProvider.GetLastUserMessages(user.Id, count);
                return new ActionResult<IEnumerable<BlogMessage>>(messages, StatusCode.Success);
            }
            catch(NotFoundInDbException<string>)
            {
                return new ActionResult<IEnumerable<BlogMessage>>(null, StatusCode.NotFound);
            }
            catch(ArgumentOutOfRangeException ex)
            {
                return new ActionResult<IEnumerable<BlogMessage>>(null, StatusCode.Error, ex);
            }
            catch(Exception ex)
            {
                return new ActionResult<IEnumerable<BlogMessage>>(null, StatusCode.Error, ex);
            }
        }

        public ActionResult<IEnumerable<BlogMessage>> AddMessages(params BlogMessage[] messages)
        {
            try
            {
                if (messages == null)
                    throw new ArgumentNullException(nameof(messages), "Message collection is null.");
                if (messages.Length == 0)
                    throw new ArgumentException("Message collection doesn't contains objects.");

                foreach(var message in messages)
                {
                    validator.ValidateObject(message);
                }

                var addedMessages = dbProvider.AddMessages(messages);
                return new ActionResult<IEnumerable<BlogMessage>>(addedMessages, StatusCode.Success);
            }
            catch(AlreadyExistException ex)
            {
                return new ActionResult<IEnumerable<BlogMessage>>(new List<BlogMessage>() { (BlogMessage)ex.Object! }, StatusCode.Error, ex);
            }
            catch(Exception ex)
            {
                return new ActionResult<IEnumerable<BlogMessage>>(null, StatusCode.Error, ex);
            }
        }

        public ActionResult<BlogMessage> RemoveMessage(string id)
        {
            try
            {
                ValidateId(id);
                var removedMessage = dbProvider.RemoveMessage(id);
                return new ActionResult<BlogMessage>(removedMessage, StatusCode.Success);
            }
            catch(NotFoundInDbException<string>)
            {
                return new ActionResult<BlogMessage>(null, StatusCode.NotFound);
            }
            catch(Exception ex)
            {
                return new ActionResult<BlogMessage>(null, StatusCode.Error, ex);
            }
        }

        public ActionResult<BlogMessage> UpdateMessage(string id, BlogMessage newMessage)
        {
            try
            {
                ValidateId(id);
                validator.ValidateObject(newMessage);

                var updatedMessage = dbProvider.UpdateMessage(id, newMessage);
                return new ActionResult<BlogMessage>(updatedMessage, StatusCode.Success);
            }
            catch(NotFoundInDbException<string>)
            {
                return new ActionResult<BlogMessage>(null, StatusCode.NotFound);
            }
            catch(Exception ex)
            {
                return new ActionResult<BlogMessage>(null, StatusCode.Error, ex);
            }
        }
    }
}
