﻿using AutoFixture.Xunit2;
using Xunit;
using Xunit.Sdk;

namespace IC.Navigation.Extension.UnitTests.DataAttributes
{
    public class InlineAutoMoqDataAttribute : CompositeDataAttribute
    {
        public InlineAutoMoqDataAttribute(params object[] values)
            : base(
                  new DataAttribute[] {
                  new InlineDataAttribute(values),
                  new AutoMoqDataAttribute() })
        {
        }
    }
}