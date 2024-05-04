using AutoMapper;
using CaptureIt.DTOs.Comment;
using CaptureIt.Models;
using CaptureIt.Repos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CaptureIt.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository commentRepository, IMapper mapper)
        {
            _commentRepository = commentRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CommentResponse>> GetAll()
        {
            var comments = await _commentRepository.GetAll();
            return _mapper.Map<IEnumerable<CommentResponse>>(comments);
        }

        public async Task<CommentResponse> GetById(int id)
        {
            var comment = await _commentRepository.GetById(id);
            return _mapper.Map<CommentResponse>(comment);
        }

        public async Task<CommentResponse> Add(CommentRequest commentRequest)
        {
            var comment = _mapper.Map<Comment>(commentRequest);
            await _commentRepository.Add(comment);
            return _mapper.Map<CommentResponse>(comment);
        }

        public async Task<CommentResponse> Update(int id, CommentRequest commentRequest)
        {
            var comment = await _commentRepository.GetById(id);
            if (comment == null)
            {
                return null;
            }

            _mapper.Map(commentRequest, comment);

            await _commentRepository.Update(comment);
            return _mapper.Map<CommentResponse>(comment);
        }

        public async Task<bool> Delete(int id)
        {
            var comment = await _commentRepository.GetById(id);
            if (comment == null)
            {
                return false;
            }

            await _commentRepository.Delete(id);
            return true;
        }
    }
}

