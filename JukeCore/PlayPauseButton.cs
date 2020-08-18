using System;
using System.Device.Gpio;

namespace JukeCore
{
    public class PlayPauseButton : Button
    {
        private readonly IMediaPlayer _mediaPlayer;
        private readonly IConsole _console;

        public PlayPauseButton(GpioController gpioController, IMediaPlayer mediaPlayer, IConsole console) : base(
            gpioController, console)
        {
            _mediaPlayer = mediaPlayer;
            _console = console;
            Pressed += OnPressed;
        }

        private void OnPressed(object sender, EventArgs e)
        {
            try
            {
                _console.WriteLine("Play/Pause button was pressed!");
                _mediaPlayer.Pause();
            }
            catch (Exception exception)
            {
                _console.WriteLine(exception.Message);
            }
        }
    }
}