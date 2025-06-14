// <copyright file="StringBooleanConverter.cs" company="Fabian Schmieder">
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
    /// Converts "true"/"false" string values to bool.
    /// Used for boolean fields that may receive string representations.
    /// </summary>
    public class StringBooleanConverter : JsonConverter<bool>
    {
        public override bool Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            try
            {
                if (reader.TokenType == JsonTokenType.True)
                {
                    return true;
                }
                else if (reader.TokenType == JsonTokenType.False)
                {
                    return false;
                }
                else if (reader.TokenType == JsonTokenType.String)
                {
                    var stringValue = reader.GetString();

                    if (string.IsNullOrEmpty(stringValue))
                    {
                        return false;
                    }

                    // Handle string boolean values
                    return stringValue.ToLowerInvariant() switch
                    {
                        "true" => true,
                        "false" => false,
                        "1" => true,
                        "0" => false,
                        "yes" => true,
                        "no" => false,
                        _ => throw new JsonException(
                            $"Unable to convert string '{stringValue}' to bool"
                        ),
                    };
                }
                else if (reader.TokenType == JsonTokenType.Number)
                {
                    // Handle numeric boolean values (1 = true, 0 = false)
                    if (reader.TryGetInt32(out int intValue))
                    {
                        return intValue != 0;
                    }
                }

                throw new JsonException($"Unable to convert token type {reader.TokenType} to bool");
            }
            catch (Exception ex) when (!(ex is JsonException))
            {
                throw new JsonException($"Error converting value to bool: {ex.Message}", ex);
            }
        }

        public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
        {
            writer.WriteBooleanValue(value);
        }
    }
}
