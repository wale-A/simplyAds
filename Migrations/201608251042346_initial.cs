namespace SimplyAds.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AdContents",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        FilePath = c.String(),
                        ContentType = c.String(),
                        FileName = c.String(),
                        DateAdded = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AdUpdates",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Text = c.String(),
                        AdvertID = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        DeletedBy = c.String(),
                        EditedBy = c.String(),
                        DateEdited = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Adverts", t => t.AdvertID, cascadeDelete: true)
                .Index(t => t.AdvertID);
            
            CreateTable(
                "dbo.Adverts",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        ReferenceNo = c.String(),
                        TransactionReference = c.String(),
                        CustomerName = c.String(),
                        CustomerPhone = c.String(),
                        CustomerEmail = c.String(),
                        DatePlaced = c.DateTime(nullable: false),
                        Urgent = c.Boolean(nullable: false),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Duration = c.String(),
                        NoOfCars = c.String(),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        Treated = c.Boolean(nullable: false),
                        AdContentID = c.Int(nullable: false),
                        Paid = c.Boolean(nullable: false),
                        DateTreated = c.DateTime(),
                        TreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.AdContents", t => t.AdContentID, cascadeDelete: true)
                .Index(t => t.AdContentID);
            
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        NumberOfCars = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        DeletedBy = c.String(),
                        EditedBy = c.String(),
                        DateEdited = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.ContentPricings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Type = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        DeletedBy = c.String(),
                        EditedBy = c.String(),
                        DateEdited = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.DurationPricings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Time = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        DeletedBy = c.String(),
                        EditedBy = c.String(),
                        DateEdited = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.MiscChargePricings",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Property = c.String(),
                        Amount = c.Decimal(nullable: false, precision: 18, scale: 2),
                        CreatedBy = c.String(),
                        DateCreated = c.DateTime(nullable: false),
                        Deleted = c.Boolean(nullable: false),
                        DeletedBy = c.String(),
                        EditedBy = c.String(),
                        DateEdited = c.DateTime(),
                        DateDeleted = c.DateTime(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.AdUpdates", "AdvertID", "dbo.Adverts");
            DropForeignKey("dbo.Adverts", "AdContentID", "dbo.AdContents");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Adverts", new[] { "AdContentID" });
            DropIndex("dbo.AdUpdates", new[] { "AdvertID" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.MiscChargePricings");
            DropTable("dbo.DurationPricings");
            DropTable("dbo.ContentPricings");
            DropTable("dbo.Cars");
            DropTable("dbo.Adverts");
            DropTable("dbo.AdUpdates");
            DropTable("dbo.AdContents");
        }
    }
}
