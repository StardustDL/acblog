using AcBlog.Tools.Sdk.Helpers;
using AcBlog.Tools.Sdk.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Commands.Remotes
{
    public class LoginCommand : BaseCommand<LoginCommand.CArgument>
    {
        public override string Name => "login";

        public override string Description => "";

        public override Command Configure()
        {
            var result = base.Configure();
            result.AddArgument(new Argument<string>(nameof(CArgument.Name).ToLowerInvariant(), () => string.Empty, "Remote server name"));
            return result;
        }

        public override async Task<int> Handle(CArgument argument, IHost host, CancellationToken cancellationToken)
        {
            Workspace workspace = host.Services.GetRequiredService<Workspace>();
            var logger = host.Services.GetRequiredService<ILogger<LoginCommand>>();
            if (string.IsNullOrWhiteSpace(argument.Name))
            {
                argument.Name = workspace.Option.CurrentRemote;
            }
            if (workspace.Option.Remotes.TryGetValue(argument.Name, out var remote))
            {
                await workspace.Connect(remote.Name);

                string userName = string.Empty, password = string.Empty;

                {
                    if (string.IsNullOrEmpty(userName))
                        userName = ConsoleExtensions.Input("Input username: ");
                    if (string.IsNullOrEmpty(password))
                        password = ConsoleExtensions.InputPassword("Input password: ");
                }

                try
                {
                    var token = await workspace.Remote.UserService.Login(new Services.Models.UserLoginRequest
                    {
                        UserName = userName,
                        Password = password
                    });
                    if (string.IsNullOrEmpty(token))
                    {
                        logger.LogError("Login failed.");
                    }
                    else
                    {
                        remote.Token = token;
                        await workspace.Save();
                    }
                }
                catch
                {
                    logger.LogError("Login failed.");
                }
            }
            return 0;
        }

        public class CArgument
        {
            public string Name { get; set; } = string.Empty;
        }
    }
}
