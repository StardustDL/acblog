using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace AcBlog.Server.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class KeywordsController : BaseRecordController<Keyword, string, IKeywordRepository, KeywordQueryRequest>
    {
        public KeywordsController(IKeywordRepository repository) : base(repository)
        {

        }
    }
}
