

using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Serilog;
using Todo_API.Controllers;
using Todo_API.Entities;
using Todo_API.Models.TodoItemDtos;
using Todo_API.Profiles;
using Todo_API.Services;
using Xunit;
using Assert = Xunit.Assert;

namespace Todo_API_Test.Controllers
{
    [TestClass]
    public class TodoItemsControllerTests
    {
        private Mock<IRepository> _mockRepository;
        private IMapper _mapper;
        private TodoItemsController _controller;
        private Fixture _fixture;
        private Serilog.ILogger _logger;

        public TodoItemsControllerTests()
        {
            _fixture = new Fixture();
            _mockRepository = new Mock<IRepository>();

            // Use Serilog for logging
            var loggerConfig = new LoggerConfiguration().WriteTo.Console();
            var logger = loggerConfig.CreateLogger();
            _logger = logger;

            var config = new MapperConfiguration(cfg => cfg.AddProfile<TodoProfile>());
            _mapper = config.CreateMapper();

            _controller = new TodoItemsController(_mockRepository.Object, _mapper, _logger);
        }

        #region GetTodoItems Test Cases

        [Fact]
        public async Task GetTodoItems_ReturnsOk_WhenItemExists()
        {
            // Arrange
            var todoItems = _fixture.CreateMany<TodoItem>();
            var expectedDto = _mapper.Map<IEnumerable<TodoItemDto>>(todoItems);

            _mockRepository.Setup(repo => repo.GetTodoItemsAsync()).ReturnsAsync(todoItems);

            // Act
            var result = await _controller.GetTodoItems();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsAssignableFrom<IEnumerable<TodoItemDto>>(okResult.Value);

            Assert.NotNull(okResult.Value);
            Assert.Equal(todoItems.Count(), items.Count());
        }

        [Fact]
        public async Task GetTodoItems_Returns200_WhenItemsExist()
        {
            // Arrange
            var todoItems = _fixture.CreateMany<TodoItem>();
            _mockRepository.Setup(repo => repo.GetTodoItemsAsync()).ReturnsAsync(todoItems);

            // Act
            var result = await _controller.GetTodoItems();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var items = Assert.IsAssignableFrom<IEnumerable<TodoItemDto>>(okResult.Value);

            Assert.NotNull(okResult.Value);
            Assert.Equal(todoItems.Count(), items.Count());
            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
        }

        [Fact]
        public async Task GetTodoItems_Returns_NotFoundResult_When_TodoItems_Does_Not_Exist()
        {
            // Arrange
            _mockRepository.Setup(repo => repo.GetTodoItemsAsync()).ReturnsAsync(new List<TodoItem>());

            // Act
            var result = await _controller.GetTodoItems();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);

            Assert.NotNull(notFoundResult);
            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        }

        #endregion

        #region GetTodoItem Test Cases

        [Fact]
        public async Task GetTodoItem_ReturnsOk_WhenItemExists()
        {
            // Arrange
            long id = _fixture.Create<long>();
            var todoItem = _fixture.Create<TodoItem>();
            var expectedDto = _mapper.Map<TodoItemDto>(todoItem);

            _mockRepository.Setup(r => r.TodoItemExistsAsync(id)).ReturnsAsync(true);
            _mockRepository.Setup(r => r.GetTodoItemAsync(id)).ReturnsAsync(todoItem);

            // Act
            var result = await _controller.GetTodoItem(id);
            var obj = result as ObjectResult;

            // Assert
            Assert.Equal(200, obj.StatusCode);

            var actualDto = obj.Value as TodoItemDto;

            Assert.NotNull(actualDto);
            Assert.Equal(expectedDto.Id, actualDto.Id);
            Assert.Equal(expectedDto.Task, actualDto.Task);

            _mockRepository.Verify(r => r.TodoItemExistsAsync(id), Times.Once);
            _mockRepository.Verify(r => r.GetTodoItemAsync(id), Times.Once);
        }

        [Fact]
        public async Task GetTodoItem_Returns_NotFoundResult_When_TodoItem_Does_Not_Exist()
        {
            // Arrange
            var todoItemId = 1;
            _mockRepository.Setup(x => x.TodoItemExistsAsync(todoItemId)).ReturnsAsync(false);

            // Act
            var result = await _controller.GetTodoItem(todoItemId);
            var obj = result as ObjectResult;

            // Assert
            Assert.Equal(404, obj.StatusCode);
            Assert.Equal($"Todo item with ID {todoItemId} does not exist, please enter correct id, and try again", obj.Value);
        }

        [Fact]
        public async Task GetTodoItem_Returns_InternalServerError_When_TodoItem_Returned_Is_Null()
        {
            // Arrange
            long id = _fixture.Create<long>();
            _mockRepository.Setup(r => r.TodoItemExistsAsync(id)).ReturnsAsync(true);
            _mockRepository.Setup(r => r.GetTodoItemAsync(id)).ReturnsAsync((TodoItem)null);


            // Act
            var result = await _controller.GetTodoItem(id);
            var obj = result as ObjectResult;

            // Assert
            Assert.Equal(500, obj.StatusCode);
            Assert.Equal($"An error occurred while getting the todo item with ID {id}.", obj.Value);

            _mockRepository.Verify(r => r.TodoItemExistsAsync(id), Times.Once);
            _mockRepository.Verify(r => r.GetTodoItemAsync(id), Times.Once);
        }

        #endregion
    }
}