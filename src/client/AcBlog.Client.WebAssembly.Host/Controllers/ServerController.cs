using AcBlog.Client.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AcBlog.Client.WebAssembly.Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        public ServerController(IOptions<ServerSettings> serverSettings, IOptions<BuildStatus> buildStatus)
        {
            ServerSettings = serverSettings.Value;
            BuildStatus = buildStatus.Value;
        }

        public ServerSettings ServerSettings { get; }

        public BuildStatus BuildStatus { get; }

        [HttpGet("Test")]
        public bool Test() => true;

        [HttpGet("Server")]
        public object Server() => new { Server = ServerSettings };

        [HttpGet("Build")]
        public object Build() => new { Build = BuildStatus };
    }
}
