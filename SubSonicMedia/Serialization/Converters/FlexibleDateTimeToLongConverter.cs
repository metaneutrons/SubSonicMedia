// <copyright file="FlexibleDateTimeToLongConverter.cs" company="Fabian Schmieder">
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
    /// Converts both ISO date strings ("2020-01-01T00:00:00.000Z") and Unix timestamps to long?.
    /// Used for fields like Child.Created that can receive either format.
    /// </summary>
    public class FlexibleDateTimeToLongConverter : JsonConverter<long?>
    {
        private static readonly DateTime UnixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public override long? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            try
            {
                if (reader.TokenType == JsonTokenType.Number)
                {
                    // Handle Unix timestamp already as number
                    if (reader.TryGetInt64(out long unixTimestamp))
                    {
                        return unixTimestamp;
                    }
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    var stringValue = reader.GetString();

                    if (string.IsNullOrEmpty(stringValue))
                    {
                        return null;
                    }

                    // Try to parse as Unix timestamp string first
                    if (long.TryParse(stringValue, out long unixTimestamp))
                    {
                        return unixTimestamp;
                    }

                    // Try to parse as ISO date string and convert to Unix timestamp
                    if (DateTime.TryParse(stringValue, out DateTime dateTime))
                    {
                        return (long)(dateTime.ToUniversalTime() - UnixEpoch).TotalMilliseconds;
                    }
                }
                else if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }

                throw new JsonException(
                    $"Unable to convert token type {reader.TokenType} to long?"
                );
            }
            catch (Exception ex) when (!(ex is JsonException))
            {
                throw new JsonException($"Error converting value to long?: {ex.Message}", ex);
            }
        }

        public override void Write(
            Utf8JsonWriter writer,
            long? value,
            JsonSerializerOptions options
        )
        {
            if (value.HasValue)
            {
                writer.WriteNumberValue(value.Value);
            }
            else
            {
                writer.WriteNullValue();
            }
        }
    }
}
