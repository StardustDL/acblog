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

namespace AcBlog.Server.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public UsersController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IdentityServerTools identityServerTools, IBlogService blogService)
        {
            UserManager = userManager;
            IdentityServerTools = identityServerTools;
            SignInManager = signInManager;

            BlogService = blogService;
        }

        IdentityServerTools IdentityServerTools { get; }

        UserManager<ApplicationUser> UserManager { get; }

        SignInManager<ApplicationUser> SignInManager { get; }

        IBlogService BlogService { get; }

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
                if (!await BlogService.UserService.Exists(user.Id))
                {
                    await BlogService.UserService.Create(new AcBlog.Data.Models.User
                    {
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

#if DEBUG
        [Authorize]
        [HttpGet("check_token")]
        public string CheckToken()
        {
            return $"{User.Identity.GetSubjectId()}:{User.GetDisplayName()}:{User.IsAuthenticated()}";
        }
#endif
    }
}
