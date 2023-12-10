using Newtonsoft.Json;

namespace DC.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static Dictionary<string, object> ToJsonDictionary(this object model)
        {
            if (model == null) return new Dictionary<string, object>();

            var serializedModel = JsonConvert.SerializeObject(model);
            return JsonConvert.DeserializeObject<Dictionary<string, object>>(serializedModel);
        }

        public static string? ToNString(this object? value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
                return null;

            return value.ToString();
        }

        public static double ToDouble(this object value)
        {
            Double.TryParse(value.ToString(), out double result);

            return result;
        }

        public static double? ToNDouble(this object? value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
                return null;

            return value.ToDouble();
        }

        public static decimal ToDecimal(this object value)
        {
            Decimal.TryParse(value.ToString(), out decimal result);

            return result;
        }

        public static decimal? ToNDecimal(this object? value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
                return null;

            return value.ToDecimal();
        }

        public static int ToInteger(this object value)
        {
            Int32.TryParse(value.ToString(), out int result);

            return result;
        }
        public static int? ToNInteger(this object? value)
        {
            if (string.IsNullOrEmpty(value?.ToString()))
                return null;

            return value.ToInteger();
        }
    }
}
