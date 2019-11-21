using IC.Navigation.Interfaces;

namespace IC.Navigation
{
    public class NavigableStatus : INavigableStatus
    {
        /// <summary>
        /// The Exist status.
        /// </summary>
        public bool Exist { get; set; }

        /// <summary>
        /// The Ready status.
        /// </summary>
        public bool Ready { get; set; }
    }
}