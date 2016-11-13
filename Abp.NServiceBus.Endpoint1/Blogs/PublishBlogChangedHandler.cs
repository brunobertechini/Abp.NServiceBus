using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.NServiceBus.Blogs.Commands;
using Abp.NServiceBus.Blogs.Events;
using Abp.Runtime.Session;
using NServiceBus;
using NServiceBus.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus.Blogs
{
    public class PublishBlogChangedHandler : IHandleMessages<PublishBlogChanged>
    {
        private ILog Logger = LogManager.GetLogger<PublishBlogChangedHandler>();

        private readonly IAbpSession _session;
        private readonly IUnitOfWorkManager _uowManager;
        private readonly IRepository<Blog> _blogRepository;

        public PublishBlogChangedHandler(
            IAbpSession session,
            IUnitOfWorkManager uowManager,
            IRepository<Blog> blogRepository)
        {
            _session = session;
            _uowManager = uowManager;
            _blogRepository = blogRepository;
        }

        public async Task Handle(PublishBlogChanged message, IMessageHandlerContext context)
        {
            Logger.InfoFormat("Message: {0}", context.MessageId);
            Logger.InfoFormat("Message/AbpSession: {0}/{1}", context.MessageId, _session.GetHashCode());
            Logger.InfoFormat("Message/UowManager: {0}/{1}", context.MessageId, _uowManager.GetHashCode());
            Logger.InfoFormat("Message/UnitOfWork: {0}/{1}", context.MessageId, _uowManager.Current.GetHashCode());

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
