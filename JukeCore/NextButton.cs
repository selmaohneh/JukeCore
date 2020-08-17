using System;
using System.Device.Gpio;
using System.Threading;

namespace JukeCore
{
    public class NextButton : Button
    {
        private readonly IMediaPlayer _mediaPlayer;
        private readonly IPlaylist _playlist;
        private readonly IConsole _console;

        public NextButton(GpioController gpioController, IMediaPlayer mediaPlayer, IPlaylist playlist,
            IConsole console) : base(
            gpioController, console)
        {
            _mediaPlayer = mediaPlayer;
            _playlist = playlist;
            _console = console;
            Pressed += OnPressed;
        }

        private void OnPressed(object? sender, EventArgs e)
        {
            try
            {
                _console.WriteLine("Next button was pressed!");
                var media = _playlist.Next();

                ThreadPool.QueueUserWorkItem(_ => _mediaPlayer.Play(media));
            }
            catch (Exception exception)
            {
                _console.WriteLine(exception.Message);
            }
        }
    }
}