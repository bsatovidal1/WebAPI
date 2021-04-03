using ArtworksClient_BrunoVidal.Models;
using ArtworksClient_BrunoVidal.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace ArtworksClient_BrunoVidal.Data
{
    class ApiArtTypeRepository : IArtTypeRepository
    {
        HttpClient client = new HttpClient();

        public ApiArtTypeRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<ArtType>> GetArtTypes()
        {
            var response = await client.GetAsync("/api/arttypes");
            if (response.IsSuccessStatusCode)
            {
                List<ArtType> arttypes = await response.Content.ReadAsAsync<List<ArtType>>();
                return arttypes;
            }
            else
            {
                throw new Exception("Could not access the list of ArtTypes.");
            }

        }

        public async Task<ArtType> GetArtType(int ArtTypeID)
        {
            var response = await client.GetAsync($"/api/arttypes/{ArtTypeID}");
            if (response.IsSuccessStatusCode)
            {
                ArtType arttype = await response.Content.ReadAsAsync<ArtType>();
                return arttype;
            }
            else
            {
                throw new Exception("Could not access that ArtType.");
            }
        }

        public async Task AddArtType(ArtType arttypeToAdd)
        {
            var response = await client.PostAsJsonAsync("/api/arttypes", arttypeToAdd);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task UpdateArtType(ArtType arttypeToUpdate)
        {
            var response = await client.PutAsJsonAsync($"/api/arttypes/{arttypeToUpdate.ID}", arttypeToUpdate);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task DeleteArtType(ArtType arttypeToDelete)
        {
            var response = await client.DeleteAsync($"/api/arttypes/{arttypeToDelete.ID}");
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }
    }
}
