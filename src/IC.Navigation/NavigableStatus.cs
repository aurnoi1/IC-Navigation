using IC.Navigation.Enums;
using IC.Navigation.Interfaces;

namespace IC.Navigation
{
    public class NavigableStatus : INavigableStatus
    {
        /// <summary>
        /// The Exist status.
        /// </summary>
        public IState<bool> Exist { get; private set; }

        /// <summary>
        /// The Ready status.
        /// </summary>
        public IState<bool> Ready { get; private set; }

        /// <summary>
        /// The Navigable observed.
        /// </summary>
        public INavigable Navigable { get; private set; }

        public NavigableStatus(INavigable navigable, bool exist, bool ready)
        {
            Navigable = navigable;
            Exist = new State<bool>(navigable, StatesNames.Exist, exist);
            Ready = new State<bool>(navigable, StatesNames.Ready, ready);
        }
    }
}