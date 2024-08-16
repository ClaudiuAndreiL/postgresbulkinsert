namespace BulkInsertAPI.Services.Helpers.Builders
{
    public static class BulkCopyTextLinesBuilder
    {

        public static List<string> ConvertToLines<T>(List<T> items)
        {
            if (items == null || items.Count == 0)
            {
                return [];
            }

            var lines = new List<string>(items.Count);
            var properties = typeof(T).GetProperties();

            foreach (var item in items)
            {
                var values = new string[properties.Length];

                for (int i = 0; i < properties.Length; i++)
                {
                    var property = properties[i];
                    var value = property.GetValue(item);
                    var type = property.PropertyType;

                    if (value is null)
                        values[i] = "\\N";

                    if(value is not null)
                        values[i] = FormatValue(value, type);
                }

                // Combine values into a tab-delimited line
                lines.Add(string.Join("\t", values));
            }

            return lines;
        }

        private static string FormatValue(object value, Type type)
        {
            if (type == typeof(string))
            {
                return Escape(value.ToString()!);
            }
            if (type == typeof(DateTime))
            {
                return ((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss.ffffff", System.Globalization.CultureInfo.InvariantCulture);
            }
            if (type == typeof(bool))
            {
                return (bool)value ? "TRUE" : "FALSE";
            }

            return value.ToString()!;
        }

        private static string Escape(string input)
        {
            // If the input is null, handle it as PostgreSQL's NULL
            if (input == null)
                return "\\N";  // PostgreSQL representation for NULL values

            // Escape special characters for PostgreSQL COPY format
            var processed = input
                .Replace("\\", "\\\\")    // Escape backslashes
                .Replace("\t", "\\t")      // Escape tabs
                .Replace("\n", "\\n")      // Escape newlines
                .Replace("\r", "\\r")      // Escape carriage returns
                .Replace("\"", "\\\"");    // Escape double quotes

            return processed;
        }
    }
}
