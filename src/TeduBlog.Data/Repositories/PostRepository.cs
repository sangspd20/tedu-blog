
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TeduBlog.Core.Domain.Content;
using TeduBlog.Core.Domain.Identity;
using TeduBlog.Core.Models;
using TeduBlog.Core.Models.Content;
using TeduBlog.Core.Repositories;

using TeduBlog.Data.SeedWorks;

namespace TeduBlog.Data.Repositories
{
    public class PostRepository : RepositoryBase<Post, Guid>, IPostRepository
    {
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public PostRepository(TeduBlogContext context, IMapper mapper,
            UserManager<AppUser> userManager) : base(context)
        {
            _userManager = userManager;
            _mapper = mapper;
        }

        public async Task<PagedResult<PostInListDto>> GetAllPaging(string? keyword, Guid currentUserId, Guid? categoryId, int pageIndex = 1, int pageSize = 10)
        {
            var user = await _userManager.FindByIdAsync(currentUserId.ToString());
            if (user == null)
            {
                throw new Exception("Không tồn tại user");
            }
            var roles = await _userManager.GetRolesAsync(user);
            var canApprove = false;
            //if (roles.Contains(Roles.Admin))
            //{
            //    canApprove = true;
            //}
            //else
            //{
            //    canApprove = await _context.RoleClaims.AnyAsync(x => roles.Contains(x.RoleId.ToString())
            //               && x.ClaimValue == Permissions.Posts.Approve);
            //}

            var query = _context.Posts.AsQueryable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(x => x.Name.Contains(keyword));
            }
            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            if (!canApprove)
            {
                query = query.Where(x => x.AuthorUserId == currentUserId);
            }

            var totalRow = await query.CountAsync();

            query = query.OrderByDescending(x => x.DateCreated)
               .Skip((pageIndex - 1) * pageSize)
               .Take(pageSize);

            return new PagedResult<PostInListDto>
            {
                Results = await _mapper.ProjectTo<PostInListDto>(query).ToListAsync(),
                CurrentPage = pageIndex,
                RowCount = totalRow,
                PageSize = pageSize
            };

        }

        public async Task<List<Post>> GetListUnpaidPublishPosts(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
