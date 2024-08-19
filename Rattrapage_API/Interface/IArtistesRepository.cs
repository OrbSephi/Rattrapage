using Rattrapage_API.Models;

namespace Rattrapage_API.Interface
{
    public interface IArtistesRepository
    {
        List<Artiste> ReadArtistes();
        void WriteArtistes(List<Artiste> artistes);
    }
}
