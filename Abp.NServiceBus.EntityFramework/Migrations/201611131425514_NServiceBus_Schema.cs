namespace Abp.NServiceBus.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NServiceBus_Schema : DbMigration
    {
        public override void Up()
        {
            Sql(string.Format("CREATE SCHEMA {0}", NServiceBusConsts.EndpointDatabaseSchema));
        }
        
        public override void Down()
        {
            Sql(string.Format("DROP SCHEMA {0}", NServiceBusConsts.EndpointDatabaseSchema));
        }
    }
}
