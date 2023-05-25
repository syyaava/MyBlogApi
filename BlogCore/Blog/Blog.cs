using BlogCore.Db;
using BlogCore.Exceptions;
using BlogCore.Validators;
using System.ComponentModel.DataAnnotations;

namespace BlogCore.Blog
{
    public class Blog //TODO: Реализовать валидацию данных
    { //TODO: Вынести валидаторы в отдельный класс
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
        }

        public ActionResult<BlogMessage> GetMessage(string id)
        {
            try
            {
                if (!validator.ValidateObject(id))
                    throw new ValidationException();

                var message = dbProvider.GetMessage(id);
                return new ActionResult<BlogMessage>(message, StatusCode.Success);
            }
            catch (NotFoundInDbException<string> ex)
            {
                return new ActionResult<BlogMessage>(null, StatusCode.NotFound);
            }
            //catch (ArgumentNullException ex)
            //{
            //    return new ActionResult<BlogMessage>(null, StatusCode.Error, ex);
            //}
            //catch (ArgumentException ex)
            //{
            //    return new ActionResult<BlogMessage>(null, StatusCode.Error, ex);
            //}
            catch (ValidatorNotFoundException)
            {
                return new ActionResult<BlogMessage>(null, StatusCode.Error, new Exception("Internal server error."));
            }
            catch (Exception ex)
            {
                return new ActionResult<BlogMessage>(null, StatusCode.Error, ex);
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
                if(count <= 0) count = 10;
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
    }
}
