﻿using api.Dtos.Stock;
using api.Models;

namespace api.Interfaces;

public interface IStockRepository
{
    Task<List<Stock>> GetAllAsync();

    Task<Stock?> GetByIdAsync(int id);

    Task<Stock> CreateAsync(Stock stockDto);

    Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateDto);

    Task<Stock?> DeleteAsync(int id);

    Task<bool> StockExists(int id);
}