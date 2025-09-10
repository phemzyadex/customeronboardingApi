using CustomerOnboarding.Core.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Infrastructure.Services
{
    public class MockOtpSender : IOtpSender
    {
        private readonly ILogger<MockOtpSender> _log;
        public MockOtpSender(ILogger<MockOtpSender> log) => _log = log;

        public Task SendOtpAsync(string phoneNumber, string otp)
        {
            // Mock sending — in dev we log to console. Production: integrate SMS provider.
            _log.LogInformation("MOCK SMS to {Phone}: OTP={Otp}", phoneNumber, otp);
            Console.WriteLine($"MOCK SMS to {phoneNumber}: OTP = {otp}");
            return Task.CompletedTask;
        }
    }

}
