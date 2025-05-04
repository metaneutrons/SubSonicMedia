using System.Net;

using FluentAssertions;

using SubSonicMedia.Models;
using SubSonicMedia.Tests.Fixtures;

using Xunit;

namespace SubSonicMedia.Tests.Clients
{
    public class SubsonicClientAnnotationTests : IClassFixture<WireMockServerFixture>
    {
        private readonly WireMockServerFixture _fixture;
        private readonly SubsonicClient _client;

        public SubsonicClientAnnotationTests(WireMockServerFixture fixture)
        {
            _fixture = fixture;
            var http = new HttpClient { BaseAddress = new Uri(_fixture.BaseUrl) };
            _client = new SubsonicClient(
                new SubsonicConnectionInfo(_fixture.BaseUrl.TrimEnd('/'), "u", "p"),
                httpClient: http);
        }

        [Fact]
        public async Task StarAsync_WhenServerReturnsOk_IsSuccess()
        {
            const string json = "{\"subsonic-response\":{\"status\":\"ok\",\"version\":\"1.16.1\"}}";
            _fixture.SetupApiEndpoint("star", json, HttpStatusCode.OK);
            var resp = await _client.Annotation.StarAsync("123");
            resp.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task UnstarAsync_WhenServerReturnsOk_IsSuccess()
        {
            const string json = "{\"subsonic-response\":{\"status\":\"ok\",\"version\":\"1.16.1\"}}";
            _fixture.SetupApiEndpoint("unstar", json, HttpStatusCode.OK);
            var resp = await _client.Annotation.UnstarAsync("123");
            resp.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task SetRatingAsync_WhenServerReturnsOk_IsSuccess()
        {
            const string json = "{\"subsonic-response\":{\"status\":\"ok\",\"version\":\"1.16.1\"}}";
            _fixture.SetupApiEndpoint("setRating", json, HttpStatusCode.OK);
            var resp = await _client.Annotation.SetRatingAsync("123", 5);
            resp.IsSuccess.Should().BeTrue();
        }
    }
}
