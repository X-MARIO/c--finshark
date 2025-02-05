using api.Data;
using api.Interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDbContext _context;

    public StockRepository(ApplicationDbContext applicationDbContext)
    {
        _context = applicationDbContext;
    }
        
    public Task<List<Stock>> GetAllAsync()
    {
        return _context.Stocks.ToListAsync();
    }
}