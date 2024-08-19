using Microsoft.AspNetCore.Mvc;
using Rattrapage_API.Models;
using NLog;
using AutoMapper;
using Rattrapage_API.Dto;
using Rattrapage_API.Exceptions; // Ajout de l'espace de noms pour les exceptions personnalisées
using Rattrapage_API.Interface;

namespace Rattrapage_API.Controllers
{
    /// <summary>
    /// Contrôleur pour gérer les opérations sur les artistes.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ArtistesController : ControllerBase
    {
        private readonly IArtistesService _artisteService;
        private readonly NLog.ILogger _logger;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initialise une nouvelle instance du contrôleur ArtistesController.
        /// </summary>
        /// <param name="artisteService">Service pour la gestion des artistes.</param>
        /// <param name="mapper">Mapper pour la conversion entre DTO et modèle.</param>
        public ArtistesController(IArtistesService artisteService, IMapper mapper)
        {
            _artisteService = artisteService;
            _logger = LogManager.GetCurrentClassLogger();
            _mapper = mapper;
        }

        /// <summary>
        /// Récupère tous les artistes.
        /// </summary>
        /// <returns>Une réponse HTTP contenant la liste de tous les artistes.</returns>
        /// <exception cref="ArtistesListNotFoundException">Lance une exception si la liste des artistes ne peut pas être récupérée.</exception>
        [HttpGet]
        public IActionResult GetAllArtistes()
        {
            try
            {
                var artistes = _artisteService.GetAllArtistes();
                _logger.Info("Récupération de tous les artistes");
                return Ok(artistes);
            }
            catch
            {
                _logger.Error("Erreur lors de la récupération de tous les artistes.");
                throw new ArtistesListNotFoundException();
            }
        }

        /// <summary>
        /// Récupère un artiste par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'artiste à récupérer.</param>
        /// <returns>Une réponse HTTP contenant l'artiste avec l'identifiant spécifié.</returns>
        /// <exception cref="ArtisteNotFoundException">Lance une exception si l'artiste avec l'identifiant spécifié n'est pas trouvé.</exception>
        [HttpGet("{id}")]
        public IActionResult GetArtisteById(string id)
        {
            var artiste = _artisteService.GetArtisteById(id);
            if (artiste == null)
            {
                _logger.Warn("Artiste avec ID {Id} non trouvé", id);
                throw new ArtisteNotFoundException(id);
            }
            _logger.Info("Artiste avec ID {Id} trouvé.", id);
            return Ok(artiste);
        }

        /// <summary>
        /// Ajoute un nouvel artiste.
        /// </summary>
        /// <param name="newArtisteDto">DTO contenant les informations de l'artiste à ajouter.</param>
        /// <returns>Une réponse HTTP avec un statut 201 Created si l'ajout est réussi.</returns>
        [HttpPost]
        public IActionResult AddArtiste([FromBody] ArtisteDto newArtisteDto)
        {
            var artistes = _artisteService.GetAllArtistes();

            // Générer un nouvel ID basé sur le nombre actuel d'artistes
            string newId = (artistes.Count() + 1).ToString();

            // Utiliser AutoMapper pour convertir le DTO en modèle, en ignorant l'ID
            var newArtiste = _mapper.Map<Artiste>(newArtisteDto);
            newArtiste.Id = newId; // Assigner l'ID généré

            _artisteService.AddArtiste(newArtiste);

            _logger.Info("Ajout d'un nouvel artiste : {Nom}", newArtiste.Nom);
            return CreatedAtAction(nameof(GetArtisteById), new { id = newArtiste.Id }, newArtiste);
        }

        /// <summary>
        /// Met à jour les informations d'un artiste existant.
        /// </summary>
        /// <param name="id">Identifiant de l'artiste à mettre à jour.</param>
        /// <param name="updatedArtisteDto">DTO contenant les nouvelles informations de l'artiste.</param>
        /// <returns>Une réponse HTTP avec un statut 204 No Content si la mise à jour est réussie.</returns>
        /// <exception cref="ArtisteNotFoundException">Lance une exception si l'artiste avec l'identifiant spécifié n'est pas trouvé pour la mise à jour.</exception>
        [HttpPut("{id}")]
        public IActionResult UpdateArtiste(string id, [FromBody] ArtisteDto updatedArtisteDto)
        {
            try
            {
                var updatedArtiste = _mapper.Map<Artiste>(updatedArtisteDto);
                _artisteService.UpdateArtiste(id, updatedArtiste);
                _logger.Info("Mise à jour de l'artiste avec ID {Id}", id);
                return NoContent();
            }
            catch
            {
                _logger.Warn("Artiste avec ID {Id} non trouvé pour la mise à jour", id);
                throw new ArtisteNotFoundException(id);
            }
        }

        /// <summary>
        /// Supprime un artiste par son identifiant.
        /// </summary>
        /// <param name="id">Identifiant de l'artiste à supprimer.</param>
        /// <returns>Une réponse HTTP avec un statut 204 No Content si la suppression est réussie.</returns>
        /// <exception cref="ArtisteNotFoundException">Lance une exception si l'artiste avec l'identifiant spécifié n'est pas trouvé pour la suppression.</exception>
        [HttpDelete("{id}")]
        public IActionResult DeleteArtiste(string id)
        {
            try
            {
                _artisteService.DeleteArtiste(id);
                _logger.Info("Suppression de l'artiste avec ID {Id}", id);
                return NoContent();
            }
            catch
            {
                _logger.Warn("Artiste avec ID {Id} non trouvé pour la suppression", id);
                throw new ArtisteNotFoundException(id);
            }
        }

        /// <summary>
        /// Recherche des artistes par leur nom.
        /// </summary>
        /// <param name="name">Nom ou partie du nom à rechercher dans les artistes.</param>
        /// <returns>Une réponse HTTP contenant la liste des artistes correspondant à la recherche.</returns>
        [HttpGet("search")]
        public IActionResult SearchArtistes(string name)
        {
            var artistes = _artisteService.SearchArtistes(name);
            _logger.Info("Recherche d'artistes avec le nom contenant : {Nom}", name);
            return Ok(artistes);
        }
    }
}
