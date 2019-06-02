using System;

namespace IC.Navigation
{
    /// <summary>
    /// Represents the attribute of an UIArtefact.
    /// </summary>
    public class UIArtefact : Attribute
    {
        /// <summary>
        /// The usage name.
        /// </summary>
        public string UsageName;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIArtefact"/> class.
        /// </summary>
        public UIArtefact()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIArtefact"/> class.
        /// </summary>
        /// <param name="usageName">The usage name.</param>
        public UIArtefact(string usageName)
        {
            UsageName = usageName;
        }
        
    }
}