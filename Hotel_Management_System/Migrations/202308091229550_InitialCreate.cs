namespace Hotel_Management_System.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Guests",
                c => new
                    {
                        GuestId = c.Int(nullable: false, identity: true),
                        Guest_Name = c.String(nullable: false, maxLength: 50),
                        Contact_Number = c.String(nullable: false, maxLength: 15),
                        Picture = c.String(nullable: false, maxLength: 150),
                        Address = c.String(nullable: false, maxLength: 150),
                        City = c.String(nullable: false, maxLength: 50),
                    })
                .PrimaryKey(t => t.GuestId);
            
            CreateTable(
                "dbo.Reservations",
                c => new
                    {
                        Reservation_Id = c.Int(nullable: false, identity: true),
                        Check_in_Date = c.DateTime(nullable: false, storeType: "date"),
                        Check_out_Date = c.DateTime(nullable: false, storeType: "date"),
                        Room_Number = c.Int(nullable: false),
                        Room_Type = c.Int(nullable: false),
                        IsCancelled = c.Boolean(nullable: false),
                        GuestId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Reservation_Id)
                .ForeignKey("dbo.Guests", t => t.GuestId, cascadeDelete: true)
                .Index(t => t.GuestId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Reservations", "GuestId", "dbo.Guests");
            DropIndex("dbo.Reservations", new[] { "GuestId" });
            DropTable("dbo.Reservations");
            DropTable("dbo.Guests");
        }
    }
}
