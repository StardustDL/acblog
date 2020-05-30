using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcBlog.Server.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : BaseRecordController<User, string, IUserRepository, UserQueryRequest>
    {
        public UsersController(IUserRepository repository) : base(repository)
        {

        }
    }
}
