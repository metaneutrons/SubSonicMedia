// <copyright file="SubsonicCollectionConverter.cs" company="Fabian Schmieder">
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
    /// Handles Subsonic's object-wrapped arrays for collection inconsistencies.
    /// Some Subsonic responses wrap single items in objects instead of arrays.
    /// </summary>
    public class SubsonicCollectionConverter<T> : JsonConverter<List<T>>
    {
        public override List<T> Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            try
            {
                var result = new List<T>();

                if (reader.TokenType == JsonTokenType.Null)
                {
                    return result; // Return empty list for null
                }

                if (reader.TokenType == JsonTokenType.StartArray)
                {
                    // Standard array handling
                    while (reader.Read())
                    {
                        if (reader.TokenType == JsonTokenType.EndArray)
                        {
                            break;
                        }

                        var item = JsonSerializer.Deserialize<T>(ref reader, options);
                        if (item != null)
                        {
                            result.Add(item);
                        }
                    }
                }
                else if (reader.TokenType == JsonTokenType.StartObject)
                {
                    // Single object - treat as single-item collection
                    var item = JsonSerializer.Deserialize<T>(ref reader, options);
                    if (item != null)
                    {
                        result.Add(item);
                    }
                }
                else
                {
                    throw new JsonException(
                        $"Unable to convert token type {reader.TokenType} to List<{typeof(T).Name}>"
                    );
                }

                return result;
            }
            catch (Exception ex) when (!(ex is JsonException))
            {
                throw new JsonException(
                    $"Error converting value to List<{typeof(T).Name}>: {ex.Message}",
                    ex
                );
            }
        }

        public override void Write(
            Utf8JsonWriter writer,
            List<T> value,
            JsonSerializerOptions options
        )
        {
            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            writer.WriteStartArray();
            foreach (var item in value)
            {
                JsonSerializer.Serialize(writer, item, options);
            }
            writer.WriteEndArray();
        }
    }
}
