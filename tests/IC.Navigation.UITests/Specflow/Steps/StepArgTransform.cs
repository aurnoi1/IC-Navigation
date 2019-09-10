using System;
using System.Collections.Generic;
using System.Text;
using TechTalk.SpecFlow;

namespace IC.Navigation.UITests.Specflow.Steps
{
    [Binding]
    public class StepArgTransform
    {
        [StepArgumentTransformation(@"(^\d+)")]
        public DateTime DaysAgoTransform(int days)
        {
            return DateTime.Today.AddDays(days);
        }
    }
}
