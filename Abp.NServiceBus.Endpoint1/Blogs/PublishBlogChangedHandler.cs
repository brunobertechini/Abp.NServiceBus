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
    public class PublishBlogChangedHandler : IHandleMessages<PublishBlogChanged>
    {
        public readonly IRepository<Blog> _blogRepository;

        public PublishBlogChangedHandler(IRepository<Blog> blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public async Task Handle(PublishBlogChanged message, IMessageHandlerContext context)
        {
            var blog = await _blogRepository.GetAsync(message.BlogId);

            await context.Publish<BlogChanged>(evt =>
            {
                evt.BlogId = blog.Id;
                evt.Name = blog.Name;
                evt.ForceException = message.ForceException;
            });
        }
    }
}
