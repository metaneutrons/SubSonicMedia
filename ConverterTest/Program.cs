using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using SubSonicMedia.Responses.Browsing;
using SubSonicMedia.Responses.Browsing.Models;
using SubSonicMedia.Serialization.Converters;

public class Program
{
    public static void Main()
    {
        // Test the exact same JSON structure as in the failing test
        var json = """
            {
                "subsonic-response": {
                    "status": "ok",
                    "version": "1.16.1",
                    "starred": {
                        "artist": [
                            {
                                "id": "ar-1",
                                "name": "ABBA"
                            }
                        ],
                        "album": [
                            {
                                "id": "al-1",
                                "name": "Gold",
                                "artist": "ABBA",
                                "artistId": "ar-1",
                                "coverArt": "al-1",
                                "songCount": 19,
                                "created": "2020-01-01T00:00:00.000Z",
                                "starred": "2021-01-01T00:00:00.000Z"
                            }
                        ],
                        "song": [
                            {
                                "id": "song-1",
                                "parent": "al-1",
                                "title": "Dancing Queen",
                                "album": "Gold",
                                "artist": "ABBA",
                                "duration": 231,
                                "starred": "2021-01-01T00:00:00.000Z"
                            }
                        ]
                    }
                }
            }
            """;

        try
        {
            // Use the same options as SimpleSubsonicSerializer (now with converters)
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Converters =
                {
                    new FlexibleDateTimeToLongConverter(),
                    new UnixTimestampToDateTimeConverter(),
                    new StringBooleanConverter(),
                    new SubsonicCollectionConverterFactory(),
                },
            };

            // Parse JSON to extract the subsonic-response wrapper (same as SimpleSubsonicSerializer)
            using var document = JsonDocument.Parse(json);
            var root = document.RootElement;

            if (!root.TryGetProperty("subsonic-response", out var responseElement))
            {
                throw new Exception("Invalid response format: subsonic-response element not found");
            }

            // Deserialize the response element to our strongly-typed model
            var response = JsonSerializer.Deserialize<StarredResponse>(
                responseElement.GetRawText(),
                options
            );

            Console.WriteLine($"Success! Response status: {response?.Status}");
            Console.WriteLine($"Album count: {response?.Starred?.Album?.Count}");
            if (response?.Starred?.Album?.Count > 0)
            {
                var album = response.Starred.Album[0];
                Console.WriteLine($"Album name: {album.Name}");
                Console.WriteLine($"Album created: {album.Created}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            Console.WriteLine($"Stack: {ex.StackTrace}");
        }
    }
}
