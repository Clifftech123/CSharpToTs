using CSharpToTs.Core.Interfaces;
using CSharpToTs.Core.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace CSharpToTs.Core.Parsers
{
    public class CSharpParser : IParser
    {
        private readonly TypeMapper _typeMapper;

        public CSharpParser(TypeMapper typeMapper)
        {
            _typeMapper = typeMapper;
        }

        public async Task<IEnumerable<CSharpModel>> ParseDirectoryAsync(string directoryPath)
        {
            var models = new List<CSharpModel>();
            var files = Directory.GetFiles(directoryPath, "*.cs", SearchOption.AllDirectories);
            
            foreach (var file in files)
            {
                try
                {
                    var model = await ParseFileAsync(file);
                    if (model != null)
                    {
                        models.Add(model);
                    }
                }
                catch (Exception ex)
                {
                    // Log error or handle exception
                    Console.WriteLine($"Error parsing file {file}: {ex.Message}");
                }
            }
            
            return models;
        }

        public async Task<CSharpModel> ParseFileAsync(string filePath)
        {
            // Uses Roslyn to read C# file
            var syntaxTree = CSharpSyntaxTree.ParseText(
                await File.ReadAllTextAsync(filePath));
            
            // Extracts class information
            var classModel = ExtractClassModel(syntaxTree);
            
            // Maps types using TypeMapper
            return MapTypes(classModel);
        }

        private CSharpModel ExtractClassModel(SyntaxTree syntaxTree)
        {
            var root = syntaxTree.GetCompilationUnitRoot();
            var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            
            if (classDeclaration == null)
                return null;
                
            var model = new CSharpModel
            {
                Name = classDeclaration.Identifier.Text,
                Properties = ExtractProperties(classDeclaration)
                
            };
            
            return model;
        }

        private List<PropertyInfo> ExtractProperties(ClassDeclarationSyntax classDeclaration)
        {
            var properties = new List<PropertyInfo>();
            
            foreach (var propertyDeclaration in classDeclaration.DescendantNodes().OfType<PropertyDeclarationSyntax>())
            {
                var property = new PropertyInfo
                {
                    Name = propertyDeclaration.Identifier.Text,
                    Type = propertyDeclaration.Type.ToString(),
                    Attributes = ExtractAttributes(propertyDeclaration)
                };
                
                properties.Add(property);
            }
            
            return properties;
        }

        private List<AttributeInfo> ExtractAttributes(PropertyDeclarationSyntax propertyDeclaration)
        {
            var attributes = new List<AttributeInfo>();
            
            foreach (var attributeList in propertyDeclaration.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    var attributeInfo = new AttributeInfo
                    {
                        Name = attribute.Name.ToString(),
                        Arguments = new Dictionary<string, string>()
                    };
                    
                    if (attribute.ArgumentList != null)
                    {
                        foreach (var argument in attribute.ArgumentList.Arguments)
                        {
                            attributeInfo.Arguments[argument.NameEquals?.Name.ToString() ?? ""] = argument.Expression.ToString();
                        }
                    }
                    
                    attributes.Add(attributeInfo);
                }
            }
            
            return attributes;
        }

        private CSharpModel MapTypes(CSharpModel model)
        {
            if (model == null)
                return null;
                
            foreach (var property in model.Properties)
            {
                property.Type = _typeMapper.MapToTypeScript(property.Type);
            }
            
            return model;
        }
    }
}
