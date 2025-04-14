

namespace CSharpToTs.Core.Models
{
   
   // Represents a C# class property with its metadata
public class PropertyInfo
{

    // Property Name from the class 
       public required string Name { get; set; }      

       // Class Type            
       public required string Type { get; set; }  
       
       // TypeScript equivalent type
       public string TypeScriptType { get; set; }
       
       // Is nullable?                  
       public bool IsNullable { get; set; } 
       
       // Has [Required]             
       public bool IsRequired { get; set; }   

       // Property attributes           
       public List<AttributeInfo> Attributes { get; set; } = new List<AttributeInfo>() ;
}

}