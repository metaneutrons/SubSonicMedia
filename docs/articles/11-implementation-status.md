# API Implementation Status Matrix

This document provides an overview of all API function groups and their endpoints, along with their implementation status in the codebase. Each endpoint is marked as:

- ✅ Implemented
- ❌ Not Implemented

| Group                      | API Endpoint               | Status |
|----------------------------|----------------------------|--------|
| **System**                 | ping                       |   ✅   |
|                            | getLicense                 |   ✅   |
| **Browsing**               | getMusicFolders            |   ✅   |
|                            | getIndexes                 |   ✅   |
|                            | getMusicDirectory          |   ✅   |
|                            | getGenres                  |   ✅   |
|                            | getArtists                 |   ✅   |
|                            | getArtist                  |   ✅   |
|                            | getAlbum                   |   ✅   |
|                            | getSong                    |   ✅   |
|                            | getVideos                  |   ✅   |
|                            | getVideoInfo               |   ✅   |
|                            | getArtistInfo              |   ✅   |
|                            | getArtistInfo2             |   ✅   |
|                            | getAlbumInfo               |   ✅   |
|                            | getAlbumInfo2              |   ✅   |
|                            | getSimilarSongs            |   ❌   |
|                            | getSimilarSongs2           |   ❌   |
|                            | getTopSongs                |   ❌   |
| **Album/song lists**       | getAlbumList               |   ✅   |
|                            | getAlbumList2              |   ✅   |
|                            | getRandomSongs             |   ✅   |
|                            | getSongsByGenre            |   ✅   |
|                            | getNowPlaying              |   ❌   |
|                            | getStarred                 |   ✅   |
|                            | getStarred2                |   ✅   |
| **Searching**              | search                     |   ✅   |
|                            | search2                    |   ✅   |
|                            | search3                    |   ✅   |
| **Playlists**              | getPlaylists               |   ✅   |
|                            | getPlaylist                |   ✅   |
|                            | createPlaylist             |   ✅   |
|                            | updatePlaylist             |   ✅   |
|                            | deletePlaylist             |   ✅   |
| **Media retrieval**        | stream                     |   ✅   |
|                            | download                   |   ✅   |
|                            | hls                        |   ✅   |
|                            | getCaptions                |   ✅   |
|                            | getCoverArt                |   ✅   |
|                            | getLyrics                  |   ✅   |
|                            | getAvatar                  |   ✅   |
| **Media annotation**       | star                       |   ✅   |
|                            | unstar                     |   ✅   |
|                            | setRating                  |   ✅   |
|                            | scrobble                   |   ✅   |
| **Sharing**                | getShares                  |   ❌   |
|                            | createShare                |   ✅   |
|                            | updateShare                |   ❌   |
|                            | deleteShare                |   ❌   |
| **Podcast**                | getPodcasts                |   ✅   |
|                            | getNewestPodcasts          |   ✅   |
|                            | refreshPodcasts            |   ✅   |
|                            | createPodcastChannel       |   ✅   |
|                            | deletePodcastChannel       |   ✅   |
|                            | deletePodcastEpisode       |   ✅   |
|                            | downloadPodcastEpisode     |   ✅   |
| **Jukebox**                | jukeboxControl             |   ✅   |
| **Internet radio**         | getInternetRadioStations   |   ✅   |
|                            | createInternetRadioStation |   ✅   |
|                            | updateInternetRadioStation |   ✅   |
|                            | deleteInternetRadioStation |   ✅   |
| **Chat**                   | getChatMessages            |   ✅   |
|                            | addChatMessage             |   ✅   |
| **User management**        | getUser                    |   ✅   |
|                            | getUsers                   |   ✅   |
|                            | createUser                 |   ✅   |
|                            | updateUser                 |   ✅   |
|                            | deleteUser                 |   ✅   |
|                            | changePassword             |   ✅   |
| **Bookmarks**              | getBookmarks               |   ✅   |
|                            | createBookmark             |   ✅   |
|                            | deleteBookmark             |   ✅   |
|                            | getPlayQueue               |   ✅   |
|                            | savePlayQueue              |   ✅   |
| **Media library scanning** | getScanStatus              |   ✅   |
|                            | startScan                  |   ✅   |

---

**Legend:**

- ✅ = Implemented
- ❌ = Not Implemented

*Last updated: 2025-05-04*
