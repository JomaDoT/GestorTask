using AutoFixture;
using FluentAssertions;
using FluentValidation;
using FluentValidation.TestHelper;
using GestorTask.Application.DTO.Task;
using GestorTask.Application.Interfaces;
using GestorTask.Controllers;
using GestorTask.Infraestructure.Models;
using GestorTask.Validators;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static GestorTask.Validators.TaskAddValidator;

namespace GestorTask.Test;
public class TaskControllerTest
{
    private readonly IFixture _fixture;
    private readonly Mock<ITaskServices> _taskServicesMock;
    private readonly Mock<IUserServices> _userServicesMock;
    private readonly TaskController _controller; //controller System Under Test
    private readonly IValidator<AddTaskDTO> _validatorAdd = new TaskAddValidator();
    private readonly IValidator<UpdateTaskDTO> _validatorUpdate = new TaskUpdateValidator();

    public TaskControllerTest()
    {
        _fixture = new Fixture();
        _taskServicesMock = _fixture.Freeze<Mock<ITaskServices>>();
        _userServicesMock = _fixture.Freeze<Mock<IUserServices>>();
        _controller = new TaskController(_taskServicesMock.Object,_userServicesMock.Object);//creates the implementation in-memory
    }
    [Fact]
    public async Task GetTasks_ReturnOkResult_WhenHaveData()
    {

        //Arrange
        var TasksMock = _fixture.Create<IEnumerable<Tasks>>();
        var TasksMockDTO = _fixture.Create<IEnumerable<GetTaskDTO>>();
        _taskServicesMock.Setup(x => x.GetAllAsync(null)).ReturnsAsync(TasksMock); //Service method here

        // Act
        var result = await _controller.Get();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ActionResult>();
        result.Should().BeAssignableTo<OkObjectResult>();
        result.As<OkObjectResult>().Value
            .Should().NotBeNull();
        _taskServicesMock.Verify(x => x.GetAllAsync(null), Times.Once());
    }
    [Fact]
    public async Task GetTasks_ReturnNotFound_WhenDontHaveData()
    {

        //Arrange
        List<Tasks>? TasksMock = null;
        _taskServicesMock.Setup(x => x.GetAllAsync(null)).ReturnsAsync(TasksMock); //Service method here

        // Act
        var result = await _controller.Get();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ActionResult>();
        result.Should().BeAssignableTo<NotFoundObjectResult>();
        result.As<NotFoundObjectResult>().Value
            .Should().NotBeNull();
        _taskServicesMock.Verify(x => x.GetAllAsync(null), Times.Once());
    }
    [Fact]

    public async Task GetTaskById_ReturnOkResponse_WhenHaveData()
    {

        //Arrange
        Tasks TasksMock = _fixture.Create<Tasks>();
        int id = _fixture.Create<int>();
        _taskServicesMock.Setup(x => x.GetAsync(x=>x.Id== id)).ReturnsAsync(TasksMock); //Service method here

        // Act
        var result = await _controller.Get(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ActionResult>();
        result.Should().BeAssignableTo<OkObjectResult>();
        result.As<OkObjectResult>().Value
            .Should().NotBeNull();
        _taskServicesMock.Verify(x => x.GetAsync(x=>x.Id == id), Times.Once());
    }
    [Fact]
    public async Task GetTaskById_ReturnNotFoundResponse_WhenDontHaveData()
    {

        //Arrange
        Tasks? TasksMock = null;
        int id = _fixture.Create<int>();
        _taskServicesMock.Setup(x => x.GetAsync(x => x.Id == id)).ReturnsAsync(TasksMock); //Service method here

        // Act
        var result = await _controller.Get(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ActionResult>();
        result.Should().BeAssignableTo<NotFoundObjectResult>();
        result.As<NotFoundObjectResult>().Value
            .Should().NotBeNull();
        _taskServicesMock.Verify(x => x.GetAsync(x => x.Id == id), Times.Once());
    }
    [Fact]
    public async Task GetTaskById_ReturnNotFoundResponse_WhenIsLessThanZero()
    {

        //Arrange
        Tasks TasksMock = _fixture.Create<Tasks>();
        int id = -1;

        // Act
        var result = await _controller.Get(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ActionResult>();
        result.Should().BeAssignableTo<BadRequestObjectResult>();
    }

    [Fact]
    public async Task CreateTask_ReturnOkResponse_WhenValidData()
    {
        //Arrange
        Tasks request = _fixture.Create<Tasks>();
        AddTaskDTO dto = _fixture.Create<AddTaskDTO>();
        _taskServicesMock.Setup(x => x.InsertAsync(request)); //Service method here

        // Act
        var result = await _controller.Post(dto);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ActionResult>();
        result.Should().BeAssignableTo<OkObjectResult>();
        _taskServicesMock.Verify(x => x.InsertAsync(request), Times.Never());
    }

    [Fact]
    public async Task CreateTask_ReturnBadRequestResponse_WhenInvalidData()
    {
        //Arrange
        Tasks request = _fixture.Create<Tasks>();
        AddTaskDTO dto = _fixture.Create<AddTaskDTO>();
        dto.Name = string.Empty;
        request.Name = string.Empty;
        _taskServicesMock.Setup(x => x.InsertAsync(request)); //Service method here

        // Act
        var result = await _validatorAdd.TestValidateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.ShouldHaveAnyValidationError();
        result.ShouldHaveValidationErrorFor(x=>x.Name);

    }

    [Fact]
    public async Task UpdateTask_ReturnNotFoundResponse_WhenInValidData()
    {
        //Arrange
        Tasks request = _fixture.Create<Tasks>();
        UpdateTaskDTO dtoUpdate = _fixture.Create<UpdateTaskDTO>();

        _taskServicesMock.Setup(x => x.UpdateAsync(request)); //Service method here
        var result = await _controller.Put(dtoUpdate);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ActionResult>();
        result.Should().BeAssignableTo<NotFoundObjectResult>();
        _taskServicesMock.Verify(x => x.UpdateAsync(request), Times.Never());
    }
    [Fact]
    public async Task UpdateTask_ReturnBadRequestResponse_WhenIsLessThanZero()
    {
        //Arrange
        UpdateTaskDTO dto = _fixture.Create<UpdateTaskDTO>();
        dto.Id = -1;

        // Act
        var result = await _validatorUpdate.TestValidateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.ShouldHaveAnyValidationError();
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact]
    public async Task DeleteTask_ReturnNotFoundResponse_WhenInValidData()
    {
        //Arrange
        Tasks TasksMock = _fixture.Create<Tasks>();
        int id = _fixture.Create<int>();
        _taskServicesMock.Setup(x => x.DeleteAsync(TasksMock));
        var result = await _controller.Delete(id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<ActionResult>();
        result.Should().BeAssignableTo<BadRequestObjectResult>();
    }

    [Fact]
    public async Task DeleteTask_ReturnBadRequestResponse_WhenIsLessThanZero()
    {
        //Arrange
        int Id = -1;

        // Act
        var result = await _controller.Delete(Id);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeAssignableTo<IActionResult>();
        result.Should().BeAssignableTo<BadRequestObjectResult>();
    }
}

