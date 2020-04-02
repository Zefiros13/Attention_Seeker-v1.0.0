namespace Attention_Seeker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDateCreatedColumnToUsersConnectionsTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.UsersConnections", "DateCreated", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.UsersConnections", "DateCreated");
        }
    }
}
