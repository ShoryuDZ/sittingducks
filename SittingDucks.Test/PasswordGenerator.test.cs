using NUnit.Framework;
using System.Linq;
using System;

namespace SittingDucks.Test
{
    [TestFixture]
    public class PasswordGeneratorTest
	{
        public string generatedPassword = PasswordGenerator.GeneratePassword();

        [Test]
        public void GeneratePassword_Mixed()
        {
            foreach (var characterGroup in PasswordGenerator.CharacterSet)
            {
                Assert.AreEqual(generatedPassword.ToArray().Count(character => characterGroup.ToCharArray().Contains(character)), 4);
            }
        }

        [Test]
        public void GeneratePassword_Length()
        { 
            Assert.That(generatedPassword.Length.Equals(16));
        }
	}
}
