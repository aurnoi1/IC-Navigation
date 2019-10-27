using AutoFixture;
using AutoFixture.AutoMoq;
using IC.Navigation.Extensions.Appium;
using IC.TimeoutEx;
using Moq;
using OpenQA.Selenium;
using System;
using System.Diagnostics;
using System.Threading;
using Xunit;
using System.Linq;
using System.Collections.Generic;

namespace IC.Navigation.UnitTests
{
    public class WebElementExTests : IDisposable
    {
        public WebElementExTests()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
            sut = fixture.Create<IWebElement>();
            cts = new CancellationTokenSource(500.Milliseconds());
            ct = cts.Token;
        }

        private readonly IFixture fixture;
        private readonly IWebElement sut;
        private readonly CancellationTokenSource cts;
        private readonly CancellationToken ct;

        [Fact]
        public void WaitUntil_With_Tuple_And_Timeout_Should_Returns_Expected_Value_Before_Timeout()
        {
            // Arrange
            var (name, value) = fixture.Create<(string name, string value)>();
            Mock.Get(sut).Setup(x => x.GetAttribute(name)).Returns(value);
            var stopwatch = Stopwatch.StartNew();
            TimeSpan timeout = 50.ms();

            // Act
            var actual = sut.WaitUntil(timeout, (name, value));
            stopwatch.Stop();

            // Assert
            Assert.True(actual);
            Assert.True(stopwatch.ElapsedMilliseconds < timeout.Ticks);
        }

        [Fact]
        public void WaitUntil_With_Tuple_And_CancellationToken_Should_Returns_Expected_Value_Before_Cancellation()
        {
            // Arrange
            var (name, value) = fixture.Create<(string name, string value)>();
            Mock.Get(sut).Setup(x => x.GetAttribute(name)).Returns(value);
            var stopwatch = Stopwatch.StartNew();

            // Act
            var actual = sut.WaitUntil(ct, (name, value));
            stopwatch.Stop();

            // Assert
            Assert.True(actual);
            Assert.False(ct.IsCancellationRequested);
        }


        [Fact]
        public void WaitUntil_With_Many_Tuples_And_Timeout_Should_Returns_Expected_Value_Before_Timeout()
        {
            // Arrange
            var attributes = fixture.CreateMany<(string name, string value)>().ToList();
            attributes.ForEach(a => Mock.Get(sut).Setup(x => x.GetAttribute(a.name)).Returns(a.value));
            var stopwatch = Stopwatch.StartNew();
            TimeSpan timeout = 50.ms();

            // Act
            var actual = sut.WaitUntil(timeout, attributes.ToArray());
            stopwatch.Stop();

            // Assert
            Assert.True(actual);
            Assert.True(stopwatch.ElapsedMilliseconds < timeout.Ticks);
        }

        [Fact]
        public void WaitUntil_With_Many_Tuples_And_CancellationToken_Should_Returns_Expected_Value_Before_Cancellation()
        {
            // Arrange
            var attributes = fixture.CreateMany<(string name, string value)>().ToList();
            attributes.ForEach(a => Mock.Get(sut).Setup(x => x.GetAttribute(a.name)).Returns(a.value));
            var stopwatch = Stopwatch.StartNew();

            // Act
            var actual = sut.WaitUntil(ct, attributes.ToArray());
            stopwatch.Stop();

            // Assert
            Assert.True(actual);
            Assert.False(ct.IsCancellationRequested);
        }

        [Fact]
        public void WaitUntil_With_One_Attribute_And_CancellationToken_Should_Returns_Expected_Value_Before_Cancellation()
        {
            // Arrange
            var (name, value) = fixture.Create<(string name, string value)>();
            Mock.Get(sut).Setup(x => x.GetAttribute(name)).Returns(value);

            // Act
            var actual = sut.WaitUntil(ct, name, value);

            // Assert
            Assert.True(actual);
            Assert.False(ct.IsCancellationRequested);
        }

        [Fact]
        public void WaitUntil_With_One_Attribute_And_Timeout_Should_Returns_Expected_Value_Before_Timeout()
        {
            // Arrange
            var (name, value) = fixture.Create<(string name, string value)>();
            Mock.Get(sut).Setup(x => x.GetAttribute(name)).Returns(value);
            var stopwatch = Stopwatch.StartNew();
            TimeSpan timeout = 50.ms();

            // Act
            var actual = sut.WaitUntil(timeout, name, value);
            stopwatch.Stop();

            // Assert
            Assert.True(actual);
            Assert.True(stopwatch.ElapsedMilliseconds < timeout.Ticks);
        }


        [Fact]
        public void WaitUntil_With_Dictionnary_And_CancellationToken_Should_Returns_Expected_Value_Before_Cancellation()
        {
            // Arrange
            var attributes = CreateMockedAttributesDictionary();

            // Act
            var actual = sut.WaitUntil(ct, attributes);

            // Assert
            Assert.True(actual);
            Assert.False(ct.IsCancellationRequested);
        }

        private Dictionary<string, string> CreateMockedAttributesDictionary()
        {
            var attributes = fixture
                .CreateMany<KeyValuePair<string, string>>()
                .ToDictionary(x => x.Key, x => x.Value);

            foreach (var keyValuePair in attributes)
            {
                Mock.Get(sut).Setup(x => x.GetAttribute(keyValuePair.Key)).Returns(keyValuePair.Value);
            }

            return attributes;
        }

        [Fact]
        public void WaitUntil_With_Dictionary_And_Timeout_Should_Returns_Expected_Value_Before_Timeout()
        {
            // Arrange
            var attributes = CreateMockedAttributesDictionary();
            var stopwatch = Stopwatch.StartNew();
            TimeSpan timeout = 50.ms();

            // Act
            var actual = sut.WaitUntil(timeout, attributes);
            stopwatch.Stop();

            // Assert
            Assert.True(actual);
            Assert.True(stopwatch.ElapsedMilliseconds < timeout.Ticks);
        }

        public void Dispose()
        {
            cts?.Dispose();
        }
    }
}