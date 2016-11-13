using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus.Blogs.Events
{
    public class BlogChanged
    {
        public int BlogId { get; set; }

        public string Name { get; set; }
    }
}
