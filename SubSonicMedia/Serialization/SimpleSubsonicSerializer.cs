// <copyright file="SimpleSubsonicSerializer.cs" company="Fabian Schmieder">
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
using SubSonicMedia.Exceptions;
using SubSonicMedia.Responses;
using SubSonicMedia.Serialization.Converters;

namespace SubSonicMedia.Serialization
{
    /// <summary>
    /// Simple JSON serializer for Subsonic API responses that replaces JsonParser.cs.
    /// Works with existing response models using standard System.Text.Json.
    /// </summary>
    internal static class SimpleSubsonicSerializer
    {
        private static readonly JsonSerializerOptions Options = new()
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new StringBooleanConverter(),
                new FlexibleDateTimeToLongConverter(),
                new UnixTimestampToDateTimeConverter(),
                new SubsonicCollectionConverterFactory(),
            },
        };

        /// <summary>
        /// Parses a JSON string into a strongly typed response object.
        /// This replaces the JsonParser.Parse method.
        /// </summary>
        /// <typeparam name="T">The type of response to parse into.</typeparam>
        /// <param name="json">The JSON string to parse.</param>
        /// <returns>The parsed response object.</returns>
        public static T Parse<T>(string json)
            where T : SubsonicResponse, new()
        {
            try
            {
                // Parse JSON to extract the subsonic-response wrapper
                using var document = JsonDocument.Parse(json);
                var root = document.RootElement;

                if (!root.TryGetProperty("subsonic-response", out var responseElement))
                {
                    throw new SubsonicApiException(
                        0,
                        "Invalid response format: subsonic-response element not found"
                    );
                }

                // Deserialize the response element to our strongly-typed model
                var response = JsonSerializer.Deserialize<T>(responseElement.GetRawText(), Options);

                if (response == null)
                {
                    throw new SubsonicApiException(
                        0,
                        $"Failed to deserialize response to {typeof(T).Name}"
                    );
                }

                // Handle error responses
                if (response.Status == "failed" && response.Error != null)
                {
                    // Check for authentication errors and throw the appropriate exception type
                    if (IsAuthenticationError(response.Error.Code))
                    {
                        throw new SubsonicAuthenticationException(
                            response.Error.Code,
                            response.Error.Message
                        );
                    }

                    throw SubsonicApiException.FromError(response.Error);
                }

                return response;
            }
            catch (JsonException ex)
            {
                throw new SubsonicApiException(
                    0,
                    $"Failed to parse JSON response: {ex.Message}",
                    ex
                );
            }
            catch (Exception ex) when (ex is not SubsonicApiException)
            {
                throw new SubsonicApiException(
                    0,
                    $"Failed to parse JSON response: {ex.Message}",
                    ex
                );
            }
        }

        /// <summary>
        /// Parses a JSON stream into a strongly typed response object.
        /// This replaces the JsonParser.Parse method for streams.
        /// </summary>
        /// <typeparam name="T">The type of response to parse into.</typeparam>
        /// <param name="stream">The JSON stream to parse.</param>
        /// <returns>The parsed response object.</returns>
        public static async Task<T> ParseAsync<T>(Stream stream)
            where T : SubsonicResponse, new()
        {
            try
            {
                // Parse JSON to extract the subsonic-response wrapper
                using var document = await JsonDocument.ParseAsync(stream);
                var root = document.RootElement;

                if (!root.TryGetProperty("subsonic-response", out var responseElement))
                {
                    throw new SubsonicApiException(
                        0,
                        "Invalid response format: subsonic-response element not found"
                    );
                }

                // Deserialize the response element to our strongly-typed model
                var response = JsonSerializer.Deserialize<T>(responseElement.GetRawText(), Options);

                if (response == null)
                {
                    throw new SubsonicApiException(
                        0,
                        $"Failed to deserialize response to {typeof(T).Name}"
                    );
                }

                // Handle error responses
                if (response.Status == "failed" && response.Error != null)
                {
                    // Check for authentication errors and throw the appropriate exception type
                    if (IsAuthenticationError(response.Error.Code))
                    {
                        throw new SubsonicAuthenticationException(
                            response.Error.Code,
                            response.Error.Message
                        );
                    }

                    throw SubsonicApiException.FromError(response.Error);
                }

                return response;
            }
            catch (JsonException ex)
            {
                throw new SubsonicApiException(
                    0,
                    $"Failed to parse JSON response: {ex.Message}",
                    ex
                );
            }
            catch (Exception ex) when (ex is not SubsonicApiException)
            {
                throw new SubsonicApiException(
                    0,
                    $"Failed to parse JSON response: {ex.Message}",
                    ex
                );
            }
        }

        /// <summary>
        /// Determines if an error code represents an authentication error.
        /// </summary>
        /// <param name="errorCode">The error code to check.</param>
        /// <returns>True if the error code represents an authentication error.</returns>
        private static bool IsAuthenticationError(int errorCode)
        {
            // Subsonic API error codes for authentication issues
            return errorCode switch
            {
                40 => true, // Wrong username or password
                _ => false,
            };
        }
    }
}
