﻿namespace StatsDownload.Parsing.Tests
{
    using Microsoft.Extensions.Logging;

    using NSubstitute;

    using NUnit.Framework;

    using StatsDownload.Core.Interfaces;

    [TestFixture]
    public class TestSlpAddressValidatorProvider
    {
        [SetUp]
        public void SetUp()
        {
            logger = Substitute.For<ILogger<SlpAddressValidatorProvider>>();

            systemUnderTest = new SlpAddressValidatorProvider(logger);
        }

        private ILogger<SlpAddressValidatorProvider> logger;

        private ISlpAddressValidatorService systemUnderTest;

        [TestCase("simpleledger:qqq3728yw0y47sqn6l2na30mcw6zm78dzqre909m2r", "16w1D5WRVKJuZUsSRzdLp9w3YGcgoxDXb")]
        [TestCase("simpleledger:ppm2qsznhks23z7629mms6s4cwef74vcwvn0h829pq", "3CWFddi6m4ndiGyKqzYvsFYagqDLPVMTzC")]
        [TestCase("qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqfnhks603", "1111111111111111111114oLvT2")]
        [TestCase("qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqyhlgn0k34", "11111111111111111111BZbvjr")]
        [TestCase("qpm2qsznhks23z7629mms6s4cwef74vcwvy22gdx6a", "1BpEi6DfDAUFd7GtittLSdBeYJvcoaVggu")]
        [TestCase("qr95sy3j9xwd2ap32xkykttr4cvcu7as4y0qverfuy", "1KXrWXciRDZUpQwQmuM1DbwsKDLYAYsVLR")]
        [TestCase("qqq3728yw0y47sqn6l2na30mcw6zm78dzqre909m2r", "16w1D5WRVKJuZUsSRzdLp9w3YGcgoxDXb")]
        [TestCase("ppm2qsznhks23z7629mms6s4cwef74vcwvn0h829pq", "3CWFddi6m4ndiGyKqzYvsFYagqDLPVMTzC")]
        [TestCase("pr95sy3j9xwd2ap32xkykttr4cvcu7as4yc93ky28e", "3LDsS579y7sruadqu11beEJoTjdFiFCdX4")]
        [TestCase("pqq3728yw0y47sqn6l2na30mcw6zm78dzq5ucqzc37", "31nwvkZwyPdgzjBJZXfDmSWsC4ZLKpYyUw")]
        public void GetBitcoinAddress_WhenInvoked_ReturnsBitcoinAddress(string address, string expected)
        {
            string actual = systemUnderTest.GetBitcoinAddress(address);

            Assert.That(actual, Is.EqualTo(expected));
        }

        [TestCase("bitcoincash:invalid")]
        [TestCase("invalid")]
        [TestCase("bitcoincash:")]
        [TestCase("bitcoincash")]
        [TestCase("1AGNa15ZQXAZUgFiqJ2i7Z2DPU2J6hW62i")]
        [TestCase("1AGNa15ZQXAZUgFiqJ2i7Z2DPU2J6hW62X")]
        public void IsValidSlpAddress_WhenInvokedWithInvalidAddress_ReturnsFalse(string address)
        {
            bool actual = systemUnderTest.IsValidSlpAddress(address);

            Assert.That(actual, Is.False);
        }

        [TestCase("simpleledger:qpm2qsznhks23z7629mms6s4cwef74vcwvy22gdx6a")]
        [TestCase("simpleledger:pqq3728yw0y47sqn6l2na30mcw6zm78dzq5ucqzc37")]
        [TestCase("qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqfnhks603")]
        [TestCase("qqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqqyhlgn0k34")]
        [TestCase("qpm2qsznhks23z7629mms6s4cwef74vcwvy22gdx6a")]
        [TestCase("qr95sy3j9xwd2ap32xkykttr4cvcu7as4y0qverfuy")]
        [TestCase("qqq3728yw0y47sqn6l2na30mcw6zm78dzqre909m2r")]
        [TestCase("ppm2qsznhks23z7629mms6s4cwef74vcwvn0h829pq")]
        [TestCase("pr95sy3j9xwd2ap32xkykttr4cvcu7as4yc93ky28e")]
        [TestCase("pqq3728yw0y47sqn6l2na30mcw6zm78dzq5ucqzc37")]
        public void IsValidSlpAddress_WhenInvokedWithValidAddress_ReturnsTrue(string address)
        {
            bool actual = systemUnderTest.IsValidSlpAddress(address);

            Assert.That(actual, Is.True);
        }
    }
}