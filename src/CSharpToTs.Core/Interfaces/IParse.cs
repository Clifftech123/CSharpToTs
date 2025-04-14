

using CSharpToTs.Core.Models;

namespace CSharpToTs.Core.Interfaces
{
    public interface IParser
{
    Task<CSharpModel> ParseFileAsync(string filePath);
    Task<IEnumerable<CSharpModel>> ParseDirectoryAsync(string directoryPath);
}

}