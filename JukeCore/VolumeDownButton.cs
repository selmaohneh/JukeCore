using System;
using System.Device.Gpio;

namespace JukeCore
{
    public class VolumeDownButton : Button
    {
        private readonly IMediaPlayer _mediaPlayer;
        private readonly IConsole _console;
        private readonly JukeCoreDataModel _dataModel;

        public VolumeDownButton(GpioController gpioController, IMediaPlayer mediaPlayer, IConsole console, JukeCoreDataModel dataModel) : base(
            gpioController, console)
        {
            _mediaPlayer = mediaPlayer;
            _console = console;
            _dataModel = dataModel;
            Pressed += OnPressed;
        }

        private void OnPressed(object sender, EventArgs e)
        {
            _console.WriteLine("Volume down button was pressed!");
            _mediaPlayer.Volume -= 5;
        }

        protected override void SetButtonStateInDataModel(EButtonState buttonState)
        {
            _dataModel.VolumeDownButtonState = buttonState;
        }
    }
}