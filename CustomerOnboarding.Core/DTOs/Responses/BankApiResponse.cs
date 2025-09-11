using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Core.DTOs.Responses
{
    public class BankApiResponse
    {
        public IEnumerable<BankDto>? Result { get; set; }
        public string? ErrorMessage { get; set; }
        public IEnumerable<string>? ErrorMessages { get; set; }
        public bool HasError { get; set; }
        public DateTime TimeGenerated { get; set; }
    }
}
