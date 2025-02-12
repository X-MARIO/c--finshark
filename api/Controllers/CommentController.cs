﻿using api.Dtos.Comment;
using api.Interfaces;
using api.Mappers;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[Route("api/comment")]
[ApiController]
public class CommentController : ControllerBase
{
    private readonly ICommentRepository _commentRepository;
    private readonly IStockRepository _stockRepository;
    private readonly UserManager<AppUser> _userManager;

    public CommentController(ICommentRepository commentRepository, IStockRepository stockRepository,
        UserManager<AppUser> userManager)
    {
        _commentRepository = commentRepository;
        _stockRepository = stockRepository;
        _userManager = userManager;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var comments = await _commentRepository.GetAllComments();

        var commentDto = comments.Select(s => s.ToCommentDto());
        return Ok(commentDto);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var comment = await _commentRepository.GetByIdAsync(id);

        if (comment == null)
        {
            return NotFound();
        }

        return Ok(comment.ToCommentDto());
    }

    [HttpPost("{stockId:int}")]
    public async Task<IActionResult> Create([FromRoute] int stockId, [FromBody] CreateCommentDto createDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (!(await _stockRepository.StockExists(stockId)))
        {
            return NotFound("Stock does not exist");
        }
        
        var userName = _userManager.GetUserName(User);
        var appUser = await _userManager.FindByNameAsync(userName);

        var commentModel = createDto.ToCommentFromCreateDto(stockId);
        commentModel.AppUserId = appUser.Id;
        await _commentRepository.CreateAsync(commentModel);
        return CreatedAtAction(nameof(GetById), new { id = commentModel.Id }, commentModel.ToCommentDto());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateCommentDto updateDto)
    {
        var comment = await _commentRepository.UpdateAsync(id, updateDto.ToCommentFromUpdateDto());

        if (comment == null)
        {
            return NotFound("Comment not found");
        }

        return Ok(comment.ToCommentDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var commentModel = await _commentRepository.DeleteAsync(id);

        if (commentModel == null)
        {
            return NotFound("Comment not found");
        }

        return Ok(commentModel.ToCommentDto());
    }
}