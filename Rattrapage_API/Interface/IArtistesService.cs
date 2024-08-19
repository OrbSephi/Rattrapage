using Rattrapage_API.Models;

namespace Rattrapage_API.Interface
{
    public interface IArtistesService
    {
        List<Artiste> GetAllArtistes();
        Artiste GetArtisteById(string id);
        void AddArtiste(Artiste newArtiste);
        void UpdateArtiste(string id, Artiste updatedArtiste);
        void DeleteArtiste(string id);
        List<Artiste> SearchArtistes(string name);
    }
}
