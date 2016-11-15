using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus
{
    public class AbpNServiceBusModuleConfig
    {
        public string EndpointName { get; set; }

        public bool Debug { get; set; }
    }
}
