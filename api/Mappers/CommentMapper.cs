﻿using api.Dtos.Comment;
using api.Models;

namespace api.Mappers;

public static class CommentMapper
{
    public static CommentDto ToCommentDto(this Comment comment)
    {
        return new CommentDto()
        {
            Id = comment.Id,
            Title = comment.Title,
            Content = comment.Content,
            CreatedOn = comment.CreatedOn,
            StockId = comment.StockId,
        };
    }

    public static Comment ToCommentFromCreate(this CreateCommentDto createDto, int stockId)
    {
        return new Comment()
        {
            Title = createDto.Title,
            Content = createDto.Content,
            StockId = stockId,
        };
    }
}