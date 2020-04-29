using System.Collections.Generic;
using System.Threading.Tasks;
using AcBlog.Data.Models;
using AcBlog.Data.Providers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcBlog.Server.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        IPostProvider Provider { get; }

        public PostsController(IPostProvider provider)
        {
            Provider = provider;
        }

        [HttpGet]
        public ActionResult<IAsyncEnumerable<Post>> All() => Ok(Provider.All());

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<Post>> Get(string id)
        {
            if (await Provider.Exists(id))
                return Ok(await Provider.Get(id));
            else
                return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> Create([FromBody] Post value)
        {
            var result = await Provider.Create(value);
            return Created(result, result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<bool>> Update(string id, [FromBody] Post value)
        {
            value.Id = id;
            if (await Provider.Exists(value.Id))
                return Ok(await Provider.Update(value));
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<bool>> Delete(string id)
        {
            if (await Provider.Exists(id))
                return Ok(await Provider.Delete(id));
            else
                return NotFound();
        }
    }
}
