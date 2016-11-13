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
    public class PublishBlogCreatedHandler : IHandleMessages<PublishBlogCreated>
    {
        private ILog Logger = LogManager.GetLogger<PublishBlogCreatedHandler>();

        private readonly IAbpSession _session;
        private readonly IUnitOfWorkManager _uowManager;
        private readonly IRepository<Blog> _blogRepository;

        public PublishBlogCreatedHandler(
            IAbpSession session,
            IUnitOfWorkManager uowManager,
            IRepository<Blog> blogRepository)
        {
            _session = session;
            _uowManager = uowManager;
            _blogRepository = blogRepository;
        }

        public async Task Handle(PublishBlogCreated message, IMessageHandlerContext context)
        {
            Logger.InfoFormat("Message: {0}", context.MessageId);
            Logger.InfoFormat("Message/AbpSession: {0}/{1}", context.MessageId, _session.GetHashCode());
            Logger.InfoFormat("Message/UowManager: {0}/{1}", context.MessageId, _uowManager.GetHashCode());
            Logger.InfoFormat("Message/UnitOfWork: {0}/{1}", context.MessageId, _uowManager.Current.GetHashCode());

            var blog = await _blogRepository.GetAsync(message.BlogId);

            await context.Publish<BlogCreated>(evt =>
            {
                evt.BlogId = blog.Id;
                evt.Name = blog.Name;
                evt.ForceException = message.ForceExceptionAtBlogHistoryHandler;
            });

            if (message.ForceExceptionAtPublishBlogEventHandler)
                throw new Exception("ForceExceptionAtPublishBlogEventHandler");
        }
    }
}
