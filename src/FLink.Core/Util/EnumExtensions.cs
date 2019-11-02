using System;

namespace FLink.Core.Util
{
    public static class EnumExtensions
    {
        public static string GetName(this Enum value) => Enum.GetName(value.GetType(), value);
    }
}
