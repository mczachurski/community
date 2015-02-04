using System;
using System.Globalization;
using CsvHelper;

namespace SunLine.Community.Common
{
    public static class CsvReaderExtensions
    {
        public static string GetStringValue(this CsvReader csvReader, string fieldName)
        {
            if (string.IsNullOrWhiteSpace(fieldName))
            {
                return string.Empty;
            }

            return csvReader.GetField<string>(fieldName);
        }

        public static Decimal? GetDecimalValue(this CsvReader csvReader, string fieldName)
        {
            return ParseToDecimal(csvReader.GetStringValue(fieldName));
        }

        private static Decimal? ParseToDecimal(string number)
        {
            if (string.IsNullOrWhiteSpace(number))
            {
                return null;
            }

            decimal parsed;

            if (decimal.TryParse(number, NumberStyles.Any, new CultureInfo("pl-PL"), out parsed))
            {
                return parsed;
            }

            if (decimal.TryParse(number, NumberStyles.Any, new CultureInfo("en-US"), out parsed))
            {
                return parsed;
            }

            if (decimal.TryParse(number, NumberStyles.Any, new CultureInfo("en-GB"), out parsed))
            {
                return parsed;
            }

            if (decimal.TryParse(number, NumberStyles.Any, new CultureInfo("fr-FR"), out parsed))
            {
                return parsed;
            }

            return null;
        }
    }
}
