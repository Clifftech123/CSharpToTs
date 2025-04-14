using CSharpToTs.Core.Interfaces;
using CSharpToTs.Core.Options;

namespace CSharpToTs.Core.Services
{
    public class GeneratorService
    {
        private readonly IParser _parser;
        private readonly IGenerator _generator;
        private readonly IFileWatcher _fileWatcher;

        public GeneratorService(IParser parser, IGenerator generator, IFileWatcher fileWatcher)
        {
            _parser = parser;
            _generator = generator;
            _fileWatcher = fileWatcher;
        }

        public async Task GenerateTypesAsync(GenerateOptions options)
        {
            // Parse C# files
            var models = await _parser.ParseDirectoryAsync(options.InputPath);

            // Generate TypeScript
            foreach (var model in models)
            {
                await _generator.GenerateAsync(model, options.OutputPath);
            }

            // Start watching if requested
            if (options.Watch)
            {
                _fileWatcher.StartWatching(options.InputPath,
                    async (file) => await RegenerateFile(file, options));
            }
        }

        private async Task RegenerateFile(string filePath, GenerateOptions options)
        {
            // Parse the changed file
            var model = await _parser.ParseFileAsync(filePath);

            // Generate TypeScript for the changed file
            await _generator.GenerateAsync(model, options.OutputPath);
        }
    }
}
