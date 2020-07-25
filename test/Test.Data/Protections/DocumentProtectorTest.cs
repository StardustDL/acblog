using AcBlog.Data.Documents;
using AcBlog.Data.Protections;
using DeepEqual.Syntax;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Test.Data.Protections
{
    [TestClass]
    public class DocumentProtectorTest
    {
        [TestMethod]
        public async Task Password()
        {
            await new DocumentProtector().TestPassword(Generator.GetDocument());
        }
    }
}
