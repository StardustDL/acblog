using AcBlog.Data.Models;
using AcBlog.Data.Models.Actions;
using AcBlog.Data.Repositories.Searchers;
using AcBlog.Services;
using AcBlog.Services.Models;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Sdk.Api
{
    internal class PageService : BaseRecordApiService<Page, PageQueryRequest>, IPageService
    {
        public PageService(IBlogService blog, HttpClient httpClient) : base(blog, httpClient)
        {
        }

        protected override string PrepUrl => "/Pages";
    }

    internal class UserService : BaseRecordApiService<User, UserQueryRequest>, IUserService
    {
        public UserService(IBlogService blog, HttpClient httpClient) : base(blog, httpClient)
        {
        }

        protected override string PrepUrl => "/Users";

        public async Task<bool> ChangePassword(UserChangePasswordRequest request, CancellationToken cancellationToken = default)
        {
            SetHeader();
            using var responseMessage = await HttpClient.PostAsJsonAsync($"{PrepUrl}/changePassword", request, cancellationToken).ConfigureAwait(false);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<bool>(cancellationToken: cancellationToken).ConfigureAwait(false);
        }

        public async Task<string> Login(UserLoginRequest request, CancellationToken cancellationToken = default)
        {
            SetHeader();
            using var responseMessage = await HttpClient.PostAsJsonAsync($"{PrepUrl}/login", request, cancellationToken).ConfigureAwait(false);
            responseMessage.EnsureSuccessStatusCode();
            return await responseMessage.Content.ReadFromJsonAsync<string>(cancellationToken: cancellationToken).ConfigureAwait(false) ?? string.Empty;
        }
    }
}
