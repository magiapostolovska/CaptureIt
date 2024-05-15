
using AutoMapper;
using CaptureIt.DTOs.Comment;
using CaptureIt.Models;

namespace CaptureIt.AutoMapper
{
    public class CommentMapper : Profile
    {
        public CommentMapper() 
        {
            CreateMap<CommentRequest, Comment>();
            CreateMap<Comment, CommentResponse>();
            CreateMap<CommentUpdate, Comment>();
        }

    }
}
