using System.Collections.Generic;
using System.Threading.Tasks;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcBlog.Server.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CategoriesController : BaseRecordController<Category, string, ICategoryRepository, CategoryQueryRequest>
    {
        public CategoriesController(ICategoryRepository repository) : base(repository)
        {

        }
    }
}
