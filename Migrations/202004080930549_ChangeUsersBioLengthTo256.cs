namespace Attention_Seeker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeUsersBioLengthTo256 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "Bio", c => c.String(maxLength: 256));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "Bio", c => c.String(maxLength: 50));
        }
    }
}
