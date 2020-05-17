namespace AcBlog.Tools.SDK.Models
{
    public class Db
    {
        public DbItemList Posts { get; set; } = new DbItemList();

        public DbItemList Categories { get; set; } = new DbItemList();

        public DbItemList Keywords { get; set; } = new DbItemList();
    }
}
