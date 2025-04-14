namespace CSharpToTs.Core.Interfaces
{
    public interface ITemplateService
    {
        Task<ITemplate> GetTemplateAsync(string templateName);
    }

    public interface ITemplate
    {
        string Render(object model);
    }
}
