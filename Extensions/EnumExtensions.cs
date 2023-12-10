using System.ComponentModel;
using System.Reflection;

namespace DC.Core.Extensions
{
    public static class EnumExtensions
    {
        private static T GetAttribute<T>(this Enum enumValue) where T : Attribute
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<T>();
        }

        public static string ToName(this Enum value)
        {
            string? attribute = value.GetAttribute<DescriptionAttribute>().Description?.ToString();
            
            return attribute == null ? value.ToString() : attribute;
        }

        public static T ToEnum<T>(this string value) where T : Enum
        {
            if (string.IsNullOrEmpty(value))
            {
                return default(T);
            }

            try
            {
                T result = (T)Enum.Parse(typeof(T), value, true);

                return result;
            }
            catch (ArgumentException ex)
            {
                throw new Exception($"{value} value can't convert.");
            }
        }
    }
}
