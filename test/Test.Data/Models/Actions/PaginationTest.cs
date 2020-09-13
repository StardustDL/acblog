using AcBlog.Data.Models.Actions;
using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Test.Data.Models.Actions
{
    [TestClass]
    public class PaginationTest
    {
        [TestMethod]
        public void Basic()
        {
            Pagination page = new Pagination
            {
                CurrentPage = 0,
                PageSize = 10,
                TotalCount = 100
            };
            Assert.AreEqual(10, page.TotalPage);
            Assert.IsTrue(page.HasNextPage);
            Assert.IsFalse(page.HasPreviousPage);

            page.NextPage().ShouldDeepEqual(new Pagination
            {
                CurrentPage = 1,
                PageSize = 10,
                TotalCount = 100
            });
        }
    }
}
