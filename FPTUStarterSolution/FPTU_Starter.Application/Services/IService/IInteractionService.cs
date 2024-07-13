using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.CommentDTO;
using FPTU_Starter.Application.ViewModel.InteractionDTO;
using FPTU_Starter.Domain.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FPTU_Starter.Application.Services.IService
{
    public interface IInteractionService
    {
        Task<List<Like>> GetAll();
        Task<List<CommentViewResponse>> GetAllComment();
        Task<ResultDTO<LikeResponse>> LikeProject(LikeRequest likeRequest);
        Task<ResultDTO<Comment>> CommentProject(CommentRequest request);
        Task<List<CommentViewResponse>> GetCommentsByProject(Guid id);
        Task<List<Like>> GetLikesByProject(Guid id);
        Task<List<Like>> CheckUserLike(Guid id);
    }
}
