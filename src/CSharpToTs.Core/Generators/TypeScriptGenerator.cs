using CSharpToTs.Core.Interfaces;
using CSharpToTs.Core.Models;

namespace CSharpToTs.Core.Generators
{
    public class TypeScriptGenerator : IGenerator
    {
        private readonly ITemplateService _templateService;

        public TypeScriptGenerator(ITemplateService templateService)
        {
            _templateService = templateService;
        }

        public async Task GenerateAsync(CSharpModel model, string outputPath)
        {
            // Load template
            var template = await _templateService.GetTemplateAsync("interface");

            // Convert CSharpModel to a format suitable for the template
            var templateModel = new
            {
                model = new
                {
                    name = model.Name,
                    properties = model.Properties.Select(p => new
                    {
                        name = p.Name,
                        typeScriptType = p.TypeScriptType,
                        isOptional = !p.Attributes.Any(a => a.Name == "Required"),
                        attributes = p.Attributes.Select(a => new
                        {
                            name = a.Name,
                            arguments = a.Arguments
                        })
                    })
                }
            };

            // Generate TypeScript code
            var typeScriptCode = template.Render(templateModel);

            // Ensure output directory exists
            Directory.CreateDirectory(outputPath);

            // Write to file
            await File.WriteAllTextAsync(
                Path.Combine(outputPath, $"{model.Name}.ts"),
                typeScriptCode);
        }

        public async Task GenerateValidationAsync(CSharpModel model, string outputPath, string validationType)
        {
            if (validationType.ToLower() != "zod")
            {
                throw new NotSupportedException($"Validation type '{validationType}' is not supported.");
            }

            // Load template
            var template = await _templateService.GetTemplateAsync("zod");

            // Convert CSharpModel to a format suitable for the template
            var templateModel = new
            {
                model = new
                {
                    name = model.Name,
                    properties = model.Properties.Select(p => new
                    {
                        name = p.Name,
                        typeScriptType = p.TypeScriptType,
                        isOptional = !p.Attributes.Any(a => a.Name == "Required"),
                        attributes = p.Attributes.Select(a => new
                        {
                            name = a.Name,
                            arguments = a.Arguments
                        })
                    })
                }
            };

            // Generate validation schema
            var validationCode = template.Render(templateModel);

            // Ensure output directory exists
            Directory.CreateDirectory(outputPath);

            // Write to file
            await File.WriteAllTextAsync(
                Path.Combine(outputPath, $"{model.Name}.schema.ts"),
                validationCode);
        }
    }
}
