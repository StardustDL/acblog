using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using AcBlog.Client.WebAssembly.Models;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using Microsoft.AspNetCore.Http;
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
