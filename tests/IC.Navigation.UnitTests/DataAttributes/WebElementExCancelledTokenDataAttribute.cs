using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Kernel;
using AutoFixture.Xunit2;
using Moq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Threading;

namespace IC.Navigation.UnitTests.DataAttributes
{
    public class WebElementExCancelledTokenDataAttribute : AutoDataAttribute
    {
        public WebElementExCancelledTokenDataAttribute() : base(() =>
         {
             var fixture = new Fixture().Customize(new AutoMoqCustomization());
             KeyValuePair<string, string> keyValuePairs() { return fixture.Create<KeyValuePair<string, string>>(); }
             fixture.Build<Dictionary<string, string>>().Do(x => x.AddMany(() => keyValuePairs(), 3));
             fixture.Register<CancellationToken>(() => new CancellationTokenSource(TimeSpan.Zero).Token);
             return fixture;
         })
        {
        }
    }
}