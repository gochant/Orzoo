namespace StandardWeb.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.People",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Email = c.String(),
                        Birth = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.UserActivities",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Ip = c.String(),
                        Operator = c.String(),
                        Date = c.DateTime(nullable: false),
                        ActionName = c.String(),
                        ObjectId = c.String(),
                        ObjectType = c.String(),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.UserActivities");
            DropTable("dbo.People");
        }
    }
}
