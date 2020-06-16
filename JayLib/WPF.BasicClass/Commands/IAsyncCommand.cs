﻿using System.Threading.Tasks;
using System.Windows.Input;

namespace JayLib.WPF.BasicClass
{
    /// <summary>
    /// Interface for Async Command
    /// </summary>
    public interface IAsyncCommand : ICommand
    {
        /// <summary>
        /// Execute the command async.
        /// </summary>
        /// <returns>Task to be awaited on.</returns>
        Task ExecuteAsync();
    }



    /// <summary>
    /// Interface for Async Command with parameter
    /// </summary>
    public interface IAsyncCommand<T> : ICommand
    {
        /// <summary>
        /// Execute the command async.
        /// </summary>
        /// <param name="parameter">Parameter to pass to command</param>
        /// <returns>Task to be awaited on.</returns
        Task ExecuteAsync(T parameter);
    }
}
