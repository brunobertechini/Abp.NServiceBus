using Abp.NServiceBus.EntityFramework;
using EntityFramework.DynamicFilters;

namespace Abp.NServiceBus.Migrations.SeedData
{
    public class InitialHostDbBuilder
    {
        private readonly NServiceBusDbContext _context;

        public InitialHostDbBuilder(NServiceBusDbContext context)
        {
            _context = context;
        }

        public void Create()
        {
            _context.DisableAllFilters();

            new DefaultEditionsCreator(_context).Create();
            new DefaultLanguagesCreator(_context).Create();
            new HostRoleAndUserCreator(_context).Create();
            new DefaultSettingsCreator(_context).Create();
        }
    }
}
