using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus.Blogs.Dto
{
    [AutoMap(typeof(Blog))]
    public class BlogDto : FullAuditedEntityDto
    {
        public string Name { get; set; }

        public bool ForceException { get; set; }
    }
}
