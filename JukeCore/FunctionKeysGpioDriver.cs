using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Threading;

namespace JukeCore
{
    /// <summary>
    /// Simulates an gpio driver by pressing function keys on the keyboard
    /// </summary>
    public class FunctionKeysGpioDriver : GpioDriver
    {
        private readonly IFunctionKeyEvents _functionKeyEvents;
        private readonly IConsole _console;
        private readonly Dictionary<ConsoleKey, int> _openedPins = new Dictionary<ConsoleKey, int>();
        private readonly Dictionary<int, PinValue> _pinValues = new Dictionary<int, PinValue>();
        private readonly List<ConsoleKey> _assignedConsoleKeys = new List<ConsoleKey>();
        private readonly PinValue _idlePinValue = PinValue.High;

        /// <summary>
        /// CTOR
        /// </summary>
        public FunctionKeysGpioDriver(IFunctionKeyEvents functionKeyEvents, IConsole console)
        {
            _functionKeyEvents = functionKeyEvents;
            _console = console;
            _functionKeyEvents.OnFunctionKeyPressed += FunctionKeyPressed;
        }

        private ConsoleKey AssignNextFreeConsoleKey()
        {
            var nextKey = ConsoleKey.F1;

            if (_assignedConsoleKeys.Count == PinCount)
            {
                throw new ArgumentException("The number of supported pins is reached. Close pins before being able to open new ones.");
            }

            if (_assignedConsoleKeys.Any())
            {
                var lastKey = _assignedConsoleKeys.Last();
                nextKey = lastKey + 1;
            }

            _assignedConsoleKeys.Add(nextKey);


            return nextKey;
        }

        private void FunctionKeyPressed(object sender, ConsoleKey e)
        {
            if (_openedPins.ContainsKey(e))
            {
                _pinValues[_openedPins[e]] = PinValue.Low;
            }
        }

        protected override void AddCallbackForPinValueChangedEvent(int pinNumber, PinEventTypes eventTypes, PinChangeEventHandler callback)
        {
            throw new NotSupportedException("Only polling is supported on this implementation");
        }

        protected override void ClosePin(int pinNumber)
        {
            _pinValues.Remove(pinNumber);
        }

        protected override int ConvertPinNumberToLogicalNumberingScheme(int pinNumber)
        {
            return pinNumber;
        }

        protected override PinMode GetPinMode(int pinNumber)
        {
            return PinMode.Input;
        }

        protected override bool IsPinModeSupported(int pinNumber, PinMode mode)
        {
            return mode != PinMode.Output;
        }

        protected override void OpenPin(int pinNumber)
        {
            var nextKey = AssignNextFreeConsoleKey();
            _openedPins.Add(nextKey, pinNumber);
            _pinValues.Add(pinNumber, _idlePinValue);

            _console.WriteLine($"Pin {pinNumber} is assigned to key {nextKey}");
        }

        protected override PinValue Read(int pinNumber)
        {
            var pinValue = _pinValues[pinNumber];
            _pinValues[pinNumber] = _idlePinValue;
            return pinValue;
        }

        protected override void RemoveCallbackForPinValueChangedEvent(int pinNumber, PinChangeEventHandler callback)
        {
            throw new NotSupportedException("Only polling is supported on this implementation");
        }

        protected override void SetPinMode(int pinNumber, PinMode mode)
        {
            if (mode == PinMode.Output)
            {
                throw new NotSupportedException("Only input pins are supported on this implementation");
            }
        }

        protected override WaitForEventResult WaitForEvent(int pinNumber, PinEventTypes eventTypes, CancellationToken cancellationToken)
        {
            throw new NotSupportedException("Only polling is supported on this implementation");
        }

        protected override void Write(int pinNumber, PinValue value)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Allow to map 12 GPIOs (keyboard keys F1 to F12)
        /// </summary>
        protected override int PinCount => 12;


        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            _functionKeyEvents.OnFunctionKeyPressed -= FunctionKeyPressed;
        }
    }
}
