using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace osc_debug
{
    public class OscArgumentParser
    {
        public object[] ParseArguments(string argsString)
        {
            if (string.IsNullOrWhiteSpace(argsString))
                return Array.Empty<object>();

            var tokens = Tokenize(argsString);

            return tokens.Select(ParseToken).ToArray();
        }

        private static IEnumerable<string> Tokenize(string input)
        {
            var currentToken = new System.Text.StringBuilder();
            bool inDoubleQuotes = false;
            bool inSingleQuotes = false;
            bool isEscaped = false;

            for (int i = 0; i < input.Length; i++)
            {
                char c = input[i];

                if (c == '\\' && !isEscaped)
                {
                    isEscaped = true;
                    currentToken.Append(c);
                    continue;
                }

                if (isEscaped)
                {
                    currentToken.Append(c);
                    isEscaped = false;
                    continue;
                }

                if (c == '"' && !inSingleQuotes)
                {
                    inDoubleQuotes = !inDoubleQuotes;
                    currentToken.Append(c);
                    continue;
                }

                if (c == '\'' && !inDoubleQuotes)
                {
                    inSingleQuotes = !inSingleQuotes;
                    currentToken.Append(c);
                    continue;
                }

                if (c == ';' && !inDoubleQuotes && !inSingleQuotes)
                {
                    yield return currentToken.ToString();
                    currentToken.Clear();
                    continue;
                }

                currentToken.Append(c);
            }

            if (currentToken.Length > 0)
            {
                yield return currentToken.ToString();
            }
        }

        private static object ParseToken(string token)
        {
            string trimmed = token.Trim();

            if (string.IsNullOrEmpty(trimmed))
                return string.Empty;

            // string parsing
            if (((trimmed.StartsWith("\"") && trimmed.EndsWith("\"")) || (trimmed.StartsWith("\'") && trimmed.EndsWith("\'"))) && trimmed.Length >= 2)
            {
                string content = trimmed.Substring(1, trimmed.Length - 2);
                return Unescape(content);
            }

            // char parsing
            //if (trimmed.StartsWith("'") && trimmed.EndsWith("'") && trimmed.Length >= 2)
            //{
            //    string content = trimmed.Substring(1, trimmed.Length - 2);
            //    string unescaped = Unescape(content);

            //    // Если внутри ровно 1 символ - возвращаем char, иначе строку (защита от ошибок)
            //    return unescaped.Length == 1 ? (object)unescaped[0] : unescaped;
            //}

            // int, float, bool parsing
            string normalized = trimmed.Replace(',', '.');

            if (int.TryParse(normalized, out int intValue))
                return intValue;

            if (float.TryParse(normalized, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatValue))
                return floatValue;

            if (bool.TryParse(normalized, out bool boolValue))
                return boolValue;

            return trimmed;
        }

        private static string Unescape(string input)
        {
            if (string.IsNullOrEmpty(input)) return input;

            var sb = new System.Text.StringBuilder(input.Length);
            bool isEscaped = false;

            foreach (char c in input)
            {
                if (!isEscaped && c == '\\')
                {
                    isEscaped = true;
                    continue;
                }

                if (isEscaped)
                {
                    switch (c)
                    {
                        case 'n': sb.Append('\n'); break;
                        case 'r': sb.Append('\r'); break;
                        case 't': sb.Append('\t'); break;
                        case '"': sb.Append('"'); break;
                        case '\'': sb.Append('\''); break;
                        case '\\': sb.Append('\\'); break;
                        default: sb.Append(c); break;
                    }
                    isEscaped = false;
                }
                else
                {
                    sb.Append(c);
                }
            }

            return sb.ToString();
        }
    }
}
