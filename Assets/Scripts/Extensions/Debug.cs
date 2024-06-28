using System;
using System.Text;
using System.Text.RegularExpressions;

using UnityEngine;

namespace Assets.Scripts
{
    public static class Debug
    {
        private const string LOG_PLACEHOLDER_SEARCH_REGEX = @"\{(\w+)\}";
        private static readonly Regex _searchRegex = new Regex(LOG_PLACEHOLDER_SEARCH_REGEX, RegexOptions.Compiled);

        private static string GetStructuredLogMessage(string message, params object[] values)
        {
            var matches = _searchRegex.Matches(message);

            if (matches.Count != values.Length)
                throw new FormatException($"The messsage '{message}' contains a different amount of parameters than the values provided. " +
                                          $"Placeholders in message: {matches.Count}, Values provided: {values.Length}");

            if (matches.Count == 0)
                return message;

            StringBuilder result = new StringBuilder(message);

            for (int i = 0; i < matches.Count; i++)
                result.Replace(matches[i].Value, values[i].ToString());

            return result.ToString();
        }

        /// <inheritdoc cref="UnityEngine.Debug.Log(object)"/>
        /// <exception cref="FormatException" />
        public static void Log(string message, params object[] values)
            => UnityEngine.Debug.Log(GetStructuredLogMessage(message, values));

        /// <inheritdoc cref="UnityEngine.Debug.DrawRay(Vector3, Vector3, Color, float)"/>
        public static void DrawRay(Ray ray, float rayLenght = 50.0f, float duration = 3.0f)
            => UnityEngine.Debug.DrawRay(ray.origin, ray.direction * rayLenght, Color.red, duration);

    }
}
