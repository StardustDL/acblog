using AcBlog.Sdk;
using AcBlog.Tools.Sdk.Commands;
using AcBlog.Tools.Sdk.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.CommandLine;
using System.CommandLine.Builder;
using System.CommandLine.Hosting;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var rootCommand = new RootCommand("AcBlog SDK for command-line.");

            rootCommand.AddCommand(new InitCommand().Build());
            rootCommand.AddCommand(new ConnectCommand().Build());
            rootCommand.AddCommand(new LoginCommand().Build());
            rootCommand.AddCommand(new LogoutCommand().Build());
            rootCommand.AddCommand(new ListCommand().Build());
            rootCommand.AddCommand(new NewCommand().Build());
            rootCommand.AddCommand(new PullCommand().Build());
            rootCommand.AddCommand(new PushCommand().Build());

            var parser = new CommandLineBuilder(rootCommand)
                .UseHost(args =>
                {
                    var host = Host.CreateDefaultBuilder();

                    host.ConfigureAppConfiguration((context, config) =>
                    {
                        config.AddJsonFile(Workspace.OptionPath,
                            optional: true,
                            reloadOnChange: true);
                        config.AddJsonFile(Workspace.DBPath,
                            optional: true,
                            reloadOnChange: true);
                        config.AddEnvironmentVariables("ACBLOG_");
                        config.AddCommandLine(args);
                    });

                    return host;
                }, host =>
                {
                    host.ConfigureServices((context, services) =>
                    {
                        services.AddLogging();
                        services.AddHttpClient();
                        services.AddOptions()
                            .Configure<WorkspaceOption>(context.Configuration.GetSection("acblog"))
                            .Configure<DB>(context.Configuration.GetSection("db"));
                        services.AddSingleton<Workspace>();
                    });
                })
                .Build();

            return await parser.InvokeAsync(args);
        }
    }
}
