using CSharpToTs.Core.Interfaces;
using Scriban;

namespace CSharpToTs.Core.Services
{
    public class TemplateService : ITemplateService
    {
        private readonly string _templateDirectory;

        public TemplateService(string templateDirectory)
        {
            _templateDirectory = templateDirectory;
        }

        public async Task<ITemplate> GetTemplateAsync(string templateName)
        {
            var templatePath = Path.Combine(_templateDirectory, $"{templateName}.scriban");
            var templateContent = await File.ReadAllTextAsync(templatePath);
            return new ScribanTemplate(templateContent);
        }

        private class ScribanTemplate : ITemplate
        {
            private readonly string _templateContent;
            private readonly Template _template;

            public ScribanTemplate(string templateContent)
            {
                _templateContent = templateContent;
                _template = Template.Parse(templateContent);
            }

            public string Render(object model)
            {
                return _template.Render(model);
            }
        }
    }
}
