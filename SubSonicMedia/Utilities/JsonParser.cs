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
            // Handle different response types based on their properties
            if (
                response is Responses.Playlists.PlaylistsResponse playlistsResponse
                && rootNode["playlists"] != null
            )
            {
                var playlistsNode = rootNode["playlists"];

                // Handle playlists array
                if (playlistsNode["playlist"] is JsonArray playlistsArray)
                {
                    foreach (var playlistNode in playlistsArray)
                    {
                        if (playlistNode == null)
                            continue;

                        var playlist = new Responses.Playlists.PlaylistSummary
                        {
                            Id = playlistNode["id"]?.GetValue<string>(),
                            Name = playlistNode["name"]?.GetValue<string>(),
                            Comment = playlistNode["comment"]?.GetValue<string>(),
                            Owner = playlistNode["owner"]?.GetValue<string>(),
                            Public = playlistNode["public"]?.GetValue<bool>() ?? false,
                            SongCount = playlistNode["songCount"]?.GetValue<int>() ?? 0,
                            Duration = playlistNode["duration"]?.GetValue<int>() ?? 0,
                            CoverArt = playlistNode["coverArt"]?.GetValue<string>()
                        };

                        // Handle dates if present
                        if (playlistNode["created"] != null)
                        {
                            DateTime.TryParse(
                                playlistNode["created"]?.GetValue<string>(),
                                out DateTime created
                            );
                            playlist.Created = created;
                        }

                        if (playlistNode["changed"] != null)
                        {
                            DateTime.TryParse(
                                playlistNode["changed"]?.GetValue<string>(),
                                out DateTime changed
                            );
                            playlist.Changed = changed;
                        }

                        playlistsResponse.Playlists.Playlist.Add(playlist);
                    }
                }
            }
            // Handle playlist details response
            else if (
                response is Responses.Playlists.PlaylistResponse playlistResponse
                && rootNode["playlist"] != null
            )
            {
                var playlistNode = rootNode["playlist"];

                // Set playlist properties
                playlistResponse.Playlist.Id = playlistNode["id"]?.GetValue<string>();
                playlistResponse.Playlist.Name = playlistNode["name"]?.GetValue<string>();
                playlistResponse.Playlist.Comment = playlistNode["comment"]?.GetValue<string>();
                playlistResponse.Playlist.Owner = playlistNode["owner"]?.GetValue<string>();
                playlistResponse.Playlist.Public =
                    playlistNode["public"]?.GetValue<bool>() ?? false;
                playlistResponse.Playlist.SongCount =
                    playlistNode["songCount"]?.GetValue<int>() ?? 0;
                playlistResponse.Playlist.Duration = playlistNode["duration"]?.GetValue<int>() ?? 0;
                playlistResponse.Playlist.CoverArt = playlistNode["coverArt"]?.GetValue<string>();

                // Handle dates if present
                if (playlistNode["created"] != null)
                {
                    DateTime.TryParse(
                        playlistNode["created"]?.GetValue<string>(),
                        out DateTime created
                    );
                    playlistResponse.Playlist.Created = created;
                }

                if (playlistNode["changed"] != null)
                {
                    DateTime.TryParse(
                        playlistNode["changed"]?.GetValue<string>(),
                        out DateTime changed
                    );
                    playlistResponse.Playlist.Changed = changed;
                }

                // Handle songs in the playlist
                if (playlistNode["entry"] is JsonArray songsArray)
                {
                    foreach (var songNode in songsArray)
                    {
                        if (songNode == null)
                            continue;

                        var song = new Responses.Search.Song
                        {
                            Id = songNode["id"]?.GetValue<string>(),
                            Title = songNode["title"]?.GetValue<string>(),
                            Album = songNode["album"]?.GetValue<string>(),
                            Artist = songNode["artist"]?.GetValue<string>(),
                            Track = songNode["track"]?.GetValue<int>(),
                            Year = songNode["year"]?.GetValue<int>(),
                            Genre = songNode["genre"]?.GetValue<string>(),
                            CoverArt = songNode["coverArt"]?.GetValue<string>(),
                            Duration = songNode["duration"]?.GetValue<int>() ?? 0,
                            Size = songNode["size"]?.GetValue<long>() ?? 0,
                            ContentType = songNode["contentType"]?.GetValue<string>(),
                            Path = songNode["path"]?.GetValue<string>()
                        };

                        playlistResponse.Playlist.Entry.Add(song);
                    }
                }
            }
            // Handle Music Folders response
            else if (
                response is Responses.Browsing.MusicFoldersResponse musicFoldersResponse
                && rootNode["musicFolders"] != null
            )
            {
                var musicFoldersNode = rootNode["musicFolders"];

                // Handle music folders array
                if (musicFoldersNode["musicFolder"] is JsonArray foldersArray)
                {
                    foreach (var folderNode in foldersArray)
                    {
                        if (folderNode == null)
                            continue;

                        var folder = new Responses.Browsing.MusicFolder
                        {
                            Id = folderNode["id"]?.GetValue<int>() ?? 0,
                            Name = folderNode["name"]?.GetValue<string>() ?? string.Empty
                        };

                        musicFoldersResponse.MusicFolders.Add(folder);
                    }
                }
            }
            // Handle Artists response
            else if (
                response is Responses.Browsing.ArtistsResponse artistsResponse
                && rootNode["artists"] != null
            )
            {
                var artistsNode = rootNode["artists"];

                // Parse ignored articles if present
                artistsResponse.Artists.IgnoredArticles =
                    artistsNode["ignoredArticles"]?.GetValue<string>() ?? string.Empty;

                // Handle index array (artists are grouped by letter/index)
                if (artistsNode["index"] is JsonArray indexArray)
                {
                    foreach (var indexNode in indexArray)
                    {
                        if (indexNode == null)
                            continue;

                        var index = new Responses.Browsing.Index
                        {
                            Name = indexNode["name"]?.GetValue<string>() ?? string.Empty
                        };

                        // Parse artists in this index
                        if (indexNode["artist"] is JsonArray artistsArray)
                        {
                            foreach (var artistNode in artistsArray)
                            {
                                if (artistNode == null)
                                    continue;

                                var artist = new Responses.Browsing.Artist
                                {
                                    Id = artistNode["id"]?.GetValue<string>() ?? string.Empty,
                                    Name = artistNode["name"]?.GetValue<string>() ?? string.Empty,
                                    CoverArt = artistNode["coverArt"]?.GetValue<string>(),
                                    AlbumCount = artistNode["albumCount"]?.GetValue<int>() ?? 0,
                                    ArtistImageUrl = artistNode[
                                        "artistImageUrl"
                                    ]?.GetValue<string>()
                                };

                                index.Artist.Add(artist);
                            }
                        }

                        artistsResponse.Artists.Index.Add(index);
                    }
                }
            }
            // Handle Search3 response
            else if (
                response is Responses.Search.Search3Response searchResponse
                && rootNode["searchResult3"] != null
            )
            {
                var searchResultNode = rootNode["searchResult3"];

                // Parse artist results
                if (searchResultNode["artist"] is JsonArray artistsArray)
                {
                    foreach (var artistNode in artistsArray)
                    {
                        if (artistNode == null)
                            continue;

                        var artist = new Responses.Search.Artist
                        {
                            Id = artistNode["id"]?.GetValue<string>() ?? string.Empty,
                            Name = artistNode["name"]?.GetValue<string>() ?? string.Empty,
                            AlbumCount = artistNode["albumCount"]?.GetValue<int>() ?? 0,
                            CoverArt = artistNode["coverArt"]?.GetValue<string>()
                        };

                        searchResponse.SearchResult.Artists.Add(artist);
                    }
                }

                // Parse album results
                if (searchResultNode["album"] is JsonArray albumsArray)
                {
                    foreach (var albumNode in albumsArray)
                    {
                        if (albumNode == null)
                            continue;

                        var album = new Responses.Search.Album
                        {
                            Id = albumNode["id"]?.GetValue<string>() ?? string.Empty,
                            Name = albumNode["name"]?.GetValue<string>() ?? string.Empty,
                            Artist = albumNode["artist"]?.GetValue<string>() ?? string.Empty,
                            ArtistId = albumNode["artistId"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = albumNode["coverArt"]?.GetValue<string>(),
                            SongCount = albumNode["songCount"]?.GetValue<int>() ?? 0,
                            Duration = albumNode["duration"]?.GetValue<int>() ?? 0,
                            Year = albumNode["year"]?.GetValue<int>() ?? 0,
                            Genre = albumNode["genre"]?.GetValue<string>()
                        };

                        searchResponse.SearchResult.Albums.Add(album);
                    }
                }

                // Parse song results
                if (searchResultNode["song"] is JsonArray songsArray)
                {
                    foreach (var songNode in songsArray)
                    {
                        if (songNode == null)
                            continue;

                        var song = new Responses.Search.Song
                        {
                            Id = songNode["id"]?.GetValue<string>() ?? string.Empty,
                            Title = songNode["title"]?.GetValue<string>() ?? string.Empty,
                            Album = songNode["album"]?.GetValue<string>() ?? string.Empty,
                            Artist = songNode["artist"]?.GetValue<string>() ?? string.Empty,
                            AlbumId = songNode["albumId"]?.GetValue<string>(),
                            ArtistId = songNode["artistId"]?.GetValue<string>(),
                            Track = songNode["track"]?.GetValue<int>() ?? 0,
                            Year = songNode["year"]?.GetValue<int>() ?? 0,
                            Genre = songNode["genre"]?.GetValue<string>(),
                            CoverArt = songNode["coverArt"]?.GetValue<string>(),
                            Size = songNode["size"]?.GetValue<long>() ?? 0,
                            ContentType = songNode["contentType"]?.GetValue<string>(),
                            Duration = songNode["duration"]?.GetValue<int>() ?? 0,
                            Path = songNode["path"]?.GetValue<string>()
                        };

                        searchResponse.SearchResult.Songs.Add(song);
                    }
                }
            }
            // Handle Artist Response (individual artist with albums)
            else if (
                response is Responses.Browsing.ArtistResponse artistResponse
                && rootNode["artist"] != null
            )
            {
                var artistNode = rootNode["artist"];
                var artist = artistResponse.Artist;

                // Parse artist fields
                artist.Id = artistNode["id"]?.GetValue<string>() ?? string.Empty;
                artist.Name = artistNode["name"]?.GetValue<string>() ?? string.Empty;
                artist.CoverArt = artistNode["coverArt"]?.GetValue<string>();
                artist.AlbumCount = artistNode["albumCount"]?.GetValue<int>() ?? 0;
                artist.ArtistImageUrl = artistNode["artistImageUrl"]?.GetValue<string>();

                // Parse albums
                if (artistNode["album"] is JsonArray albumsArray)
                {
                    foreach (var albumNode in albumsArray)
                    {
                        if (albumNode == null)
                            continue;

                        // Handle different types for Created field (it might be a string date or a long timestamp)
                        long? createdValue = null;
                        if (albumNode["created"] != null)
                        {
                            try
                            {
                                createdValue = albumNode["created"]?.GetValue<long>();
                            }
                            catch
                            {
                                // If it's not a long, it might be a date string, just leave it null
                            }
                        }

                        var album = new Responses.Browsing.AlbumSummary
                        {
                            Id = albumNode["id"]?.GetValue<string>() ?? string.Empty,
                            Name = albumNode["name"]?.GetValue<string>() ?? string.Empty,
                            Artist = albumNode["artist"]?.GetValue<string>() ?? string.Empty,
                            ArtistId = albumNode["artistId"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = albumNode["coverArt"]?.GetValue<string>(),
                            SongCount = albumNode["songCount"]?.GetValue<int>() ?? 0,
                            Duration = albumNode["duration"]?.GetValue<int>() ?? 0,
                            Created = createdValue,
                            Year = albumNode["year"]?.GetValue<int>(),
                            Genre = albumNode["genre"]?.GetValue<string>()
                        };

                        artist.Album.Add(album);
                    }
                }
            }
            // Handle Random Songs response
            else if (
                response is Responses.Browsing.RandomSongsResponse randomSongsResponse
                && rootNode["randomSongs"] != null
            )
            {
                var randomSongsNode = rootNode["randomSongs"];

                // Parse songs
                if (randomSongsNode["song"] is JsonArray songsArray)
                {
                    foreach (var songNode in songsArray)
                    {
                        if (songNode == null)
                            continue;

                        var song = new Responses.Search.Song
                        {
                            Id = songNode["id"]?.GetValue<string>() ?? string.Empty,
                            Title = songNode["title"]?.GetValue<string>() ?? string.Empty,
                            Album = songNode["album"]?.GetValue<string>() ?? string.Empty,
                            Artist = songNode["artist"]?.GetValue<string>() ?? string.Empty,
                            AlbumId = songNode["albumId"]?.GetValue<string>(),
                            ArtistId = songNode["artistId"]?.GetValue<string>(),
                            Track = songNode["track"]?.GetValue<int>() ?? 0,
                            Year = songNode["year"]?.GetValue<int>() ?? 0,
                            Genre = songNode["genre"]?.GetValue<string>(),
                            CoverArt = songNode["coverArt"]?.GetValue<string>(),
                            Size = songNode["size"]?.GetValue<long>() ?? 0,
                            ContentType = songNode["contentType"]?.GetValue<string>(),
                            Duration = songNode["duration"]?.GetValue<int>() ?? 0,
                            Path = songNode["path"]?.GetValue<string>()
                        };

                        randomSongsResponse.RandomSongs.Song.Add(song);
                    }
                }
            }
            // Handle Internet Radio Stations response
            else if (
                response is Responses.Radio.InternetRadioStationsResponse radioStationsResponse
                && rootNode["internetRadioStations"] != null
            )
            {
                var stationsNode = rootNode["internetRadioStations"];

                // Parse radio stations
                if (stationsNode["internetRadioStation"] is JsonArray stationsArray)
                {
                    foreach (var stationNode in stationsArray)
                    {
                        if (stationNode == null)
                            continue;

                        var station = new Responses.Radio.InternetRadioStation
                        {
                            Id = stationNode["id"]?.GetValue<string>() ?? string.Empty,
                            Name = stationNode["name"]?.GetValue<string>() ?? string.Empty,
                            StreamUrl =
                                stationNode["streamUrl"]?.GetValue<string>() ?? string.Empty,
                            HomepageUrl = stationNode["homepageUrl"]?.GetValue<string>()
                        };

                        radioStationsResponse.InternetRadioStations.InternetRadioStation.Add(
                            station
                        );
                    }
                }
            }
            // Handle Other Responses - placeholder for base responses
            else if (response is Responses.System.BaseResponse)
            {
                // Base response doesn't need additional parsing
            }
        }

        /// <summary>
        /// Parses a datetime value from a JSON node.
        /// </summary>
        /// <param name="jsonNode">The JSON node potentially containing a datetime value.</param>
        /// <returns>The parsed DateTime, or DateTime.MinValue if parsing fails.</returns>
        private static DateTime ParseDateTimeValue(JsonNode jsonNode)
        {
            if (jsonNode == null)
                return DateTime.MinValue;

            try
            {
                var stringValue = jsonNode.GetValue<string>();
                if (DateTime.TryParse(stringValue, out DateTime result))
                {
                    return result;
                }
            }
            catch
            {
                try
                {
                    // It might be a timestamp
                    var longValue = jsonNode.GetValue<long>();
                    return DateTimeOffset.FromUnixTimeMilliseconds(longValue).DateTime;
                }
                catch
                {
                    // If all parsing fails, return the min value
                }
            }

            return DateTime.MinValue;
        }

        /// <summary>
        /// Parses a timespan value from a JSON node (in seconds).
        /// </summary>
        /// <param name="jsonNode">The JSON node potentially containing a timespan value in seconds.</param>
        /// <returns>The parsed TimeSpan, or TimeSpan.Zero if parsing fails.</returns>
        private static TimeSpan ParseTimeSpanSeconds(JsonNode jsonNode)
        {
            if (jsonNode == null)
                return TimeSpan.Zero;

            try
            {
                var intValue = jsonNode.GetValue<int>();
                return TimeSpan.FromSeconds(intValue);
            }
            catch
            {
                return TimeSpan.Zero;
            }
        }

        /// <summary>
        /// Parses an array of integers from a JSON node.
        /// </summary>
        /// <param name="jsonNode">The JSON node containing integers.</param>
        /// <returns>A list of integers, or an empty list if parsing fails.</returns>
        private static List<int> ParseIntArrayValue(JsonNode jsonNode)
        {
            var result = new List<int>();

            if (jsonNode is JsonArray jsonArray)
            {
                foreach (var item in jsonArray)
                {
                    if (item == null)
                        continue;

                    try
                    {
                        result.Add(item.GetValue<int>());
                    }
                    catch
                    {
                        // Skip items that can't be parsed as integers
                    }
                }
            }
            else if (jsonNode != null)
            {
                try
                {
                    // It might be a single int
                    result.Add(jsonNode.GetValue<int>());
                }
                catch
                {
                    // If parsing fails, just return an empty list
                }
            }

            return result;
        }
    }
}
