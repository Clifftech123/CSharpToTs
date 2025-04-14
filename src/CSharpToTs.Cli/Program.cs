using System.CommandLine;
using CSharpToTs.Cli.Commands;
using CSharpToTs.Core.Generators;
using CSharpToTs.Core.Interfaces;
using CSharpToTs.Core.Parsers;
using CSharpToTs.Core.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CSharpToTs.Cli
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            // Setup dependency injection
            var services = new ServiceCollection();
            
            // Register services
            services.AddSingleton<TypeMapper>();
            services.AddSingleton<IParser, CSharpParser>();
            services.AddSingleton<IGenerator, TypeScriptGenerator>();
            services.AddSingleton<IFileWatcher, FileWatcherService>();
            services.AddSingleton<ValidationGenerator>();
            services.AddSingleton<GeneratorService>();
            services.AddSingleton<ITemplateService>(provider => 
                new TemplateService(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates")));
            
            var serviceProvider = services.BuildServiceProvider();
            
            // Create root command
            var rootCommand = new RootCommand("CSharpToTs - Generate TypeScript from C# models");
            
            // Add subcommands
            rootCommand.AddCommand(serviceProvider.GetRequiredService<GenerateCommand>());
         
            
            // Execute command
            return await rootCommand.InvokeAsync(args);
        }
    }
}
