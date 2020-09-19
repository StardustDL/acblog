using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AcBlog.Server.Api.Controllers
{
    public class RecordControllerBase<T, TId, TService, TQuery> : ControllerBase where TId : class where T : class, IHasId<TId> where TService : IRecordRepository<T, TId, TQuery> where TQuery : QueryRequest, new()
    {
        protected TService Service { get; }

        protected RecordControllerBase(TService repository)
        {
            Service = repository;
        }

        [HttpGet("status")]
        public virtual async Task<ActionResult<RepositoryStatus>> GetStatus()
        {
            return await Service.GetStatus();
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public virtual IAsyncEnumerable<TId> All()
        {
            return Service.All();
        }

        [HttpPut("query")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public virtual IAsyncEnumerable<TId> Query([FromBody] TQuery query)
        {
            return Service.Query(query);
        }

        [HttpPut("stats")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesDefaultResponseType]
        public virtual async Task<ActionResult<QueryStatistic>> Statistic([FromBody] TQuery query)
        {
            return await Service.Statistic(query);
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public virtual async Task<ActionResult<T>> Get(TId id)
        {
            if (await Service.Exists(id))
                return Ok(await Service.Get(id));
            else
                return NotFound();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesDefaultResponseType]
        [Authorize]
        public virtual async Task<ActionResult<TId>> Create([FromBody] T value)
        {
            var result = await Service.Create(value);
            return Created(result.ToString(), result);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [Authorize]
        public virtual async Task<ActionResult<bool>> Update(TId id, [FromBody] T value)
        {
            value.Id = id;
            if (await Service.Exists(value.Id))
                return Ok(await Service.Update(value));
            else
                return NotFound();
        }

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        [Authorize]
        public virtual async Task<ActionResult<bool>> Delete(TId id)
        {
            if (await Service.Exists(id))
                return Ok(await Service.Delete(id));
            else
                return NotFound();
        }
    }
}
