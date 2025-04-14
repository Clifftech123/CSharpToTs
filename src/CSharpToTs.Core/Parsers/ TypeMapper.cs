

namespace CSharpToTs.Core.Parsers
{
   public class TypeMapper
{
    private readonly Dictionary<string, string> _typeMap = new()
    {
        ["string"] = "string",
        ["int"] = "number",
        ["DateTime"] = "string",
        ["bool"] = "boolean",
        ["decimal"] = "number",
        ["List<string>"] = "string[]",
        ["List<int>"] = "number[]",
        ["List<decimal>"] = "number[]",
        ["List<bool>"] = "boolean[]",
        ["List<DateTime>"] = "string[]",
        ["List<object>"] = "any[]",
        
    };

    public string MapToTypeScript(string csharpType)
    {
        return _typeMap.GetValueOrDefault(csharpType, "any");
    }
}

}