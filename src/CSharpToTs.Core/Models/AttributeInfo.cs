
namespace CSharpToTs.Core.Models
{
   // Represents C# attributes like [Required], [EmailAddress]
public class AttributeInfo
{
    public string Name { get; set; }           // Attribute name
    public Dictionary<string, string> Arguments { get; set; } // Attribute parameters
}

}