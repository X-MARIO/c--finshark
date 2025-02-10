using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _context;

    public StockRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Stock>> GetAllAsync(QueryObject query)
    {
        var stocks = _context.Stocks
            .Include(s => s.Comments)
            .ThenInclude(a => a.AppUser)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.CompanyName))
        {
            stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
        }

        if (!string.IsNullOrWhiteSpace(query.Symbol))
        {
            stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
        }

        if (!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if (query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.IsDecsending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
            }
        }
        
        var skipNumber = (query.PageNumber - 1) * query.PageSize;

        return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await _context.Stocks.Include(s => s.Comments).FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Stock?> GetBySymbolAsync(string symbol)
    {
        return await _context.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
    }

    public async Task<Stock> CreateAsync(Stock stockModel)
    {
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockDto updateDto)
    {
        var existingStock = await _context.Stocks.FindAsync(id);

        if (existingStock == null)
        {
            return null;
        }

        existingStock.Symbol = updateDto.Symbol;
        existingStock.CompanyName = updateDto.CompanyName;
        existingStock.Purchase = updateDto.Purchase;
        existingStock.LastDiv = updateDto.LastDiv;
        existingStock.Industry = updateDto.Industry;
        existingStock.MarketCap = updateDto.MarketCap;

        await _context.SaveChangesAsync();

        return existingStock;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(s => s.Id == id);

        if (stockModel == null)
        {
            return null;
        }

        _context.Stocks.Remove(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<bool> StockExists(int id)
    {
        return await _context.Stocks.AnyAsync(s => s.Id == id);
    }
}