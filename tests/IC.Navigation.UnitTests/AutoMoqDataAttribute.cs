using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.Xunit2;
using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.UnitTests
{
    public class AutoMoqDataAttribute : AutoDataAttribute
    {
        public AutoMoqDataAttribute() : base(() =>
         {
             var fixture = new Fixture().Customize(new AutoMoqCustomization());
             return fixture;
         })
        {

        }
    }
}
