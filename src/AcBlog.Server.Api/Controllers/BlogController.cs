using AcBlog.Data.Models;
using IdentityServer4;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AcBlog.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        public BlogController(IdentityServerTools identityServerTools)
        {
            IdentityServerTools = identityServerTools;
        }

        IdentityServerTools IdentityServerTools { get; }

        [HttpGet]
        public Task<BlogOptions> Options()
        {
            // TODO: update options
            return Task.FromResult(new BlogOptions());
        }

        [HttpGet("token")]
        public async Task<string> GetToken()
        {
            var token = await IdentityServerTools.IssueClientJwtAsync(
                clientId: "Internal",
                lifetime: 3600,
                audiences: new string[] { "AcBlog.Server.ApiAPI" });
            return token;
        }

        [Authorize]
        [HttpGet("check_token")]
        public async Task<string> CheckToken()
        {
            return "Passed";
        }
    }
}
