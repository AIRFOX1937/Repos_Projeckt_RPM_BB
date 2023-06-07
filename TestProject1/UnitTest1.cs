using AspNetCore;
using BB_RPM_PROJEKT;
using BB_RPM_PROJEKT.Controllers;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestVivodNews()
        {
            bool expected = true;
            var af = new Tests();
            bool actual = af.vivodNews();
            Assert.AreEqual(expected, actual);
        }
    }
}