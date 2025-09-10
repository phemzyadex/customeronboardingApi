using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Core.Interfaces
{
    public interface IOtpSender
    {
        Task SendOtpAsync(string phoneNumber, string otp); // mocked
    }
}
