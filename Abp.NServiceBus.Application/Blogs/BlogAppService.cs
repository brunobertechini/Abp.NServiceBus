using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.NServiceBus.Blogs.Dto;
using Abp.Domain.Repositories;
using NServiceBus;
using Abp.AutoMapper;
using Abp.NServiceBus.Blogs.Commands;
using Abp.Domain.Uow;

namespace Abp.NServiceBus.Blogs
{
    public class BlogAppService : NServiceBusAppServiceBase, IBlogAppService
    {
        private readonly IEndpointInstance _endpointInstance;
        private readonly IRepository<Blog> _blogRepository;
        private readonly IUnitOfWorkManager _uowManager;
        
        public BlogAppService(
            IUnitOfWorkManager uowManager,
            IEndpointInstance endpointInstance,
            IRepository<Blog> blogRepository
        )
        {
            _uowManager = uowManager;
            _endpointInstance = endpointInstance;
            _blogRepository = blogRepository;
        }

        public async Task CreateBlog(BlogDto input)
        {
            var blog = input.MapTo<Blog>();
            await _blogRepository.InsertAsync(blog);

            // Abp Standard: To get Entity<int> id before send command
            await _uowManager.Current.SaveChangesAsync();

            await _endpointInstance.Send<PublishBlogCreated>(cmd =>
            {
                cmd.BlogId = blog.Id;
            });
        }

        public async Task DeleteBlog(BlogDto input)
        {
            await _blogRepository.DeleteAsync(input.Id);
        }

        public async Task<BlogDto> GetBlog(EntityDto input)
        {
            var blog = await _blogRepository.GetAsync(input.Id);
            var dto = blog.MapTo<BlogDto>();
            return dto;
        }

        public async Task<ListResultDto<BlogDto>> GetBlogs(GetBlogsInput input)
        {
            var blogs = await _blogRepository.GetAllListAsync();
            var dtos = blogs.MapTo<List<BlogDto>>();
            return new ListResultDto<BlogDto>(dtos);
        }

        public async Task UpdateBlog(BlogDto input)
        {
            var blog = await _blogRepository.GetAsync(input.Id);
            input.MapTo(blog);

            await _endpointInstance.Send<PublishBlogChanged>(cmd =>
            {
                cmd.BlogId = blog.Id;
            });
        }
    }
}
