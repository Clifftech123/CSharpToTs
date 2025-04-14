

using CSharpToTs.Core.Models;

namespace CSharpToTs.Core.Interfaces
{
    public interface IGenerator
{
    Task GenerateAsync(CSharpModel model, string outputPath);
    Task GenerateValidationAsync(CSharpModel model, string outputPath, string validationType);
}

}