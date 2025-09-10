using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Core.DTOs.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerOnboarding.Core.Interfaces
{
    public interface ILgaService
    {
        Task<LgaResponseDto> AddLgaAsync(LgaDto dto);
        Task<List<LgaResponseDto>> GetLgasByStateAsync(Guid stateId);
    }
}