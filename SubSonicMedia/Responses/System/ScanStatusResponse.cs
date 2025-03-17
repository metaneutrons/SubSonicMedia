// <copyright file="ScanStatusResponse.cs" company="Fabian Schmieder">
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

namespace SubSonicMedia.Responses.System
{
    /// <summary>
    /// Response containing media library scan status.
    /// </summary>
    public class ScanStatusResponse : SubsonicResponse
    {
        /// <summary>
        /// Gets or sets the scan status.
        /// </summary>
        public ScanStatus ScanStatus { get; set; } = new ScanStatus();
    }

    /// <summary>
    /// Details about the media library scan status.
    /// </summary>
    public class ScanStatus
    {
        /// <summary>
        /// Gets or sets a value indicating whether a scan is currently running.
        /// </summary>
        public bool Scanning { get; set; }

        /// <summary>
        /// Gets or sets the number of files scanned so far.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Gets or sets the number of folders scanned.
        /// </summary>
        public int FolderCount { get; set; }

        /// <summary>
        /// Gets or sets the folder that is currently being scanned.
        /// </summary>
        public string Folder { get; set; }
    }
}
