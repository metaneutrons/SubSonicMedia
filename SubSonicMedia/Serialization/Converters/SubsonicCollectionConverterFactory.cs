// <copyright file="SubsonicCollectionConverterFactory.cs" company="Fabian Schmieder">
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
    /// Factory for creating SubsonicCollectionConverter instances.
    /// </summary>
    public class SubsonicCollectionConverterFactory : JsonConverterFactory
    {
        /// <summary>
        /// Determines whether this factory can convert the specified type.
        /// Returns true for generic List types.
        /// </summary>
        /// <param name="typeToConvert">The type to check for conversion capability.</param>
        /// <returns>True if the type can be converted, false otherwise.</returns>
        public override bool CanConvert(Type typeToConvert)
        {
            if (!typeToConvert.IsGenericType)
            {
                return false;
            }

            var genericTypeDefinition = typeToConvert.GetGenericTypeDefinition();
            return genericTypeDefinition == typeof(List<>);
        }

        /// <summary>
        /// Creates a JsonConverter instance for the specified type.
        /// Creates a SubsonicCollectionConverter for the generic List type.
        /// </summary>
        /// <param name="typeToConvert">The type to create a converter for.</param>
        /// <param name="options">JSON serializer options.</param>
        /// <returns>A JsonConverter instance for the specified type.</returns>
        public override JsonConverter CreateConverter(
            Type typeToConvert,
            JsonSerializerOptions options
        )
        {
            var elementType = typeToConvert.GetGenericArguments()[0];
            var converterType = typeof(SubsonicCollectionConverter<>).MakeGenericType(elementType);
            return (JsonConverter)Activator.CreateInstance(converterType)!;
        }
    }
}
