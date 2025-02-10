using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class PortfolioRepository : IPortfolioRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    public PortfolioRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<List<Stock>> GetUserPortfolio(AppUser user)
    {
        return await _applicationDbContext.Portfolios.Where(u => u.AppUserId == user.Id)
            .Select(stock => new Stock
            {
                Id = stock.StockId,
                Symbol = stock.Stock.Symbol,
                CompanyName = stock.Stock.CompanyName,
                Purchase = stock.Stock.Purchase,
                Industry = stock.Stock.Industry,
                MarketCap = stock.Stock.MarketCap,
            }).ToListAsync();
    }

    public async Task<Portfolio> CreateAsync(Portfolio portfolio)
    {
        await _applicationDbContext.Portfolios.AddAsync(portfolio);
        await _applicationDbContext.SaveChangesAsync();
        return portfolio;
    }

    public async Task<Portfolio?> DeletePortfolio(AppUser appUser, string symbol)
    {
        var portfolioModel = await _applicationDbContext.Portfolios.FirstOrDefaultAsync(x => x.AppUserId == appUser.Id && x.Stock.Symbol.ToLower() == symbol.ToLower());

        if (portfolioModel == null) return null;
        _applicationDbContext.Portfolios.Remove(portfolioModel);
        await _applicationDbContext.SaveChangesAsync();

        return portfolioModel;
    }
}