﻿//using IC.Navigation.Interfaces;
//using System;
//using System.Threading;
//using IC.Navigation.Exceptions;

//namespace IC.Navigation.CoreExtensions
//{
//    public static class NavigableEx
//    {
//        /// <summary>
//        /// Executes the action passed in parameter.
//        /// </summary>
//        /// <param name="navigable">The Navigable.</param>
//        /// <param name="action">The action to execute.</param>m>
//        /// <returns>This Navigable.</returns>
//        public static INavigable Do(this INavigable navigable, Action action)
//        {
//            using var infinitTokenSource = new CancellationTokenSource();
//            void actionNotCancellable(CancellationToken ct) => action();
//            return navigable.Navigator.Do(navigable, actionNotCancellable, infinitTokenSource.Token);
//        }

//        /// <summary>
//        /// Executes the Function passed in parameter.
//        /// </summary>
//        /// <param name="navigable">This Navigable.</param>
//        /// <param name="function">The non-cancellable Function to execute.</param>
//        /// <returns>The expected Navigable returns by the Function.</returns>
//        public static INavigable Do<T>(
//            this INavigable navigable,
//            Func<INavigable> function) where T : INavigable
//        {
//            using var infinitTokenSource = new CancellationTokenSource();
//            INavigable functionNotCancellable(CancellationToken ct) => function();
//            return navigable.Navigator.Do<T>(navigable, functionNotCancellable, infinitTokenSource.Token);
//        }

//        /// <summary>
//        /// Executes the action passed in parameter.
//        /// </summary>
//        /// <param name="navigable">This Navigable.</param>
//        /// <param name="action">The Action to execute.</param>
//        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
//        /// If <c>None</c> then the GlobalCancellationToken will be used otherwise will run in concurrence of it.</param>
//        /// <returns>This Navigable.</returns>
//        public static INavigable Do(this INavigable navigable, Action<CancellationToken> action, CancellationToken cancellationToken = default)
//        {
//            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
//                navigable.Navigator.GlobalCancellationToken,
//                cancellationToken);

//            linkedCts.Token.ThrowIfCancellationRequested();
//            return navigable.Navigator.Do(navigable, action, linkedCts.Token);
//        }

//        /// <summary>
//        /// Executes the Function passed in parameter.
//        /// </summary>
//        /// <param name="navigable">This Navigable.</param>
//        /// <param name="function">The Function to execute.</param>
//        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
//        /// If <c>None</c> then the GlobalCancellationToken will be used otherwise will run in concurrence of it.</param>
//        /// <returns>The expected Navigable returns by the Function.</returns>
//        public static INavigable Do<T>(
//            this INavigable navigable,
//            Func<CancellationToken, INavigable> function,
//            CancellationToken cancellationToken = default) where T : INavigable
//        {
//            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
//                navigable.Navigator.GlobalCancellationToken,
//                cancellationToken);

//            linkedCts.Token.ThrowIfCancellationRequested();
//            return navigable.Navigator.Do<T>(navigable, function, linkedCts.Token);
//        }

//        /// <summary>
//        /// Go to the destination from the origin, using the shortest way.
//        /// </summary>
//        /// <param name="origin">This Navigable.</param>
//        /// <param name="destination">The destination.</param>
//        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
//        /// If <c>None</c> then the GlobalCancellationToken will be used.</param>
//        /// <returns>The destination.</returns>
//        /// <exception cref="UninitializedGraphException">Thrown when the Graph is unitialized.</exception>
//        /// <exception cref="PathNotFoundException">Thrown when no path was found between the origin and the destination.</exception>
//        public static INavigable GoTo(
//                this INavigable origin,
//                INavigable destination,
//                CancellationToken cancellationToken = default)
//        {
//            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
//                origin.Navigator.GlobalCancellationToken,
//                cancellationToken);

//            linkedCts.Token.ThrowIfCancellationRequested();
//            return origin.Navigator.GoTo(origin, destination, linkedCts.Token);
//        }

//        /// <summary>
//        /// Go back to the previous Navigable from <see cref="ILog.Historic"/>.
//        /// </summary>
//        /// <param name="origin">This Navigable.</param>
//        /// <param name="cancellationToken">An optional CancellationToken to interrupt the task as soon as possible.
//        /// If <c>None</c> then the GlobalCancellationToken will be used.</param>
//        /// <returns>The previous Navigable.</returns>
//        public static INavigable Back(this INavigable origin, CancellationToken cancellationToken = default)
//        {
//            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(
//                origin.Navigator.GlobalCancellationToken,
//                cancellationToken);

//            linkedCts.Token.ThrowIfCancellationRequested();
//            return origin.Navigator.Back(linkedCts.Token);
//        }

//        /// <summary>
//        /// Get the <see cref="INavigableStatus.Exist"/> status of this Navigable./>.
//        /// </summary>
//        /// <param name="origin">This Navigable.</param>
//        /// <returns><c>true</c> if exists, otherwise <c>false</c>.</returns>
//        public static bool Exists(this INavigable origin)
//        {
//            return origin.PublishStatus().Exist.Value;
//        }

//        /// <summary>
//        /// Wait until this navigable exists.
//        /// </summary>
//        /// <param name="origin">This Navigable.</param>
//        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
//        /// <returns><c>true</c> if exists before the CancellationToken is canceled.
//        /// Otherwise <c>false</c>.</returns>
//        public static bool WaitForExist(this INavigable origin, CancellationToken cancellationToken)
//        {
//            origin.Navigator.WaitForExist(origin, cancellationToken);
//            return !cancellationToken.IsCancellationRequested;
//        }

//        /// <summary>
//        /// Wait until this navigable is ready.
//        /// </summary>
//        /// <param name="origin">This Navigable.</param>
//        /// <param name="cancellationToken">The CancellationToken to interrupt the task as soon as possible.</param>
//        /// <returns><c>true</c> if ready before the CancellationToken is canceled.
//        /// Otherwise <c>false</c>.</returns>
//        public static bool WaitForReady(this INavigable origin, CancellationToken cancellationToken)
//        {
//            origin.Navigator.WaitForReady(origin, cancellationToken);
//            return !cancellationToken.IsCancellationRequested;
//        }

//    }
//}