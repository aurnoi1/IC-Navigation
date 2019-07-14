using System;

namespace IC.Navigation
{
    /// <summary>
    /// Represents the attribute of an UIArtifact.
    /// </summary>
    public class UIArtifact : Attribute
    {
        /// <summary>
        /// The usage name.
        /// </summary>
        public string UsageName;

        /// <summary>
        /// Initializes a new instance of the <see cref="UIArtifact"/> class.
        /// </summary>
        public UIArtifact()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UIArtifact"/> class.
        /// </summary>
        /// <param name="usageName">The usage name.</param>
        public UIArtifact(string usageName)
        {
            UsageName = usageName;
        }
        
    }
}