namespace Attention_Seeker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsSeenColumnToMessagesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "IsSeen", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Messages", "IsSeen");
        }
    }
}
