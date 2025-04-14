using CSharpToTs.Core.Interfaces;
using CSharpToTs.Core.Models;


namespace CSharpToTs.Core.Generators
{
    public class ValidationGenerator
    {
        private readonly ITemplateService _templateService;
        
        public ValidationGenerator(ITemplateService templateService)
        {
            _templateService = templateService;
        }
        
        public async Task GenerateValidationAsync(CSharpModel model, string outputPath, string validationType)
        {
            // Normalize validation type to lowercase
            validationType = validationType.ToLower();
            
            // Check if validation type is supported
            if (validationType != "zod" && validationType != "yup")
            {
                throw new NotSupportedException($"Validation type '{validationType}' is not supported. Supported types: zod, yup");
            }
            
            // Skip models without any validation attributes
            if (!HasValidationAttributes(model))
            {
                Console.WriteLine($"Skipping validation generation for {model.Name} - no validation attributes found");
                return;
            }
            
            // Load appropriate template
            var template = await _templateService.GetTemplateAsync(validationType);
            
            // Prepare model for template
            var templateModel = new
            {
                model = new
                {
                    name = model.Name,
                    properties = model.Properties.Select(p => new
                    {
                        name = p.Name,
                        typeScriptType = p.TypeScriptType,
                        isRequired = p.IsRequired || p.Attributes.Any(a => a.Name == "Required"),
                        isNullable = p.IsNullable,
                        attributes = p.Attributes.Select(a => new
                        {
                            name = a.Name,
                            arguments = a.Arguments
                        }).ToList()
                    }).ToList()
                }
            };
            
            // Generate validation schema
            var validationCode = template.Render(templateModel);
            
            // Ensure output directory exists
            Directory.CreateDirectory(outputPath);
            
            // Write to file
            string fileName = $"{model.Name}.{validationType}.ts";
            await File.WriteAllTextAsync(
                Path.Combine(outputPath, fileName),
                validationCode);
                
            Console.WriteLine($"Generated {validationType} validation schema for {model.Name}");
        }
        
        private bool HasValidationAttributes(CSharpModel model)
        {
            // Check if any property has validation attributes
            return model.Properties.Any(p => 
                p.Attributes.Any(a => IsValidationAttribute(a.Name)));
        }
        
        private bool IsValidationAttribute(string attributeName)
        {
            // List of common validation attributes in C#
            var validationAttributes = new[]
            {
                "Required",
                "Range",
                "StringLength",
                "MinLength",
                "MaxLength",
                "RegularExpression",
                "EmailAddress",
                "Phone",
                "Url",
                "CreditCard",
                "Compare"
            };
            
            return validationAttributes.Contains(attributeName.Replace("Attribute", ""));
        }
    }
}
