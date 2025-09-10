using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Core.Interfaces;
using CustomerOnboarding.Core.Models;
using CustomerOnboarding.Infrastructure.Data;
using CustomerOnboarding.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CustomerOnboarding.Tests
{
    public class CustomerServiceTests
    {
        [Fact]
        public async Task Onboard_GeneratesOtp_And_SavesCustomer()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            using var ctx = new AppDbContext(options);
            // seed state + lga
            var state = new State { Id = Guid.NewGuid(), Name = "Test State" };
            var lga = new Lga { Id = Guid.NewGuid(), Name = "Test LGA", StateId = state.Id };
            ctx.States.Add(state);
            ctx.Lgas.Add(lga);
            await ctx.SaveChangesAsync();

            var mockOtpSender = new Mock<IOtpSender>();
            var svc = new CustomerService(ctx, mockOtpSender.Object);

            var dto = new OnboardCustomerDto { Email = "phemyadex@yahoo.com", PhoneNumber = "+2348069419299", StateId = state.Id, LgaId = lga.Id };
            var id = await svc.OnboardAsync(dto);

            var saved = await ctx.Customers.FindAsync(id);
            Assert.NotNull(saved);
            Assert.False(saved.IsPhoneVerified);
            Assert.NotNull(saved.PendingOtp);
            mockOtpSender.Verify(m => m.SendOtpAsync(saved.PhoneNumber, saved.PendingOtp), Times.Once);
        }

        [Fact]
        public async Task VerifyPhone_Succeeds_With_CorrectOtp()
        {
            // setup, create customer with OTP
            var options = new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            using var ctx = new AppDbContext(options);
            var state = new State { Id = Guid.NewGuid(), Name = "S1" };
            var lga = new Lga { Id = Guid.NewGuid(), Name = "L1", StateId = state.Id };
            ctx.States.Add(state);
            ctx.Lgas.Add(lga);
            var customer = new Customer { Id = Guid.NewGuid(), Email = "a@b.com", PhoneNumber = "0801", StateId = state.Id, LgaId = lga.Id, PendingOtp = "123456", OtpExpiresAt = DateTime.UtcNow.AddMinutes(5) };
            ctx.Customers.Add(customer);
            await ctx.SaveChangesAsync();

            var svc = new CustomerService(ctx, Mock.Of<IOtpSender>());
            var ok = await svc.VerifyPhoneAsync(new VerifyPhoneDto { CustomerId = customer.Id, Otp = "123456" });
            Assert.True(ok);
            var reloaded = await ctx.Customers.FindAsync(customer.Id);
            Assert.True(reloaded.IsPhoneVerified);
        }
    }

}
