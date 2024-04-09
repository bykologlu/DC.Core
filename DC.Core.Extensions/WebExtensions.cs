using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace DC.Core.Extensions
{
    public static class WebExtensions
    {
        public static string ToSeoUrl(this string value)
        {
            value = value.ConvertToAscii();

            value = value.ToLower();

            value = Regex.Replace(value, @"&\w+;", "",RegexOptions.None,TimeSpan.FromMilliseconds(200));

            value = Regex.Replace(value, @"[^a-z0-9\-\s]", "",RegexOptions.None, TimeSpan.FromMilliseconds(200));

            value = value.Replace(' ', '-');

            value = Regex.Replace(value, @"-{2,}", "-",RegexOptions.None, TimeSpan.FromMilliseconds(200));

            value = value.TrimStart(new[] { '-' });

            if (value.Length > 80)
                value = value.Substring(0, 79);

            value = value.TrimEnd(new[] { '-' });

            return value;
        }


        public static string ConvertToAscii(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            string noDiacritics = RemoveDiacriticCharacters(input);

            Encoding utf8 = Encoding.UTF8;
            Encoding ascii = Encoding.GetEncoding(1252);

            byte[] utf8Bytes = utf8.GetBytes(noDiacritics);
            byte[] asciiBytes = Encoding.Convert(utf8, ascii, utf8Bytes);

            return ascii.GetString(asciiBytes);
        }

        public static string RemoveDiacriticCharacters(this string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            string normalized = input.Normalize(NormalizationForm.FormD);
            StringBuilder builder = new StringBuilder();

            foreach (char c in normalized)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    builder.Append(c);
                }
            }

            return builder.ToString();
        }

    }
}
