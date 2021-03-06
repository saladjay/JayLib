﻿using JayLib.WPF.BasicClass.Commands;
using System;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;


// Lovely code from our good friend John Tririet
// https://johnthiriet.com/mvvm-going-async-with-async-command

namespace JayLib.WPF.BasicClass
{
    /// <summary>
    /// Implementation of an Async Command
    /// </summary>
    public class AsyncCommand : IAsyncCommand
    {
        readonly Func<Task> execute;
        readonly Func<object, bool> canExecute;
        readonly Action<Exception> onException;
        readonly bool continueOnCapturedContext;
        readonly WeakEventManager weakEventManager = new WeakEventManager();
        
        /// <summary>
        /// Create a new AsyncCommand
        /// </summary>
        /// <param name="execute">Function to execute</param>
        /// <param name="canExecute">Function to call to determine if it can be executed</param>
        /// <param name="onException">Action callback when an exception occurs</param>
        /// <param name="continueOnCapturedContext">If the context should be captured on exception</param>
        public AsyncCommand(Func<Task> execute,
                            Func<object, bool> canExecute = null,
                            Action<Exception> onException = null,
                            bool continueOnCapturedContext = false)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
            this.onException = onException;
            this.continueOnCapturedContext = continueOnCapturedContext;
        }


        /// <summary>
        /// Event triggered when Can Excecute changes.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { weakEventManager.AddEventHandler(value); }
            remove { weakEventManager.RemoveEventHandler(value); }
        }
        

        /// <summary>
        /// Invoke the CanExecute method and return if it can be executed.
        /// </summary>
        /// <param name="parameter">Parameter to pass to CanExecute.</param>
        /// <returns>If it can be executed.</returns>
        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;


        /// <summary>
        /// Execute the command async.
        /// </summary>
        /// <returns>Task of action being executed that can be awaited.</returns>
        public Task ExecuteAsync() => execute();


        /// <summary>
        /// Raise a CanExecute change event.
        /// </summary>
        public void RaiseCanExecuteChanged() => weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));


        #region Explicit implementations
        void ICommand.Execute(object parameter) => ExecuteAsync().SafeFireAndForget(onException, continueOnCapturedContext);
        #endregion
    }

    /// <summary>
    /// Implementation of a generic Async Command
    /// </summary>
    public class AsyncCommand<T> : IAsyncCommand<T>
    {
        readonly Func<T, Task> execute;
        readonly Func<object, bool> canExecute;
        readonly Action<Exception> onException;
        readonly bool continueOnCapturedContext;
        readonly WeakEventManager weakEventManager = new WeakEventManager();


        /// <summary>
        /// Create a new AsyncCommand
        /// </summary>
        /// <param name="execute">Function to execute</param>
        /// <param name="canExecute">Function to call to determine if it can be executed</param>
        /// <param name="onException">Action callback when an exception occurs</param>
        /// <param name="continueOnCapturedContext">If the context should be captured on exception</param>
        public AsyncCommand(Func<T, Task> execute,
                            Func<object, bool> canExecute = null,
                            Action<Exception> onException = null,
                            bool continueOnCapturedContext = false)
        {
            this.execute = execute ?? throw new ArgumentNullException(nameof(execute));
            this.canExecute = canExecute;
            this.onException = onException;
            this.continueOnCapturedContext = continueOnCapturedContext;
        }



        /// <summary>
        /// Event triggered when Can Excecute changes.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { weakEventManager.AddEventHandler(value); }
            remove { weakEventManager.RemoveEventHandler(value); }
        }



        /// <summary>
        /// Invoke the CanExecute method and return if it can be executed.
        /// </summary>
        /// <param name="parameter">Parameter to pass to CanExecute.</param>
        /// <returns>If it can be executed</returns>
        public bool CanExecute(object parameter) => canExecute?.Invoke(parameter) ?? true;


        /// <summary>
        /// Execute the command async.
        /// </summary>
        /// <returns>Task that is executing and can be awaited.</returns>
        public Task ExecuteAsync(T parameter) => execute(parameter);



        /// <summary>
        /// Raise a CanExecute change event.
        /// </summary>
        public void RaiseCanExecuteChanged() => weakEventManager.HandleEvent(this, EventArgs.Empty, nameof(CanExecuteChanged));

        #region Explicit implementations



        void ICommand.Execute(object parameter)
        {
            if (CommandUtils.IsValidCommandParameter<T>(parameter))
                ExecuteAsync((T)parameter).SafeFireAndForget(onException, continueOnCapturedContext);
        }
        #endregion
    }


    internal static class CommandUtils
    {
        internal static bool IsValidCommandParameter<T>(object o)
        {
            bool valid;
            if (o != null)
            {
                // The parameter isn't null, so we don't have to worry whether null is a valid option
                valid = o is T;
                if (!valid)
                    throw new InvalidCommandParameterException(typeof(T), o.GetType());
                return valid;
            }

            var t = typeof(T);

            // The parameter is null. Is T Nullable?
            if (Nullable.GetUnderlyingType(t) != null)
            {
                return true;
            }

            // Not a Nullable, if it's a value type then null is not valid
            valid = !t.GetTypeInfo().IsValueType;

            if (!valid)
                throw new InvalidCommandParameterException(typeof(T));
            return valid;
        }
    }

    /// <summary>
    /// Represents errors that occur during IAsyncCommand execution.
    /// </summary>
    public class InvalidCommandParameterException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:MvvmHelpersInvalidCommandParameterException"/> class.
        /// </summary>
        /// <param name="expectedType">Expected parameter type for AsyncCommand.Execute.</param>
        /// <param name="actualType">Actual parameter type for AsyncCommand.Execute.</param>
        /// <param name="innerException">Inner Exception</param>
        public InvalidCommandParameterException(Type expectedType, Type actualType, Exception innerException) : base(CreateErrorMessage(expectedType, actualType), innerException)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:TaskExtensions.MVVM.InvalidCommandParameterException"/> class.
        /// </summary>
        /// <param name="expectedType">Expected parameter type for AsyncCommand.Execute.</param>
        /// <param name="innerException">Inner Exception</param>
        public InvalidCommandParameterException(Type expectedType, Exception innerException) : base(CreateErrorMessage(expectedType), innerException)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:MvvmHelpers.InvalidCommandParameterException"/> class.
        /// </summary>
        /// <param name="expectedType">Expected parameter type for AsyncCommand.Execute.</param>
        /// <param name="actualType">Actual parameter type for AsyncCommand.Execute.</param>
        public InvalidCommandParameterException(Type expectedType, Type actualType) : base(CreateErrorMessage(expectedType, actualType))
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="T:TaskExtensions.MVVM.InvalidCommandParameterException"/> class.
        /// </summary>
        /// <param name="expectedType">Expected parameter type for AsyncCommand.Execute.</param>
        public InvalidCommandParameterException(Type expectedType) : base(CreateErrorMessage(expectedType))
        {
        }

        static string CreateErrorMessage(Type expectedType, Type actualType) =>
            $"Invalid type for parameter. Expected Type: {expectedType}, but received Type: {actualType}";


        static string CreateErrorMessage(Type expectedType) =>
            $"Invalid type for parameter. Expected Type {expectedType}";
    }
}
