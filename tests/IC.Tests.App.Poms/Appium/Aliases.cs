using System;
using System.Collections.Generic;
using System.Linq;

namespace IC.Tests.App.Poms.Appium
{
    /// <summary>
    /// Represents the attribute of an Aliases.
    /// </summary>
    public class Aliases : Attribute
    {
        /// <summary>
        /// The aliases.
        /// </summary>
        public List<string> Values;

        /// <summary>
        /// Initializes a new instance of the <see cref="Aliases"/> class.
        /// </summary>
        public Aliases()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Aliases"/> class.
        /// </summary>
        /// <param name="alias">The aliases.</param>
        public Aliases(params string[] alias)
        {
            Values = alias.ToList();
        }
    }
}