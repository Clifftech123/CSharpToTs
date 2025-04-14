

namespace CSharpToTs.Core.Models
{
  // Represents a C# class structure
public class CSharpModel
{

    // Class Name 
    public required string  Name { get; set; }  
    
     // Namespace                
    public string? Namespace { get; set; }  

    // Class properties : Properties going to be in the class            
    public   List<PropertyInfo>? Properties { get; set; } 
     // Class attribute is provided 
    public List<AttributeInfo>? Attributes { get; set; } 

    // Is it a class or interface
    public bool IsClass { get; set; }                  
}

}