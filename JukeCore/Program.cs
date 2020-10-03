using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.IO.Abstractions;
using System.Threading.Tasks;
using LibVLCSharp.Shared;
using MQTTnet;
using MQTTnet.Client.Options;
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
                var dataModel = new JukeCoreDataModel();
                
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
                _mediaPlayer = new MediaPlayerWrapper(new MediaPlayer(libVlc), _console, _playlist, dataModel);
      
                var commandFactory =
                    new MediaFactory(fileSystem.Directory,
                        fileSystem.Path, _console, libVlc);
                var processor = new IdProcessor(commandFactory, _console, _playlist, _mediaPlayer);
                var mainLoop = new MainLoop(_console, processor);

                CreateMqttService(processor, arguments, dataModel);

                var gpioDriverFactory = new GpioDriverFactory(_console, _console);
                var driver = gpioDriverFactory.Create();
                var controller = new GpioController(PinNumberingScheme.Logical, driver);
                var volDownButton = new VolumeDownButton(controller, _mediaPlayer, _console, dataModel);
                var volUpButton = new VolumeUpButton(controller, _mediaPlayer, _console, dataModel);
                var previousButton = new PreviousButton(controller, _mediaPlayer, _playlist, _console, dataModel);
                var nextButton = new NextButton(controller, _mediaPlayer, _playlist, _console, dataModel);
                var playPlauseButton = new PlayPauseButton(controller, _mediaPlayer, _console, dataModel);

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

        private static void CreateMqttService(IdProcessor processor,
            CommandLineArguments arguments, JukeCoreDataModel dataModel)
        {

            if (string.IsNullOrWhiteSpace(arguments.MqttPrefix))
            {
                _console.WriteLine("No mqtt ip was given. Mqtt service disabled.");
                return;
            }

            _console.WriteLine($"Starting mqtt service with prefix {arguments.MqttPrefix}");

            var mqttFactory = new MqttFactory();
            var mqttClient = mqttFactory.CreateMqttClient();
            var optionBuilder = new MqttClientOptionsBuilder()
                .WithClientId("jukeCore")
                .WithTcpServer(arguments.MqttBrokerIp, arguments.MqttPort)
                .WithCleanSession();

            if (!string.IsNullOrEmpty(arguments.MqttUsername) &&
                !string.IsNullOrEmpty(arguments.MqttPassword))
            {
                //optionBuilder = optionBuilder.WithCredentials("fhem", "M9x#X/bqx")
                optionBuilder = optionBuilder.WithCredentials(arguments.MqttUsername, arguments.MqttPassword);
            }

            var options = optionBuilder.Build();

            var mqttPrefix = arguments.MqttPrefix;
            var volumeTopicHandler = new VolumeTopicHandler(_mediaPlayer, mqttPrefix);
            var playIdTopicHandler = new PlayIdTopicHandler(processor, mqttPrefix, arguments.JukeCoreMediaPath);
            var commandTopicHandler = new CommandTopicHandler(_mediaPlayer, _playlist, mqttPrefix);
            List<ITopicHandler> topicHandlers = new List<ITopicHandler>
            {
                volumeTopicHandler,
                playIdTopicHandler,
                commandTopicHandler
            };

            // only data model members are supported in the value / nameof clause ...
            var topicsToPublish = new Dictionary<string, string>
            {
                {$"jukeCore/{arguments.MqttPrefix}/status/playback", nameof(dataModel.PlaybackState)},
                {$"jukeCore/{arguments.MqttPrefix}/status/mediaDurationMs", nameof(dataModel.MediaDurationMs)},
                {$"jukeCore/{arguments.MqttPrefix}/status/mediaFilename", nameof(dataModel.MediaFilename)},
                {$"jukeCore/{arguments.MqttPrefix}/status/mediaPositionMs", nameof(dataModel.MediaPositionMs)},
                {$"jukeCore/{arguments.MqttPrefix}/status/volumePercent", nameof(dataModel.VolumePercent)},
                {$"jukeCore/{arguments.MqttPrefix}/status/buttons/volumeDown", nameof(dataModel.VolumeDownButtonState)},
                {$"jukeCore/{arguments.MqttPrefix}/status/buttons/volumeUp", nameof(dataModel.VolumeUpButtonState)},
                {$"jukeCore/{arguments.MqttPrefix}/status/buttons/previous", nameof(dataModel.PreviousButtonState)},
                {$"jukeCore/{arguments.MqttPrefix}/status/buttons/next", nameof(dataModel.NextButtonState)},
                {$"jukeCore/{arguments.MqttPrefix}/status/buttons/playPause", nameof(dataModel.PlayPauseButtonState)},
            };

        var mqttService = new MqttService(mqttClient, options, _console, topicHandlers, topicsToPublish, dataModel, 2000, mqttPrefix);
        }
    }
}