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
    public class ApiArtworkRepository : IArtworkRepository
    {
        HttpClient client = new HttpClient();

        public ApiArtworkRepository()
        {
            client.BaseAddress = Jeeves.DBUri;
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        public async Task<List<Artwork>> GetArtworks()
        {
            var response = await client.GetAsync("/api/artworks");
            if (response.IsSuccessStatusCode)
            {
                List<Artwork> artworks = await response.Content.ReadAsAsync<List<Artwork>>();
                return artworks;
            }
            else
            {
                throw new Exception("Could not access the list of Artworks.");
            }
        }

        public async Task<List<Artwork>> GetArtworksByArtType(int ArtTypeID)
        {
            var response = await client.GetAsync($"/api/artworks/ByArtType/{ArtTypeID}");
            if (response.IsSuccessStatusCode)
            {
                List<Artwork> artworks = await response.Content.ReadAsAsync<List<Artwork>>();
                return artworks;
            }
            else
            {
                throw new Exception("Could not access the list of Artworks.");
            }
        }

        public async Task<Artwork> GetArtwork(int ID)
        {
            var response = await client.GetAsync($"/api/artworks/{ID}");
            if (response.IsSuccessStatusCode)
            {
                Artwork artwork = await response.Content.ReadAsAsync<Artwork>();
                return artwork;
            }
            else
            {
                throw new Exception("Could not access that Artwork.");
            }
        }

        public async Task AddArtwork(Artwork artworkToAdd)
        {
            var response = await client.PostAsJsonAsync("/api/Artworks", artworkToAdd);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task UpdateArtwork(Artwork artworkToUpdate)
        {
            var response = await client.PutAsJsonAsync($"/api/Artworks/{artworkToUpdate.ID}", artworkToUpdate);
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }

        public async Task DeleteArtwork(Artwork artworkToDelete)
        {
            var response = await client.DeleteAsync($"/api/artworks/{artworkToDelete.ID}");
            if (!response.IsSuccessStatusCode)
            {
                var ex = Jeeves.CreateApiException(response);
                throw ex;
            }
        }
    }
}
