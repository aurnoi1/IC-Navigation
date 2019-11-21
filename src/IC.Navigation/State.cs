using IC.Navigation.Enums;
using IC.Navigation.Interfaces;

namespace IC.Navigation
{
    public class State<T> : IState<T>
    {
        /// <summary>
        /// The Navigable observed.
        /// </summary>
        public INavigable Navigable { get; }

        /// <summary>
        /// The State's name.
        /// </summary>
        public StatesNames Name { get; }

        /// <summary>
        /// The State's value.
        /// </summary>
        public T Value { get; private set; }

        public State(INavigable navigable, StatesNames name, T value)
        {
            Navigable = navigable;
            Name = name;
            Value = value;
        }
    }
}