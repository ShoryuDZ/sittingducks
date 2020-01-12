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

        [TestCase("password", false)]
        [TestCase("lotsandlotsoflettersinthispassword", false)]
        [TestCase("password1234", false)]
        [TestCase("password1234!", false)]
        [TestCase("Pa$sword1234!", false)]
        [TestCase("PA$$word1234!", true)]
        [TestCase("Pw1!", false)]
        [TestCase("ABcd12!@", true)]
        public void IsSecurePassword(string potentialPassword, bool isStrong)
        {
            Assert.AreEqual(PasswordGenerator.IsSecure(potentialPassword), isStrong);
        }
	}
}
