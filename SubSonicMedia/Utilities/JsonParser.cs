// <copyright file="JsonParser.cs" company="Fabian Schmieder">
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
// along with SubSonicMedia. If not, see <https://www.gnu.org/licenses/>.
// </copyright>

using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;
using SubSonicMedia.Exceptions;
using SubSonicMedia.Responses;

namespace SubSonicMedia.Utilities
{
    /// <summary>
    /// Utility class for parsing JSON responses from the Subsonic API.
    /// </summary>
    internal static class JsonParser
    {
        /// <summary>
        /// Parses a JSON string into a strongly-typed response object.
        /// </summary>
        /// <typeparam name="T">The type of response to parse into.</typeparam>
        /// <param name="json">The JSON string to parse.</param>
        /// <returns>The parsed response object.</returns>
        public static T Parse<T>(string json)
            where T : SubsonicResponse, new()
        {
            try
            {
                // Parse JSON to JsonNode
                var jsonNode = JsonNode.Parse(json);
                var rootNode = jsonNode?["subsonic-response"];

                if (rootNode == null)
                {
                    throw new SubsonicApiException(
                        0,
                        "Invalid response format: subsonic-response element not found"
                    );
                }

                var response = new T
                {
                    Status = rootNode["status"]?.GetValue<string>() ?? string.Empty,
                    Version = rootNode["version"]?.GetValue<string>() ?? string.Empty,
                };

                if (response.Status == "failed")
                {
                    var errorNode = rootNode["error"];
                    if (errorNode != null)
                    {
                        response.Error = new SubsonicError
                        {
                            Code = errorNode["code"]?.GetValue<int>() ?? 0,
                            Message = errorNode["message"]?.GetValue<string>() ?? string.Empty,
                        };
                    }

                    return response;
                }

                // Parse specific response type based on T
                ParseResponseBody(rootNode, response);

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
        /// Parses a JSON stream into a strongly-typed response object.
        /// </summary>
        /// <typeparam name="T">The type of response to parse into.</typeparam>
        /// <param name="stream">The JSON stream to parse.</param>
        /// <returns>The parsed response object.</returns>
        public static T Parse<T>(Stream stream)
            where T : SubsonicResponse, new()
        {
            using (var reader = new StreamReader(stream))
            {
                string json = reader.ReadToEnd();
                return Parse<T>(json);
            }
        }

        /// <summary>
        /// Parses the response body based on the specific response type.
        /// </summary>
        /// <typeparam name="T">The type of response to parse.</typeparam>
        /// <param name="rootNode">The root JSON node.</param>
        /// <param name="response">The response object to populate.</param>
        private static void ParseResponseBody<T>(JsonNode rootNode, T response)
            where T : SubsonicResponse
        {
            // This will need to be expanded based on specific response types
            // For now, this is a placeholder for custom parsing logic
            // Each response type will have its own specialized parsing
        }
    }
}
