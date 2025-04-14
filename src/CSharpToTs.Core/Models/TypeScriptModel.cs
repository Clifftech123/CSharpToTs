namespace CSharpToTs.Core.Models
{
    
    public class TypeScriptModel
    {
        public string Name { get; set; }
        public List<TypeScriptProperty> Properties { get; set; } = new List<TypeScriptProperty>();
    }

    public class TypeScriptProperty
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public bool IsOptional { get; set; }
    }
}
