using System;
using MarketingBox.Reporting.Service.Domain.Crm;
using NUnit.Framework;

namespace MarketingBox.Reporting.Service.Tests
{
    public class TestExample
    {
        [SetUp]
        public void Setup()
        {
        }

        
        
        [Test]
        public void Test1()
        {
            CrmStatus status = CrmStatus.New;
            string str1 = status.ToString();
            string str2 = status.ToCrmStatus();
            Assert.Pass();
        }
    }
}
