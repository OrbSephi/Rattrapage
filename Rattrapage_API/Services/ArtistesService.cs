using Rattrapage_API.Exceptions;
using Rattrapage_API.Interface;
using Rattrapage_API.Models;
using NLog;

namespace Rattrapage_API.Services
{
    public class ArtistesService : IArtistesService
    {
        private readonly IArtistesRepository _repository;
        private readonly NLog.ILogger _logger;

        public ArtistesService(IArtistesRepository repository)
        {
            _repository = repository;
            _logger = LogManager.GetCurrentClassLogger();
        }

        /// <summary>
        /// Récupère tous les artistes.
        /// </summary>
        /// <returns>Une liste de tous les artistes.</returns>
        public List<Artiste> GetAllArtistes()
        {
            _logger.Info("Début de la récupération de tous les artistes.");
            var artistes = _repository.ReadArtistes();
            _logger.Info("Récupération de {Count} artistes réussie.", artistes.Count);
            return artistes;
        }

        /// <summary>
        /// Récupère un artiste par son ID.
        /// </summary>
        /// <param name="id">L'ID de l'artiste à récupérer.</param>
        /// <returns>L'artiste correspondant à l'ID.</returns>
        /// <exception cref="ArtisteNotFoundException">Lance une exception si l'artiste avec l'ID spécifié n'existe pas.</exception>
        public Artiste GetArtisteById(string id)
        {
            _logger.Info("Recherche de l'artiste avec ID {Id}.", id);
            var artistes = _repository.ReadArtistes();
            var artiste = artistes.FirstOrDefault(a => a.Id == id);

            if (artiste == null)
            {
                _logger.Warn("Artiste avec ID {Id} non trouvé.", id);
                throw new ArtisteNotFoundException(id);
            }

            _logger.Info("Artiste avec ID {Id} trouvé : {Nom}.", id, artiste.Nom);
            return artiste;
        }

        /// <summary>
        /// Ajoute un nouvel artiste.
        /// </summary>
        /// <param name="newArtiste">L'artiste à ajouter.</param>
        /// <exception cref="ArtisteAlreadyExistsException">Lance une exception si un artiste avec le même nom existe déjà.</exception>
        public void AddArtiste(Artiste newArtiste)
        {
            _logger.Info("Début de l'ajout de l'artiste {Nom}.", newArtiste.Nom);
            var artistes = _repository.ReadArtistes();

            if (artistes.Any(a => a.Nom == newArtiste.Nom))
            {
                _logger.Warn("L'artiste {Nom} existe déjà.", newArtiste.Nom);
                throw new ArtisteAlreadyExistsException(newArtiste.Nom);
            }

            artistes.Add(newArtiste);
            _repository.WriteArtistes(artistes);
            _logger.Info("Artiste {Nom} ajouté avec succès.", newArtiste.Nom);
        }

        /// <summary>
        /// Met à jour les informations d'un artiste existant.
        /// </summary>
        /// <param name="id">L'ID de l'artiste à mettre à jour.</param>
        /// <param name="updatedArtiste">L'artiste avec les nouvelles informations.</param>
        /// <exception cref="ArtisteNotFoundException">Lance une exception si l'artiste avec l'ID spécifié n'existe pas.</exception>
        public void UpdateArtiste(string id, Artiste updatedArtiste)
        {
            _logger.Info("Début de la mise à jour de l'artiste avec ID {Id}.", id);
            var artistes = _repository.ReadArtistes();
            var artiste = artistes.FirstOrDefault(a => a.Id == id);

            if (artiste == null)
            {
                _logger.Warn("Artiste avec ID {Id} non trouvé pour la mise à jour.", id);
                throw new ArtisteNotFoundException(id);
            }

            artiste.Nom = updatedArtiste.Nom;
            artiste.Genre = updatedArtiste.Genre;
            _repository.WriteArtistes(artistes);
            _logger.Info("Artiste avec ID {Id} mis à jour avec succès : Nouveau Nom = {Nom}, Nouveau Genre = {Genre}.", id, updatedArtiste.Nom, updatedArtiste.Genre);
        }

        /// <summary>
        /// Supprime un artiste par son ID.
        /// </summary>
        /// <param name="id">L'ID de l'artiste à supprimer.</param>
        /// <exception cref="ArtisteNotFoundException">Lance une exception si l'artiste avec l'ID spécifié n'existe pas.</exception>
        public void DeleteArtiste(string id)
        {
            _logger.Info("Début de la suppression de l'artiste avec ID {Id}.", id);
            var artistes = _repository.ReadArtistes();
            var artiste = artistes.FirstOrDefault(a => a.Id == id);

            if (artiste == null)
            {
                _logger.Warn("Artiste avec ID {Id} non trouvé pour la suppression.", id);
                throw new ArtisteNotFoundException(id);
            }

            artistes.Remove(artiste);
            _repository.WriteArtistes(artistes);
            _logger.Info("Artiste avec ID {Id} supprimé avec succès.", id);
        }

        /// <summary>
        /// Recherche des artistes dont le nom contient une chaîne de caractères spécifiée.
        /// </summary>
        /// <param name="name">Le nom ou la partie du nom à rechercher.</param>
        /// <returns>Une liste d'artistes dont le nom contient la chaîne spécifiée.</returns>
        public List<Artiste> SearchArtistes(string name)
        {
            _logger.Info("Début de la recherche d'artistes avec le nom contenant : {Nom}.", name);
            var artistes = _repository.ReadArtistes()
                            .Where(a => a.Nom.Contains(name, StringComparison.OrdinalIgnoreCase))
                            .ToList();
            _logger.Info("Recherche d'artistes avec le nom contenant : {Nom} terminée. {Count} artistes trouvés.", name, artistes.Count);
            return artistes;
        }
    }
}
