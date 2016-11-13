using Abp.Domain.Entities;
using Abp.EntityFramework;
using Abp.EntityFramework.Repositories;

namespace Abp.NServiceBus.EntityFramework.Repositories
{
    public abstract class NServiceBusRepositoryBase<TEntity, TPrimaryKey> : EfRepositoryBase<NServiceBusDbContext, TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        protected NServiceBusRepositoryBase(IDbContextProvider<NServiceBusDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //add common methods for all repositories
    }

    public abstract class NServiceBusRepositoryBase<TEntity> : NServiceBusRepositoryBase<TEntity, int>
        where TEntity : class, IEntity<int>
    {
        protected NServiceBusRepositoryBase(IDbContextProvider<NServiceBusDbContext> dbContextProvider)
            : base(dbContextProvider)
        {

        }

        //do not add any method here, add to the class above (since this inherits it)
    }
}
