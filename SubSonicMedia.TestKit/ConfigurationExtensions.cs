// <copyright file="ConfigurationExtensions.cs" company="Fabian Schmieder">
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Microsoft.Extensions.Configuration;

using SubSonicMedia.TestKit.Helpers;

namespace SubSonicMedia.TestKit
{
    /// <summary>
    /// Extension methods for configuration.
    /// </summary>
    public static class ConfigurationExtensions
    {
        /// <summary>
        /// Adds a .env file as a configuration source.
        /// </summary>
        /// <param name="builder">The configuration builder instance.</param>
        /// <param name="path">The path to the .env file to load.</param>
        /// <param name="optional">Whether the file is optional or required.</param>
        /// <returns>The configuration builder for chaining.</returns>
        public static IConfigurationBuilder AddDotEnvFile(
            this IConfigurationBuilder builder,
            string path,
            bool optional
        )
        {
            ArgumentNullException.ThrowIfNull(builder);

            try
            {
                if (!File.Exists(path) && !optional)
                {
                    ConsoleHelper.LogError($".env file not found at: {path}");
                    throw new FileNotFoundException($".env file not found at: {path}");
                }

                if (!File.Exists(path))
                {
                    return builder;
                }

                // Read all lines from the file
                var lines = File.ReadAllLines(path);
                var envVars = new Dictionary<string, string>();

                foreach (var line in lines)
                {
                    // Skip comments and empty lines
                    if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                    {
                        continue;
                    }

                    // Parse key-value pairs
                    var parts = line.Split('=', 2);
                    if (parts.Length != 2)
                    {
                        continue;
                    }

                    var key = parts[0].Trim();
                    var value = parts[1].Trim();

                    // Remove quotes if present
                    if (value.StartsWith('"') && value.EndsWith('"'))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }

                    if (value.StartsWith('\'') && value.EndsWith('\''))
                    {
                        value = value.Substring(1, value.Length - 2);
                    }

                    envVars[key] = value;
                }

                // Add the environment variables
                builder.AddInMemoryCollection(
                    envVars.Select(kvp => new KeyValuePair<string, string?>(kvp.Key, kvp.Value))
                );

                return builder;
            }
            catch (Exception ex) when (ex is not FileNotFoundException)
            {
                ConsoleHelper.LogError($"Error reading .env file: {ex.Message}");
                if (!optional)
                {
                    throw;
                }

                return builder;
            }
        }
    }
}
