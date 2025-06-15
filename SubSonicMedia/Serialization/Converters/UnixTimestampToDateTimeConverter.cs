// <copyright file="UnixTimestampToDateTimeConverter.cs" company="Fabian Schmieder">
// This file is part of SubSonicMedia.
//
// SubSonicMedia is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// SubSonicMedia is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with SubSonicMedia. If not, see https://www.gnu.org/licenses/.
// </copyright>

using System.Text.Json;
using System.Text.Json.Serialization;

namespace SubSonicMedia.Serialization.Converters
{
    /// <summary>
    /// Converts Unix timestamps (milliseconds as numbers) to DateTime objects.
    /// Used for fields like Indexes.LastModified that receive numbers but expect DateTime.
    /// </summary>
    public class UnixTimestampToDateTimeConverter : JsonConverter<DateTime>
    {
        private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Reads and converts JSON tokens to DateTime values.
        /// Handles Unix timestamps (in milliseconds) as numbers or strings, and ISO date strings.
        /// </summary>
        /// <param name="reader">The Utf8JsonReader to read from.</param>
        /// <param name="typeToConvert">The type to convert to.</param>
        /// <param name="options">JSON serializer options.</param>
        /// <returns>A DateTime representing the converted timestamp.</returns>
        public override DateTime Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            try
            {
                if (reader.TokenType == JsonTokenType.Number)
                {
                    // Handle Unix timestamp in milliseconds
                    if (reader.TryGetInt64(out long unixTimestamp))
                    {
                        return UnixEpoch.AddMilliseconds(unixTimestamp);
                    }
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    var stringValue = reader.GetString();

                    // Try to parse as Unix timestamp string
                    if (long.TryParse(stringValue, out long unixTimestamp))
                    {
                        return UnixEpoch.AddMilliseconds(unixTimestamp);
                    }

                    // Try to parse as ISO date string
                    if (DateTime.TryParse(stringValue, out DateTime dateTime))
                    {
                        return dateTime;
                    }
                }
                else if (reader.TokenType == JsonTokenType.Null)
                {
                    return DateTime.MinValue;
                }

                throw new JsonException(
                    $"Unable to convert token type {reader.TokenType} to DateTime"
                );
            }
            catch (Exception ex) when (!(ex is JsonException))
            {
                throw new JsonException($"Error converting value to DateTime: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Writes a DateTime value to JSON as a Unix timestamp in milliseconds.
        /// Writes null if the DateTime is MinValue, otherwise converts to Unix timestamp.
        /// </summary>
        /// <param name="writer">The Utf8JsonWriter to write to.</param>
        /// <param name="value">The DateTime value to write.</param>
        /// <param name="options">JSON serializer options.</param>
        public override void Write(
            Utf8JsonWriter writer,
            DateTime value,
            JsonSerializerOptions options
        )
        {
            if (value == DateTime.MinValue)
            {
                writer.WriteNullValue();
            }
            else
            {
                // Convert DateTime to Unix timestamp in milliseconds
                var unixTimestamp = (long)(value.ToUniversalTime() - UnixEpoch).TotalMilliseconds;
                writer.WriteNumberValue(unixTimestamp);
            }
        }
    }
}
