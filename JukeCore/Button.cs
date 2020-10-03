using System;
using System.Device.Gpio;
using System.Threading;
using System.Threading.Tasks;

namespace JukeCore
{
    public abstract class Button
    {
        private readonly GpioController _gpioController;
        private readonly IConsole _console;
        public event EventHandler Pressed;

        protected Button(GpioController gpioController, IConsole console)
        {
            _gpioController = gpioController;
            _console = console;
        }


#if USE_GPIO_POLLING
        public await Task Activate(int gpioNumber)
#else
        public Task Activate(int gpioNumber)
#endif
        {
            try
            {
                _console.WriteLine($"Opening GPIO {gpioNumber} for {GetType().Name}...");
                _gpioController.OpenPin(gpioNumber);
                _console.WriteLine(
                    $"GPIO {gpioNumber} was opened! Initial value: {_gpioController.Read(gpioNumber).ToString()}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Task.CompletedTask;
            }

#if USE_GPIO_POLLING

            while (true)
            {
                var pinValue = _gpioController.Read(gpioNumber);

                if (pinValue == PinValue.Low)
                {
                    Pressed?.Invoke(this, EventArgs.Empty);
                    await Task.Delay(400);

                    continue;
                }

                await Task.Delay(20);
            }
#else
            _gpioController.RegisterCallbackForPinValueChangedEvent(gpioNumber, PinEventTypes.Falling | PinEventTypes.Rising, OnPinLevelChanged);
            return Task.CompletedTask;
#endif
        }

        private void OnPinLevelChanged(object sender, PinValueChangedEventArgs args)
        {
            if (args.ChangeType == PinEventTypes.Falling)
            {
                Pressed?.Invoke(this, EventArgs.Empty);
                Thread.Sleep(50);
            }

        }
    }
}