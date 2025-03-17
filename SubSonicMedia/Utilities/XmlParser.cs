// <copyright file="XmlParser.cs" company="Fabian Schmieder">
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
// along with SubSonicMedia. If not, see &lt;https://www.gnu.org/licenses/&gt;.
// </copyright>

using System;
using System.IO;
using System.Xml.Linq;
using SubSonicMedia.Exceptions;
using SubSonicMedia.Responses;

namespace SubSonicMedia.Utilities
{
    /// <summary>
    /// Utility class for parsing XML responses from the Subsonic API.
    /// </summary>
    internal static class XmlParser
    {
        /// <summary>
        /// Parses an XML string into a strongly-typed response object.
        /// </summary>
        /// <typeparam name="T">The type of response to parse into.</typeparam>
        /// <param name="xml">The XML string to parse.</param>
        /// <returns>The parsed response object.</returns>
        public static T Parse<T>(string xml)
            where T : SubsonicResponse, new()
        {
            try
            {
                var doc = XDocument.Parse(xml);
                var root = doc.Element(
                    XName.Get("subsonic-response", "http://subsonic.org/restapi")
                );

                if (root == null)
                {
                    throw new SubsonicApiException(
                        0,
                        "Invalid response format: subsonic-response element not found"
                    );
                }

                var response = new T
                {
                    Status = root.Attribute("status")?.Value ?? string.Empty,
                    Version = root.Attribute("version")?.Value ?? string.Empty,
                };

                if (response.Status == "failed")
                {
                    var errorElement = root.Element(
                        XName.Get("error", "http://subsonic.org/restapi")
                    );
                    if (errorElement != null)
                    {
                        response.Error = new SubsonicError
                        {
                            Code = int.Parse(errorElement.Attribute("code")?.Value ?? "0"),
                            Message = errorElement.Attribute("message")?.Value ?? string.Empty,
                        };
                    }

                    return response;
                }

                // Parse specific response type based on T
                ParseResponseBody(root, response);

                return response;
            }
            catch (Exception ex) when (ex is not SubsonicApiException)
            {
                throw new SubsonicApiException(
                    0,
                    $"Failed to parse XML response: {ex.Message}",
                    ex
                );
            }
        }

        /// <summary>
        /// Parses an XML stream into a strongly-typed response object.
        /// </summary>
        /// <typeparam name="T">The type of response to parse into.</typeparam>
        /// <param name="stream">The XML stream to parse.</param>
        /// <returns>The parsed response object.</returns>
        public static T Parse<T>(Stream stream)
            where T : SubsonicResponse, new()
        {
            using (var reader = new StreamReader(stream))
            {
                string xml = reader.ReadToEnd();
                return Parse<T>(xml);
            }
        }

        /// <summary>
        /// Parses the response body based on the specific response type.
        /// </summary>
        /// <typeparam name="T">The type of response to parse.</typeparam>
        /// <param name="root">The root XML element.</param>
        /// <param name="response">The response object to populate.</param>
        private static void ParseResponseBody<T>(XElement root, T response)
            where T : SubsonicResponse
        {
            // This will need to be expanded based on specific response types
            // For now, this is a placeholder for custom parsing logic
            // Each response type will have its own specialized parsing

            // The actual implementation would populate the properties of the response object
            // based on the XML content in the root element
        }
    }
}
