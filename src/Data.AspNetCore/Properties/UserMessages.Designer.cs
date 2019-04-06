// <auto-generated />
namespace DevZest.Data.AspNetCore
{
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Resources;

    internal static class UserMessages
    {
        private static readonly ResourceManager _resourceManager
            = new ResourceManager("DevZest.Data.AspNetCore.UserMessages", typeof(UserMessages).GetTypeInfo().Assembly);

        /// <summary>
        /// The DataSet field {0} is declared as scalar.
        /// </summary>
        public static string ScalarAttribute_ValidationError
        {
            get { return GetString("ScalarAttribute_ValidationError"); }
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