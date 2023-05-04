using BlogCore.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MyBlogAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private ILogger logger;

        public BlogController(ILogger logger)
        {
            this.logger = logger;
        }

        //public ActionResult<BlogMessageDTO> AddMessage()
        //{
            
        //}

        //public ActionResult RemoveMessage()
        //{

        //}

        //public ActionResult UpdateMessage()
        //{

        //}

        //[HttpGet]
        //public ActionResult<BlogMessageDTO> GetMessage()
        //{

        //}

        //public ActionResult<IEnumerable<BlogMessageDTO>> GetAllMessages(string userId)
        //{

        //}

        //public ActionResult<IEnumerable<BlogMessageDTO>> GetLastMessages(string userId, int count)
        //{

        //}
    }
}
