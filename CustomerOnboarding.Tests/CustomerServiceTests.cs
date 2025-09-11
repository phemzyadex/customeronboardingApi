using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Core.DTOs.Responses;
using CustomerOnboarding.Core.Interfaces;
using CustomerOnboarding.Core.Models;
using CustomerOnboarding.Infrastructure.Data;
using CustomerOnboarding.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CustomerOnboarding.Tests
{
    public class CustomerServiceTests
    {
        private readonly DbContextOptions<AppDbContext> _dbOptions;

        public CustomerServiceTests()
        {
            _dbOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        private CustomerService CreateService(Mock<IOtpSender>? otpMock = null)
        {
            var db = new AppDbContext(_dbOptions);
            var otpSender = otpMock?.Object ?? new Mock<IOtpSender>().Object;
            return new CustomerService(db, otpSender);
        }

        [Fact]
        public async Task OnboardAsync_ShouldReturnFail_WhenStateNotFound()
        {
            using var db = new AppDbContext(_dbOptions);
            var service = new CustomerService(db, new Mock<IOtpSender>().Object);

            var dto = new OnboardCustomerDto
            {
                Email = "test@example.com",
                PhoneNumber = "+123456789",
                Password = "password",
                StateId = Guid.NewGuid(),
                LgaId = Guid.NewGuid()
            };

            var result = await service.OnboardAsync(dto);

            Assert.False(result.Success);
            Assert.Equal("Invalid state ID.", result.Message);
        }

        [Fact]
        public async Task OnboardAsync_ShouldCreateCustomer_WhenValid()
        {
            using var db = new AppDbContext(_dbOptions);
            var otpMock = new Mock<IOtpSender>();

            var state = new State
            {
                Id = Guid.NewGuid(),
                Name = "TestState",
                Lgas = new List<Lga>
                {
                    new Lga { Id = Guid.NewGuid(), Name = "TestLga" }
                }
            };
            db.States.Add(state);
            await db.SaveChangesAsync();

            var service = new CustomerService(db, otpMock.Object);

            var dto = new OnboardCustomerDto
            {
                Email = "user@example.com",
                PhoneNumber = "+111111111",
                Password = "password",
                StateId = state.Id,
                LgaId = state.Lgas.First().Id
            };

            var result = await service.OnboardAsync(dto);

            Assert.True(result.Success);
            Assert.Equal("Customer onboarded successfully. OTP sent.", result.Message);
            Assert.NotNull(result.Data);
            otpMock.Verify(m => m.SendOtpAsync(dto.PhoneNumber, It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task VerifyPhoneAsync_ShouldFail_WhenCustomerNotFound()
        {
            using var db = new AppDbContext(_dbOptions);
            var service = new CustomerService(db, new Mock<IOtpSender>().Object);

            var result = await service.VerifyPhoneAsync(new VerifyPhoneDto
            {
                CustomerId = Guid.NewGuid(),
                Otp = "123456"
            });

            Assert.False(result.Success);
            Assert.Equal("Customer not found.", result.Message);
        }

        [Fact]
        public async Task VerifyPhoneAsync_ShouldPass_WhenOtpValid()
        {
            using var db = new AppDbContext(_dbOptions);
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Email = "verify@example.com",
                PhoneNumber = "+222222222",
                PasswordHash = "hash",
                PendingOtp = "654321",
                OtpExpiresAt = DateTime.UtcNow.AddMinutes(5)
            };
            db.Customers.Add(customer);
            await db.SaveChangesAsync();

            var service = new CustomerService(db, new Mock<IOtpSender>().Object);

            var result = await service.VerifyPhoneAsync(new VerifyPhoneDto
            {
                CustomerId = customer.Id,
                Otp = "654321"
            });

            Assert.True(result.Success);
            Assert.True(result.Data);
            Assert.Equal("Phone verified successfully. \r\n Customer onboarding is completed", result.Message);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnCustomers()
        {
            using var db = new AppDbContext(_dbOptions);
            var state = new State { Id = Guid.NewGuid(), Name = "State" };
            var lga = new Lga { Id = Guid.NewGuid(), Name = "LGA", State = state };
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Email = "all@example.com",
                PhoneNumber = "+333333333",
                PasswordHash = "hash",
                State = state,
                Lga = lga
            };
            db.Customers.Add(customer);
            await db.SaveChangesAsync();

            var service = new CustomerService(db, new Mock<IOtpSender>().Object);
            var result = await service.GetAllAsync();

            Assert.True(result.Success);
            Assert.Single(result.Data);
            Assert.Equal("all@example.com", result.Data.First().Email);
        }

        [Fact]
        public async Task SoftDeleteCustomerAsync_ShouldMarkCustomerAsDeleted()
        {
            using var db = new AppDbContext(_dbOptions);
            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Email = "delete@example.com",
                PasswordHash = "hash",
                PhoneNumber = "+444444444"
            };
            db.Customers.Add(customer);
            await db.SaveChangesAsync();

            var service = new CustomerService(db, new Mock<IOtpSender>().Object);
            var result = await service.SoftDeleteCustomerAsync(customer.Id);

            Assert.True(result.Success);
            Assert.True(result.Data);
            var deleted = await db.Customers.FindAsync(customer.Id);
            Assert.True(deleted!.IsDeleted);
        }
    }
}
