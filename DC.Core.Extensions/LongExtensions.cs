namespace DC.Core.Extensions
{
    public static class LongExtensions
    {
        public static DateTime ToDateTime(this long value)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds(value).ToLocalTime();
        }
    }
}
