﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var cn = new CNDate(new DateTime(1989, 8, 19));

            var a = cn.GetLunarHolDay();

            int spsp = 0;
            spsp++;
        }
    }
}