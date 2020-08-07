using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace InteropAssistant
{
    public static class HeaderProcessor
    {
        private static readonly Regex functionRegex = new Regex(@"\w+_EXPORT ((?:const )?\w+\**) (\w+)\(([\w*,\[\]\s\n]*)\);");
        private static readonly Regex parameterRegex = new Regex(@"(?:const )?([\w\*]+) ([\w\*\[\]]+)");
        private static readonly Regex constantRegex = new Regex(@"#define (\w+)[\s\n]*\s+((?:\"".*\"")|(?:\w+))");
        private static readonly Regex functionPointerTypeDefRegex = new Regex(@"typedef (\w+)\s*\(\*(\w+)\)\(([\w*,\[\]\s\n]*)\)");

        private static readonly Regex enumRegex = new Regex(@"enum (\w+)[\s\n]*{([\w =,\s\n]+)};");
        private static readonly Regex enumValueRegex = new Regex(@"(\w+)\s*=?\s*(\w+)?");
        private static readonly Regex enumTypeDefRegex = new Regex(@"typedef enum (\w+) (\w+);");

        private static readonly Regex structRegex = new Regex(@"struct (\w+)[\s\n]*{([\w =,; \[\]\s\n]+)};");
        private static readonly Regex structTypeDefRegex = new Regex(@"typedef struct (\w+) (\w+);");
        private static readonly Regex typeDefedStructRegex = new Regex(@"typedef struct (\w+)[\s\n]*{([\w =,; \[\]\s\n]+)} (\w+);");
        private static readonly Regex structFieldRegex = new Regex(@"(?:const )?([\w*]+)[\s\n]*([\w\[\]]+);");
        private static readonly Regex structFixedSizeFieldRegex = new Regex(@"(?:const )?([\w*]+)\s+(\w+)\[(\w+)\];");

        public static ProcessedHeader ProcessInput(string input)
        {
            return new ProcessedHeader
            {
                Functions = GetFunctions(input),
                Constants = GetConstants(input),
                FunctionPointerTypeDefs = GetFunctionPointerTypeDefs(input),
                Enums = GetEnums(input),
                EnumTypeDefs = GetEnumTypeDefs(input),
                StructTypeDefs = GetStructTypeDefs(input),
                Structs = GetStructs(input),
                TypeDefedStructs = GetTypeDefedStructs(input)
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

        private static ExportEnum[] GetEnums(string input) => GetMatches(enumRegex, input).Select(m => new ExportEnum
        {
            Name = m[1],
            Values = GetEnumValues(m[2])
        }).ToArray();

        private static ExportEnumValue[] GetEnumValues(string input) => GetMatches(enumValueRegex, input).Select(m => new ExportEnumValue
        {
            Name = m[1],
            Value = m[2]
        }).ToArray();

        private static ExportEnumTypeDef[] GetEnumTypeDefs(string input) => GetMatches(enumTypeDefRegex, input).Select(m => new ExportEnumTypeDef
        {
            OriginalType = m[1],
            DefinedType = m[2]
        }).ToArray();

        private static ExportStruct[] GetStructs(string input) => GetMatches(structRegex, input).Select(m => new ExportStruct
        {
            Name = m[1],
            Fields = GetStructFields(m[2])
        }).ToArray();

        private static ExportStructTypeDef[] GetStructTypeDefs(string input) => GetMatches(structTypeDefRegex, input).Select(m => new ExportStructTypeDef
        {
            OriginalType = m[1],
            DefinedType = m[2]
        }).ToArray();

        private static ExportTypeDefedStruct[] GetTypeDefedStructs(string input) => GetMatches(typeDefedStructRegex, input).Select(m => new ExportTypeDefedStruct
        {
            OriginalType = m[1],
            Fields = GetStructFields(m[2]),
            DefinedType = m[3]
        }).ToArray();

        private static ExportStructField[] GetStructFields(string input) => GetMatches(structFieldRegex, input).Select(m => new ExportStructField
        {
            Type = m[1],
            Name = m[2]
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
}
