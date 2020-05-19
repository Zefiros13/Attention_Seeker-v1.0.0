namespace Attention_Seeker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMessagesTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Messages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        MessageContent = c.String(),
                        Connection_Id = c.Int(),
                        MessageSender_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.UsersConnections", t => t.Connection_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.MessageSender_Id)
                .Index(t => t.Connection_Id)
                .Index(t => t.MessageSender_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Messages", "MessageSender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Messages", "Connection_Id", "dbo.UsersConnections");
            DropIndex("dbo.Messages", new[] { "MessageSender_Id" });
            DropIndex("dbo.Messages", new[] { "Connection_Id" });
            DropTable("dbo.Messages");
        }
    }
}
