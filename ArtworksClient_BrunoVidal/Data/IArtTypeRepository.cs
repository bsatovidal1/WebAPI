using ArtworksClient_BrunoVidal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtworksClient_BrunoVidal.Data
{
    public interface IArtTypeRepository
    {
        Task<List<ArtType>> GetArtTypes();
        Task<ArtType> GetArtType(int id);
        Task AddArtType(ArtType arttypeToAdd);
        Task UpdateArtType(ArtType arttypeToUpdate);
        Task DeleteArtType(ArtType arttypeToDelete);
    }
}
