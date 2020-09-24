using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Xunit;

namespace BooksBackEndIntegrationTests
{
    public class GettingStatus:IClassFixture<WebTestFixture>
    {
        private HttpClient _client;

        public GettingStatus(WebTestFixture fixture)
        {
            _client = fixture.CreateClient();
        }


        [Fact]
        public async void WeGetASucessStatusCode()
        {
            var response = await _client.GetAsync("/status");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
        [Fact]
        public async void FormattedJson()
        {
            var response = await _client.GetAsync("/status");
            var mediaType = response.Content.Headers.ContentType.MediaType;
            Assert.Equal("application/json", mediaType);
        }
        [Fact]
        public async void HasCorrectBody()
        {
            var response = await _client.GetAsync("/status");

            var content = await response.Content.ReadAsAsync<StatusResponse>();
            Assert.Equal("Looks good, Boss!", content.message);
            Assert.Equal("Joe", content.checkedBy);
            Assert.Equal(new DateTime(1998, 02, 05, 23, 59, 00), content.lastChecked);
        }
    }
    public class StatusResponse
    {
        public string message { get; set; }
        public string checkedBy { get; set; }
        public DateTime lastChecked { get; set; }
    }
}
