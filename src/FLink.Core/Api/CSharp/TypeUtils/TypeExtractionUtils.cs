using System;

namespace FLink.Core.Api.CSharp.TypeUtils
{
    public static class TypeExtractionUtils
    {
        public static bool IsClassType(Type t) => t.IsClass;
    }
}
