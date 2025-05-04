// <copyright file="Share.cs" company="Fabian Schmieder">
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
namespace SubSonicMedia.Responses.Sharing.Models
{
    /// <summary>
    /// Details of a share.
    /// </summary>
    public class Share
    {
        /// <summary>
        /// Gets or sets the share ID.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the URL to access the share.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Gets or sets the share description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the username of the user who created the share.
        /// </summary>
        public string? Username { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the share was created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the share expires.
        /// </summary>
        public DateTime? Expires { get; set; }

        /// <summary>
        /// Gets or sets the date and time when the share was last visited.
        /// </summary>
        public DateTime? LastVisited { get; set; }

        /// <summary>
        /// Gets or sets the number of times the share has been visited.
        /// </summary>
        public int VisitCount { get; set; }

        /// <summary>
        /// Gets or sets the list of entries in the share.
        /// </summary>
        public List<ShareEntry> Entry { get; set; } = new List<ShareEntry>();
    }
}
