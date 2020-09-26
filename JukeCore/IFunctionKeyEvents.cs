using System;

namespace JukeCore
{
    /// <summary>
    /// Interface for allowing to register for events when function keys are pressed in console
    /// </summary>
    public interface IFunctionKeyEvents
    {
        event EventHandler<ConsoleKey> OnFunctionKeyPressed;
    }
}