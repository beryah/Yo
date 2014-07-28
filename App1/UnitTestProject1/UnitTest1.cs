using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var a = new DateTime(1912, 2, 5).GetChineseLunarDate();
            var b = new DateTime(1912, 2, 24).GetChineseLunarDate();
            var c = new DateTime(1912, 2, 25).GetChineseLunarDate();
            var d = new DateTime(1912, 2, 26).GetChineseLunarDate();
            
            int spsp = 0;
            spsp++;
        }
    }
}