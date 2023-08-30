using System;
using System.Text.RegularExpressions;

namespace BusinessCalendar.Client
{
    internal class IdentifierRegex
    {
        public const string Pattern = "^(State|Custom)_[A-z_]{1,}$";

        /// <summary>
        /// Ensures an identifier matches regex or throws otherwise
        /// </summary>
        /// <param name="identifier"></param>
        /// <exception cref="ArgumentOutOfRangeException">Identifier doesn't match regex</exception>
        public static void EnsureMatch(string identifier)
        {
            if (string.IsNullOrEmpty(identifier)
                || !IsMatch(identifier))
            {
                throw new ArgumentOutOfRangeException(nameof(identifier), $"Identifier should match regex /${IdentifierRegex.Pattern}/");
            }
        }

        public static bool IsMatch(string identifier)
        {
            return _regex.IsMatch(identifier);
        }

        private static Regex _regex = CreateRegEx();

        private static Regex CreateRegEx() {
            const RegexOptions options = RegexOptions.Compiled |RegexOptions.ExplicitCapture;
            return new Regex(Pattern, options, TimeSpan.FromSeconds(2.0));
        }
    }
}