using System.Reflection;
using Abp.AutoMapper;
using Abp.Modules;

namespace Abp.NServiceBus
{
    [DependsOn(typeof(NServiceBusCoreModule), typeof(AbpAutoMapperModule))]
    public class NServiceBusApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Modules.AbpAutoMapper().Configurators.Add(mapper =>
            {
                //Add your custom AutoMapper mappings here...
                //mapper.CreateMap<,>()
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());
        }
    }
}
