using AutoMapper;
using FPTU_Starter.Application.IRepository;
using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel;
using FPTU_Starter.Application.ViewModel.CommentDTO;
using FPTU_Starter.Application.ViewModel.InteractionDTO;
using FPTU_Starter.Domain.Entity;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FPTU_Starter.Application.Services
{
    public class InteractionService : IInteractionService
    {
        private readonly ILikeRepository _likeRepository;
        private readonly ICommentRepository _commentRepository;
        private readonly IUserManagementService _userManagementService;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public InteractionService(ILikeRepository likeRepository,
            ICommentRepository commentRepository,
            IUserManagementService userManagementService,
            IMapper mapper,
            IUnitOfWork unitOfWork)
        {
            _likeRepository = likeRepository;
            _commentRepository = commentRepository;
            _userManagementService = userManagementService;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Like>> CheckUserLike(Guid id)
        {
            try
            {
                var user = _userManagementService.GetUserInfo().Result;
                ApplicationUser exitUser = _mapper.Map<ApplicationUser>(user._data);
                var list = _likeRepository.GetAll().Where(l => l.ProjectId == id && l.UserId.ToString() == exitUser.Id);
                return list.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<ResultDTO<Comment>> CommentProject(CommentRequest request)
        {
            try
            {
                var user = _userManagementService.GetUserInfo().Result;
                ApplicationUser exitUser = _mapper.Map<ApplicationUser>(user._data);
                var project = await _unitOfWork.ProjectRepository.GetAsync(x => x.Id.Equals(request.ProjectId));
                if (user is null)
                {
                    return ResultDTO<Comment>.Fail("User is null");
                }
                if (project is null)
                {
                    return ResultDTO<Comment>.Fail("Project can not found");
                }

                    // add new comment
                    Comment newComment = new Comment
                    {
                        Id = Guid.NewGuid(),
                        Content = request.Content,
                        CreateDate = DateTime.Now,
                        ProjectId = project.Id,
                        UserID = Guid.Parse(exitUser.Id),
                    };
                    _commentRepository.Create(newComment);
                    return ResultDTO<Comment>.Success(newComment, "Successfully Add Comment");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        public async Task<List<Like>> GetAll()
        {
            try
            {
                var list = _likeRepository.GetAll();
                return list.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }
        }

        public async Task<List<CommentViewResponse>> GetAllComment()
        {
            try
            {
                var list = _commentRepository.GetAll();

                List<CommentViewResponse> comments = new List<CommentViewResponse>();

                foreach (var comment in list)
                {
                    var user = _unitOfWork.UserRepository.GetById(comment.UserID.ToString());

                    comments.Add(new CommentViewResponse
                    {
                        Content = comment.Content,
                        CreateDate = comment.CreateDate,
                        UserName = user?.AccountName,
                        AvatarUrl = user?.Avatar
                    });
                }

                return comments;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }
        }

        public async Task<List<CommentViewResponse>> GetCommentsByProject(Guid id)
        {
            try
            {
                var list = _commentRepository.GetAll().Where(c => c.ProjectId == id);
                List<CommentViewResponse> comments = new List<CommentViewResponse>();
                foreach (var comment in list)
                {
                    var user = _unitOfWork.UserRepository.GetById(comment.UserID.ToString());

                    comments.Add(new CommentViewResponse
                    {
                        Content = comment.Content,
                        CreateDate = comment.CreateDate,
                        UserName = user?.AccountName,
                        AvatarUrl = user?.Avatar
                    });
                }

                return comments;
                return comments;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }
        }

        public async Task<List<Like>> GetLikesByProject(Guid id)
        {
            try
            {
                var list = _likeRepository.GetAll().Where(l => l.ProjectId == id);
                return list.ToList();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);

            }
        }

        public async Task<ResultDTO<LikeResponse>> LikeProject(LikeRequest likeRequest)
        {
            try
            {
                var user = _userManagementService.GetUserInfo().Result;
                ApplicationUser exitUser = _mapper.Map<ApplicationUser>(user._data);
                var project = await _unitOfWork.ProjectRepository.GetAsync(x => x.Id.Equals(likeRequest.ProjectId));
                if (user is null)
                {
                    return ResultDTO<LikeResponse>.Fail("User is null");
                }
                if (project is null)
                {
                    return ResultDTO<LikeResponse>.Fail("Project can not found");
                }
                //check if the user and project already liked 
                var getLikedProjects = _likeRepository.GetAsync(x => x.ProjectId.Equals(project.Id) && x.UserId.Equals(Guid.Parse(exitUser.Id)));
                if (getLikedProjects == null)
                {
                    //liked a project
                    Like newLikeProject = new Like
                    {
                        ProjectId = likeRequest.ProjectId,
                        UserId = Guid.Parse(exitUser.Id),
                        IsLike = true,
                        CreateDate = DateTime.Now,
                        Id = Guid.NewGuid(),
                    };
                    _likeRepository.Create(newLikeProject);
                    return ResultDTO<LikeResponse>.Success(new LikeResponse { ProjectId = newLikeProject.ProjectId, UserID = newLikeProject.UserId }, "Succesfull like the project");
                }
                else
                {
                    if (getLikedProjects.IsLike) //isLike == true ? "dislike" : "liked"
                    {
                        _likeRepository.Remove(x => x.Id.Equals(getLikedProjects.Id));
                        return ResultDTO<LikeResponse>.Success(new LikeResponse { ProjectId = likeRequest.ProjectId, UserID = Guid.Parse(exitUser.Id) }, "Succesfull dislike the project");
                    }
                }

                return ResultDTO<LikeResponse>.Fail("some thing wrong : error ");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    }
}
