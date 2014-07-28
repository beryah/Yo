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
            var cn = new CNDate(new DateTime(2014, 11, 5));

            var a = cn.GetLunarHolDay();

            int spsp = 0;
            spsp++;
        }
    }
}