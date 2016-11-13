using Abp.Domain.Repositories;
using Abp.NServiceBus.Blogs.Events;
using Abp.NServiceBus.Tests.Commands;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus.Blogs
{
    public class BlogHistoryHandler : IHandleMessages<BlogCreated>,
                                      IHandleMessages<BlogChanged>
    {

        public readonly IRepository<Blog> _blogRepository;
        public readonly IRepository<BlogHistory> _blogHistoryRepository;

        public BlogHistoryHandler(
            IRepository<Blog> blogRepository,
            IRepository<BlogHistory> blogHistoryRepository)
        {
            _blogRepository = blogRepository;
            _blogHistoryRepository = blogHistoryRepository;
        }

        public async Task Handle(BlogCreated message, IMessageHandlerContext context)
        {
            await _blogHistoryRepository.InsertAsync(new BlogHistory()
            {
                BlogId = message.BlogId,
                Action = "Created"
            });

            await _blogHistoryRepository.InsertAsync(new BlogHistory()
            {
                BlogId = message.BlogId,
                Action = "Created Test2"
            });

            await _blogHistoryRepository.InsertAsync(new BlogHistory()
            {
                BlogId = message.BlogId,
                Action = "Created Test3"
            });

            var blog = await _blogRepository.GetAsync(message.BlogId);
            blog.Name = blog.Name + DateTime.Now.ToString();

            if (message.ForceException)
                throw new Exception("Forced Exception");
        }

        public async Task Handle(BlogChanged message, IMessageHandlerContext context)
        {
            await _blogHistoryRepository.InsertAsync(new BlogHistory()
            {
                BlogId = message.BlogId,
                Action = "Changed"
            });

            if (message.ForceException)
                throw new Exception("Forced Exception");
        }
    }
}
