// <copyright file="Directory.cs" company="Fabian Schmieder">
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
namespace SubSonicMedia.Responses.Browsing.Models
{
    /// <summary>
    /// A directory in the music library.
    /// </summary>
    public class Directory
    {
        /// <summary>
        /// Gets or sets the directory ID.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the parent directory ID.
        /// </summary>
        public string Parent { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the directory name.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the directory path.
        /// </summary>
        public string Path { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the play count.
        /// </summary>
        public int? PlayCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this directory is starred.
        /// </summary>
        public bool Starred { get; set; }

        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        public int? Year { get; set; }

        /// <summary>
        /// Gets or sets the genre.
        /// </summary>
        public string Genre { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the child directories and files.
        /// </summary>
        public List<DirectoryChild> Child { get; set; } = new List<DirectoryChild>();
    }
}
