using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Abp.NServiceBus.Blogs.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus.Blogs
{
    public interface IBlogAppService : IApplicationService
    {
        Task<ListResultDto<BlogDto>> GetBlogs(GetBlogsInput input);

        Task<BlogDto> GetBlog(EntityDto input);

        Task CreateBlog(BlogDto input);

        Task UpdateBlog(BlogDto input);

        Task DeleteBlog(BlogDto input);
    }
}
