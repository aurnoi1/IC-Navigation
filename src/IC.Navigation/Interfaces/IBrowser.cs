using System;
using System.Threading;

namespace IC.Navigation.Interfaces
{
    public interface IBrowser
    {
        IMap Map { get; }
        INavigator Navigator { get; }
        ILog Log { get; }

        IBrowser Back(CancellationToken cancellationToken = default);

        IBrowser Do(Action action);

        IBrowser Do(Action<CancellationToken> action, CancellationToken cancellationToken = default);

        IBrowser Do<T>(Func<CancellationToken, INavigable> function, CancellationToken cancellationToken = default) where T : INavigable;

        IBrowser Do<T>(Func<INavigable> function) where T : INavigable;

        bool Exists(INavigable navigable);

        Navigation.Interfaces.IBrowser GoTo(INavigable destination, CancellationToken cancellationToken = default);

        bool WaitForExist(INavigable navigable, CancellationToken cancellationToken);

        bool WaitForReady(INavigable navigable, CancellationToken cancellationToken);
    }
}