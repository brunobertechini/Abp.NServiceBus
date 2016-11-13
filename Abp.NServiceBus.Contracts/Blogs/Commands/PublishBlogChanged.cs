using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus.Blogs.Commands
{
    public class PublishBlogChanged
    {
        public int BlogId { get; set; }

        public bool ForceBlogException { get; set; }

        public bool ForceBlogHistoryException { get; set; }
    }
}
