using AcBlog.Data.Models.Actions;
using AcBlog.Server.Api.Models;
using AcBlog.Services;
using AcBlog.Services.Models;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using AcBlog.Data.Repositories;
using AcBlog.Data.Models;

namespace AcBlog.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : RecordControllerBase<User, IUserService, UserQueryRequest>
    {
        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IdentityServerTools identityServerTools, IBlogService service) : base(service.UserService)
        {
            UserManager = userManager;
            IdentityServerTools = identityServerTools;
            SignInManager = signInManager;
        }

        IdentityServerTools IdentityServerTools { get; }

        UserManager<ApplicationUser> UserManager { get; }

        SignInManager<ApplicationUser> SignInManager { get; }


        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<string>> Login([FromBody] UserLoginRequest request)
        {
            var user = await UserManager.FindByNameAsync(request.UserName);

            if (user is null)
            {
                return NotFound();
            }

            var result = await SignInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded)
            {
                if (!await Service.Exists(user.Id))
                {
                    await Service.Create(new AcBlog.Data.Models.User
                    {
                        Name = user.UserName,
                        NickName = user.UserName,
                        Email = user.UserName,
                        Id = user.Id
                    });
                }

                var token = await IdentityServerTools.IssueClientJwtAsync(
                   clientId: "Internal",
                   lifetime: 3600,
                   scopes: new string[] { IdentityServerConstants.StandardScopes.OpenId, IdentityServerConstants.StandardScopes.Profile },
                   audiences: new string[] { "AcBlog.Server.ApiAPI" },
                   additionalClaims: new Claim[] {
                       new Claim(JwtClaimTypes.Subject, user.Id),
                       new Claim(JwtClaimTypes.Name, user.UserName),
                   });
                return token;
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost("changePassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<bool>> ChangePassword([FromBody] UserChangePasswordRequest request)
        {
            var user = await UserManager.FindByNameAsync(request.LoginRequest.UserName);

            if (user is null)
            {
                return NotFound();
            }

            var result = await UserManager.ChangePasswordAsync(user, request.LoginRequest.Password, request.NewPassword);

            if (result.Succeeded)
            {
                return true;
            }
            else
            {
                return Unauthorized();
            }
        }

        [Authorize]
        [HttpGet("current")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public async Task<ActionResult<User>> Current()
        {
            return await Service.Get(User.Identity.GetSubjectId());
        }
    }
}
