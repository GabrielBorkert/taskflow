using FluentAssertions;
using Moq;
using TaskFlow.Application.DTOs;
using TaskFlow.Application.Interfaces;
using TaskFlow.Application.Services;
using TaskFlow.Domain.Entities;
using TaskFlow.Domain.Enums;

namespace TaskFlow.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _repositoryMock;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _repositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_repositoryMock.Object);
        }

        [Fact]
        public async Task GetTaskById_DeveRetornarTarefaExistente()
        {
            // Arrange
            var tarefa = new TaskEntity
            {
                Id = 1,
                Title = "Tarefa Teste",
                Description = "Descrição",
                Status = TaskItemStatus.ToDo,
                CreatedAt = DateTime.Now
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tarefa);

            // Act
            var resultado = await _taskService.GetTaskByIdAsync(1);

            // Assert
            resultado.Should().NotBeNull();
            resultado!.Title.Should().Be("Tarefa Teste");
        }

        [Fact]
        public async Task GetTaskById_DeveRetornarNullParaIdInexistente()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((TaskEntity?)null);

            // Act
            var resultado = await _taskService.GetTaskByIdAsync(99);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task CreateTask_DeveCriarTarefaComSucesso()
        {
            // Arrange
            var dto = new CreateTaskDto
            {
                Title = "Nova Tarefa",
                Description = "Descrição",
                Status = TaskItemStatus.ToDo,
                Priority = TaskPriority.Media
            };

            var tarefaCriada = new TaskEntity
            {
                Id = 1,
                Title = dto.Title,
                Description = dto.Description,
                Status = dto.Status,
                Priority = dto.Priority,
                CreatedAt = DateTime.Now
            };

            _repositoryMock.Setup(r => r.CreateAsync(It.IsAny<TaskEntity>())).ReturnsAsync(tarefaCriada);

            // Act
            var resultado = await _taskService.CreateTaskAsync(dto);

            // Assert
            resultado.Should().NotBeNull();
            resultado.Title.Should().Be("Nova Tarefa");
            resultado.Id.Should().Be(1);
        }

        [Fact]
        public async Task UpdateTask_DeveAtualizarTarefaExistente()
        {
            // Arrange
            var tarefaExistente = new TaskEntity
            {
                Id = 1,
                Title = "Título Antigo",
                Description = "Descrição Antiga",
                IsCompleted = false,
                Status = TaskItemStatus.ToDo,
                CreatedAt = DateTime.Now
            };

            var dto = new UpdateTaskDto
            {
                Title = "Título Novo",
                Description = "Descrição Nova",
                IsCompleted = false,
                Status = TaskItemStatus.InProgress,
                Priority = TaskPriority.Alta
            };

            _repositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(tarefaExistente);
            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<TaskEntity>())).ReturnsAsync(tarefaExistente);

            // Act
            var resultado = await _taskService.UpdateTaskAsync(1, dto);

            // Assert
            resultado.Should().NotBeNull();
            resultado!.Title.Should().Be("Título Novo");
        }

        [Fact]
        public async Task UpdateTask_DeveRetornarNullParaIdInexistente()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetByIdAsync(99)).ReturnsAsync((TaskEntity?)null);

            var dto = new UpdateTaskDto
            {
                Title = "Título",
                Description = "Descrição",
                IsCompleted = false,
                Status = TaskItemStatus.ToDo,
                Priority = TaskPriority.Media
            };

            // Act
            var resultado = await _taskService.UpdateTaskAsync(99, dto);

            // Assert
            resultado.Should().BeNull();
        }

        [Fact]
        public async Task DeleteTask_DeveRetornarTrueParaTarefaExistente()
        {
            // Arrange
            _repositoryMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            // Act
            var resultado = await _taskService.DeleteTaskAsync(1);

            // Assert
            resultado.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteTask_DeveRetornarFalseParaIdInexistente()
        {
            // Arrange
            _repositoryMock.Setup(r => r.DeleteAsync(99)).ReturnsAsync(false);

            // Act
            var resultado = await _taskService.DeleteTaskAsync(99);

            // Assert
            resultado.Should().BeFalse();
        }
    }
}