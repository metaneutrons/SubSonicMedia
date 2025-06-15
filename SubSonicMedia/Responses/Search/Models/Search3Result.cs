// <copyright file="Search3Result.cs" company="Fabian Schmieder">
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
using System.Text.Json.Serialization;
using SubSonicMedia.Serialization.Converters;

namespace SubSonicMedia.Responses.Search.Models
{
    /// <summary>
    /// Contains search results for search3.
    /// </summary>
    public class Search3Result
    {
        /// <summary>
        /// Gets or sets the list of matching artists.
        /// </summary>
        [JsonPropertyName("artist")]
        [JsonConverter(typeof(SubsonicCollectionConverter<Artist>))]
        public List<Artist> Artists { get; set; } = new List<Artist>();

        /// <summary>
        /// Gets or sets the list of matching albums.
        /// </summary>
        [JsonPropertyName("album")]
        [JsonConverter(typeof(SubsonicCollectionConverter<Album>))]
        public List<Album> Albums { get; set; } = new List<Album>();

        /// <summary>
        /// Gets or sets the list of matching songs.
        /// </summary>
        [JsonPropertyName("song")]
        [JsonConverter(typeof(SubsonicCollectionConverter<Song>))]
        public List<Song> Songs { get; set; } = new List<Song>();

        /// <summary>
        /// Gets or sets the offset for artists.
        /// </summary>
        [JsonPropertyName("artistOffset")]
        public int ArtistOffset { get; set; }

        /// <summary>
        /// Gets or sets the offset for albums.
        /// </summary>
        [JsonPropertyName("albumOffset")]
        public int AlbumOffset { get; set; }

        /// <summary>
        /// Gets or sets the offset for songs.
        /// </summary>
        [JsonPropertyName("songOffset")]
        public int SongOffset { get; set; }

        /// <summary>
        /// Gets or sets the total number of artists found.
        /// </summary>
        [JsonPropertyName("artistCount")]
        public int ArtistCount { get; set; }

        /// <summary>
        /// Gets or sets the total number of albums found.
        /// </summary>
        [JsonPropertyName("albumCount")]
        public int AlbumCount { get; set; }

        /// <summary>
        /// Gets or sets the total number of songs found.
        /// </summary>
        [JsonPropertyName("songCount")]
        public int SongCount { get; set; }
    }
}
