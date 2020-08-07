using System;
using System.Collections.Generic;
using System.Text;

namespace InteropAssistant
{
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

    public class ExportStruct
    {
        public string Name { get; set; }
        public ExportStructField[] Fields { get; set; }
    }

    public class ExportTypeDefedStruct
    {
        public ExportStructField[] Fields { get; set; }
        public string OriginalType { get; set; }
        public string DefinedType { get; set; }
    }

    public class ExportStructField
    {
        public string Type { get; set; }
        public string Name { get; set; }
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
