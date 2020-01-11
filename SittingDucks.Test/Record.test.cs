using NUnit.Framework;
using System;
using SittingDucks;

namespace SittingDucks.Test
{
    [TestFixture]
    public class Test
    {
        [Test]
        public void CreateRecord()
        {
            var newRecord = new Record("www.test.com", "test@test.com", "test123");
            Assert.That(newRecord.Website.Equals("www.test.com"));
            Assert.That(newRecord.AccountName.Equals("test@test.com"));
            Assert.That(newRecord.Password.Equals("test123"));
        }
    }
}
