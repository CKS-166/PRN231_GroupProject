using FPTU_Starter.API.Exception;
using FPTU_Starter.Application.IRepository;
using FPTU_Starter.Application.Services.IService;
using FPTU_Starter.Application.ViewModel.InteractionDTO;
using FPTU_Starter.Domain.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FPTU_Starter.API.Controllers
{
    [Route("api/interactions")]
    [ApiController]
    public class InteractionController : ControllerBase
    {
        private readonly IInteractionService _interactionService;

        public InteractionController(IInteractionService interactionService)
        {
            _interactionService = interactionService;
        }
        [HttpGet("get-all-liked-projects")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _interactionService.GetAll();
            return Ok(result);
        }
        [HttpGet("get-all-comment-projects")]
        public async Task<IActionResult> AllCommentProjects()
        {
            var result = await _interactionService.GetAllComment();
            return Ok(result);
        }

        [HttpPost("like-project")]
        public async Task<IActionResult> likeProject([FromBody] LikeRequest likeRequest)
        {
            var result = await _interactionService.LikeProject(likeRequest);
            if (!result._isSuccess)
            {
                return BadRequest(result._message);
            }
            return Ok(result);
        }

        [HttpPost("comment-project")]
        public async Task<IActionResult> commentProject([FromBody] CommentRequest request)
        {
            var result = await _interactionService.CommentProject(request);
            if (!result._isSuccess)
            {
                return BadRequest(result._message);
            }
            return Ok(result);
        }
        [HttpGet("get-like/{id}")]
        public async Task<IActionResult> GetProjectLike(Guid id)
        {
            try
            {
                var result = _interactionService.GetLikesByProject(id);
                return Ok(result);  
            }catch(ExceptionError ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("get-comments/{id}")]
        public async Task<IActionResult> GetProjecComment(Guid id)
        {
            try
            {
                var result = _interactionService.GetCommentsByProject(id);
                return Ok(result);
            }
            catch (ExceptionError ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("check-user-like/{id}")]
        [Authorize]
        public async Task<IActionResult> CheckUserLike(Guid id)
        {
            try
            {
                var result = _interactionService.CheckUserLike(id);
                return Ok(result);
            }
            catch (ExceptionError ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
