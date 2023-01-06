namespace Lect1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PostsTableCreated : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.PostCategories", "Post_Id", "dbo.Posts");
            DropForeignKey("dbo.PostCategories", "Category_Id", "dbo.Categories");
            DropIndex("dbo.PostCategories", new[] { "Post_Id" });
            DropIndex("dbo.PostCategories", new[] { "Category_Id" });
            CreateIndex("dbo.Posts", "CatId");
            AddForeignKey("dbo.Posts", "CatId", "dbo.Categories", "Id", cascadeDelete: true);
            DropTable("dbo.PostCategories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PostCategories",
                c => new
                    {
                        Post_Id = c.Int(nullable: false),
                        Category_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Post_Id, t.Category_Id });
            
            DropForeignKey("dbo.Posts", "CatId", "dbo.Categories");
            DropIndex("dbo.Posts", new[] { "CatId" });
            CreateIndex("dbo.PostCategories", "Category_Id");
            CreateIndex("dbo.PostCategories", "Post_Id");
            AddForeignKey("dbo.PostCategories", "Category_Id", "dbo.Categories", "Id", cascadeDelete: true);
            AddForeignKey("dbo.PostCategories", "Post_Id", "dbo.Posts", "Id", cascadeDelete: true);
        }
    }
}
