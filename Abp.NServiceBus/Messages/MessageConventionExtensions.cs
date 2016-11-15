using NServiceBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Abp.NServiceBus
{
    public static class MessageConventionExtensions
    {
        public static void UseAbpMessageConventions(this EndpointConfiguration endpointConfiguration)
        {
            var conventions = endpointConfiguration.Conventions();
            conventions.DefiningCommandsAs(type =>
            {
                return type.Namespace != null &&
                       type.Namespace.EndsWith("Commands");
            });
            conventions.DefiningEventsAs(type =>
            {
                return type.Namespace != null &&
                       type.Namespace.EndsWith("Events");
            });
            conventions.DefiningMessagesAs(type =>
            {
                return type.Namespace != null &&
                       type.Namespace.EndsWith("RequestResponse");
            });
            conventions.DefiningEncryptedPropertiesAs(property =>
            {
                return property.Name.StartsWith("Encrypted");
            });
            conventions.DefiningDataBusPropertiesAs(property =>
            {
                return property.Name.EndsWith("DataBus");
            });
            conventions.DefiningExpressMessagesAs(type =>
            {
                return type.Name.EndsWith("Express");
            });
            conventions.DefiningTimeToBeReceivedAs(type =>
            {
                if (type.Name.EndsWith("Expires"))
                {
                    return TimeSpan.FromSeconds(30);
                }
                return TimeSpan.MaxValue;
            });
        }
    }
}
