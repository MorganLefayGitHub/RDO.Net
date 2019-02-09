﻿using System;
using System.Globalization;
using System.Text;

namespace DevZest.Data
{
    public struct JsonValue
    {
        public static readonly JsonValue True = new JsonValue("true", JsonValueType.True);
        public static readonly JsonValue False = new JsonValue("false", JsonValueType.False);
        public static readonly JsonValue Null = new JsonValue("null", JsonValueType.Null);

        internal static JsonValue Char(char? x)
        {
            return x.HasValue ? String(new string(x.GetValueOrDefault(), 1)) : Null;
        }

        internal static JsonValue DateTime(DateTime? x)
        {
            if (!x.HasValue)
                return Null;

            var value = x.GetValueOrDefault();
            return String(value.ToString("yyyy-MM-ddTHH:mm:ss.FFFFFFFK", CultureInfo.InvariantCulture));
        }

        internal static JsonValue Guid(Guid? x)
        {
            return x.HasValue ? String(x.GetValueOrDefault().ToString()) : Null;
        }

        internal static JsonValue Number(byte? x)
        {
            return x.HasValue ? Number(x.Value) : Null;
        }

        public static JsonValue Number(byte x)
        {
            return Number(x.ToString(NumberFormatInfo.InvariantInfo));
        }

        internal static JsonValue Number(short? x)
        {
            return x.HasValue ? Number(x.Value) : Null;
        }

        public static JsonValue Number(short x)
        {
            return Number(x.ToString(NumberFormatInfo.InvariantInfo));
        }

        internal static JsonValue Number(int? x)
        {
            return x.HasValue ? Number(x.Value) : Null;
        }

        public static JsonValue Number(int x)
        {
            return Number(x.ToString(NumberFormatInfo.InvariantInfo));
        }

        internal static JsonValue Number(long? x)
        {
            return x.HasValue ? Number(x.Value) : Null;
        }

        public static JsonValue Number(long x)
        {
            return Number(x.ToString(NumberFormatInfo.InvariantInfo));
        }

        internal static JsonValue Number(float? x)
        {
            return x.HasValue ? Number(x.Value) : Null;
        }

        public static JsonValue Number(float x)
        {
            return Number(x.ToString(NumberFormatInfo.InvariantInfo));
        }

        internal static JsonValue Number(double? x)
        {
            return x.HasValue ? Number(x.Value) : Null;
        }

        public static JsonValue Number(double x)
        {
            return Number(x.ToString(NumberFormatInfo.InvariantInfo));
        }

        internal static JsonValue Number(decimal? x)
        {
            return x.HasValue ? Number(x.Value) : Null;
        }

        public static JsonValue Number(decimal x)
        {
            return Number(x.ToString(NumberFormatInfo.InvariantInfo));
        }

        public static JsonValue Number(string x)
        {
            return new JsonValue(x, JsonValueType.Number);
        }

        public static JsonValue String(string x)
        {
            return x == null ? Null : new JsonValue(x, JsonValueType.String);
        }

        internal JsonValue(string text, JsonValueType type)
        {
            text.VerifyNotNull(nameof(text));

            Text = text;
            Type = type;
        }

        public readonly string Text;

        public readonly JsonValueType Type;

        public bool IsDefault
        {
            get { return Type == 0 && Text == null; }
        }

        public override string ToString()
        {
            return Text;
        }

        internal void Write(StringBuilder stringBuilder)
        {
            if (Type == JsonValueType.String)
                stringBuilder.Append('"');

            if (Type != JsonValueType.String)
                stringBuilder.Append(Text);
            else
                WriteEscapedText(Text, stringBuilder);

            if (Type == JsonValueType.String)
                stringBuilder.Append('"');
        }

        private static void WriteEscapedText(string value, StringBuilder stringBuilder)
        {
            if (string.IsNullOrEmpty(value))
                return;

            int startIndex = 0;
            int num = 0;
            int i = 0;
            for (i = 0; i < value.Length; i++)
            {
                char c = value[i];
                if (!RequiresEscape(c))
                {
                    num++;
                    continue;
                }

                if (num > 0)
                    stringBuilder.Append(value, startIndex, num);

                startIndex = i + 1;
                num = 0;
                WriteEscape(c, stringBuilder);
            }

            if (num > 0)
                stringBuilder.Append(value, startIndex, num);
        }

        private static bool RequiresEscape(char c)
        {
            return c < ' ' || c == '"' || c == '\\' || c == '\u0085' || c == '\u2028' || c == '\u2029';
        }

        private static void WriteEscape(char c, StringBuilder stringBuilder)
        {
            switch (c)
            {
                case '"':
                    stringBuilder.Append("\\\"");
                    break;
                case '\\':
                    stringBuilder.Append("\\\\");
                    break;
                case '\b':
                    stringBuilder.Append("\\b");
                    break;
                case '\f':
                    stringBuilder.Append("\\f");
                    break;
                case '\n':
                    stringBuilder.Append("\\n");
                    break;
                case '\r':
                    stringBuilder.Append("\\r");
                    break;
                case '\t':
                    stringBuilder.Append("\\t");
                    break;
                default:
                    stringBuilder.Append("\\u");
                    int num = (int)c;
                    stringBuilder.Append(num.ToString("x4", CultureInfo.InvariantCulture));
                    break;
            }
        }
    }
}
