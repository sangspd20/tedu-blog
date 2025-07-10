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

        [HttpPost]
        //[Authorize(Posts.Create)]
        public async Task<IActionResult> CreatePost([FromBody] CreateUpdatePostRequest request)
        {
            if (await _unitOfWork.Posts.IsSlugAlreadyExisted(request.Slug))
            {
                return BadRequest("Đã tồn tại slug");
            }
            var post = _mapper.Map<CreateUpdatePostRequest, Post>(request);
            var postId = Guid.NewGuid();
            //var category = await _unitOfWork.PostCategories.GetByIdAsync(request.CategoryId);
            post.Id = postId;
            //post.CategoryName = category.Name;
            //post.CategorySlug = category.Slug;

            //var userId = User.GetUserId();
            //var user = await _userManager.FindByIdAsync(userId.ToString());
            //post.AuthorUserId = userId;
           // post.AuthorName = user.GetFullName();
            //post.AuthorUserName = user.UserName;
            _unitOfWork.Posts.Add(post);

            //Process tag
            //if (request.Tags != null && request.Tags.Length > 0)
            //{
            //    foreach (var tagName in request.Tags)
            //    {
            //        var tagSlug = TextHelper.ToUnsignedString(tagName);
            //        var tag = await _unitOfWork.Tags.GetBySlug(tagSlug);
            //        Guid tagId;
            //        if (tag == null)
            //        {
            //            tagId = Guid.NewGuid();
            //            _unitOfWork.Tags.Add(new Tag() { Id = tagId, Name = tagName, Slug = tagSlug });

            //        }
            //        else
            //        {
            //            tagId = tag.Id;
            //        }
            //        await _unitOfWork.Posts.AddTagToPost(postId, tagId);
            //    }
            //}

            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }

        [HttpPut]
        //[Authorize(Posts.Edit)]
        public async Task<IActionResult> UpdatePost(Guid id, [FromBody] CreateUpdatePostRequest request)
        {
            if (await _unitOfWork.Posts.IsSlugAlreadyExisted(request.Slug, id))
            {
                return BadRequest("Đã tồn tại slug");
            }
            var post = await _unitOfWork.Posts.GetByIdAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            //if (post.CategoryId != request.CategoryId)
            //{
            //    var category = await _unitOfWork.PostCategories.GetByIdAsync(request.CategoryId);
            //    post.CategoryName = category.Name;
            //    post.CategorySlug = category.Slug;
            //}
            _mapper.Map(request, post);

            //Process tag
            //if (request.Tags != null && request.Tags.Length > 0)
            //{
            //    foreach (var tagName in request.Tags)
            //    {
            //        var tagSlug = TextHelper.ToUnsignedString(tagName);
            //        var tag = await _unitOfWork.Tags.GetBySlug(tagSlug);
            //        Guid tagId;
            //        if (tag == null)
            //        {
            //            tagId = Guid.NewGuid();
            //            _unitOfWork.Tags.Add(new Tag() { Id = tagId, Name = tagName, Slug = tagSlug });

            //        }
            //        else
            //        {
            //            tagId = tag.Id;
            //        }
            //        await _unitOfWork.Posts.AddTagToPost(id, tagId);

            //    }
            //}
            await _unitOfWork.CompleteAsync();

            return Ok();
        }

        [HttpDelete]
        [Authorize(Posts.Delete)]
        public async Task<IActionResult> DeletePosts([FromQuery] Guid[] ids)
        {
            foreach (var id in ids)
            {
                var post = await _unitOfWork.Posts.GetByIdAsync(id);
                if (post == null)
                {
                    return NotFound();
                }
                _unitOfWork.Posts.Remove(post);
            }
            var result = await _unitOfWork.CompleteAsync();
            return result > 0 ? Ok() : BadRequest();
        }


    }
}
