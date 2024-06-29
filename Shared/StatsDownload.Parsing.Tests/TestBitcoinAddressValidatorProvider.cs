﻿namespace StatsDownload.Parsing.Tests
{
    using NUnit.Framework;

    using StatsDownload.Core.Interfaces;

    [TestFixture]
    public class TestBitcoinAddressValidatorProvider
    {
        [SetUp]
        public void SetUp()
        {
            systemUnderTest = new BitcoinAddressValidatorProvider();
        }

        private IBitcoinAddressValidatorService systemUnderTest;

        [TestCase("1AGNa15ZQXAZUgFiqJ2i7Z2DPU2J6hW62X")]
        [TestCase("1ANNa15ZQXAZUgFiqJ2i7Z2DPU2J6hW62i")]
        [TestCase("1A Na15ZQXAZUgFiqJ2i7Z2DPU2J6hW62i")]
        [TestCase("BZbvjr")]
        public void IsValidBitcoinAddress_WhenInvalidBitcoinAddress_ReturnFalse(string bitcoinAddress)
        {
            bool actual = systemUnderTest.IsValidBitcoinAddress(bitcoinAddress);

            Assert.That(actual, Is.False);
        }

        [TestCase("1AGNa15ZQXAZUgFiqJ2i7Z2DPU2J6hW62i")]
        [TestCase("1Q1pE5vPGEEMqRcVRMbtBK842Y6Pzo6nK9")]
        public void IsValidBitcoinAddress_WhenValidBitcoinAddress_ReturnTrue(string bitcoinAddress)
        {
            bool actual = systemUnderTest.IsValidBitcoinAddress(bitcoinAddress);

            Assert.That(actual, Is.True);
        }
    }
}