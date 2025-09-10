using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Core.DTOs.Responses
{
    public class LgaResponseDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public Guid StateId { get; set; }
        public string StateName { get; set; } = null!;
    }
}
