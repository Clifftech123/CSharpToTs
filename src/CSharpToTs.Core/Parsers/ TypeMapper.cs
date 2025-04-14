
using System.Text.RegularExpressions;

namespace CSharpToTs.Core.Parsers
{
    public class TypeMapper
    {
        private readonly Dictionary<string, string> _typeMap = new()
        {
            // Basic types
            ["string"] = "string",
            ["char"] = "string",
            ["int"] = "number",
            ["long"] = "number",
            ["short"] = "number",
            ["byte"] = "number",
            ["uint"] = "number",
            ["ulong"] = "number",
            ["ushort"] = "number",
            ["sbyte"] = "number",
            ["float"] = "number",
            ["double"] = "number",
            ["decimal"] = "number",
            ["bool"] = "boolean",
            ["DateTime"] = "string",
            ["DateTimeOffset"] = "string",
            ["TimeSpan"] = "string",
            ["Guid"] = "string",
            ["object"] = "any",
            
            // Collection types (specific instances)
            ["List<string>"] = "string[]",
            ["List<int>"] = "number[]",
            ["List<decimal>"] = "number[]",
            ["List<bool>"] = "boolean[]",
            ["List<DateTime>"] = "string[]",
            ["List<object>"] = "any[]",
        };

        // Regular expressions for matching generic types
        private static readonly Regex _listRegex = new Regex(@"^List<(.+)>$", RegexOptions.Compiled);
        private static readonly Regex _arrayRegex = new Regex(@"^(.+)\[\]$", RegexOptions.Compiled);
        private static readonly Regex _enumerableRegex = new Regex(@"^IEnumerable<(.+)>$", RegexOptions.Compiled);
        private static readonly Regex _collectionRegex = new Regex(@"^ICollection<(.+)>$", RegexOptions.Compiled);
        private static readonly Regex _dictionaryRegex = new Regex(@"^Dictionary<(.+),\s*(.+)>$", RegexOptions.Compiled);
        private static readonly Regex _nullableRegex = new Regex(@"^(.+)\?$", RegexOptions.Compiled);

        public string MapToTypeScript(string csharpType)
        {
            // Check if we have a direct mapping
            if (_typeMap.TryGetValue(csharpType, out var tsType))
            {
                return tsType;
            }

            // Check for nullable value types (int?, bool?, etc.)
            var nullableMatch = _nullableRegex.Match(csharpType);
            if (nullableMatch.Success)
            {
                var innerType = nullableMatch.Groups[1].Value;
                return $"{MapToTypeScript(innerType)} | null";
            }

            // Check for arrays
            var arrayMatch = _arrayRegex.Match(csharpType);
            if (arrayMatch.Success)
            {
                var elementType = arrayMatch.Groups[1].Value;
                return $"{MapToTypeScript(elementType)}[]";
            }

            // Check for List<T>
            var listMatch = _listRegex.Match(csharpType);
            if (listMatch.Success)
            {
                var elementType = listMatch.Groups[1].Value;
                return $"{MapToTypeScript(elementType)}[]";
            }

            // Check for IEnumerable<T>
            var enumerableMatch = _enumerableRegex.Match(csharpType);
            if (enumerableMatch.Success)
            {
                var elementType = enumerableMatch.Groups[1].Value;
                return $"{MapToTypeScript(elementType)}[]";
            }

            // Check for ICollection<T>
            var collectionMatch = _collectionRegex.Match(csharpType);
            if (collectionMatch.Success)
            {
                var elementType = collectionMatch.Groups[1].Value;
                return $"{MapToTypeScript(elementType)}[]";
            }

            // Check for Dictionary<K,V>
            var dictionaryMatch = _dictionaryRegex.Match(csharpType);
            if (dictionaryMatch.Success)
            {
                var keyType = dictionaryMatch.Groups[1].Value;
                var valueType = dictionaryMatch.Groups[2].Value;
                
                // In TypeScript, object keys are typically strings or numbers
                var mappedKeyType = MapToTypeScript(keyType);
                if (mappedKeyType != "string" && mappedKeyType != "number")
                {
                    mappedKeyType = "string";
                }
                
                return $"{{ [key: {mappedKeyType}]: {MapToTypeScript(valueType)} }}";
            }

            
            return csharpType;
        }
    }
}
