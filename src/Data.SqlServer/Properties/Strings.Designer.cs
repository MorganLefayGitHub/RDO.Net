// <auto-generated />
namespace DevZest.Data.SqlServer
{
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Resources;

    internal static class Strings
    {
        private static readonly ResourceManager _resourceManager
            = new ResourceManager("DevZest.Data.SqlServer.Strings", typeof(Strings).GetTypeInfo().Assembly);

        /// <summary>
        /// The column type "{columnType}" is not supported.
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
        /// The function "{functionName}" is not supported.
        /// </summary>
        public static string FunctionNotSupported(object functionName)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("FunctionNotSupported", "functionName"), functionName);
        }

        /// <summary>
        /// The SQL Server version "{sqlServerVersion}" is not supported.
        /// </summary>
        public static string SqlVersionNotSupported(object sqlServerVersion)
        {
            return string.Format(CultureInfo.CurrentCulture, GetString("SqlVersionNotSupported", "sqlServerVersion"), sqlServerVersion);
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
