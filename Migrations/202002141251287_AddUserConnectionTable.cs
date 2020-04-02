namespace Attention_Seeker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserConnectionTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UsersConnections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        WaitingFlag = c.Boolean(nullable: false),
                        ApproveFlag = c.Boolean(nullable: false),
                        RejectFlag = c.Boolean(nullable: false),
                        BlockFlag = c.Boolean(nullable: false),
                        SpamFlag = c.Boolean(nullable: false),
                        ConnectionReceiver_Id = c.String(nullable: false, maxLength: 128),
                        ConnectionSender_Id = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ConnectionReceiver_Id, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ConnectionSender_Id)
                .Index(t => t.ConnectionReceiver_Id)
                .Index(t => t.ConnectionSender_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UsersConnections", "ConnectionSender_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.UsersConnections", "ConnectionReceiver_Id", "dbo.AspNetUsers");
            DropIndex("dbo.UsersConnections", new[] { "ConnectionSender_Id" });
            DropIndex("dbo.UsersConnections", new[] { "ConnectionReceiver_Id" });
            DropTable("dbo.UsersConnections");
        }
    }
}
