using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.NServiceBus.Blogs.Events;
using Abp.NServiceBus.Tests.Commands;
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
    public class BlogHistoryHandler : IHandleMessages<BlogCreated>,
                                      IHandleMessages<BlogChanged>
    {
        private ILog Logger = LogManager.GetLogger<BlogHistoryHandler>();

        private readonly IAbpSession _session;
        private readonly IUnitOfWorkManager _uowManager;
        private readonly IRepository<Blog> _blogRepository;
        private readonly IRepository<BlogHistory> _blogHistoryRepository;

        public BlogHistoryHandler(
            IAbpSession session,
            IUnitOfWorkManager uowManager,
            IRepository<Blog> blogRepository,
            IRepository<BlogHistory> blogHistoryRepository)
        {
            _session = session;
            _uowManager = uowManager;
            _blogRepository = blogRepository;
            _blogHistoryRepository = blogHistoryRepository;
        }

        public async Task Handle(BlogCreated message, IMessageHandlerContext context)
        {
            Logger.InfoFormat("Message: {0}", context.MessageId);
            Logger.InfoFormat("Message/AbpSession: {0}/{1}", context.MessageId, _session.GetHashCode());
            Logger.InfoFormat("Message/UowManager: {0}/{1}", context.MessageId, _uowManager.GetHashCode());
            Logger.InfoFormat("Message/UnitOfWork: {0}/{1}", context.MessageId, _uowManager.Current.GetHashCode());

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
                blog.Name = null; // Force Abp Entity Validation
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
