using System;
using System.Collections.Generic;
using System.Text;

namespace InteropAssistant
{
    public class ProcessedHeader
    {
        public ExportFunction[] Functions { get; set; }
        public ExportConstant[] Constants { get; set; }
        public ExportFunctionPointerTypeDef[] FunctionPointerTypeDefs { get; set; }

        public ExportEnum[] Enums { get; set; }
        public ExportEnumTypeDef[] EnumTypeDefs { get; set; }

        public ExportStruct[] Structs { get; set; }
        public ExportStructTypeDef[] StructTypeDefs { get; set; }
        public ExportTypeDefedStruct[] TypeDefedStructs { get; set; }
    }
}
