using ArtworksClient_BrunoVidal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworksClient_BrunoVidal.Data
{
    public interface IArtworkRepository
    {
        Task<List<Artwork>> GetArtworks();
        Task<Artwork> GetArtwork(int id);
        Task<List<Artwork>> GetArtworksByArtType(int ArtTypeID);
        Task AddArtwork(Artwork artworkToAdd);
        Task UpdateArtwork(Artwork artworkToUpdate);
        Task DeleteArtwork(Artwork artworkToDelete);
    }
}
