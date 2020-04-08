namespace Attention_Seeker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPathToUsersTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Path", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Path");
        }
    }
}
