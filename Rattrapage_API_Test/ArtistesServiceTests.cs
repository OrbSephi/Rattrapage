using Moq;
using Rattrapage_API.Exceptions;
using Rattrapage_API.Interface;
using Rattrapage_API.Models;
using Rattrapage_API.Services;
using NUnit.Framework; // Assurez-vous que NUnit est inclus

namespace Rattrapage_API.Tests
{
    [TestFixture]
    public class ArtistesServiceTests
    {
        private Mock<IArtistesRepository> _repositoryMock;
        private ArtistesService _artistesService;

        /// <summary>
        /// Initialise les objets nécessaires pour les tests avant chaque test.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _repositoryMock = new Mock<IArtistesRepository>();
            _artistesService = new ArtistesService(_repositoryMock.Object);
        }

        /// <summary>
        /// Vérifie que la méthode GetAllArtistes retourne la liste des artistes correctement.
        /// </summary>
        [Test]
        public void GetAllArtistes_ReturnsListOfArtistes()
        {
            // Arrange
            var expectedArtistes = new List<Artiste>
            {
                new Artiste { Id = "1", Nom = "Artiste1", Genre = "Pop" },
                new Artiste { Id = "2", Nom = "Artiste2", Genre = "Rock" }
            };

            // Configure le mock pour retourner la liste d'artistes attendue
            _repositoryMock.Setup(repo => repo.ReadArtistes()).Returns(expectedArtistes);

            // Act
            var result = _artistesService.GetAllArtistes();

            // Assert
            Assert.AreEqual(expectedArtistes.Count, result.Count);
            Assert.AreEqual(expectedArtistes[0].Nom, result[0].Nom);
        }

        /// <summary>
        /// Vérifie que la méthode GetArtisteById retourne l'artiste correspondant à l'ID spécifié.
        /// </summary>
        [Test]
        public void GetArtisteById_ArtisteExists_ReturnsArtiste()
        {
            // Arrange
            var expectedArtiste = new Artiste { Id = "1", Nom = "Artiste1", Genre = "Pop" };
            var artistes = new List<Artiste> { expectedArtiste };

            // Configure le mock pour retourner la liste contenant l'artiste attendu
            _repositoryMock.Setup(repo => repo.ReadArtistes()).Returns(artistes);

            // Act
            var result = _artistesService.GetArtisteById("1");

            // Assert
            Assert.AreEqual(expectedArtiste, result);
        }

        /// <summary>
        /// Vérifie que la méthode GetArtisteById lance une exception si l'artiste avec l'ID spécifié n'existe pas.
        /// </summary>
        [Test]
        public void GetArtisteById_ArtisteDoesNotExist_ThrowsArtisteNotFoundException()
        {
            // Arrange
            var artistes = new List<Artiste>();

            // Configure le mock pour retourner une liste vide
            _repositoryMock.Setup(repo => repo.ReadArtistes()).Returns(artistes);

            // Act & Assert
            Assert.Throws<ArtisteNotFoundException>(() => _artistesService.GetArtisteById("1"));
        }

        /// <summary>
        /// Vérifie que la méthode AddArtiste ajoute un nouvel artiste à la liste.
        /// </summary>
        [Test]
        public void AddArtiste_ArtisteDoesNotExist_AddsArtiste()
        {
            // Arrange
            var newArtiste = new Artiste { Id = "3", Nom = "Artiste3", Genre = "Jazz" };
            var artistes = new List<Artiste>
            {
                new Artiste { Id = "1", Nom = "Artiste1", Genre = "Pop" }
            };

            // Configure le mock pour retourner la liste initiale d'artistes
            _repositoryMock.Setup(repo => repo.ReadArtistes()).Returns(artistes);
            // Configure le mock pour simuler l'écriture des artistes sans erreur
            _repositoryMock.Setup(repo => repo.WriteArtistes(It.IsAny<List<Artiste>>()));

            // Act
            _artistesService.AddArtiste(newArtiste);

            // Assert
            // Vérifie que WriteArtistes a été appelé une seule fois avec une liste d'artistes contenant le nouvel artiste
            _repositoryMock.Verify(repo => repo.WriteArtistes(It.Is<List<Artiste>>(a => a.Count == 2)), Times.Once);
        }

        /// <summary>
        /// Vérifie que la méthode AddArtiste lance une exception si l'artiste à ajouter existe déjà.
        /// </summary>
        [Test]
        public void AddArtiste_ArtisteAlreadyExists_ThrowsArtisteAlreadyExistsException()
        {
            // Arrange
            var existingArtiste = new Artiste { Id = "1", Nom = "Artiste1", Genre = "Pop" };
            var newArtiste = new Artiste { Id = "2", Nom = "Artiste1", Genre = "Rock" };
            var artistes = new List<Artiste> { existingArtiste };

            // Configure le mock pour retourner la liste contenant l'artiste existant
            _repositoryMock.Setup(repo => repo.ReadArtistes()).Returns(artistes);

            // Act & Assert
            Assert.Throws<ArtisteAlreadyExistsException>(() => _artistesService.AddArtiste(newArtiste));
        }

        /// <summary>
        /// Vérifie que la méthode UpdateArtiste met à jour correctement un artiste existant.
        /// </summary>
        [Test]
        public void UpdateArtiste_ArtisteExists_UpdatesArtiste()
        {
            // Arrange
            var updatedArtiste = new Artiste { Id = "1", Nom = "UpdatedArtiste", Genre = "Classical" };
            var existingArtiste = new Artiste { Id = "1", Nom = "OldArtiste", Genre = "Pop" };
            var artistes = new List<Artiste> { existingArtiste };

            // Configure le mock pour retourner la liste contenant l'artiste existant
            _repositoryMock.Setup(repo => repo.ReadArtistes()).Returns(artistes);
            // Configure le mock pour simuler l'écriture des artistes sans erreur
            _repositoryMock.Setup(repo => repo.WriteArtistes(It.IsAny<List<Artiste>>()));

            // Act
            _artistesService.UpdateArtiste("1", updatedArtiste);

            // Assert
            // Vérifie que l'artiste a été mis à jour dans la liste et que WriteArtistes a été appelé avec la liste mise à jour
            Assert.AreEqual("UpdatedArtiste", artistes.First(a => a.Id == "1").Nom);
            _repositoryMock.Verify(repo => repo.WriteArtistes(It.Is<List<Artiste>>(a => a.First().Nom == "UpdatedArtiste")), Times.Once);
        }

        /// <summary>
        /// Vérifie que la méthode UpdateArtiste lance une exception si l'artiste à mettre à jour n'existe pas.
        /// </summary>
        [Test]
        public void UpdateArtiste_ArtisteDoesNotExist_ThrowsArtisteNotFoundException()
        {
            // Arrange
            var updatedArtiste = new Artiste { Id = "2", Nom = "UpdatedArtiste", Genre = "Classical" };
            var artistes = new List<Artiste>
            {
                new Artiste { Id = "1", Nom = "Artiste1", Genre = "Pop" }
            };

            // Configure le mock pour retourner la liste d'artistes existants
            _repositoryMock.Setup(repo => repo.ReadArtistes()).Returns(artistes);

            // Act & Assert
            Assert.Throws<ArtisteNotFoundException>(() => _artistesService.UpdateArtiste("2", updatedArtiste));
        }

        /// <summary>
        /// Vérifie que la méthode DeleteArtiste supprime un artiste existant de la liste.
        /// </summary>
        [Test]
        public void DeleteArtiste_ArtisteExists_RemovesArtiste()
        {
            // Arrange
            var existingArtiste = new Artiste { Id = "1", Nom = "Artiste1", Genre = "Pop" };
            var artistes = new List<Artiste> { existingArtiste };

            // Configure le mock pour retourner la liste contenant l'artiste à supprimer
            _repositoryMock.Setup(repo => repo.ReadArtistes()).Returns(artistes);
            // Configure le mock pour simuler l'écriture des artistes sans erreur
            _repositoryMock.Setup(repo => repo.WriteArtistes(It.IsAny<List<Artiste>>()));

            // Act
            _artistesService.DeleteArtiste("1");

            // Assert
            // Vérifie que l'artiste a été supprimé de la liste et que WriteArtistes a été appelé avec une liste vide
            Assert.IsEmpty(artistes);
            _repositoryMock.Verify(repo => repo.WriteArtistes(It.Is<List<Artiste>>(a => !a.Any())), Times.Once);
        }

        /// <summary>
        /// Vérifie que la méthode DeleteArtiste lance une exception si l'artiste à supprimer n'existe pas.
        /// </summary>
        [Test]
        public void DeleteArtiste_ArtisteDoesNotExist_ThrowsArtisteNotFoundException()
        {
            // Arrange
            var artistes = new List<Artiste>
            {
                new Artiste { Id = "1", Nom = "Artiste1", Genre = "Pop" }
            };

            // Configure le mock pour retourner la liste d'artistes existants
            _repositoryMock.Setup(repo => repo.ReadArtistes()).Returns(artistes);

            // Act & Assert
            Assert.Throws<ArtisteNotFoundException>(() => _artistesService.DeleteArtiste("2"));
        }
    }
}