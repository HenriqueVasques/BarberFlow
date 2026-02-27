using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace BarberFlow.API.Helpers
{
    public static class StringHelper
    {
        public static string ToSlug(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            // 1. Converte para minúsculo
            string str = text.ToLowerInvariant();

            // 2. Remove acentos e diacríticos (Ex: 'ç' vira 'c', 'ã' vira 'a')
            str = RemoveAccents(str);

            // 3. Remove caracteres que não são letras, números ou espaços
            str = Regex.Replace(str, @"[^a-z0-9\s-]", "");

            // 4. Substitui múltiplos espaços ou hifens por um único hífen
            str = Regex.Replace(str, @"[\s-]+", " ").Trim();

            // 5. Substitui o espaço final por hífen
            str = str.Replace(" ", "-");

            return str;
        }

        private static string RemoveAccents(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
