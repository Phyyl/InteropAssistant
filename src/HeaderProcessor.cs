using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace InteropAssistant
{
    public class HeaderProcessor
    {
        private static readonly Regex functionRegex = new Regex(@"\w+_EXPORT ((?:const )?\w+\**) (\w+)\(([\w*,\s]*)\);");
        private static readonly Regex parameterRegex = new Regex(@"(?:const )?([\w\*]+) ([\w\*]+)");
        private static readonly Regex enumRegex = new Regex(@"enum (\w+)\s*{([\w =,\s]+)};");
        private static readonly Regex enumValueRegex = new Regex(@"(\w+)\s*=\s*(\w+)");
        private static readonly Regex enumTypeDefRegex = new Regex(@"typedef enum (\w+) (\w+);");
        private static readonly Regex structTypeDefRegex = new Regex(@"typedef struct (\w+) (\w+);");
        private static readonly Regex constantRegex = new Regex(@"#define (\w+)\s+((?:\"".*\"")|(?:\w+))");
        private static readonly Regex functionPointerTypeDefRegex = new Regex(@"typedef (\w+) \(\*(\w+)\)\(([\w*,\s]*)\)");

        public static HeaderProcessorResult ProcessInput(string input)
        {
            return new HeaderProcessorResult
            {
                Functions = GetFunctions(input),
                Constants = GetConstants(input),
                FunctionPointerTypeDefs = GetFunctionPointerTypeDefs(input)
            };
        }

        private static ExportFunction[] GetFunctions(string input) => GetMatches(functionRegex, input).Select(m => new ExportFunction
        {
            ReturnType = m[1],
            Name = m[2],
            Parameters = GetParameters(m[3])
        }).ToArray();

        private static ExportParameter[] GetParameters(string input) => GetMatches(parameterRegex, input).Select(m => new ExportParameter
        {
            Type = m[1],
            Name = m[2]
        }).ToArray();

        private static ExportConstant[] GetConstants(string input) => GetMatches(constantRegex, input).Select(m => new ExportConstant
        {
            Name = m[1],
            Value = m[2]
        }).ToArray();

        private static ExportFunctionPointerTypeDef[] GetFunctionPointerTypeDefs(string input) => GetMatches(functionPointerTypeDefRegex, input).Select(m => new ExportFunctionPointerTypeDef
        {
            ReturnType = m[1],
            Name = m[2],
            Parameters = GetParameters(m[3])
        }).ToArray();

        private static string[][] GetMatches(Regex regex, string input)
        {
            string FixValue(string value)
            {
                return value
                    .Replace("  ", " ")
                    .Replace("\t", "")
                    .Replace("\r", "")
                    .Replace("\n", "");
            }

            return regex
                .Matches(input)
                .Cast<Match>()
                .Select(m => m
                    .Groups
                    .Cast<Group>()
                    .Select(g => FixValue(g.Value))
                    .ToArray())
                .ToArray();
        }
    }

    public class HeaderProcessorResult
    {
        public ExportFunction[] Functions { get; set; }
        public ExportEnum[] Enums { get; set; }
        public ExportConstant[] Constants { get; set; }
        public ExportEnumTypeDef[] EnumTypeDefs { get; set; }
        public ExportStructTypeDef[] StructTypeDefs { get; set; }
        public ExportFunctionPointerTypeDef[] FunctionPointerTypeDefs { get; set; }
    }

    public class ExportStructTypeDef
    {
        public string OriginalType { get; set; }
        public string DefinedType { get; set; }
    }

    public class ExportEnumTypeDef
    {
        public string OriginalType { get; set; }
        public string DefinedType { get; set; }
    }

    public class ExportFunctionPointerTypeDef
    {
        public string ReturnType { get; set; }
        public string Name { get; set; }
        public ExportParameter[] Parameters { get; set; }
    }

    public class ExportFunction
    {
        public string Name { get; set; }
        public string ReturnType { get; set; }
        public ExportParameter[] Parameters { get; set; }
    }

    public class ExportParameter
    {
        public string Type { get; set; }
        public string Name { get; set; }
    }

    public class ExportEnum
    {
        public string Name { get; set; }
        public ExportEnumValue[] Values { get; set; }
    }

    public class ExportEnumValue
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }

    public class ExportConstant
    {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}
