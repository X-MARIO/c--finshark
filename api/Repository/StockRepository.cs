﻿using api.Data;
using api.Dtos.Stock;
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

    public async Task<List<Stock>> GetAllAsync()
    {
        return await _context.Stocks.Include(c => c.Comments).ToListAsync();
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(i => i.Id == id);
    }

    public async Task<Stock> CreateAsync(Stock stockModel)
    {
        await _context.Stocks.AddAsync(stockModel);
        await _context.SaveChangesAsync();
        return stockModel;
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateDto)
    {
        var existingStock = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);
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
        var stockModel = await _context.Stocks.FirstOrDefaultAsync(x => x.Id == id);

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