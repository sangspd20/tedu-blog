using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TeduBlog.Core.Models.Content;
using TeduBlog.Core.Models;
using TeduBlog.Core.SeedWorks;
using TeduBlog.Core.Domain.Content;
using Microsoft.AspNetCore.Authorization;
using static TeduBlog.Core.SeedWorks.Constants.Permissions;
//using TeduBlog.Api.Extensions;
using Microsoft.AspNetCore.Identity;
using TeduBlog.Core.Domain.Identity;
//using TeduBlog.Core.Helpers;

namespace TeduBlog.Api.Controllers.AdminApi
{
    [Route("api/admin/post")]
    public class PostController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        public PostController(IUnitOfWork unitOfWork, IMapper mapper, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
        }


        [HttpGet]
        [Route("paging")]
        //[Authorize(Posts.View)]
        public async Task<ActionResult<PagedResult<PostInListDto>>> GetPostsPaging(string? keyword, Guid? categoryId,
            int pageIndex, int pageSize = 10)
        {
            //var userId = User.GetUserId();
            var result = await _unitOfWork.Posts.GetAllPaging(keyword, Guid.NewGuid(), categoryId, pageIndex, pageSize);
            return Ok(result);
        }

    }
}
