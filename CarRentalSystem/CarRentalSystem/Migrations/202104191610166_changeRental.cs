namespace CarRentalSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeRental : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Rentals", "Returned");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Rentals", "Returned", c => c.Boolean(nullable: false));
        }
    }
}
