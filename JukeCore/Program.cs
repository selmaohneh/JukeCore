using System;
using System.Device.Gpio;
using System.IO.Abstractions;
using System.Linq;
using System.Threading.Tasks;
using LibVLCSharp.Shared;
using PowerArgs;

namespace JukeCore
{
    public class Program
    {
        private static Playlist _playlist;
        private static MediaPlayerWrapper _mediaPlayer;
        private static ConsoleWrapper _console;

        public static async Task Main(string[] args)
        {
            CommandLineArguments arguments;
            try
            {
               arguments = Args.Parse<CommandLineArguments>(args);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(ArgUsage.GenerateUsageFromTemplate<CommandLineArguments>());
                return;
            }

            try
            {
                
                _console = new ConsoleWrapper();
                _console.WriteLine("JukeCore started!");

                string jukeCoreMediaPath = arguments.JukeCoreMediaPath;
                _console.WriteLine($"Path to media folder is: '{jukeCoreMediaPath}'.");

                _console.WriteLine("Initialzing LibVLC ... ");
                Core.Initialize();

                _console.WriteLine("Creating app registry ... ");
                var fileSystem = new FileSystem();
                using var libVlc = new LibVLC();
                _playlist = new Playlist(_console);
                _mediaPlayer = new MediaPlayerWrapper(new MediaPlayer(libVlc), _console, _playlist);
                var commandFactory =
                    new MediaFactory(fileSystem.Directory,
                        fileSystem.Path, _console, libVlc);
                var processor = new IdProcessor(commandFactory, _console, _playlist, _mediaPlayer);
                var mainLoop = new MainLoop(_console, processor);

                var gpioDriverFactory = new GpioDriverFactory(_console, _console);
                var driver = gpioDriverFactory.Create();
                var controller = new GpioController(PinNumberingScheme.Logical, driver);
                var volDownButton = new VolumeDownButton(controller, _mediaPlayer, _console);
                var volUpButton = new VolumeUpButton(controller, _mediaPlayer, _console);
                var previousButton = new PreviousButton(controller, _mediaPlayer, _playlist, _console);
                var nextButton = new NextButton(controller, _mediaPlayer, _playlist, _console);
                var playPlauseButton = new PlayPauseButton(controller, _mediaPlayer, _console);

#pragma warning disable 4014
                volDownButton.Activate(4);
                volUpButton.Activate(17);
                previousButton.Activate(22);
                nextButton.Activate(24);
                playPlauseButton.Activate(23);
#pragma warning restore 4014

                await mainLoop.Run(jukeCoreMediaPath);
            }
            catch (Exception e)
            {
                _console.WriteLine(e.Message);
            }
        }
    }
}