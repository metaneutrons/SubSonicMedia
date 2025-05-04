// <copyright file="VersionInfo.cs" company="Fabian Schmieder">
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
using System.Reflection;

namespace SubSonicMedia
{
    /// <summary>
    /// Provides version information for the SubSonicMedia library.
    /// </summary>
    public static class VersionInfo
    {
        private static readonly Lazy<string> LazyVersion = new Lazy<string>(() =>
        {
            var assembly = typeof(VersionInfo).Assembly;
            var infoVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            return infoVersion?.InformationalVersion ?? "0.0.0";
        });

        private static readonly Lazy<string> LazySubsonicApiVersion = new Lazy<string>(() =>
        {
            var assembly = typeof(VersionInfo).Assembly;
            var apiVersionAttr = assembly
                .GetCustomAttributes<AssemblyMetadataAttribute>()
                .FirstOrDefault(attr =>
                    attr.Key.Equals("SubsonicApiVersion", StringComparison.OrdinalIgnoreCase)
                );
            return apiVersionAttr?.Value ?? "1.16.1"; // Fallback if not found
        });

        /// <summary>
        /// Gets the client library version.
        /// </summary>
        public static string Version => LazyVersion.Value;

        /// <summary>
        /// Gets the maximum supported Subsonic API version.
        /// </summary>
        public static string SubsonicApiVersion => LazySubsonicApiVersion.Value;

        /// <summary>
        /// Gets a value indicating whether a specified Subsonic API version is supported.
        /// </summary>
        /// <param name="apiVersion">The Subsonic API version to check.</param>
        /// <returns>True if the specified version is supported, otherwise false.</returns>
        public static bool IsApiVersionSupported(string apiVersion)
        {
            if (string.IsNullOrEmpty(apiVersion))
            {
                return false;
            }

            // Parse versions into Subsonic-style numeric format (e.g., "1.16.1" -> 1161)
            if (
                !TryParseSubsonicVersion(apiVersion, out var requestedVersion)
                || !TryParseSubsonicVersion(SubsonicApiVersion, out var supportedVersion)
            )
            {
                return false;
            }

            return requestedVersion <= supportedVersion;
        }

        private static bool TryParseSubsonicVersion(string versionString, out int versionNumber)
        {
            versionNumber = 0;

            if (string.IsNullOrEmpty(versionString))
            {
                return false;
            }

            var parts = versionString.Split('.');
            if (parts.Length < 2 || parts.Length > 3)
            {
                return false;
            }

            if (!int.TryParse(parts[0], out var major) || !int.TryParse(parts[1], out var minor))
            {
                return false;
            }

            var patch = 0;
            if (parts.Length > 2 && !int.TryParse(parts[2], out patch))
            {
                return false;
            }

            versionNumber = (major * 1000) + (minor * 10) + patch;
            return true;
        }
    }
}
