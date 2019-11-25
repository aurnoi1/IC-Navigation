using System;
using System.Collections.Generic;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;

namespace IC.Navigation.Extension.UnitTests.DataAttributes
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(() =>
        {
            var fixture = new Fixture().Customize(new AutoMoqCustomization());
            KeyValuePair<string, string> keyValuePairs() { return fixture.Create<KeyValuePair<string, string>>(); }
            fixture.Build<Dictionary<string, string>>().Do(x => x.AddMany(() => keyValuePairs(), 3));
            return fixture;
        })
        {
        }
    }
}