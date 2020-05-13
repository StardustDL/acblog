using AcBlog.Data.Repositories;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AcBlog.Client.WebAssembly
{
    public static class Utils
    {
        public static async Task UseToken(this IRepository repository, IAccessTokenProvider provider)
        {
            var tokenResult = await provider.RequestAccessToken();
            if (tokenResult.TryGetToken(out var token))
            {
                if (repository.Context == null)
                    repository.Context = new RepositoryAccessContext();
                repository.Context.Token = token.Value;
            }
        }
    }
}
