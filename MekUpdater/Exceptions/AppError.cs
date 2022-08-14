/// Copyright 2021 Henri Vainio 
using System.Diagnostics;

namespace MekUpdater.Exceptions
{
    public static class AppError
    {
        /// <summary>
        /// Generate exception header
        /// <para/>Format: "[class.method]"
        /// <para/>Also log on console
        /// </summary>
        /// <param name="message"></param>
        /// <returns>formatted exception text</returns>
        public static string Text()
        {
            return GetFormattedText("", 1);
        }

        /// <summary>
        /// Generate exception text in right format
        /// <para/>Format: "[class.method] message"
        /// <para/>Also log on console
        /// </summary>
        /// <param name="message"></param>
        /// <returns>formatted exception text</returns>
        public static string Text(string message)
        {
            return GetFormattedText(message, 1);
        }

        /// <summary>
        /// Generate exception text in right format
        /// <para/>Format: "[class.method]"
        /// <para/>Also log on console
        /// </summary>
        /// <param name="message"></param>
        /// <returns>formatted exception text</returns>
        public static string Text(int stackDepth)
        {
            return GetFormattedText(string.Empty, stackDepth);
        }

        /// <summary>
        /// Generate exception text with variable value in right format
        /// <para/>"class.method" is taken from call stack depth (default 1)
        /// <para/>Format: "[class.method] message ; was given variableValue"
        /// <para/>Also log on console
        /// </summary>
        /// <param name="message"></param>
        /// <param name="variableValue"></param>
        /// <returns>formatted exception text</returns>
        public static string Text(string message, int stackDepth)
        {
            return GetFormattedText(message, stackDepth);
        }

        /// <summary>
        /// Generate exception text with variable value in right format
        /// <para/>Format: "[class.method] message ; was given variableValue"
        /// <para/>Also log on console
        /// </summary>
        /// <param name="message"></param>
        /// <param name="variableValue"></param>
        /// <returns>formatted exception text</returns>
        public static string Text(string message, string? variableValue)
        {
            return GetFormattedText(message, 1, variableValue ??= "#null#");
        }

        /// <summary>
        /// Generate exception text with variable value in right format
        /// <para/>"class.method" is taken from call stack depth (default 1)
        /// <para/>Format: "[class.method] message ; was given variableValue"
        /// <para/>Also log on console
        /// </summary>
        /// <param name="message"></param>
        /// <param name="variableValue"></param>
        /// <returns>formatted exception text</returns>
        public static string Text(string message, int stackDepth, string? variableValue)
        {
            return GetFormattedText(message, stackDepth, variableValue ??= "#null#");
        }

        /// <summary>
        /// Generate return value for Text()
        /// </summary>
        /// <param name="message"></param>
        /// <param name="stackDepth"></param>
        /// <param name="variableValue"></param>
        /// <returns>formatted Text() return value</returns>
        private static string GetFormattedText(string message, int stackDepth, string? variableValue = null)
        {
            stackDepth++; // add this function to depth

            StackFrame frame = new(stackDepth);
            var className = frame.GetMethod()?.DeclaringType?.Name ?? $"{nameof(AppError)}";
            var methodName = frame.GetMethod()?.Name ?? $"{nameof(Text)}";

            variableValue = variableValue is null ?
                "" : $" ; was given \"{variableValue?.ToString()}\"";

            var error = $"[{className}.{methodName}] {message}{variableValue}";
            Console.WriteLine(error);
            return error;
        }
    }
}
