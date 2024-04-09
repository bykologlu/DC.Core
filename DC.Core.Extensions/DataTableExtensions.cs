using System.Data;

namespace DC.Core.Extensions
{
    public static class DataTableExtensions
    {
        public static object GetValueByEnum<TEnum>(this DataRow row, TEnum enumType) where TEnum : Enum
        {
            return row[enumType.ToString()];
        }
    }
}
