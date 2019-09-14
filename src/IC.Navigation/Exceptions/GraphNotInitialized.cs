using System;
using System.Collections.Generic;
using System.Text;

namespace IC.Navigation.Exceptions
{
    public class GraphNotInitialized : Exception
    {
        public GraphNotInitialized() : base($"Graph is not initialized.")
        {
        }
    }
}
