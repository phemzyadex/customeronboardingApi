using CustomerOnboarding.Core.DTOs;
using CustomerOnboarding.Core.DTOs.Responses;
using CustomerOnboarding.Core.Models;
using CustomerOnboarding.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using CustomerOnboarding.Core.Interfaces;

namespace CustomerOnboarding.Infrastructure.Services
{
    public class LgaService: ILgaService
    {
        private readonly AppDbContext _db;

        public LgaService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<LgaResponseDto> AddLgaAsync(LgaDto dto)
        {
            // Check if parent state exists
            var state = await _db.States.FirstOrDefaultAsync(s => s.Id == dto.StateId);
            if (state == null)
                throw new InvalidOperationException("Invalid state ID.");

            // Check duplicate (case-insensitive within same state)
            var duplicate = await _db.Lgas.AnyAsync(l =>
                l.StateId == dto.StateId &&
                l.Name.ToLower() == dto.Name.ToLower());

            if (duplicate)
                throw new InvalidOperationException($"LGA '{dto.Name}' already exists in state '{state.Name}'.");

            // Create LGA
            var lga = new Lga
            {
                Name = dto.Name,
                StateId = dto.StateId
            };

            _db.Lgas.Add(lga);
            await _db.SaveChangesAsync();

            // Return DTO with State + LGA info
            return new LgaResponseDto
            {
                Id = lga.Id,
                Name = lga.Name,
                StateId = state.Id,
                StateName = state.Name
            };
        }



        public async Task<List<LgaResponseDto>> GetLgasByStateAsync(Guid stateId)
        {
            return await _db.Lgas
                .Where(l => l.StateId == stateId)
                .Include(l => l.State)  
                .Select(l => new LgaResponseDto
                {
                    Id = l.Id,
                    Name = l.Name,
                    StateId = l.StateId,
                    StateName = l.State.Name
                })
                .AsNoTracking()
                .ToListAsync();
        }
    }
}