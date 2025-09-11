using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Core.DTOs.Responses;
using CustomerOnboarding.Core.Models;
using CustomerOnboarding.Infrastructure.Data;
using CustomerOnboarding.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CustomerOnboarding.Tests
{
    public class LgaServiceTests
    {
        private AppDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddLgaAsync_ShouldAdd_WhenStateExistsAndNoDuplicate()
        {
            // Arrange
            var db = GetDbContext(nameof(AddLgaAsync_ShouldAdd_WhenStateExistsAndNoDuplicate));
            var state = new State { Id = Guid.NewGuid(), Name = "Lagos" };
            db.States.Add(state);
            await db.SaveChangesAsync();

            var service = new LgaService(db);
            var dto = new LgaDto { Name = "Ikeja", StateId = state.Id };

            // Act
            var result = await service.AddLgaAsync(dto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(dto.Name, result.Name);
            Assert.Equal(state.Id, result.StateId);
            Assert.Equal("Lagos", result.StateName);
        }

        [Fact]
        public async Task AddLgaAsync_ShouldThrow_WhenStateDoesNotExist()
        {
            // Arrange
            var db = GetDbContext(nameof(AddLgaAsync_ShouldThrow_WhenStateDoesNotExist));
            var service = new LgaService(db);
            var dto = new LgaDto { Name = "Ikeja", StateId = Guid.NewGuid() };

            // Act + Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddLgaAsync(dto));
        }

        [Fact]
        public async Task AddLgaAsync_ShouldThrow_WhenDuplicateExists()
        {
            // Arrange
            var db = GetDbContext(nameof(AddLgaAsync_ShouldThrow_WhenDuplicateExists));
            var state = new State { Id = Guid.NewGuid(), Name = "Lagos" };
            db.States.Add(state);
            db.Lgas.Add(new Lga { Name = "Ikeja", StateId = state.Id });
            await db.SaveChangesAsync();

            var service = new LgaService(db);
            var dto = new LgaDto { Name = "Ikeja", StateId = state.Id };

            // Act + Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddLgaAsync(dto));
        }

        [Fact]
        public async Task GetLgasByStateAsync_ShouldReturnLgas_WhenStateHasLgas()
        {
            // Arrange
            var db = GetDbContext(nameof(GetLgasByStateAsync_ShouldReturnLgas_WhenStateHasLgas));
            var state = new State { Id = Guid.NewGuid(), Name = "Oyo" };
            db.States.Add(state);

            db.Lgas.AddRange(
                new Lga { Name = "Ibadan North", StateId = state.Id, State = state },
                new Lga { Name = "Ibadan South", StateId = state.Id, State = state }
            );
            await db.SaveChangesAsync();

            var service = new LgaService(db);

            // Act
            var result = await service.GetLgasByStateAsync(state.Id);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.All(result, l => Assert.Equal("Oyo", l.StateName));
        }

        [Fact]
        public async Task GetLgasByStateAsync_ShouldReturnEmpty_WhenNoLgasExist()
        {
            // Arrange
            var db = GetDbContext(nameof(GetLgasByStateAsync_ShouldReturnEmpty_WhenNoLgasExist));
            var state = new State { Id = Guid.NewGuid(), Name = "Ekiti" };
            db.States.Add(state);
            await db.SaveChangesAsync();

            var service = new LgaService(db);

            // Act
            var result = await service.GetLgasByStateAsync(state.Id);

            // Assert
            Assert.Empty(result);
        }
    }
}
