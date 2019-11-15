using AutoFixture;
using AutoFixture.AutoMoq;
using IC.Navigation.Extensions.Appium;
using IC.TimeoutEx;
using Moq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Xunit;

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

        #region Wait Tests

        [Fact]
        public void Wait_With_One_Attribute_And_CancellationToken_Should_Returns_Element_When_Attribute_Match_Expected_Value()
        {
            // Arrange
            var (name, value) = fixture.Create<(string name, string value)>();
            Mock.Get(sut).Setup(x => x.GetAttribute(name)).Returns(value);

            // Act
            var actual = sut.Wait(ct, name, value);

            // Assert
            Assert.Equal(sut, actual);
            Assert.False(ct.IsCancellationRequested);
        }

        [Fact]
        public void Wait_With_One_Attribute_And_Timeout_Should_Returns_Element_When_Attribute_Match_Expected_Value()
        {
            // Arrange
            var (name, value) = fixture.Create<(string name, string value)>();
            Mock.Get(sut).Setup(x => x.GetAttribute(name)).Returns(value);
            var stopwatch = Stopwatch.StartNew();
            TimeSpan timeout = 50.ms();

            // Act
            var actual = sut.Wait(timeout, name, value);

            // Assert
            Assert.Equal(sut, actual);
            Assert.True(stopwatch.ElapsedMilliseconds < timeout.Ticks);
        }

        [Fact]
        public void Wait_With_Dictionary_And_Timeout_Should_Returns_Element_When_Attribute_Match_Expected_Value()
        {
            // Arrange
            var attributes = CreateMockedAttributesDictionary();
            var stopwatch = Stopwatch.StartNew();
            TimeSpan timeout = 50.ms();

            // Act
            var actual = sut.Wait(timeout, attributes);

            // Assert
            Assert.Equal(sut, actual);
            Assert.True(stopwatch.ElapsedMilliseconds < timeout.Ticks);
        }

        [Fact]
        public void Wait_With_Tuple_And_CancellationToken_Should_Returns_Element_When_Attribute_Match_Expected_Value()
        {
            // Arrange
            var (name, value) = fixture.Create<(string name, string value)>();
            Mock.Get(sut).Setup(x => x.GetAttribute(name)).Returns(value);
            var stopwatch = Stopwatch.StartNew();

            // Act
            var actual = sut.Wait(ct, (name, value));
            stopwatch.Stop();

            // Assert
            Assert.Equal(sut, actual);
            Assert.False(ct.IsCancellationRequested);
        }

        [Fact]
        public void Wait_With_Tuple_And_Timeout_Should_Returns_Element_When_Attribute_Match_Expected_Value()
        {
            // Arrange
            var (name, value) = fixture.Create<(string name, string value)>();
            Mock.Get(sut).Setup(x => x.GetAttribute(name)).Returns(value);
            var stopwatch = Stopwatch.StartNew();
            TimeSpan timeout = 50.ms();

            // Act
            var actual = sut.Wait(timeout, (name, value));
            stopwatch.Stop();

            // Assert
            Assert.Equal(sut, actual);
            Assert.True(stopwatch.ElapsedMilliseconds < timeout.Ticks);
        }

        [Fact]
        public void Wait_With_Dic_And_CancellationToken_Should_Returns_Element_When_Attribute_Match_Expected_Value()
        {
            // Arrange
            var attributes = CreateMockedAttributesDictionary();

            // Act
            var actual = sut.Wait(ct, attributes);

            // Assert
            Assert.Equal(sut, actual);
            Assert.False(ct.IsCancellationRequested);
        }

        [Fact]
        public void Wait_With_CancellationToken_Should_Throws_OperationCanceledException_When_AttribueName_Is_Invalid()
        {
            // Arrange
            var (name, value) = fixture.Create<(string name, string value)>();
            Mock.Get(sut).Setup(x => x.GetAttribute(name)).Returns(value);

            // Act
            Assert.Throws<OperationCanceledException>(() => sut.Wait(ct, ("invalidAttribName", "True")));

            // Assert
            Assert.NotNull(sut);
            Assert.True(ct.IsCancellationRequested);
        }

        [Fact]
        public void Wait_With_Dictionary_And_Timeout_Should_TimeoutException_When_Timeout_Is_Reached()
        {
            // Arrange
            var attributes = CreateMockedAttributesDictionary();
            var stopwatch = Stopwatch.StartNew();
            TimeSpan timeout = TimeSpan.Zero;

            // Assert
            Assert.Throws<TimeoutException>(() => sut.Wait(timeout, attributes));
            Assert.NotNull(sut);
        }

        [Fact]
        public void Wait_With_Tuple_CancellationToken_Should_Throws_OperationCanceledException_When_CancellationToken_Is_Cancelled()
        {
            // Arrange
            var (name, value) = fixture.Create<(string name, string value)>();
            Mock.Get(sut).Setup(x => x.GetAttribute(name)).Returns(value);
            using var ctsLocal = new CancellationTokenSource(TimeSpan.Zero);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut.Wait(ctsLocal.Token, (name, value)));
            Assert.NotNull(sut);
        }

        [Fact]
        public void Wait_With_One_Attribute_And_CancellationToken_Should_Throws_OperationCanceledException_When_CancellationToken_Is_Cancelled()
        {
            // Arrange
            var (name, value) = fixture.Create<(string name, string value)>();
            Mock.Get(sut).Setup(x => x.GetAttribute(name)).Returns(value);
            using var ctsLocal = new CancellationTokenSource(TimeSpan.Zero);

            // Assert
            Assert.Throws<OperationCanceledException>(() => sut.Wait(ctsLocal.Token, name, value));
            Assert.NotNull(sut);
        }

        [Fact]
        public void Wait_With_One_Attribute_And_Timeout_Should_Throws_OperationCanceledException_When_CancellationToken_Is_Cancelled()
        {
            // Arrange
            var (name, value) = fixture.Create<(string name, string value)>();
            Mock.Get(sut).Setup(x => x.GetAttribute(name)).Returns(value);

            // Assert
            Assert.Throws<TimeoutException>(() => sut.Wait(TimeSpan.Zero, name, value));
            Assert.NotNull(sut);
        }

        [Fact]
        public void Wait_With_Timeout_Should_Throws_TimeoutException_When_Timeout_Is_Reached()
        {
            // Arrange
            var (name, value) = fixture.Create<(string name, string value)>();
            Mock.Get(sut).Setup(x => x.GetAttribute(name)).Returns(value);

            // Assert
            Assert.Throws<TimeoutException>(() => sut.Wait(TimeSpan.Zero, (name, value)));
            Assert.NotNull(sut);
        }

        [Fact]
        public void Wait_With_Timeout_Should_Throws_ArgumentNullException_When_WebElement_Is_Null()
        {
            var (name, value) = fixture.Create<(string name, string value)>();
            IWebElement sut = null;
            Assert.Throws<ArgumentNullException>(() => sut.Wait(TimeSpan.Zero, (name, value)));
        }

        [Fact]
        public void Wait_With_CancellationToken_Should_Throws_ArgumentNullException_When_WebElement_Is_Null()
        {
            var (name, value) = fixture.Create<(string name, string value)>();
            IWebElement sut = null;
            Assert.Throws<ArgumentNullException>(() => sut.Wait(ct, (name, value)));
        }

        [Fact]
        public void Wait_With_Dic_And_CancellationToken_Should_Throws_OperationCanceledException_When_CancellationToken_Is_Cancelled()
        {
            // Arrange
            var attributes = CreateMockedAttributesDictionary();
            using var ctsLocal = new CancellationTokenSource(TimeSpan.Zero);

            Assert.Throws<OperationCanceledException>(() => sut.Wait(ctsLocal.Token, attributes));
            Assert.True(ctsLocal.IsCancellationRequested);
            Assert.NotNull(sut);
        }

        #endregion Wait Tests

        #region WaitUntil Tests

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
        public void WaitUntil_With_Tuple_And_Timeout_Should_Returns_Expected_Value_With_Delay_But_Before_Timeout()
        {
            // Arrange
            using CancellationTokenSource delayBeforeReturnsValue = new CancellationTokenSource(50.ms());
            var (name, value) = CreateMockAttributesReturningValueAfterDelay(delayBeforeReturnsValue.Token);
            var stopwatch = Stopwatch.StartNew();
            TimeSpan timeout = 500.ms();

            // Act
            var actual = sut.WaitUntil(timeout, name, value);
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

        [Fact]
        public void WaitUntil_With_Dictionary_And_Timeout_Should_Returns_Throws_ArgumentNullException_When_IWebElement_Is_Null()
        {
            // Arrange
            Dictionary<string, string> attributes = null;

            TimeSpan timeout = 50.ms();
            IWebElement nulSut = null;
            // Act
            bool waitUntilWithNullValues() => nulSut.WaitUntil(timeout, attributes);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(() => waitUntilWithNullValues());
            Assert.Equal("The WebElement is null. (Parameter 'webElement')", exception.Message);
        }

        [Fact]
        public void WaitUntil_With_Dictionary_And_Timeout_Should_Returns_Throws_ArgumentNullException_When_Dictionary_Is_Null()
        {
            // Arrange
            Dictionary<string, string> attributes = null;

            TimeSpan timeout = 50.ms();

            // Act
            bool waitUntilWithNullValues() => sut.WaitUntil(timeout, attributes);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(() => waitUntilWithNullValues());
            Assert.Equal("Expected keyValuePairs Attribute is null. (Parameter 'expectedAttributesNamesValues')", exception.Message);
        }

        #endregion WaitUntil Tests

        public void Dispose()
        {
            cts?.Dispose();
        }

        #region Private

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

        private (string name, string value) CreateMockAttributesReturningValueAfterDelay(CancellationToken token)
        {
            var (name, value) = fixture.Create<(string name, string value)>();
            static string retValueAfterDelay(CancellationToken cancellationToken, string value)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    return null;
                }
                else
                {
                    return value;
                }
            }

            Mock.Get(sut).Setup(x => x.GetAttribute(name)).Returns(() => retValueAfterDelay(token, value));
            return (name, value);
        }

        #endregion Private
    }
}