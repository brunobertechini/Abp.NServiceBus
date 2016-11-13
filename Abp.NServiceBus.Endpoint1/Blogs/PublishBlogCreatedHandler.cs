using Abp.Domain.Repositories;
using Abp.NServiceBus.Blogs.Commands;
using Abp.NServiceBus.Blogs.Events;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus.Blogs
{
    public class PublishBlogCreatedHandler : IHandleMessages<PublishBlogCreated>
    {
        public readonly IRepository<Blog> _blogRepository;

        public PublishBlogCreatedHandler(IRepository<Blog> blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task Handle(PublishBlogCreated message, IMessageHandlerContext context)
        {
            var blog = await _blogRepository.GetAsync(message.BlogId);

            await context.Publish<BlogCreated>(evt =>
            {
                evt.BlogId = blog.Id;
                evt.Name = blog.Name;
            });
        }
    }
}
