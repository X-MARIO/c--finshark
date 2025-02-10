using api.Models;

namespace api.Interfaces;

public interface IPortfolioRepository
{
    Task<List<Stock>> GetPortfolios(AppUser user);
    Task<Portfolio> CreateAsync(Portfolio portfolio);
}