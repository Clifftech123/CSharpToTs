using System.CommandLine;
using CSharpToTs.Core.Options;
using CSharpToTs.Core.Services;

namespace CSharpToTs.Cli.Commands
{
    public class GenerateCommand : Command
    {
        private readonly GeneratorService _generatorService;

        public GenerateCommand(GeneratorService generatorService)
            : base("generate", "Generate TypeScript types")
        {
            _generatorService = generatorService;

            var inputOption = new Option<string>(
                name: "--input",
                description: "Input directory containing C# models");
            inputOption.IsRequired = true;
            AddOption(inputOption);

            var outputOption = new Option<string>(
                name: "--output",
                description: "Output directory for TypeScript files");
            outputOption.IsRequired = true;
            AddOption(outputOption);

            var watchOption = new Option<bool>(
                name: "--watch",
                description: "Watch for changes in input directory");
            AddOption(watchOption);


            var validationOption = new Option<string>(
          name: "--validation",
             description: "Generate validation schemas (zod, yup)");

            this.SetHandler(async (string input, string output, bool watch, string validation) =>
             {
                 var options = new GenerateOptions
                 {
                     InputPath = input,
                     OutputPath = output,
                     Watch = watch,
                     ValidationSchema = validation
                 };

                 await _generatorService.GenerateTypesAsync(options);
             }, inputOption, outputOption, watchOption, validationOption);
        }
    }
}
