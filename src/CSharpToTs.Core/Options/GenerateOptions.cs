

namespace CSharpToTs.Core.Options
{
    public class GenerateOptions
    {

        public string? InputPath { get; set; }
        public string? OutputPath { get; set; }
        public bool Watch { get; set; }
        public string? ValidationSchema { get; set; }
    }
}