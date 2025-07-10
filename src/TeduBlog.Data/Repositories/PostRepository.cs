
using Microsoft.AspNetCore.Identity;
using TeduBlog.Core.Domain.Content;
using TeduBlog.Core.Domain.Identity;

using TeduBlog.Core.Repositories;

using TeduBlog.Data.SeedWorks;

namespace TeduBlog.Data.Repositories
{
    public class PostRepository : RepositoryBase<Post, Guid>, IPostRepository
    {
        //private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        public PostRepository(TeduBlogContext context,
            UserManager<AppUser> userManager) : base(context)
        {
            _userManager = userManager;
        }

        public async Task<List<Post>> GetListUnpaidPublishPosts(Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
