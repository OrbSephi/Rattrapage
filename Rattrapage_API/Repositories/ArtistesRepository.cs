using Rattrapage_API.Exceptions;
using Rattrapage_API.Interface;
using Rattrapage_API.Models;
using System.Text.Json;
using NLog;

namespace Rattrapage_API.Repositories
{
    /// <summary>
    /// Implémentation de l'interface <see cref="IArtistesRepository"/> pour la gestion des artistes via des opérations de lecture et d'écriture dans un fichier JSON.
    /// </summary>
    public class ArtistesRepository : IArtistesRepository
    {
        private readonly string _filePath = "Data/artistes.json";
        private static readonly NLog.ILogger _logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Lit la liste des artistes depuis un fichier JSON.
        /// </summary>
        /// <returns>Une liste d'artistes lus depuis le fichier.</returns>
        /// <exception cref="FileNotFoundException">Lance une exception si le fichier des artistes n'existe pas à l'emplacement spécifié.</exception>
        /// <exception cref="FileReadException">Lance une exception si une erreur survient lors de la lecture du fichier.</exception>
        public List<Artiste> ReadArtistes()
        {
            _logger.Info("Début de la lecture des artistes à partir du fichier {FilePath}.", _filePath);

            if (!File.Exists(_filePath))
            {
                _logger.Error("Le fichier des artistes n'existe pas à l'emplacement {FilePath}.", _filePath);
                throw new Exceptions.FileNotFoundException(_filePath); // Lancer une exception si le fichier n'existe pas
            }

            try
            {
                var json = File.ReadAllText(_filePath);
                var artistes = JsonSerializer.Deserialize<List<Artiste>>(json) ?? new List<Artiste>();
                _logger.Info("Lecture réussie : {Count} artistes trouvés.", artistes.Count);
                return artistes;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erreur lors de la lecture du fichier des artistes à {FilePath}.", _filePath);
                throw new FileReadException(_filePath); // Lancer une exception si la lecture du fichier échoue
            }
        }

        /// <summary>
        /// Écrit une liste d'artistes dans un fichier JSON.
        /// </summary>
        /// <param name="artistes">La liste des artistes à écrire dans le fichier.</param>
        /// <exception cref="FileWriteException">Lance une exception si une erreur survient lors de l'écriture du fichier.</exception>
        public void WriteArtistes(List<Artiste> artistes)
        {
            _logger.Info("Début de l'écriture des artistes dans le fichier {FilePath}.", _filePath);

            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                var json = JsonSerializer.Serialize(artistes, options);
                File.WriteAllText(_filePath, json);
                _logger.Info("Écriture réussie : {Count} artistes écrits dans le fichier.", artistes.Count);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Erreur lors de l'écriture dans le fichier des artistes à {FilePath}.", _filePath);
                throw new FileWriteException(_filePath); // Lancer une exception si l'écriture du fichier échoue
            }
        }
    }
}
