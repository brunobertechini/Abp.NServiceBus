using Abp.Domain.Repositories;
using Abp.NServiceBus.Blogs.Events;
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

        public readonly IRepository<BlogHistory> _blogHistoryRepository;

        public BlogHistoryHandler(IRepository<BlogHistory> blogHistoryRepository)
        {
            _blogHistoryRepository = blogHistoryRepository;
        }

        public async Task Handle(BlogCreated message, IMessageHandlerContext context)
        {
            await _blogHistoryRepository.InsertAsync(new BlogHistory()
            {
                BlogId = message.BlogId,
                Action = "Created"
            });
        }

        public async Task Handle(BlogChanged message, IMessageHandlerContext context)
        {
            await _blogHistoryRepository.InsertAsync(new BlogHistory()
            {
                BlogId = message.BlogId,
                Action = "Changed"
            });
        }
    }
}
