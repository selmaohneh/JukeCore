using System;
using System.Device.Gpio;

namespace JukeCore
{
    public class PlayPauseButton : Button
    {
        private readonly IMediaPlayer _mediaPlayer;
        private readonly IConsole _console;
        private readonly JukeCoreDataModel _dataModel;

        public PlayPauseButton(GpioController gpioController, IMediaPlayer mediaPlayer, IConsole console, JukeCoreDataModel dataModel) : base(
            gpioController, console)
        {
            _mediaPlayer = mediaPlayer;
            _console = console;
            _dataModel = dataModel;
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

        protected override void SetButtonStateInDataModel(EButtonState buttonState)
        {
            _dataModel.PlayPauseButtonState = buttonState;
        }
    }
}