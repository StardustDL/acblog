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
    public class BaseRecordController<T, TId, TRepo, TQuery> : ControllerBase where TId : class where T : class, IHasId<TId> where TRepo : IRecordRepository<T, TId, TQuery>
    {
        TRepo Repository { get; }

        protected BaseRecordController(TRepo repository)
        {
            Repository = repository;
        }

        [HttpGet("actions/read")]
        public async Task<ActionResult<bool>> CanRead()
        {
            return await Repository.CanRead();
        }

        [HttpGet("actions/write")]
        public async Task<ActionResult<bool>> CanWrite()
        {
            return await Repository.CanWrite();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<IEnumerable<TId>>> All()
        {
            if (!await Repository.CanRead())
                return BadRequest();
            return Ok(await Repository.All());
        }

        [HttpPut("query")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<QueryResponse<TId>>> Query([FromBody] TQuery query)
        {
            if (!await Repository.CanRead())
                return BadRequest();
            return Ok(await Repository.Query(query));
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<T>> Get(TId id)
        {
            if (!await Repository.CanRead())
                return BadRequest();
            if (await Repository.Exists(id))
                return Ok(await Repository.Get(id));
            else
                return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesDefaultResponseType]
        [Authorize]
        public async Task<ActionResult<TId>> Create([FromBody] T value)
        {
            if (!await Repository.CanWrite())
                return BadRequest();
            var result = await Repository.Create(value);
            return Created(result.ToString(), result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [Authorize]
        public async Task<ActionResult<bool>> Update(TId id, [FromBody] T value)
        {
            if (!await Repository.CanWrite())
                return BadRequest();
            value.Id = id;
            if (await Repository.Exists(value.Id))
                return Ok(await Repository.Update(value));
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [Authorize]
        public async Task<ActionResult<bool>> Delete(TId id)
        {
            if (!await Repository.CanWrite())
                return BadRequest();
            if (await Repository.Exists(id))
                return Ok(await Repository.Delete(id));
            else
                return NotFound();
        }
    }
}
