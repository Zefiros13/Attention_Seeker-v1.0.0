namespace Attention_Seeker.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangePathColumnNameToProfilePicturePathInUsersTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ProfilePicturePath", c => c.String());
            DropColumn("dbo.AspNetUsers", "Path");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "Path", c => c.String());
            DropColumn("dbo.AspNetUsers", "ProfilePicturePath");
        }
    }
}
