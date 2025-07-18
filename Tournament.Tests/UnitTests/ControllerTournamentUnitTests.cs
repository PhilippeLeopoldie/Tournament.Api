using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Moq;
using Services.Contracts;
using Tournament.Core.Dtos;
using Tournament.Core.Entities;
using Tournament.Core.Exceptions;
using Tournament.Core.Request;
using Tournament.Presentation.Controllers;

namespace Tournament.Tests.UnitTests;

public class ControllerTournamentUnitTests
{
    private readonly Mock<IServiceManager> _mockServiceManager;
    private readonly Mock<ITournamentService> _mockTournamentService;
    private readonly Mock<IObjectModelValidator> _mockValidator;
    private readonly TournamentsController _tournamentsController;
    private readonly SeedData _data;

    private static readonly DateTime _now = new DateTime(2025, 1, 1);

    public ControllerTournamentUnitTests()
    {
        _data = new SeedData();
        _mockServiceManager = new Mock<IServiceManager>();
        _mockTournamentService = new Mock<ITournamentService>();
        _mockValidator = new Mock<IObjectModelValidator>();

        _mockServiceManager.Setup(x => x.TournamentService).Returns(_mockTournamentService.Object);
        _tournamentsController = new TournamentsController(_mockServiceManager.Object);

        // to create a basic HttpContext so that Response.Headers from controller is available during the test
        _tournamentsController.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        _mockValidator.Setup(v =>
            v.Validate(
                It.IsAny<ActionContext>(),
                It.IsAny<ValidationStateDictionary>(),
                It.IsAny<string>(),
                It.IsAny<object>())
        );

        _tournamentsController.ObjectValidator = _mockValidator.Object;
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

    [Fact]
    public async Task PatchTournamentAsync_ValidPatch_ReturnsNoContent()
    {
        // Arrange
        var patchDoc = new JsonPatchDocument<TournamentUpdateDto>();
        patchDoc.Replace(t => t.Title, "Updated Title");

        var dto = new TournamentUpdateDto { Title = "Original Title" };
        var entity = new TournamentDetail { Id = 1, Title = "Original Title" };

        _mockTournamentService.Setup(s => s.TournamentToPatchAsync(1))
            .ReturnsAsync((entity, dto));

        _mockTournamentService.Setup(s => s.SavePatchTournamentAsync(entity, dto))
            .Returns(Task.CompletedTask);

        _mockValidator.Setup(v =>
            v.Validate(It.IsAny<ActionContext>(), It.IsAny<ValidationStateDictionary>(), It.IsAny<string>(), It.IsAny<object>())
        );
        _tournamentsController.ObjectValidator = _mockValidator.Object;


        // Act
        var result = await _tournamentsController.PatchTournamentAsync(1, patchDoc);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockServiceManager.Verify(s => s.TournamentService.TournamentToPatchAsync(It.IsAny<int>()), Times.Once);
        _mockServiceManager.Verify(s => s.TournamentService.SavePatchTournamentAsync(
            It.IsAny<TournamentDetail>(), It.IsAny<TournamentUpdateDto>()), Times.Once);
    }

    [Fact]
    public async Task PatchTournamentAsync_NullPatchDocument_ThrowsInvalidEntryBadRequestException()
    {
       
        // Act & Assert
        await Assert.ThrowsAsync<InvalidEntryBadRequestException>(() =>
            _tournamentsController.PatchTournamentAsync(1, null!));
        _mockServiceManager.Verify(s => s.TournamentService.TournamentToPatchAsync(It.IsAny<int>()), Times.Never);
    }

    [Fact]
    public async Task PatchTournamentAsync_InvalidModelState_ReturnsUnprocessableEntity()
    {
        // Arrange
        var patchDoc = new JsonPatchDocument<TournamentUpdateDto>();
        patchDoc.Replace(t => t.Title, ""); // Invalid (assuming required)

        var dto = new TournamentUpdateDto { Title = "Valid Title" };
        var tournamentEntity = new TournamentDetail { Id = 1, Title = "Tournament" };

        _mockTournamentService.Setup(s => s.TournamentToPatchAsync(1))
            .ReturnsAsync((tournamentEntity, dto));

        _tournamentsController.ModelState.AddModelError("Title", "The Title field is required."); // Simulate validation failure

        // Act
        var result = await _tournamentsController.PatchTournamentAsync(1, patchDoc);

        // Assert
        var unprocessable = Assert.IsType<UnprocessableEntityObjectResult>(result);
        Assert.Equal(StatusCodes.Status422UnprocessableEntity, unprocessable.StatusCode);
        _mockServiceManager.Verify(s => s.TournamentService.TournamentToPatchAsync(It.IsAny<int>()), Times.Once);
        _mockServiceManager.Verify(s => s.TournamentService.SavePatchTournamentAsync(
            It.IsAny<TournamentDetail>(), It.IsAny<TournamentUpdateDto>()), Times.Never);
    }   

    [Fact]
    public async Task PatchTournamentAsync_TournamentNotFound_ThrowsException()
    {
        // Arrange
        var patchDoc = new JsonPatchDocument<TournamentUpdateDto>();

        _mockTournamentService.Setup(s => s.TournamentToPatchAsync(99))
            .ThrowsAsync(new TournamentNotFoundException(99));

        // Act & Assert
        await Assert.ThrowsAsync<TournamentNotFoundException>(() =>
            _tournamentsController.PatchTournamentAsync(99, patchDoc));
        _mockServiceManager.Verify(s => s.TournamentService.TournamentToPatchAsync(It.IsAny<int>()), Times.Once);
        _mockServiceManager.Verify(s => s.TournamentService.SavePatchTournamentAsync(
            It.IsAny<TournamentDetail>(), It.IsAny<TournamentUpdateDto>()), Times.Never);
    }

    [Fact]
    public async Task PostTournament_ValidTournament_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var createDto = new TournamentCreateDto
        {
            Title = "Test Tournament",
            StartDate = DateTime.Now
        };

        var createdDto = new TournamentDto
        {
            Id = 1,
            Title = "Test Tournament",
            StartDate = createDto.StartDate
        };

        _mockTournamentService
            .Setup(s => s.PostTournamentAsync(createDto))
            .ReturnsAsync(createdDto);

        _mockValidator.Setup(v => v.Validate(It.IsAny<ActionContext>(), null, null, null));
        _tournamentsController.ObjectValidator = _mockValidator.Object;

        // Act
        var result = await _tournamentsController.PostTournament(createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal("GetTournamentById", createdResult.ActionName);
        Assert.Equal(createdDto, createdResult.Value);
        _mockServiceManager.Verify(s => s.TournamentService.PostTournamentAsync(createDto), Times.Once);
    }

    [Fact]
    public async Task PostTournament_InvalidModel_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new TournamentCreateDto(); // Invalid due to missing required fields
        _tournamentsController.ModelState.AddModelError("Title", "Required");

        // Act
        var result = await _tournamentsController.PostTournament(createDto);

        // Assert
        var badRequest = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.IsType<SerializableError>(badRequest.Value);
        _mockServiceManager.Verify(s => s.TournamentService.PostTournamentAsync(It.IsAny<TournamentCreateDto>()), Times.Never);
    }

    [Fact]
    public async Task DeleteTournamentAsync_ExistingTournament_ReturnsNoContent()
    {
        // Arrange
        _mockTournamentService
            .Setup(s => s.DeleteTournamentAsync(1))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _tournamentsController.DeleteTournamentAsync(1);

        // Assert
        Assert.IsType<NoContentResult>(result);
        _mockServiceManager.Verify(s => s.TournamentService.DeleteTournamentAsync(1), Times.Once);
    }

    [Fact]
    public async Task DeleteTournamentAsync_TournamentNotFound_ThrowsTournamentNotFoundException()
    {
        // Arrange
        _mockTournamentService
            .Setup(s => s.DeleteTournamentAsync(99))
            .ThrowsAsync(new TournamentNotFoundException(99));

        // Act & Assert
        await Assert.ThrowsAsync<TournamentNotFoundException>(() =>
            _tournamentsController.DeleteTournamentAsync(99));

        _mockServiceManager.Verify(s => s.TournamentService.DeleteTournamentAsync(99), Times.Once);
    }

}