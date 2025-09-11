using CustomerOnboarding.Core.DTOs.Responses;
using CustomerOnboarding.Core.Models;
using CustomerOnboarding.Infrastructure.Data;
using CustomerOnboarding.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CustomerOnboarding.Tests
{
    public class StateServiceTests
    {
        private AppDbContext GetDbContext(string dbName)
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: dbName)
                .Options;
            return new AppDbContext(options);
        }

        [Fact]
        public async Task AddStateAsync_Should_Add_New_State()
        {
            // Arrange
            var db = GetDbContext(nameof(AddStateAsync_Should_Add_New_State));
            var service = new StateService(db);

            // Act
            var result = await service.AddStateAsync("Lagos");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Lagos", result.Name);
            Assert.True(db.States.Any(s => s.Name == "Lagos"));
        }

        [Fact]
        public async Task AddStateAsync_Should_Throw_When_State_Already_Exists()
        {
            // Arrange
            var db = GetDbContext(nameof(AddStateAsync_Should_Throw_When_State_Already_Exists));
            db.States.Add(new State { Name = "Lagos" });
            await db.SaveChangesAsync();

            var service = new StateService(db);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => service.AddStateAsync("Lagos"));
        }

        [Fact]
        public async Task GetAllStatesAsync_Should_Return_Empty_List_When_No_States()
        {
            // Arrange
            var db = GetDbContext(nameof(GetAllStatesAsync_Should_Return_Empty_List_When_No_States));
            var service = new StateService(db);

            // Act
            var result = await service.GetAllStatesAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllStatesAsync_Should_Return_States_With_Lgas()
        {
            // Arrange
            var db = GetDbContext(nameof(GetAllStatesAsync_Should_Return_States_With_Lgas));
            var state = new State
            {
                Name = "Lagos",
                Lgas = new List<Lga>
                {
                    new Lga { Name = "Ikeja" },
                    new Lga { Name = "Surulere" }
                }
            };
            db.States.Add(state);
            await db.SaveChangesAsync();

            var service = new StateService(db);

            // Act
            var result = await service.GetAllStatesAsync();

            // Assert
            Assert.Single(result);
            Assert.Equal("Lagos", result[0].Name);
            Assert.Equal(2, result[0].Lgas.Count);
            Assert.Contains(result[0].Lgas, l => l.Name == "Ikeja");
            Assert.Contains(result[0].Lgas, l => l.Name == "Surulere");
        }
    }
}
