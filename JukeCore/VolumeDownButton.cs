using System;
using System.Device.Gpio;

namespace JukeCore
{
    public class VolumeDownButton : Button
    {
        private readonly IMediaPlayer _mediaPlayer;
        private readonly IConsole _console;

        public VolumeDownButton(GpioController gpioController, IMediaPlayer mediaPlayer, IConsole console) : base(
            gpioController, console)
        {
            _mediaPlayer = mediaPlayer;
            _console = console;
            Pressed += OnPressed;
        }

        private void OnPressed(object sender, EventArgs e)
        {
            _console.WriteLine("Volume down button was pressed!");
            _mediaPlayer.Volume -= 5;
        }
    }
}