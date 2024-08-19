using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Rattrapage_API.Controllers;
using Rattrapage_API.Dto;
using Rattrapage_API.Exceptions;
using Rattrapage_API.Interface;
using Rattrapage_API.Models;
using Rattrapage_API.Services;

namespace Rattrapage_API.Tests
{
    [TestFixture]
    public class ArtistesControllerTests
    {
        private Mock<IArtistesService> _mockArtistesService;
        private Mock<IMapper> _mockMapper;
        private ArtistesController _controller;

        [SetUp]
        public void SetUp()
        {
            _mockArtistesService = new Mock<IArtistesService>();
            _mockMapper = new Mock<IMapper>();
            _controller = new ArtistesController(_mockArtistesService.Object, _mockMapper.Object);
        }

        [Test]
        public void GetAllArtistes_ReturnsOkResult_WithListOfArtistes()
        {
            // Arrange
            var artistes = new List<Artiste>
            {
                new Artiste { Id = "1", Nom = "Artiste 1", Genre = "Genre 1" },
                new Artiste { Id = "2", Nom = "Artiste 2", Genre = "Genre 2" }
            };
            _mockArtistesService.Setup(service => service.GetAllArtistes()).Returns(artistes);

            // Act
            var result = _controller.GetAllArtistes();

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var returnValue = okResult.Value as List<Artiste>;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual(2, returnValue.Count);
        }

        [Test]
        public void GetAllArtistes_ThrowsArtistesListNotFoundException()
        {
            // Arrange
            _mockArtistesService.Setup(service => service.GetAllArtistes()).Throws(new ArtistesListNotFoundException());

            // Act & Assert
            Assert.Throws<ArtistesListNotFoundException>(() => _controller.GetAllArtistes());
        }

        [Test]
        public void GetArtisteById_ReturnsOkResult_WithArtiste()
        {
            // Arrange
            var artiste = new Artiste { Id = "1", Nom = "Artiste 1", Genre = "Genre 1" };
            _mockArtistesService.Setup(service => service.GetArtisteById("1")).Returns(artiste);

            // Act
            var result = _controller.GetArtisteById("1");

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Result should be an OkObjectResult");

            var returnValue = okResult.Value as Artiste;
            Assert.IsNotNull(returnValue, "Result value should be of type Artiste");
            Assert.AreEqual("Artiste 1", returnValue.Nom, "Artiste name should be 'Artiste 1'");
        }

        [Test]
        public void GetArtisteById_ThrowsArtisteNotFoundException()
        {
            // Arrange
            _mockArtistesService.Setup(service => service.GetAllArtistes()).Returns(new List<Artiste>());

            // Act & Assert
            Assert.Throws<ArtisteNotFoundException>(() => _controller.GetArtisteById("1"));
        }

        [Test]
        public void AddArtiste_ReturnsCreatedAtActionResult()
        {
            // Arrange
            var newArtisteDto = new ArtisteDto { Nom = "Artiste 3", Genre = "Genre 3" };
            var newArtiste = new Artiste { Id = "3", Nom = "Artiste 3", Genre = "Genre 3" };

            _mockArtistesService.Setup(service => service.GetAllArtistes()).Returns(new List<Artiste>());
            _mockMapper.Setup(mapper => mapper.Map<Artiste>(newArtisteDto)).Returns(newArtiste);

            // Act
            var result = _controller.AddArtiste(newArtisteDto);

            // Assert
            var createdResult = result as CreatedAtActionResult;
            Assert.IsNotNull(createdResult);
            var returnValue = createdResult.Value as Artiste;
            Assert.IsNotNull(returnValue);
            Assert.AreEqual("Artiste 3", returnValue.Nom);
        }

        [Test]
        public void UpdateArtiste_ReturnsNoContentResult()
        {
            // Arrange
            var updatedArtisteDto = new ArtisteDto { Nom = "Artiste Updated", Genre = "Genre Updated" };
            var updatedArtiste = new Artiste { Id = "1", Nom = "Artiste Updated", Genre = "Genre Updated" };

            _mockMapper.Setup(mapper => mapper.Map<Artiste>(updatedArtisteDto)).Returns(updatedArtiste);

            // Act
            var result = _controller.UpdateArtiste("1", updatedArtisteDto);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public void UpdateArtiste_ThrowsArtisteNotFoundException()
        {
            // Arrange
            var updatedArtisteDto = new ArtisteDto { Nom = "Artiste Updated", Genre = "Genre Updated" };
            _mockArtistesService.Setup(service => service.UpdateArtiste("1", It.IsAny<Artiste>())).Throws(new ArtisteNotFoundException("1"));

            // Act & Assert
            Assert.Throws<ArtisteNotFoundException>(() => _controller.UpdateArtiste("1", updatedArtisteDto));
        }

        [Test]
        public void DeleteArtiste_ReturnsNoContentResult()
        {
            // Act
            var result = _controller.DeleteArtiste("1");

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public void DeleteArtiste_ThrowsArtisteNotFoundException()
        {
            // Arrange
            _mockArtistesService.Setup(service => service.DeleteArtiste("1")).Throws(new ArtisteNotFoundException("1"));

            // Act & Assert
            Assert.Throws<ArtisteNotFoundException>(() => _controller.DeleteArtiste("1"));
        }

        [Test]
        public void SearchArtistes_ReturnsOkResult_WithArtistes()
        {
            // Arrange
            var searchName = "Artiste";
            var artistes = new List<Artiste>
            {
                new Artiste { Id = "1", Nom = "Artiste 1", Genre = "Genre 1" },
                new Artiste { Id = "2", Nom = "Artiste 2", Genre = "Genre 2" }
            };

            _mockArtistesService.Setup(service => service.SearchArtistes(searchName)).Returns(artistes);

            // Act
            var result = _controller.SearchArtistes(searchName);

            // Assert
            var okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult, "Result should be an OkObjectResult");

            var returnValue = okResult.Value as List<Artiste>;
            Assert.IsNotNull(returnValue, "Result value should be a list of Artiste");
            Assert.AreEqual(2, returnValue.Count, "The number of artistes returned should be 2");
            Assert.IsTrue(returnValue.Any(a => a.Nom.Contains(searchName)), "The returned list should contain artistes with names containing the search term");
        }

    }
}