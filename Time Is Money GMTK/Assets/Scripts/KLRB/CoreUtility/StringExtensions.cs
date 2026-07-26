using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace  KLRB.Utility
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            string noSpaces = Regex.Replace(input, @"[\s_]+", " ");
            string spaced = Regex.Replace(noSpaces, "(?<!^)([A-Z])", " $1");

            string[] words = spaced.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < words.Length; i++)
            {
                words[i] = i == 0
                    ? words[i].ToLower()
                    : char.ToUpper(words[i][0]) + words[i][1..].ToLower();
            }

            return string.Concat(words);
        }

        
        public static string ToPascalCase(this string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
            string titleCase = textInfo.ToTitleCase(input.ToLower());
            return Regex.Replace(titleCase, @"\s+", "");
        }
    }
    
   
}