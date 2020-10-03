using System;
using System.Device.Gpio;

namespace JukeCore
{
    public class VolumeUpButton : Button
    {
        private readonly IMediaPlayer _mediaPlayer;
        private readonly IConsole _console;
        private readonly JukeCoreDataModel _dataModel;

        public VolumeUpButton(GpioController gpioController, IMediaPlayer mediaPlayer, IConsole console, JukeCoreDataModel dataModel) : base(
            gpioController, console)
        {
            _mediaPlayer = mediaPlayer;
            _console = console;
            _dataModel = dataModel;
            Pressed += OnPressed;
        }

        private void OnPressed(object sender, EventArgs e)
        {
            _console.WriteLine("Volume up button was pressed!");
            _mediaPlayer.Volume += 5;
        }

        protected override void SetButtonStateInDataModel(EButtonState buttonState)
        {
            _dataModel.VolumeUpButtonState = buttonState;
        }
    }
}