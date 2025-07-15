using Domain.Models.Entities;
using Domain.Models.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Services.Contracts;
using Tournament.Presentation.Controllers;
using Tournaments.Shared.Dtos;
using Tournaments.Shared.Request;

namespace Tournament.Tests;

public class ControllerTournamentUnitTests
{
    private readonly Mock<IServiceManager> _mockServiceManager;
    private readonly Mock<ITournamentService> _mockTournamentService;
    private readonly TournamentsController _tournamentsController;

    private static readonly DateTime _now = new DateTime(2025, 1, 1);

    public ControllerTournamentUnitTests()
    {
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
        var tournaments = GetTournaments();
       var pagedList = new PagedList<TournamentDetail>(tournaments,tournaments.Count,pageNumber:1,pageSize:6);
        var dtos = GetTournamentsDto();
        _mockTournamentService.Setup(x => x.GetAllTournamentsAsync(It.IsAny<TournamentRequestParams>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync((dtos,pagedList.MetaData));

        // Act
        var tournamentsDto = await _tournamentsController.GetTournamentsAsync(new TournamentRequestParams(), sortByTitle: true);


        // Assert

        var okResult = Assert.IsType<OkObjectResult>(tournamentsDto.Result);
        var okResultTournamentsDto = Assert.IsAssignableFrom<IEnumerable<TournamentDto>>(okResult.Value);
        Assert.Equal(3, okResultTournamentsDto.Count());
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

    }

    [Fact]
    public async Task GetTournamentById_WhenNotFound_ShouldReturnNotFound()
    {
        // Arrange
        _mockTournamentService.Setup(s => s.GetTournamentByIdAsync(99, false))
            .ThrowsAsync(new TournamentNotFoundException(99));

        // Act & Assert
        await Assert.ThrowsAsync<TournamentNotFoundException>(() => _tournamentsController.GetTournamentById(99, false));
    }

    private List<TournamentDto> GetTournamentsDto()
    {
        return new List<TournamentDto>()
        {
            new()
            {
                Id = 1,
                Title = "Tournament1",
                StartDate = DateTime.Now,
                Games = []
            },
            new()
            {
                Id = 2,
                Title = "Tournament2",
                StartDate = DateTime.Now,
                Games = []
            },
            new()
            {
                Id = 3,
                Title = "Tournament3",
                StartDate = DateTime.Now,
                Games = []
            }
        };
    }

    private List<TournamentDetail> GetTournaments()
    {
        return new List<TournamentDetail>
        {
            new()
            {
                Id = 1,
                Title = "Tournament1",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(3),
                Games = []
            },
            new()
            {
                Id = 2,
                Title = "Tournament2",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(3),
                Games = []
            },
            new()
            {
                Id = 3,
                Title = "Tournament3",
                StartDate = DateTime.Now,
                EndDate = DateTime.Now.AddMonths(3),
                Games = []
            },

        };
    }
}