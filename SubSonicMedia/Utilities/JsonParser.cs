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
// along with SubSonicMedia. If not, see https://www.gnu.org/licenses/.
// </copyright>
using System.Text.Json;
using System.Text.Json.Nodes;

using SubSonicMedia.Exceptions;
using SubSonicMedia.Responses;
using SubSonicMedia.Responses.Browsing.Models;

namespace SubSonicMedia.Utilities
{
    /// <summary>
    /// Utility class for parsing JSON responses from the Subsonic API.
    /// </summary>
    internal static class JsonParser
    {
        /// <summary>
        /// Parses a JSON string into a strongly typed response object.
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
        /// Parses a JSON stream into a strongly typed response object.
        /// </summary>
        /// <typeparam name="T">The type of response to parse into.</typeparam>
        /// <param name="stream">The JSON stream to parse.</param>
        /// <returns>The parsed response object.</returns>
        public static T Parse<T>(Stream stream)
            where T : SubsonicResponse, new()
        {
            using (var reader = new StreamReader(stream))
            {
                var json = reader.ReadToEnd();
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
                if (playlistsNode?["playlist"] is JsonArray playlistsArray)
                {
                    foreach (var playlistNode in playlistsArray)
                    {
                        if (playlistNode == null)
                        {
                            continue;
                        }

                        var playlist = new Responses.Playlists.Models.PlaylistSummary
                        {
                            Id = playlistNode["id"]?.GetValue<string>() ?? string.Empty,
                            Name = playlistNode["name"]?.GetValue<string>() ?? string.Empty,
                            Comment = playlistNode["comment"]?.GetValue<string>(),
                            Owner = playlistNode["owner"]?.GetValue<string>(),
                            Public = playlistNode["public"]?.GetValue<bool>() ?? false,
                            SongCount = playlistNode["songCount"]?.GetValue<int>() ?? 0,
                            Duration = playlistNode["duration"]?.GetValue<int>() ?? 0,
                            CoverArt = playlistNode["coverArt"]?.GetValue<string>(),
                        };

                        // Handle dates if present
                        if (playlistNode["created"] != null)
                        {
                            DateTime.TryParse(
                                playlistNode["created"]?.GetValue<string>(),
                                out var created
                            );
                            playlist.Created = created;
                        }

                        if (playlistNode["changed"] != null)
                        {
                            DateTime.TryParse(
                                playlistNode["changed"]?.GetValue<string>(),
                                out var changed
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
                var playlistNode = rootNode?["playlist"];

                // Set playlist properties
                playlistResponse.Playlist.Id =
                    playlistNode?["id"]?.GetValue<string>() ?? string.Empty;
                playlistResponse.Playlist.Name =
                    playlistNode?["name"]?.GetValue<string>() ?? string.Empty;
                playlistResponse.Playlist.Comment = playlistNode?["comment"]?.GetValue<string>();
                playlistResponse.Playlist.Owner = playlistNode?["owner"]?.GetValue<string>();
                playlistResponse.Playlist.Public =
                    playlistNode?["public"]?.GetValue<bool>() ?? false;
                playlistResponse.Playlist.SongCount =
                    playlistNode?["songCount"]?.GetValue<int>() ?? 0;
                playlistResponse.Playlist.Duration =
                    playlistNode?["duration"]?.GetValue<int>() ?? 0;
                playlistResponse.Playlist.CoverArt = playlistNode?["coverArt"]?.GetValue<string>();

                // Handle dates if present
                if (playlistNode?["created"] != null)
                {
                    DateTime.TryParse(
                        playlistNode?["created"]?.GetValue<string>(),
                        out var created
                    );
                    playlistResponse.Playlist.Created = created;
                }

                if (playlistNode?["changed"] != null)
                {
                    DateTime.TryParse(
                        playlistNode?["changed"]?.GetValue<string>(),
                        out var changed
                    );
                    playlistResponse.Playlist.Changed = changed;
                }

                // Handle songs in the playlist
                if (playlistNode?["entry"] is JsonArray songsArray)
                {
                    foreach (var songNode in songsArray)
                    {
                        if (songNode == null)
                        {
                            continue;
                        }

                        var song = new Responses.Search.Models.Song
                        {
                            Id = songNode["id"]?.GetValue<string>() ?? string.Empty,
                            Title = songNode["title"]?.GetValue<string>() ?? string.Empty,
                            Album = songNode["album"]?.GetValue<string>() ?? string.Empty,
                            Artist = songNode["artist"]?.GetValue<string>() ?? string.Empty,
                            Track = songNode["track"]?.GetValue<int>() ?? 0,
                            Year = songNode["year"]?.GetValue<int>() ?? 0,
                            Genre = songNode["genre"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = songNode["coverArt"]?.GetValue<string>() ?? string.Empty,
                            Duration = songNode["duration"]?.GetValue<int>() ?? 0,
                            Size = songNode["size"]?.GetValue<long>() ?? 0,
                            ContentType =
                                songNode["contentType"]?.GetValue<string>() ?? string.Empty,
                            Path = songNode["path"]?.GetValue<string>() ?? string.Empty,
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
                if (musicFoldersNode?["musicFolder"] is JsonArray foldersArray)
                {
                    foreach (var folderNode in foldersArray)
                    {
                        if (folderNode == null)
                        {
                            continue;
                        }

                        var folder = new MusicFolder
                        {
                            Id = folderNode["id"]?.GetValue<int>() ?? 0,
                            Name = folderNode["name"]?.GetValue<string>() ?? string.Empty,
                        };

                        musicFoldersResponse.MusicFolders.Add(folder);
                    }
                }
            }
            // Handle Indexes response
            else if (
                response is Responses.Browsing.IndexesResponse indexesResponse
                && rootNode["indexes"] != null
            )
            {
                var indexesNode = rootNode["indexes"];

                // Parse last modified timestamp (convert long timestamp to DateTime)
                if (indexesNode?["lastModified"] != null)
                {
                    try
                    {
                        var lastModifiedLong = indexesNode["lastModified"]?.GetValue<long>() ?? 0;
                        indexesResponse.Indexes.LastModified = DateTimeOffset
                            .FromUnixTimeMilliseconds(lastModifiedLong)
                            .DateTime;
                    }
                    catch
                    {
                        // If we can't parse, leave it as default
                    }
                }

                // Handle index array (artists are grouped by letter/index)
                if (indexesNode?["index"] is JsonArray indexArray)
                {
                    foreach (var indexNode in indexArray)
                    {
                        if (indexNode == null)
                        {
                            continue;
                        }

                        var index = new SubSonicMedia.Responses.Browsing.Models.Index
                        {
                            Name = indexNode["name"]?.GetValue<string>() ?? string.Empty,
                        };

                        // Parse artists in this index
                        if (indexNode["artist"] is JsonArray artistsArray)
                        {
                            foreach (var artistNode in artistsArray)
                            {
                                if (artistNode == null)
                                {
                                    continue;
                                }

                                var artist = new SubSonicMedia.Responses.Browsing.Artist
                                {
                                    Id = artistNode["id"]?.GetValue<string>() ?? string.Empty,
                                    Name = artistNode["name"]?.GetValue<string>() ?? string.Empty,
                                    CoverArt = artistNode["coverArt"]?.GetValue<string>(),
                                    AlbumCount = artistNode["albumCount"]?.GetValue<int>() ?? 0,
                                    ArtistImageUrl = artistNode["artistImageUrl"]
                                        ?.GetValue<string>(),
                                };

                                index.Artist.Add(artist);
                            }
                        }

                        indexesResponse.Indexes.Index.Add(index);
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
                    artistsNode?["ignoredArticles"]?.GetValue<string>() ?? string.Empty;

                // Handle index array (artists are grouped by letter/index)
                if (artistsNode?["index"] is JsonArray indexArray)
                {
                    foreach (var indexNode in indexArray)
                    {
                        if (indexNode == null)
                        {
                            continue;
                        }

                        var index = new SubSonicMedia.Responses.Browsing.Models.Index
                        {
                            Name = indexNode["name"]?.GetValue<string>() ?? string.Empty,
                        };

                        // Parse artists in this index
                        if (indexNode["artist"] is JsonArray artistsArray)
                        {
                            foreach (var artistNode in artistsArray)
                            {
                                if (artistNode == null)
                                {
                                    continue;
                                }

                                var artist = new SubSonicMedia.Responses.Browsing.Artist
                                {
                                    Id = artistNode["id"]?.GetValue<string>() ?? string.Empty,
                                    Name = artistNode["name"]?.GetValue<string>() ?? string.Empty,
                                    CoverArt = artistNode["coverArt"]?.GetValue<string>(),
                                    AlbumCount = artistNode["albumCount"]?.GetValue<int>() ?? 0,
                                    ArtistImageUrl = artistNode["artistImageUrl"]
                                        ?.GetValue<string>(),
                                };

                                index.Artist.Add(artist);
                            }
                        }

                        artistsResponse.Artists.Index.Add(index);
                    }
                }
            }
            // Handle Search response
            else if (
                response is Responses.Search.SearchResponse searchResponse
                && rootNode["searchResult"] != null
            )
            {
                var searchResultNode = rootNode?["searchResult"];

                // Parse artist results
                if (searchResultNode?["artist"] is JsonArray artistsArray)
                {
                    foreach (var artistNode in artistsArray)
                    {
                        if (artistNode == null)
                        {
                            continue;
                        }

                        var artist = new Responses.Search.Models.Artist
                        {
                            Id = artistNode["id"]?.GetValue<string>() ?? string.Empty,
                            Name = artistNode["name"]?.GetValue<string>() ?? string.Empty,
                            AlbumCount = artistNode["albumCount"]?.GetValue<int>() ?? 0,
                            CoverArt = artistNode["coverArt"]?.GetValue<string>(),
                        };

                        searchResponse.SearchResult.Artists.Add(artist);
                    }
                }

                // Parse album results
                if (searchResultNode?["album"] is JsonArray albumsArray)
                {
                    foreach (var albumNode in albumsArray)
                    {
                        if (albumNode == null)
                        {
                            continue;
                        }

                        var album = new Responses.Search.Models.Album
                        {
                            Id = albumNode["id"]?.GetValue<string>() ?? string.Empty,
                            Name = albumNode["name"]?.GetValue<string>() ?? string.Empty,
                            Artist = albumNode["artist"]?.GetValue<string>() ?? string.Empty,
                            ArtistId = albumNode["artistId"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = albumNode["coverArt"]?.GetValue<string>(),
                            SongCount = albumNode["songCount"]?.GetValue<int>() ?? 0,
                            Duration = albumNode["duration"]?.GetValue<int>() ?? 0,
                            Year = albumNode["year"]?.GetValue<int>() ?? 0,
                            Genre = albumNode["genre"]?.GetValue<string>(),
                        };

                        searchResponse.SearchResult.Albums.Add(album);
                    }
                }

                // Parse song results
                if (searchResultNode?["song"] is JsonArray songsArray)
                {
                    foreach (var songNode in songsArray)
                    {
                        if (songNode == null)
                        {
                            continue;
                        }

                        var song = new Responses.Search.Models.Song
                        {
                            Id = songNode["id"]?.GetValue<string>() ?? string.Empty,
                            Title = songNode["title"]?.GetValue<string>() ?? string.Empty,
                            Album = songNode["album"]?.GetValue<string>() ?? string.Empty,
                            Artist = songNode["artist"]?.GetValue<string>() ?? string.Empty,
                            AlbumId = songNode["albumId"]?.GetValue<string>() ?? string.Empty,
                            ArtistId = songNode["artistId"]?.GetValue<string>() ?? string.Empty,
                            Track = songNode["track"]?.GetValue<int>() ?? 0,
                            Year = songNode["year"]?.GetValue<int>() ?? 0,
                            Genre = songNode["genre"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = songNode["coverArt"]?.GetValue<string>() ?? string.Empty,
                            Size = songNode["size"]?.GetValue<long>() ?? 0,
                            ContentType =
                                songNode["contentType"]?.GetValue<string>() ?? string.Empty,
                            Duration = songNode["duration"]?.GetValue<int>() ?? 0,
                            Path = songNode["path"]?.GetValue<string>() ?? string.Empty,
                        };

                        searchResponse.SearchResult.Songs.Add(song);
                    }
                }
            }
            // Handle Search2 response (same format as Search)
            else if (
                response is Responses.Search.SearchResponse search2Response
                && rootNode["searchResult2"] != null
            )
            {
                var searchResultNode = rootNode?["searchResult2"];

                // Parse artist results
                if (searchResultNode?["artist"] is JsonArray artistsArray)
                {
                    foreach (var artistNode in artistsArray)
                    {
                        if (artistNode == null)
                        {
                            continue;
                        }

                        var artist = new Responses.Search.Models.Artist
                        {
                            Id = artistNode["id"]?.GetValue<string>() ?? string.Empty,
                            Name = artistNode["name"]?.GetValue<string>() ?? string.Empty,
                            AlbumCount = artistNode["albumCount"]?.GetValue<int>() ?? 0,
                            CoverArt = artistNode["coverArt"]?.GetValue<string>(),
                        };

                        search2Response.SearchResult.Artists.Add(artist);
                    }
                }

                // Parse album results
                if (searchResultNode?["album"] is JsonArray albumsArray)
                {
                    foreach (var albumNode in albumsArray)
                    {
                        if (albumNode == null)
                        {
                            continue;
                        }

                        var album = new Responses.Search.Models.Album
                        {
                            Id = albumNode["id"]?.GetValue<string>() ?? string.Empty,
                            Name = albumNode["name"]?.GetValue<string>() ?? string.Empty,
                            Artist = albumNode["artist"]?.GetValue<string>() ?? string.Empty,
                            ArtistId = albumNode["artistId"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = albumNode["coverArt"]?.GetValue<string>(),
                            SongCount = albumNode["songCount"]?.GetValue<int>() ?? 0,
                            Duration = albumNode["duration"]?.GetValue<int>() ?? 0,
                            Year = albumNode["year"]?.GetValue<int>() ?? 0,
                            Genre = albumNode["genre"]?.GetValue<string>(),
                        };

                        search2Response.SearchResult.Albums.Add(album);
                    }
                }

                // Parse song results
                if (searchResultNode?["song"] is JsonArray songsArray)
                {
                    foreach (var songNode in songsArray)
                    {
                        if (songNode == null)
                        {
                            continue;
                        }

                        var song = new Responses.Search.Models.Song
                        {
                            Id = songNode["id"]?.GetValue<string>() ?? string.Empty,
                            Title = songNode["title"]?.GetValue<string>() ?? string.Empty,
                            Album = songNode["album"]?.GetValue<string>() ?? string.Empty,
                            Artist = songNode["artist"]?.GetValue<string>() ?? string.Empty,
                            AlbumId = songNode["albumId"]?.GetValue<string>() ?? string.Empty,
                            ArtistId = songNode["artistId"]?.GetValue<string>() ?? string.Empty,
                            Track = songNode["track"]?.GetValue<int>() ?? 0,
                            Year = songNode["year"]?.GetValue<int>() ?? 0,
                            Genre = songNode["genre"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = songNode["coverArt"]?.GetValue<string>() ?? string.Empty,
                            Size = songNode["size"]?.GetValue<long>() ?? 0,
                            ContentType =
                                songNode["contentType"]?.GetValue<string>() ?? string.Empty,
                            Duration = songNode["duration"]?.GetValue<int>() ?? 0,
                            Path = songNode["path"]?.GetValue<string>() ?? string.Empty,
                        };

                        search2Response.SearchResult.Songs.Add(song);
                    }
                }
            }
            // Handle Search3 response
            else if (
                response is Responses.Search.Search3Response search3Response
                && rootNode["searchResult3"] != null
            )
            {
                var searchResultNode = rootNode?["searchResult3"];

                // Parse artist results
                if (searchResultNode?["artist"] is JsonArray artistsArray)
                {
                    foreach (var artistNode in artistsArray)
                    {
                        if (artistNode == null)
                        {
                            continue;
                        }

                        var artist = new Responses.Search.Models.Artist
                        {
                            Id = artistNode["id"]?.GetValue<string>() ?? string.Empty,
                            Name = artistNode["name"]?.GetValue<string>() ?? string.Empty,
                            AlbumCount = artistNode["albumCount"]?.GetValue<int>() ?? 0,
                            CoverArt = artistNode["coverArt"]?.GetValue<string>(),
                        };

                        search3Response.SearchResult.Artists.Add(artist);
                    }
                }

                // Parse additional count/offset information
                search3Response.SearchResult.ArtistCount =
                    searchResultNode?["artistCount"]?.GetValue<int>() ?? 0;
                search3Response.SearchResult.ArtistOffset =
                    searchResultNode?["artistOffset"]?.GetValue<int>() ?? 0;
                search3Response.SearchResult.AlbumCount =
                    searchResultNode?["albumCount"]?.GetValue<int>() ?? 0;
                search3Response.SearchResult.AlbumOffset =
                    searchResultNode?["albumOffset"]?.GetValue<int>() ?? 0;
                search3Response.SearchResult.SongCount =
                    searchResultNode?["songCount"]?.GetValue<int>() ?? 0;
                search3Response.SearchResult.SongOffset =
                    searchResultNode?["songOffset"]?.GetValue<int>() ?? 0;

                // Parse album results
                if (searchResultNode?["album"] is JsonArray albumsArray)
                {
                    foreach (var albumNode in albumsArray)
                    {
                        if (albumNode == null)
                        {
                            continue;
                        }

                        var album = new Responses.Search.Models.Album
                        {
                            Id = albumNode["id"]?.GetValue<string>() ?? string.Empty,
                            Name = albumNode["name"]?.GetValue<string>() ?? string.Empty,
                            Artist = albumNode["artist"]?.GetValue<string>() ?? string.Empty,
                            ArtistId = albumNode["artistId"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = albumNode["coverArt"]?.GetValue<string>(),
                            SongCount = albumNode["songCount"]?.GetValue<int>() ?? 0,
                            Duration = albumNode["duration"]?.GetValue<int>() ?? 0,
                            Year = albumNode["year"]?.GetValue<int>() ?? 0,
                            Genre = albumNode["genre"]?.GetValue<string>() ?? string.Empty,
                        };

                        search3Response.SearchResult.Albums.Add(album);
                    }
                }

                // Parse song results
                if (searchResultNode?["song"] is JsonArray songsArray)
                {
                    foreach (var songNode in songsArray)
                    {
                        if (songNode == null)
                        {
                            continue;
                        }

                        var song = new Responses.Search.Models.Song
                        {
                            Id = songNode["id"]?.GetValue<string>() ?? string.Empty,
                            Title = songNode["title"]?.GetValue<string>() ?? string.Empty,
                            Album = songNode["album"]?.GetValue<string>() ?? string.Empty,
                            Artist = songNode["artist"]?.GetValue<string>() ?? string.Empty,
                            AlbumId = songNode["albumId"]?.GetValue<string>() ?? string.Empty,
                            ArtistId = songNode["artistId"]?.GetValue<string>() ?? string.Empty,
                            Track = songNode["track"]?.GetValue<int>() ?? 0,
                            Year = songNode["year"]?.GetValue<int>() ?? 0,
                            Genre = songNode["genre"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = songNode["coverArt"]?.GetValue<string>() ?? string.Empty,
                            Size = songNode["size"]?.GetValue<long>() ?? 0,
                            ContentType =
                                songNode["contentType"]?.GetValue<string>() ?? string.Empty,
                            Duration = songNode["duration"]?.GetValue<int>() ?? 0,
                            Path = songNode["path"]?.GetValue<string>() ?? string.Empty,
                        };

                        search3Response.SearchResult.Songs.Add(song);
                    }
                }
            }
            // Handle PlayQueue response
            else if (
                response is Responses.Playlists.PlayQueueResponse playQueueResponse
                && rootNode["playQueue"] != null
            )
            {
                var playQueueNode = rootNode["playQueue"];

                // Set playQueue properties
                playQueueResponse.PlayQueue.Current = playQueueNode?["current"]?.GetValue<string>();
                playQueueResponse.PlayQueue.Position =
                    playQueueNode?["position"]?.GetValue<long>() ?? 0;
                playQueueResponse.PlayQueue.Username = playQueueNode?[
                    "username"
                ]?.GetValue<string>();

                // Handle changed timestamp (might be in different formats)
                if (playQueueNode?["changed"] != null)
                {
                    try
                    {
                        // It might be a datetime string
                        var changedString = playQueueNode?["changed"]?.GetValue<string>();
                        if (DateTime.TryParse(changedString, out var changedDateTime))
                        {
                            playQueueResponse.PlayQueue.Changed = new DateTimeOffset(
                                changedDateTime
                            ).ToUnixTimeMilliseconds();
                        }
                    }
                    catch
                    {
                        try
                        {
                            // Or it might be a timestamp already
                            playQueueResponse.PlayQueue.Changed =
                                playQueueNode?["changed"]?.GetValue<long>() ?? 0;
                        }
                        catch
                        {
                            // If parsing fails, leave it as 0
                        }
                    }
                }

                // Parse entries/songs in the play queue
                if (playQueueNode?["entry"] is JsonArray entriesArray)
                {
                    foreach (var entryNode in entriesArray)
                    {
                        if (entryNode == null)
                        {
                            continue;
                        }

                        var entry = new Responses.Browsing.Models.Child
                        {
                            Id = entryNode["id"]?.GetValue<string>() ?? string.Empty,
                            Title = entryNode["title"]?.GetValue<string>() ?? string.Empty,
                            Album = entryNode["album"]?.GetValue<string>() ?? string.Empty,
                            Artist = entryNode["artist"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = entryNode["coverArt"]?.GetValue<string>() ?? string.Empty,
                            Duration = entryNode["duration"]?.GetValue<int>() ?? 0,
                            BitRate = entryNode["bitRate"]?.GetValue<int>() ?? 0,
                            Track = entryNode["track"]?.GetValue<int>() ?? 0,
                            Year = entryNode["year"]?.GetValue<int>() ?? 0,
                            Genre = entryNode["genre"]?.GetValue<string>() ?? string.Empty,
                            Size = entryNode["size"]?.GetValue<long>() ?? 0,
                            ContentType =
                                entryNode["contentType"]?.GetValue<string>() ?? string.Empty,
                            Path = entryNode["path"]?.GetValue<string>() ?? string.Empty,
                        };

                        playQueueResponse.PlayQueue.Entry.Add(entry);
                    }
                }
            }
            // Handle Internet Radio Stations response
            else if (
                response is Responses.Radio.InternetRadioStationsResponse radioStationsResponse
                && rootNode["internetRadioStations"] != null
            )
            {
                var stationsNode = rootNode?["internetRadioStations"];

                // Parse radio stations
                if (stationsNode?["internetRadioStation"] is JsonArray stationsArray)
                {
                    foreach (var stationNode in stationsArray)
                    {
                        if (stationNode == null)
                        {
                            continue;
                        }

                        var station = new Responses.Radio.Models.InternetRadioStation
                        {
                            Id = stationNode["id"]?.GetValue<string>() ?? string.Empty,
                            Name = stationNode["name"]?.GetValue<string>() ?? string.Empty,
                            StreamUrl =
                                stationNode["streamUrl"]?.GetValue<string>() ?? string.Empty,
                            HomepageUrl = stationNode["homepageUrl"]?.GetValue<string>(),
                        };

                        radioStationsResponse.InternetRadioStations.InternetRadioStation.Add(
                            station
                        );
                    }
                }
            }
            // Handle Music Directory response
            else if (
                response is Responses.Browsing.MusicDirectoryResponse directoryResponse
                && rootNode["directory"] != null
            )
            {
                var directoryNode = rootNode["directory"];

                // Parse directory fields
                directoryResponse.Directory.Id =
                    directoryNode?["id"]?.GetValue<string>() ?? string.Empty;
                directoryResponse.Directory.Name =
                    directoryNode?["name"]?.GetValue<string>() ?? string.Empty;
                directoryResponse.Directory.Parent = directoryNode?["parent"]?.GetValue<string>();
                directoryResponse.Directory.Starred =
                    directoryNode?["starred"]?.GetValue<bool>() ?? false;
                directoryResponse.Directory.UserRating = directoryNode?[
                    "userRating"
                ]?.GetValue<int>();
                directoryResponse.Directory.AverageRating = directoryNode?[
                    "averageRating"
                ]?.GetValue<double>();
                directoryResponse.Directory.PlayCount = directoryNode?[
                    "playCount"
                ]?.GetValue<int>();

                // Parse child elements
                if (directoryNode?["child"] is JsonArray childrenArray)
                {
                    foreach (var childNode in childrenArray)
                    {
                        if (childNode == null)
                        {
                            continue;
                        }

                        var child = new Responses.Browsing.Models.Child
                        {
                            Id = childNode["id"]?.GetValue<string>() ?? string.Empty,
                            Parent = childNode["parent"]?.GetValue<string>(),
                            IsDir = childNode["isDir"]?.GetValue<bool>() ?? false,
                            Title = childNode["title"]?.GetValue<string>() ?? string.Empty,
                            Album = childNode["album"]?.GetValue<string>() ?? string.Empty,
                            Artist = childNode["artist"]?.GetValue<string>() ?? string.Empty,
                            AlbumArtist =
                                childNode["albumArtist"]?.GetValue<string>() ?? string.Empty,
                            Track = childNode["track"]?.GetValue<int>(),
                            Year = childNode["year"]?.GetValue<int>(),
                            Genre = childNode["genre"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = childNode["coverArt"]?.GetValue<string>() ?? string.Empty,
                            Size = childNode["size"]?.GetValue<long>(),
                            ContentType =
                                childNode["contentType"]?.GetValue<string>() ?? string.Empty,
                            Suffix = childNode["suffix"]?.GetValue<string>() ?? string.Empty,
                            TranscodedContentType =
                                childNode["transcodedContentType"]?.GetValue<string>()
                                ?? string.Empty,
                            TranscodedSuffix =
                                childNode["transcodedSuffix"]?.GetValue<string>() ?? string.Empty,
                            Duration = childNode["duration"]?.GetValue<int>(),
                            BitRate = childNode["bitRate"]?.GetValue<int>(),
                            Path = childNode["path"]?.GetValue<string>() ?? string.Empty,
                            IsVideo = childNode["isVideo"]?.GetValue<bool>(),
                            PlayCount = childNode["playCount"]?.GetValue<int>(),
                            Created = childNode["created"]?.GetValue<long>(),
                            Starred = childNode["starred"]?.GetValue<bool>() ?? false,
                            AlbumId = childNode["albumId"]?.GetValue<string>() ?? string.Empty,
                            ArtistId = childNode["artistId"]?.GetValue<string>() ?? string.Empty,
                            Type = childNode["type"]?.GetValue<string>() ?? string.Empty,
                            DiscNumber = childNode["discNumber"]?.GetValue<int>(),
                            UserRating = childNode["userRating"]?.GetValue<int>(),
                            AverageRating = childNode["averageRating"]?.GetValue<double>(),
                        };

                        directoryResponse.Directory.Children.Add(child);
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
                artist.Id = artistNode?["id"]?.GetValue<string>() ?? string.Empty;
                artist.Name = artistNode?["name"]?.GetValue<string>() ?? string.Empty;
                artist.CoverArt = artistNode?["coverArt"]?.GetValue<string>();
                artist.AlbumCount = artistNode?["albumCount"]?.GetValue<int>() ?? 0;
                artist.ArtistImageUrl = artistNode?["artistImageUrl"]?.GetValue<string>();

                // Parse albums
                if (artistNode?["album"] is JsonArray albumsArray)
                {
                    foreach (var albumNode in albumsArray)
                    {
                        if (albumNode == null)
                        {
                            continue;
                        }

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

                        var album = new Responses.Browsing.Models.AlbumSummary
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
                            Genre = albumNode["genre"]?.GetValue<string>(),
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
                if (randomSongsNode?["song"] is JsonArray songsArray)
                {
                    foreach (var songNode in songsArray)
                    {
                        if (songNode == null)
                        {
                            continue;
                        }

                        var song = new Responses.Search.Models.Song
                        {
                            Id = songNode["id"]?.GetValue<string>() ?? string.Empty,
                            Title = songNode["title"]?.GetValue<string>() ?? string.Empty,
                            Album = songNode["album"]?.GetValue<string>() ?? string.Empty,
                            Artist = songNode["artist"]?.GetValue<string>() ?? string.Empty,
                            AlbumId = songNode["albumId"]?.GetValue<string>() ?? string.Empty,
                            ArtistId = songNode["artistId"]?.GetValue<string>() ?? string.Empty,
                            Track = songNode["track"]?.GetValue<int>() ?? 0,
                            Year = songNode["year"]?.GetValue<int>() ?? 0,
                            Genre = songNode["genre"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = songNode["coverArt"]?.GetValue<string>() ?? string.Empty,
                            Size = songNode["size"]?.GetValue<long>() ?? 0,
                            ContentType =
                                songNode["contentType"]?.GetValue<string>() ?? string.Empty,
                            Duration = songNode["duration"]?.GetValue<int>() ?? 0,
                            Path = songNode["path"]?.GetValue<string>() ?? string.Empty,
                        };

                        randomSongsResponse.RandomSongs.Song.Add(song);
                    }
                }
            }
            // Handle System.LicenseResponse
            else if (
                response is Responses.System.LicenseResponse licenseResponse
                && rootNode["license"] != null
            )
            {
                var licenseNode = rootNode["license"];

                licenseResponse.License.Valid = licenseNode?["valid"]?.GetValue<bool>() ?? false;
                licenseResponse.License.Email =
                    licenseNode?["email"]?.GetValue<string>() ?? string.Empty;
                licenseResponse.License.Key =
                    licenseNode?["key"]?.GetValue<string>() ?? string.Empty;
                licenseResponse.License.LicenseVersion =
                    licenseNode?["licenseVersion"]?.GetValue<string>() ?? string.Empty;
                licenseResponse.License.Trial =
                    licenseNode?["trial"]?.GetValue<string>() ?? string.Empty;

                // Parse dates if present
                if (licenseNode?["expires"] != null)
                {
                    DateTime.TryParse(
                        licenseNode["expires"]?.GetValue<string>(),
                        out var expires
                    );
                    licenseResponse.License.Expires = expires;
                }

                if (licenseNode?["licenseExpires"] != null)
                {
                    DateTime.TryParse(
                        licenseNode["licenseExpires"]?.GetValue<string>(),
                        out var licenseExpires
                    );
                    licenseResponse.License.LicenseExpires = licenseExpires;
                }

                if (licenseNode?["trialExpires"] != null)
                {
                    DateTime.TryParse(
                        licenseNode["trialExpires"]?.GetValue<string>(),
                        out var trialExpires
                    );
                    licenseResponse.License.TrialExpires = trialExpires;
                }
            }
            // Handle System.ScanStatusResponse
            else if (
                response is Responses.System.ScanStatusResponse scanStatusResponse
                && rootNode["scanStatus"] != null
            )
            {
                var scanStatusNode = rootNode["scanStatus"];

                scanStatusResponse.ScanStatus.Scanning =
                    scanStatusNode?["scanning"]?.GetValue<bool>() ?? false;
                scanStatusResponse.ScanStatus.Count =
                    scanStatusNode?["count"]?.GetValue<int>() ?? 0;
                scanStatusResponse.ScanStatus.FolderCount =
                    scanStatusNode?["folderCount"]?.GetValue<int>() ?? 0;
                scanStatusResponse.ScanStatus.Folder =
                    scanStatusNode?["folder"]?.GetValue<string>() ?? string.Empty;
            }
            // Handle UserResponse - for a single user
            else if (
                response is Responses.User.UserResponse userResponse
                && rootNode["user"] != null
            )
            {
                var userNode = rootNode["user"];

                userResponse.User.Username =
                    userNode?["username"]?.GetValue<string>() ?? string.Empty;
                userResponse.User.Email = userNode?["email"]?.GetValue<string>() ?? string.Empty;

                // Parse scrobblingEnabled
                {
                    var scrobbleNode = userNode["scrobblingEnabled"];
                    if (scrobbleNode is JsonValue jval)
                    {
                        var element = jval.GetValue<JsonElement>();
                        if (element.ValueKind == JsonValueKind.True || element.ValueKind == JsonValueKind.False)
                        {
                            userResponse.User.ScrobblingEnabled = element.GetBoolean();
                        }
                        else if (element.ValueKind == JsonValueKind.String)
                        {
                            var s = element.GetString() ?? string.Empty;
                            if (!bool.TryParse(s, out var boolVal))
                            {
                                boolVal = false;
                            }

                            userResponse.User.ScrobblingEnabled = boolVal;
                        }
                    }
                }

                userResponse.User.AvatarScheme =
                    userNode?["avatarScheme"]?.GetValue<string>() ?? string.Empty;

                // Parse boolean roles
                userResponse.User.IsAdmin = userNode?["adminRole"]?.GetValue<bool>() ?? false;
                userResponse.User.SettingsRole =
                    userNode?["settingsRole"]?.GetValue<bool>() ?? false;
                userResponse.User.DownloadRole =
                    userNode?["downloadRole"]?.GetValue<bool>() ?? false;
                userResponse.User.UploadRole = userNode?["uploadRole"]?.GetValue<bool>() ?? false;
                userResponse.User.PlaylistRole =
                    userNode?["playlistRole"]?.GetValue<bool>() ?? false;
                userResponse.User.CoverArtRole =
                    userNode?["coverArtRole"]?.GetValue<bool>() ?? false;
                userResponse.User.CommentRole = userNode?["commentRole"]?.GetValue<bool>() ?? false;
                userResponse.User.PodcastRole = userNode?["podcastRole"]?.GetValue<bool>() ?? false;
                userResponse.User.StreamRole = userNode?["streamRole"]?.GetValue<bool>() ?? false;
                userResponse.User.JukeboxRole = userNode?["jukeboxRole"]?.GetValue<bool>() ?? false;
                userResponse.User.ShareRole = userNode?["shareRole"]?.GetValue<bool>() ?? false;
                userResponse.User.VideoConversionRole =
                    userNode?["videoConversionRole"]?.GetValue<bool>() ?? false;

                // Parse nullable values
                if (userNode?["ldapAuthenticated"] != null)
                {
                    userResponse.User.LdapAuthenticated = userNode["ldapAuthenticated"]
                        ?.GetValue<bool>();
                }

                if (userNode?["maxBitRate"] != null)
                {
                    userResponse.User.MaxBitRate = userNode["maxBitRate"]?.GetValue<int>();
                }

                // Parse folder IDs if present
                if (userNode?["folder"] is JsonArray folderArray)
                {
                    var folderIds = new List<string>();
                    foreach (var folderId in folderArray)
                    {
                        if (folderId != null)
                        {
                            try
                            {
                                // Try to get it as a number first (most likely)
                                folderIds.Add(folderId.GetValue<int>().ToString());
                            }
                            catch
                            {
                                try
                                {
                                    // Fall back to string if necessary
                                    folderIds.Add(folderId.GetValue<string>());
                                }
                                catch
                                {
                                    // Ignore if we can't parse it
                                }
                            }
                        }
                    }
                    userResponse.User.FolderIds = folderIds.ToArray();
                }
            }
            // Handle UsersResponse - for multiple users
            else if (
                response is Responses.User.UsersResponse usersResponse
                && rootNode["users"] != null
            )
            {
                var usersNode = rootNode["users"];

                if (usersNode?["user"] is JsonArray usersArray)
                {
                    foreach (var userNode in usersArray)
                    {
                        if (userNode == null)
                        {
                            continue;
                        }

                        var user = new Responses.User.Models.User
                        {
                            Username = userNode["username"]?.GetValue<string>() ?? string.Empty,
                            Email = userNode["email"]?.GetValue<string>() ?? string.Empty,

                            // Parse scrobblingEnabled
                            ScrobblingEnabled = false,
                        };

                        // Parse scrobblingEnabled
                        {
                            var scrobbleNode = userNode["scrobblingEnabled"];
                            if (scrobbleNode is JsonValue jval)
                            {
                                var element = jval.GetValue<JsonElement>();
                                if (element.ValueKind == JsonValueKind.True || element.ValueKind == JsonValueKind.False)
                                {
                                    user.ScrobblingEnabled = element.GetBoolean();
                                }
                                else if (element.ValueKind == JsonValueKind.String)
                                {
                                    var s = element.GetString() ?? string.Empty;
                                    if (!bool.TryParse(s, out var boolVal))
                                    {
                                        boolVal = false;
                                    }

                                    user.ScrobblingEnabled = boolVal;
                                }
                            }
                        }

                        // Parse boolean roles
                        user.IsAdmin = userNode["adminRole"]?.GetValue<bool>() ?? false;
                        user.SettingsRole = userNode["settingsRole"]?.GetValue<bool>() ?? false;
                        user.DownloadRole = userNode["downloadRole"]?.GetValue<bool>() ?? false;
                        user.UploadRole = userNode["uploadRole"]?.GetValue<bool>() ?? false;
                        user.PlaylistRole = userNode["playlistRole"]?.GetValue<bool>() ?? false;
                        user.CoverArtRole = userNode["coverArtRole"]?.GetValue<bool>() ?? false;
                        user.CommentRole = userNode["commentRole"]?.GetValue<bool>() ?? false;
                        user.PodcastRole = userNode["podcastRole"]?.GetValue<bool>() ?? false;
                        user.StreamRole = userNode["streamRole"]?.GetValue<bool>() ?? false;
                        user.JukeboxRole = userNode["jukeboxRole"]?.GetValue<bool>() ?? false;
                        user.ShareRole = userNode["shareRole"]?.GetValue<bool>() ?? false;
                        user.VideoConversionRole =
                            userNode["videoConversionRole"]?.GetValue<bool>() ?? false;

                        // Parse nullable values
                        if (userNode["ldapAuthenticated"] != null)
                        {
                            user.LdapAuthenticated = userNode["ldapAuthenticated"]
                                ?.GetValue<bool>();
                        }

                        if (userNode["maxBitRate"] != null)
                        {
                            user.MaxBitRate = userNode["maxBitRate"]?.GetValue<int>();
                        }

                        // Parse folder IDs if present
                        if (userNode["folder"] is JsonArray folderArray)
                        {
                            var folderIds = new List<string>();
                            foreach (var folderId in folderArray)
                            {
                                if (folderId != null)
                                {
                                    try
                                    {
                                        // Try to get it as a number first (most likely)
                                        folderIds.Add(folderId.GetValue<int>().ToString());
                                    }
                                    catch
                                    {
                                        try
                                        {
                                            // Fall back to string if necessary
                                            folderIds.Add(folderId.GetValue<string>());
                                        }
                                        catch
                                        {
                                            // Ignore if we can't parse it
                                        }
                                    }
                                }
                            }
                            user.FolderIds = folderIds.ToArray();
                        }

                        usersResponse.Users.User.Add(user);
                    }
                }
            }
            // Handle Genres response
            else if (
                response is Responses.Browsing.GenresResponse genresResponse
                && rootNode["genres"] != null
            )
            {
                var genresNode = rootNode["genres"];

                // Parse genres array
                if (genresNode?["genre"] is JsonArray genresArray)
                {
                    foreach (var genreNode in genresArray)
                    {
                        if (genreNode == null)
                        {
                            continue;
                        }

                        var genre = new Responses.Browsing.Models.Genre
                        {
                            Name = genreNode["value"]?.GetValue<string>() ?? string.Empty,
                            SongCount = genreNode["songCount"]?.GetValue<int>() ?? 0,
                            AlbumCount = genreNode["albumCount"]?.GetValue<int>() ?? 0,
                        };

                        genresResponse.Genres.Genre.Add(genre);
                    }
                }
            }
            // Handle Starred response
            else if (
                response is Responses.Browsing.StarredResponse starredResponse
                && rootNode["starred"] != null
            )
            {
                var starredNode = rootNode["starred"];

                // Parse starred artists - create Artist objects (from Browsing.Models) as expected by Starred.Artist collection
                if (starredNode?["artist"] is JsonArray artistsArray)
                {
                    foreach (var artistNode in artistsArray)
                    {
                        if (artistNode == null)
                        {
                            continue;
                        }

                        var artist = new Artist
                        {
                            Id = artistNode["id"]?.GetValue<string>() ?? string.Empty,
                            Name = artistNode["name"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = artistNode["coverArt"]?.GetValue<string>(),
                            AlbumCount = artistNode["albumCount"]?.GetValue<int>() ?? 0,
                            ArtistImageUrl = artistNode["artistImageUrl"]?.GetValue<string>(),
                        };

                        starredResponse.Starred.Artist.Add(artist);
                    }
                }

                // Parse starred albums - create Album objects (from Search.Models) as expected by Starred.Album collection
                if (starredNode?["album"] is JsonArray albumsArray)
                {
                    foreach (var albumNode in albumsArray)
                    {
                        if (albumNode == null)
                        {
                            continue;
                        }

                        var album = new Responses.Search.Models.Album
                        {
                            Id = albumNode["id"]?.GetValue<string>() ?? string.Empty,
                            Name = albumNode["name"]?.GetValue<string>() ?? string.Empty,
                            Artist = albumNode["artist"]?.GetValue<string>() ?? string.Empty,
                            ArtistId = albumNode["artistId"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = albumNode["coverArt"]?.GetValue<string>() ?? string.Empty,
                            SongCount = albumNode["songCount"]?.GetValue<int>() ?? 0,
                            Duration = albumNode["duration"]?.GetValue<int>() ?? 0,
                            Year = albumNode["year"]?.GetValue<int>() ?? 0,
                            Genre = albumNode["genre"]?.GetValue<string>(),
                        };

                        // Parse other fields if present
                        if (albumNode["created"] != null)
                        {
                            try
                            {
                                album.Created = albumNode["created"]?.GetValue<long>();
                            }
                            catch
                            {
                                // If parsing fails, leave it as null
                            }
                        }

                        if (albumNode["playCount"] != null)
                        {
                            try
                            {
                                album.PlayCount = albumNode["playCount"]?.GetValue<int>();
                            }
                            catch
                            {
                                // If parsing fails, leave it as null
                            }
                        }

                        // Handle the starred attribute - it's a string timestamp in StarredAlbum but a boolean in Album
                        if (albumNode["starred"] != null)
                        {
                            album.Starred = true;
                        }

                        starredResponse.Starred.Album.Add(album);
                    }
                }

                // Parse starred songs - create Song objects (from Search.Models) as expected by Starred.Song collection
                if (starredNode?["song"] is JsonArray songsArray)
                {
                    foreach (var songNode in songsArray)
                    {
                        if (songNode == null)
                        {
                            continue;
                        }

                        var song = new Responses.Search.Models.Song
                        {
                            Id = songNode["id"]?.GetValue<string>() ?? string.Empty,
                            Title = songNode["title"]?.GetValue<string>() ?? string.Empty,
                            Album = songNode["album"]?.GetValue<string>() ?? string.Empty,
                            Artist = songNode["artist"]?.GetValue<string>() ?? string.Empty,
                            Duration = songNode["duration"]?.GetValue<int>() ?? 0,
                            Genre = songNode["genre"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = songNode["coverArt"]?.GetValue<string>() ?? string.Empty,
                            Size = songNode["size"]?.GetValue<long>() ?? 0,
                            ContentType =
                                songNode["contentType"]?.GetValue<string>() ?? string.Empty,
                            Path = songNode["path"]?.GetValue<string>() ?? string.Empty,
                            AlbumId = songNode["albumId"]?.GetValue<string>() ?? string.Empty,
                            ArtistId = songNode["artistId"]?.GetValue<string>() ?? string.Empty,
                            Track = songNode["track"]?.GetValue<int>(),
                            Year = songNode["year"]?.GetValue<int>(),
                            BitRate = songNode["bitRate"]?.GetValue<int>() ?? 0,
                        };

                        // Handle play count if present
                        if (songNode["playCount"] != null)
                        {
                            try
                            {
                                song.PlayCount = songNode["playCount"]?.GetValue<int>();
                            }
                            catch
                            {
                                // If parsing fails, leave it as null
                            }
                        }

                        // Handle created if present
                        if (songNode["created"] != null)
                        {
                            try
                            {
                                song.Created = songNode["created"]?.GetValue<long>();
                            }
                            catch
                            {
                                // If parsing fails, leave it as null
                            }
                        }

                        // Handle starred attribute - it's a string timestamp in StarredSong but a boolean in Song
                        if (songNode["starred"] != null)
                        {
                            song.Starred = true;
                        }

                        starredResponse.Starred.Song.Add(song);
                    }
                }
            }
            // Handle SongsByGenre response
            else if (
                response is Responses.Browsing.SongsByGenreResponse songsByGenreResponse
                && rootNode["songsByGenre"] != null
            )
            {
                var songsNode = rootNode["songsByGenre"];

                // Parse songs
                if (songsNode?["song"] is JsonArray songsArray)
                {
                    foreach (var songNode in songsArray)
                    {
                        if (songNode == null)
                        {
                            continue;
                        }

                        var song = new Responses.Browsing.Models.Child
                        {
                            Id = songNode["id"]?.GetValue<string>() ?? string.Empty,
                            Title = songNode["title"]?.GetValue<string>() ?? string.Empty,
                            Album = songNode["album"]?.GetValue<string>() ?? string.Empty,
                            Artist = songNode["artist"]?.GetValue<string>() ?? string.Empty,
                            Duration = songNode["duration"]?.GetValue<int>(),
                            Genre = songNode["genre"]?.GetValue<string>() ?? string.Empty,
                        };

                        songsByGenreResponse.SongsByGenre.Song.Add(song);
                    }
                }
            }
            // Handle Album List responses
            else if (
                response is Responses.Browsing.AlbumListResponse albumListResponse
                && rootNode["albumList"] != null
            )
            {
                var albumListNode = rootNode["albumList"];

                // Parse albums - using Album objects (from Search.Models) as expected by AlbumList.Album collection
                if (albumListNode?["album"] is JsonArray albumsArray)
                {
                    foreach (var albumNode in albumsArray)
                    {
                        if (albumNode == null)
                        {
                            continue;
                        }

                        var album = new Responses.Search.Models.Album
                        {
                            Id = albumNode["id"]?.GetValue<string>() ?? string.Empty,
                            Name = albumNode["name"]?.GetValue<string>() ?? string.Empty,
                            Artist = albumNode["artist"]?.GetValue<string>() ?? string.Empty,
                            ArtistId = albumNode["artistId"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = albumNode["coverArt"]?.GetValue<string>() ?? string.Empty,
                            SongCount = albumNode["songCount"]?.GetValue<int>() ?? 0,
                            Duration = albumNode["duration"]?.GetValue<int>() ?? 0,
                            Year = albumNode["year"]?.GetValue<int>() ?? 0,
                            Genre = albumNode["genre"]?.GetValue<string>(),
                        };

                        // Parse other fields if present
                        if (albumNode["created"] != null)
                        {
                            try
                            {
                                album.Created = albumNode["created"]?.GetValue<long>();
                            }
                            catch
                            {
                                // If parsing fails, leave it as null
                            }
                        }

                        if (albumNode["playCount"] != null)
                        {
                            try
                            {
                                album.PlayCount = albumNode["playCount"]?.GetValue<int>();
                            }
                            catch
                            {
                                // If parsing fails, leave it as null
                            }
                        }

                        // Handle starred attribute if present
                        if (albumNode["starred"] != null)
                        {
                            album.Starred = true;
                        }

                        albumListResponse.AlbumList.Album.Add(album);
                    }
                }
            }
            // Handle Album List 2 responses
            else if (
                response is Responses.Browsing.AlbumList2Response albumList2Response
                && rootNode["albumList2"] != null
            )
            {
                var albumListNode = rootNode["albumList2"];

                // Parse albums
                if (albumListNode?["album"] is JsonArray albumsArray)
                {
                    foreach (var albumNode in albumsArray)
                    {
                        if (albumNode == null)
                        {
                            continue;
                        }

                        var album = new Responses.Search.Models.Album
                        {
                            Id = albumNode["id"]?.GetValue<string>() ?? string.Empty,
                            Name = albumNode["name"]?.GetValue<string>() ?? string.Empty,
                            Artist = albumNode["artist"]?.GetValue<string>() ?? string.Empty,
                            ArtistId = albumNode["artistId"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = albumNode["coverArt"]?.GetValue<string>() ?? string.Empty,
                            SongCount = albumNode["songCount"]?.GetValue<int>() ?? 0,
                            Duration = albumNode["duration"]?.GetValue<int>() ?? 0,
                            Year = albumNode["year"]?.GetValue<int>() ?? 0,
                            Genre = albumNode["genre"]?.GetValue<string>() ?? string.Empty,
                        };

                        albumList2Response.AlbumList2.Album.Add(album);
                    }
                }
            }
            // Handle Song response
            else if (
                response is Responses.Browsing.SongResponse songResponse
                && rootNode["song"] != null
            )
            {
                var songNode = rootNode["song"];

                songResponse.Song.Id = songNode?["id"]?.GetValue<string>() ?? string.Empty;
                songResponse.Song.Title = songNode?["title"]?.GetValue<string>() ?? string.Empty;
                songResponse.Song.Album = songNode?["album"]?.GetValue<string>() ?? string.Empty;
                songResponse.Song.Artist = songNode?["artist"]?.GetValue<string>() ?? string.Empty;
                songResponse.Song.Track = songNode?["track"]?.GetValue<int>() ?? 0;
                songResponse.Song.Year = songNode?["year"]?.GetValue<int>() ?? 0;
                songResponse.Song.Genre = songNode?["genre"]?.GetValue<string>() ?? string.Empty;
                songResponse.Song.CoverArt =
                    songNode?["coverArt"]?.GetValue<string>() ?? string.Empty;
                songResponse.Song.Size = songNode?["size"]?.GetValue<long>() ?? 0;
                songResponse.Song.ContentType =
                    songNode?["contentType"]?.GetValue<string>() ?? string.Empty;
                songResponse.Song.Suffix = songNode?["suffix"]?.GetValue<string>() ?? string.Empty;
                songResponse.Song.Duration = songNode?["duration"]?.GetValue<int>() ?? 0;
                songResponse.Song.BitRate = songNode?["bitRate"]?.GetValue<int>() ?? 0;
                songResponse.Song.Path = songNode?["path"]?.GetValue<string>() ?? string.Empty;
                songResponse.Song.PlayCount = songNode?["playCount"]?.GetValue<int>() ?? 0;
                songResponse.Song.AlbumId =
                    songNode?["albumId"]?.GetValue<string>() ?? string.Empty;
                songResponse.Song.ArtistId =
                    songNode?["artistId"]?.GetValue<string>() ?? string.Empty;
                songResponse.Song.Type = songNode?["type"]?.GetValue<string>() ?? string.Empty;
            }
            // Handle Album response
            else if (
                response is Responses.Browsing.AlbumResponse albumResponse
                && rootNode["album"] != null
            )
            {
                var albumNode = rootNode["album"];

                albumResponse.Album.Id = albumNode?["id"]?.GetValue<string>() ?? string.Empty;
                albumResponse.Album.Name = albumNode?["name"]?.GetValue<string>() ?? string.Empty;
                albumResponse.Album.Artist =
                    albumNode?["artist"]?.GetValue<string>() ?? string.Empty;
                albumResponse.Album.ArtistId =
                    albumNode?["artistId"]?.GetValue<string>() ?? string.Empty;
                albumResponse.Album.CoverArt =
                    albumNode?["coverArt"]?.GetValue<string>() ?? string.Empty;
                albumResponse.Album.SongCount = albumNode?["songCount"]?.GetValue<int>() ?? 0;
                albumResponse.Album.Duration = albumNode?["duration"]?.GetValue<int>() ?? 0;
                albumResponse.Album.Year = albumNode?["year"]?.GetValue<int>() ?? 0;
                albumResponse.Album.Genre = albumNode?["genre"]?.GetValue<string>() ?? string.Empty;

                // Parse songs
                if (albumNode?["song"] is JsonArray songsArray)
                {
                    foreach (var songNode in songsArray)
                    {
                        if (songNode == null)
                        {
                            continue;
                        }

                        var song = new Responses.Search.Models.Song
                        {
                            Id = songNode["id"]?.GetValue<string>() ?? string.Empty,
                            Title = songNode["title"]?.GetValue<string>() ?? string.Empty,
                            Album = songNode["album"]?.GetValue<string>() ?? string.Empty,
                            Artist = songNode["artist"]?.GetValue<string>() ?? string.Empty,
                            Track = songNode["track"]?.GetValue<int>() ?? 0,
                            Year = songNode["year"]?.GetValue<int>() ?? 0,
                            Genre = songNode["genre"]?.GetValue<string>() ?? string.Empty,
                            CoverArt = songNode["coverArt"]?.GetValue<string>() ?? string.Empty,
                            Duration = songNode["duration"]?.GetValue<int>() ?? 0,
                            BitRate = songNode["bitRate"]?.GetValue<int>() ?? 0,
                            Size = songNode["size"]?.GetValue<long>() ?? 0,
                        };

                        albumResponse.Album.Song.Add(song);
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
            {
                return DateTime.MinValue;
            }

            try
            {
                var stringValue = jsonNode.GetValue<string>();
                if (DateTime.TryParse(stringValue, out var result))
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
            {
                return TimeSpan.Zero;
            }

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
        /// <returns>A list of integers or an empty list if parsing fails.</returns>
        private static List<int> ParseIntArrayValue(JsonNode jsonNode)
        {
            var result = new List<int>();

            if (jsonNode is JsonArray jsonArray)
            {
                foreach (var item in jsonArray)
                {
                    if (item == null)
                    {
                        continue;
                    }

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
