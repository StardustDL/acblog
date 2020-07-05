using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AcBlog.Tools.Sdk.Commands
{
    public abstract class BaseCommand<T>
    {
        public abstract string Name { get; }

        public abstract string Description { get; }

        protected virtual bool DisableHandler { get; } = false;

        public virtual Command Configure()
        {
            var result = new Command(Name, Description);
            return result;
        }

        public virtual Task<int> Handle(T argument, IHost host, CancellationToken cancellationToken) => Task.FromResult(0);

        private async Task<int> HandleWrapper(T argument, IHost host, IConsole console, CancellationToken cancellationToken)
        {
            try
            {
                try
                {
                    return await Handle(argument, host, cancellationToken);
                }
                catch (OperationCanceledException)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        console.Error.WriteLine("Operation has been cancelled.");
                        return -1;
                    }
                    throw;
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                throw;
#else
                console.Error.WriteLine($"Error occurs: {ex.Message}.");
                return 1;
#endif
            }
        }

        public virtual Command Build()
        {
            Command command = Configure();
            if (!DisableHandler)
                command.Handler = CommandHandler.Create<T, IHost, IConsole, CancellationToken>(HandleWrapper);
            return command;
        }
    }
}
