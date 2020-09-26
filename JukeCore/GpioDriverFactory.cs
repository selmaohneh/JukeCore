using System.Device.Gpio;
using System.Device.Gpio.Drivers;
using System.Runtime.InteropServices;

namespace JukeCore
{
    /// <summary>
    /// Factory class to create a platform specific Gpio driver instance
    /// </summary>
    public class GpioDriverFactory
    {
        private readonly IConsole _console;
        private readonly IFunctionKeyEvents _functionKeyEvents;

        /// <summary>
        /// CTOR
        /// </summary>
        public GpioDriverFactory(IConsole console, IFunctionKeyEvents functionKeyEvents)
        {
            _console = console;
            _functionKeyEvents = functionKeyEvents;
        }


        /// <summary>
        /// Create the right GPIO driver for the detected platform
        /// </summary>
        public GpioDriver Create()
        {
            bool isWindows = RuntimeInformation
                .IsOSPlatform(OSPlatform.Windows);

            GpioDriver driver;
            if (isWindows)
            {
                driver = new FunctionKeysGpioDriver(_functionKeyEvents, _console);
            }
            else
            {
                driver = new SysFsDriver();
            }

            return driver;
        }
    }
}
