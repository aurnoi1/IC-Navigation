using IC.Navigation.Interfaces;

namespace IC.Navigation
{
    public class State<T> : IState<T>
    {
        /// <summary>
        /// The State's value.
        /// </summary>
        public T Value { get; private set; }

        public State(T value)
        {
            Value = value;
        }
    }
}