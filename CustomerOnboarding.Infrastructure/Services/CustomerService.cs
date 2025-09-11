using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Core.DTOs.Responses;
using CustomerOnboarding.Core.Interfaces;
using CustomerOnboarding.Core.Models;
using CustomerOnboarding.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerOnboarding.Infrastructure.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly AppDbContext _db;
        private readonly IOtpSender _otpSender;

        public CustomerService(AppDbContext db, IOtpSender otpSender)
        {
            _db = db;
            _otpSender = otpSender;
        }

        public async Task<ApiResponse<CustomerResponse>> OnboardAsync(OnboardCustomerDto dto)
        {
            try
            {
                var state = await _db.States.Include(s => s.Lgas)
                    .FirstOrDefaultAsync(s => s.Id == dto.StateId);
                if (state == null)
                    return ApiResponse<CustomerResponse>.Fail("Invalid state ID.", null);

                var lga = state.Lgas.FirstOrDefault(l => l.Id == dto.LgaId);
                if (lga == null)
                    return ApiResponse<CustomerResponse>.Fail("Selected LGA does not belong to the chosen State.", null);

                if (await _db.Customers.AnyAsync(c => c.PhoneNumber == dto.PhoneNumber))
                    return ApiResponse<CustomerResponse>.Fail("Customer with this phone already exists.", null);

                if (await _db.Customers.AnyAsync(c => c.Email == dto.Email))
                    return ApiResponse<CustomerResponse>.Fail("Customer with this email already exists.", null);

                var customer = new Customer
                {
                    Email = dto.Email,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                    PhoneNumber = dto.PhoneNumber,
                    StateId = dto.StateId,
                    LgaId = dto.LgaId,
                    IsPhoneVerified = false,
                    OnboardingCompleted = false,
                    CreatedAt = DateTime.UtcNow
                };

                // Generate OTP (mock)
                var otp = new Random().Next(100000, 999999).ToString();
                customer.PendingOtp = otp;
                customer.OtpExpiresAt = DateTime.UtcNow.AddMinutes(5);

                _db.Customers.Add(customer);
                await _db.SaveChangesAsync();

                await _otpSender.SendOtpAsync(customer.PhoneNumber, otp);

                // Map to DTO (exclude password, include OTP, State, LGA names)
                var customerResponse = new CustomerResponse
                {
                    Id = customer.Id,
                    Email = customer.Email,
                    PhoneNumber = customer.PhoneNumber,
                    IsPhoneVerified = customer.IsPhoneVerified,
                    OnboardingCompleted = customer.OnboardingCompleted,
                    PendingOtp = customer.PendingOtp,
                    OtpExpiresAt = customer.OtpExpiresAt,
                    StateId = customer.StateId,
                    StateName = state.Name,
                    LgaId = customer.LgaId,
                    LgaName = lga.Name,
                    CreatedAt = customer.CreatedAt
                };
                return ApiResponse<CustomerResponse>.Ok(customerResponse, "Customer onboarded successfully. OTP sent.");
            }
            catch (Exception ex)
            {
                return ApiResponse<CustomerResponse>.Fail($"Error while onboarding customer: {ex.Message}", null);
            }
        }

        public async Task<ApiResponse<bool>> VerifyPhoneAsync(VerifyPhoneDto dto)
        {
            try
            {
                var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == dto.CustomerId);
                if (customer == null)
                    return ApiResponse<bool>.Fail("Customer not found.", null);

                if (customer.OtpExpiresAt == null || customer.OtpExpiresAt < DateTime.UtcNow)
                    return ApiResponse<bool>.Fail("OTP expired.", null);

                if (customer.PendingOtp != dto.Otp)
                    return ApiResponse<bool>.Fail("Invalid OTP.", null);

                customer.IsPhoneVerified = true;
                customer.OnboardingCompleted = true;
                customer.PendingOtp = null;
                customer.OtpExpiresAt = null;

                await _db.SaveChangesAsync();
                return ApiResponse<bool>.Ok(true, ("Phone verified successfully. Customer onboarding is completed"));
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Error verifying phone: {ex.Message}", null);
            }
        }

        public async Task<ApiResponse<IEnumerable<CustomerDto>>> GetAllAsync()
        {
            try
            {
                var customers = await _db.Customers
                    .Include(c => c.State)
                    .Include(c => c.Lga)
                    .Select(c => new CustomerDto
                    {
                        Id = c.Id,
                        Email = c.Email,
                        PhoneNumber = c.PhoneNumber,
                        IsPhoneVerified = c.IsPhoneVerified,
                        OnboardingCompleted = c.OnboardingCompleted,
                        State = c.State.Name,
                        Lga = c.Lga.Name
                    })
                    .ToListAsync();

                return ApiResponse<IEnumerable<CustomerDto>>.Ok(customers, "Customers retrieved successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<IEnumerable<CustomerDto>>.Fail($"Error fetching customers: {ex.Message}", null);
            }
        }

        public async Task<ApiResponse<bool>> SoftDeleteCustomerAsync(Guid customerId)
        {
            try
            {
                var customer = await _db.Customers.FirstOrDefaultAsync(c => c.Id == customerId);
                if (customer == null)
                    return ApiResponse<bool>.Fail("Customer not found.", null);

                customer.IsDeleted = true;
                customer.DeletedAt = DateTime.UtcNow;
                await _db.SaveChangesAsync();

                return ApiResponse<bool>.Ok(true, "Customer deleted successfully.");
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail($"Error deleting customer: {ex.Message}", null);
            }
        }
    }
}
