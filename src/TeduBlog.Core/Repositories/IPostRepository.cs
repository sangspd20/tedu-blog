using TeduBlog.Core.Domain.Content;
using TeduBlog.Core.Models;
using TeduBlog.Core.Models.Content;


//using TeduBlog.Core.Models;
//using TeduBlog.Core.Models.Content;
using TeduBlog.Core.SeedWorks;

namespace TeduBlog.Core.Repositories
{
    public interface IPostRepository : IRepository<Post,Guid>
    {
        Task<List<Post>> GetListUnpaidPublishPosts(Guid userId);
        Task<PagedResult<PostInListDto>> GetAllPaging(string? keyword, Guid currentUserId, Guid? categoryId, int pageIndex = 1, int pageSize = 10);



    }
}
