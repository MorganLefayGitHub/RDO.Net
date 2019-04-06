// <auto-generated />
namespace DevZest.Data.MySql
{
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Resources;

    internal static class DiagnosticMessages
    {
        private static readonly ResourceManager _resourceManager
            = new ResourceManager("DevZest.Data.MySql.DiagnosticMessages", typeof(DiagnosticMessages).GetTypeInfo().Assembly);

        /// <summary>
        /// Bad version format.
        /// </summary>
        public static string BadVersionFormat
        {
            get { return GetString("BadVersionFormat"); }
        }

        /// <summary>
        /// The column type {columnType} is not supported.
        /// </summary>
        public static string ColumnTypeNotSupported(object columnType)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ColumnTypeNotSupported", "columnType"), columnType);
        }

        /// <summary>
        /// The constraint type "{constraintType}" is not supported.
        /// </summary>
        public static string ConstraintTypeNotSupported(object constraintType)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("ConstraintTypeNotSupported", "constraintType"), constraintType);
        }

        /// <summary>
        /// The function {function} is not supported.
        /// </summary>
        public static string FunctionNotSupported(object function)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("FunctionNotSupported", "function"), function);
        }

        /// <summary>
        /// Version {version} is not supported. Only version {lowestSupported} onwards are supported.
        /// </summary>
        public static string VersionNotSupported(object version, object lowestSupported)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("VersionNotSupported", "version", "lowestSupported"), version, lowestSupported);
        }

        private static string GetString(string name, params string[] formatterNames)
        {
            var value = _resourceManager.GetString(name);

            Debug.Assert(value != null);

            if (formatterNames != null)
            {
                for (var i = 0; i < formatterNames.Length; i++)
                {
                    value = value.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
                }
            }

            return value;
        }
    }
}