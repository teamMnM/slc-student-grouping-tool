using Newtonsoft.Json;
using SlcClient.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SlcClient.Services
{
    public class SectionService : ISlcService<Section>
    {
        private SlcHttpClient _client;

        public SectionService(string token)
        {
            _client = new SlcHttpClient(token);
        }

        public async Task<IEnumerable<Section>> GetAll()
        {
            var response = await _client.GetDataString(Constants.Section.ALL);
            var content = await response.Content.ReadAsStringAsync();
            var sections = JsonConvert.DeserializeObject<IEnumerable<Section>>(content);
            return sections;
        }

        public async Task<Section> GetById(string id)
        {
            var response = await _client.GetDataString(Constants.Section.ALL + id);
            var content = await response.Content.ReadAsStringAsync();
            var section = JsonConvert.DeserializeObject<Section>(content);
            return section;
        }

        public async Task<HttpResponseMessage> Create(Section obj)
        {
            throw new NotImplementedException("Creating section is not allowed");
        }

        public async Task<HttpStatusCode> Update(Section obj)
        {
            throw new NotImplementedException("Updating section is not allowed");
        }

        public async Task<HttpStatusCode> DeleteById(string id)
        {
            throw new NotImplementedException("Deleting section is not allowed");
        }
    }
}
