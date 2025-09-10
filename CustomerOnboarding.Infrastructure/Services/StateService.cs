using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Core.DTOs.Responses;
using CustomerOnboarding.Core.Interfaces;
using CustomerOnboarding.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CustomerOnboarding.Infrastructure.Services
{
    public class StateService: IStateService
    {
        private readonly AppDbContext _db;

        public StateService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<StateResponseDto> AddStateAsync(string name)
        {
            var exists = await _db.States.AnyAsync(s => s.Name.ToLower() == name.ToLower());
            if (exists)
                throw new InvalidOperationException($"State '{name}' already exists.");

            var state = new State { Name = name };
            _db.States.Add(state);
            await _db.SaveChangesAsync();

            return new StateResponseDto
            {
                Id = state.Id,
                Name = state.Name,
                Lgas = new List<LgaResponseOnly>()
            };
        }

        public async Task<List<StateResponseDto>> GetAllStatesAsync()
        {
            return await _db.States
                .Include(s => s.Lgas)
                .Select(s => new StateResponseDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Lgas = s.Lgas.Select(l => new LgaResponseOnly
                    {
                        Id = l.Id,
                        Name = l.Name
                    }).ToList()
                })
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
