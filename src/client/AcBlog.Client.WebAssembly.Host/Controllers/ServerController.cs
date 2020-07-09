using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using AcBlog.Client.WebAssembly.Models;
using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AcBlog.Client.WebAssembly.Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {
        public ServerController(ServerSettings serverSettings, BuildStatus buildStatus, IdentityProvider identityProvider)
        {
            ServerSettings = serverSettings;
            BuildStatus = buildStatus;
            IdentityProvider = identityProvider;
        }

        public ServerSettings ServerSettings { get; }

        public BuildStatus BuildStatus { get; }

        public IdentityProvider IdentityProvider { get; }

        [HttpGet("Identity")]
        public IdentityProvider Identity() => IdentityProvider;

        [HttpGet("Test")]
        public bool Test() => true;

        [HttpGet("Server")]
        public ServerSettings Server() => ServerSettings;

        [HttpGet("Build")]
        public BuildStatus Build() => BuildStatus;
    }
}
