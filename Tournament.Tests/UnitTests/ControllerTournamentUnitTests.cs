using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Contracts;
using Tournament.Presentation.Controllers;
using Tournaments.Shared.Dtos;
using Tournaments.Shared.Request;

namespace Tournament.Tests.UnitTests;

public class ControllerTournamentUnitTests
{
    private readonly Mock<IServiceManager> _mockServiceManager;
    private readonly Mock<ITournamentService> _mockTournamentService;
    private readonly TournamentsController _tournamentsController;
    private readonly SeedData _data;

    private static readonly DateTime _now = new DateTime(2025, 1, 1);

    public ControllerTournamentUnitTests()
    {
        _data = new SeedData();
        _mockServiceManager = new Mock<IServiceManager>();
        _mockTournamentService = new Mock<ITournamentService>();

        _mockServiceManager.Setup(x => x.TournamentService).Returns(_mockTournamentService.Object);

        _tournamentsController = new TournamentsController(_mockServiceManager.Object);

        // to create a basic HttpContext so that Response.Headers from controller is available during the test
        _tournamentsController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };
    }

    

    [Fact]
    public async Task GetTournaments_ShouldReturnAllTournaments()
    {
        // Arrange
        var tournaments = _data.GetTournaments();
       var pagedList = new PagedList<TournamentDetail>(tournaments,tournaments.Count,pageNumber:1,pageSize:6);
        var dtos = _data.GetTournamentsDto();
        _mockTournamentService.Setup(x => x.GetAllTournamentsAsync(It.IsAny<TournamentRequestParams>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync((dtos,pagedList.MetaData));

        // Act
        var tournamentsDto = await _tournamentsController.GetTournamentsAsync(new TournamentRequestParams(), sortByTitle: true);


        // Assert

        var okResult = Assert.IsType<OkObjectResult>(tournamentsDto.Result);
        var okResultTournamentsDto = Assert.IsAssignableFrom<IEnumerable<TournamentDto>>(okResult.Value);
        Assert.Equal(3, okResultTournamentsDto.Count());
        _mockServiceManager.Verify(s => s.TournamentService.GetAllTournamentsAsync(
            It.IsAny<TournamentRequestParams>(),
            It.IsAny<bool>(),
            It.IsAny<bool>()),
            Times.Once()
            );
    }

    [Fact]
    public async Task GetTournamentById_ShouldReturnTournament()
    {
        // Arrange
        var dto = new TournamentDto { Id = 1, Title = "Title1", StartDate = _now };
        _mockServiceManager.Setup(x => x.TournamentService.GetTournamentByIdAsync(It.IsAny<int>(), It.IsAny<bool>()))
            .ReturnsAsync(dto);

        // Act
        var resultDto = await _tournamentsController.GetTournamentById(dto.Id, false);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(resultDto.Result);
        var okResultDto = Assert.IsType<TournamentDto>(okResult.Value);
        Assert.Equal("Title1", okResultDto.Title);
        Assert.Equal(1, okResultDto.Id);
        _mockServiceManager.Verify(s =>
        s.TournamentService.GetTournamentByIdAsync(It.IsAny<int>(), It.IsAny<bool>()), Times.Once());
    }

    [Fact]
    public async Task GetTournamentById_ShouldReturnNotFound()
    {
        // Arrange
        _mockTournamentService.Setup(s => s.GetTournamentByIdAsync(99, false))
            .ThrowsAsync(new TournamentNotFoundException(99));

        // Act & Assert
        await Assert.ThrowsAsync<TournamentNotFoundException>(() => _tournamentsController.GetTournamentById(99, false));
        _mockServiceManager.Verify(s => 
        s.TournamentService.GetTournamentByIdAsync(It.IsAny<int>(), It.IsAny<bool>()), Times.Once());
    }

    [Fact]
    public async Task GetTournamentByTitle_ShouldReturnTournament()
    {
        // Arrange
        var dto = new TournamentDto { Id = 1, Title = "Title1", StartDate = _now };
        _mockServiceManager.Setup(x => x.TournamentService.GetTournamentByTitleAsync(It.IsAny<string>()))
            .ReturnsAsync(dto);

        // Act
        var resultDto = await _tournamentsController.GetTournamentByTitleAsync("Title1");

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(resultDto.Result);
        var okResultDto = Assert.IsType<TournamentDto>(okResult.Value);
        Assert.Equal("Title1", okResultDto.Title);
        Assert.Equal(1, okResultDto.Id);
        _mockServiceManager.Verify(s => s.TournamentService.GetTournamentByTitleAsync(It.IsAny<string>()), Times.Once());
    }

    [Fact]
    public async Task GetTournamentByTitle_ShouldReturnNotFound()
    {
        // Arrange
        _mockTournamentService.Setup(s => s.GetTournamentByTitleAsync(It.IsAny<string>()))
            .ThrowsAsync(new TournamentNotFoundException(99));

        // Act & Assert
        await Assert.ThrowsAsync<TournamentNotFoundException>(() => 
        _tournamentsController.GetTournamentByTitleAsync(It.IsAny<string>()));
        _mockServiceManager.Verify(s => s.TournamentService.GetTournamentByTitleAsync(It.IsAny<string>()), Times.Once());
    }

    [Fact]
    public async Task PutTournamentAsync_ValidDto_ReturnsNoContent()
    {
        // Arrange
        var dto = new TournamentUpdateDto
        {
            Title = "Updated Title",
            StartDate = DateTime.Now,
        };

        _mockTournamentService
            .Setup(s => s.PutTournamentAsync(1, dto))
            .Returns(Task.CompletedTask); 

        // Act
        var result = await _tournamentsController.PutTournamentAsync(1, dto);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockServiceManager.Verify(s => s.TournamentService.PutTournamentAsync(1, dto), Times.Once);
    }

    [Fact]
    public async Task PutTournamentAsync_InvalidModelState_ReturnsBadRequest()
    {
        // Arrange
        _tournamentsController.ModelState.AddModelError("Title", "Required");

        var dto = new TournamentUpdateDto(); // Missing Title, etc.

        // Act
        var result = await _tournamentsController.PutTournamentAsync(1, dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.IsType<SerializableError>(badRequestResult.Value);
    }

    [Fact]
    public async Task PutTournamentAsync_TournamentNotFound_ThrowsException()
    {
        // Arrange
        var dto = new TournamentUpdateDto
        {
            Title = "Nonexistent Tournament",
            StartDate = DateTime.Now,
        };

        _mockTournamentService
            .Setup(s => s.PutTournamentAsync(99, dto))
            .ThrowsAsync(new TournamentNotFoundException(99));

        // Act & Assert
        await Assert.ThrowsAsync<TournamentNotFoundException>(() => _tournamentsController.PutTournamentAsync(99, dto));
        _mockServiceManager.Verify(s => s.TournamentService.PutTournamentAsync(99, dto), Times.Once);
    }

}