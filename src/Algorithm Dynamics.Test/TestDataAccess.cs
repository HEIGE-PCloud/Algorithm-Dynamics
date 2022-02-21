using Microsoft.VisualStudio.TestTools.UnitTesting;
using Algorithm_Dynamics.Core.Helpers;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Collections.Generic;

namespace Algorithm_Dynamics.Test
{
    [TestClass]
    public class TestDataAccess
    {
        [TestMethod]
        public void TestSingleData()
        {
            DataAccess.InitializeDatabase(@"temp\TestSingleData.db");
            DataAccess.AddData("Text1");
            Assert.AreEqual(DataAccess.GetData().Count, 1);
            Assert.AreEqual(DataAccess.GetData()[0], "Text1");
        }
        [TestMethod]
        public void TestMultipleData()
        {
            DataAccess.InitializeDatabase(@"temp\TestMultipleData.db");
            int count = 100;
            List<string> list = new();
            for (int i = 0; i < count; i++)
            {
                DataAccess.AddData($"Text{count}");
                list.Add($"Text{count}");
            }
            Assert.AreEqual(DataAccess.GetData().Count, count);
            CollectionAssert.AreEqual(list, DataAccess.GetData());

        }
    }
}
