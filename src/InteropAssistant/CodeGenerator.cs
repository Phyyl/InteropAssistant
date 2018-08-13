using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InteropAssistant
{
    public class CodeGenerator
    {
        public static void Generate(ProcessedHeader processedHeader, ExportFunction[] selectedFunctions, ExportConstant[] selectedConstants)
        {
            List<string> lines = new List<string>();

            foreach (var function in selectedFunctions)
            {
                string parameterString = string.Join(", ", function.Parameters.Select(p => $"{p.Type} {p.Name}"));

                lines.Add("[DllImport(LibName, CallingConvention = CallingConvention.Cdecl)]");
                lines.Add($"public static extern {function.ReturnType} {function.Name}({parameterString});");
            }
        }
    }
}
