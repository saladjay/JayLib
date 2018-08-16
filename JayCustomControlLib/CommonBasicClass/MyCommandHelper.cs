using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace JayCustomControlLib.CommonBasicClass
{

    internal static class MyCommandHelper
    {

        // Lots of specialized registration methods to avoid new'ing up more common stuff (like InputGesture's) at the callsite, as that's frequently
        // repeated and increases code size.  Do it once, here.  

        internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler)
        {
            PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, null);
        }

        internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler,
                                                    InputGesture inputGesture)
        {
            PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, inputGesture);
        }

        internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler,
                                                    Key key)
        {
            PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, new KeyGesture(key));
        }

        internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler,
                                                    InputGesture inputGesture, InputGesture inputGesture2)
        {
            PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, null, inputGesture, inputGesture2);
        }

        internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler,
                                                    CanExecuteRoutedEventHandler canExecuteRoutedEventHandler)
        {
            PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, null);
        }

        internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler,
                                                    CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, InputGesture inputGesture)
        {
            PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, inputGesture);
        }

        internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler,
                                                    CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, Key key)
        {
            PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(key));
        }

        internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler,
                                                    CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, InputGesture inputGesture, InputGesture inputGesture2)
        {
            PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, inputGesture, inputGesture2);
        }

        internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler,
                                                    CanExecuteRoutedEventHandler canExecuteRoutedEventHandler,
                                                    InputGesture inputGesture, InputGesture inputGesture2, InputGesture inputGesture3, InputGesture inputGesture4)
        {
            PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler,
                                          inputGesture, inputGesture2, inputGesture3, inputGesture4);
        }

        internal static void RegisterCommandHandler(Type controlType, RoutedCommand command, Key key, ModifierKeys modifierKeys,
                                                    ExecutedRoutedEventHandler executedRoutedEventHandler, CanExecuteRoutedEventHandler canExecuteRoutedEventHandler)
        {
            PrivateRegisterCommandHandler(controlType, command, executedRoutedEventHandler, canExecuteRoutedEventHandler, new KeyGesture(key, modifierKeys));
        }


        // 'params' based method is private.  Call sites that use this bloat unwittingly due to implicit construction of the params array that goes into IL.
        private static void PrivateRegisterCommandHandler(Type controlType, RoutedCommand command, ExecutedRoutedEventHandler executedRoutedEventHandler,
                                                          CanExecuteRoutedEventHandler canExecuteRoutedEventHandler, params InputGesture[] inputGestures)
        {
            // Validate parameters
            Debug.Assert(controlType != null);
            Debug.Assert(command != null);
            Debug.Assert(executedRoutedEventHandler != null);
            // All other parameters may be null

            // Create command link for this command
            CommandManager.RegisterClassCommandBinding(controlType, new CommandBinding(command, executedRoutedEventHandler, canExecuteRoutedEventHandler));

            // Create additional input binding for this command
            if (inputGestures != null)
            {
                for (int i = 0; i < inputGestures.Length; i++)
                {
                    CommandManager.RegisterClassInputBinding(controlType, new InputBinding(command, inputGestures[i]));
                }
            }

        }

        internal static bool CanExecuteCommandSource(ICommandSource commandSource)
        {
            ICommand command = commandSource.Command;
            if (command != null)
            {
                object parameter = commandSource.CommandParameter;
                IInputElement target = commandSource.CommandTarget;

                if (command is RoutedCommand routed)
                {
                    if (target == null)
                    {
                        target = commandSource as IInputElement;
                    }
                    return routed.CanExecute(parameter, target);
                }
                else
                {
                    return command.CanExecute(parameter);
                }
            }

            return false;
        }

        // This allows a caller to override its ICommandSource values (used by Button and ScrollBar)
        internal static void ExecuteCommand(ICommand command, object parameter, IInputElement target)
        {
            if (command is RoutedCommand routed)
            {
                if (routed.CanExecute(parameter, target))
                {
                    routed.Execute(parameter, target);
                }
            }
            else if (command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }
    }

}
