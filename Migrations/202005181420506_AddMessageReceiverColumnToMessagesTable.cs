namespace Attention_Seeker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessageReceiverColumnToMessagesTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Messages", "MessageReceiver_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Messages", "MessageReceiver_Id");
            AddForeignKey("dbo.Messages", "MessageReceiver_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "MessageReceiver_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Messages", new[] { "MessageReceiver_Id" });
            DropColumn("dbo.Messages", "MessageReceiver_Id");
        }
    }
}
