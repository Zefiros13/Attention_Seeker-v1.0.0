namespace Attention_Seeker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPasswordHistoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PasswordHistory",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        PasswordHash = c.String(nullable: false, maxLength: 128),
                        CreatedDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.PasswordHash })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PasswordHistory", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.PasswordHistory", new[] { "UserId" });
            DropTable("dbo.PasswordHistory");
        }
    }
}
