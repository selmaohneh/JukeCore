using System;
using LibVLCSharp.Shared;

namespace JukeCore
{
    public class MediaPlayerWrapper : IMediaPlayer, IDisposable
    {
        private readonly MediaPlayer _mediaPlayer;
        private readonly IConsole _console;

        public bool IsPlaying => _mediaPlayer.IsPlaying;

        public event EventHandler<EventArgs> Stopped;

        public MediaPlayerWrapper(MediaPlayer mediaPlayer, IConsole console)
        {
            _mediaPlayer = mediaPlayer;
            _console = console;
            _mediaPlayer.Volume = 50;
            _mediaPlayer.EndReached += OnStopped;
        }

        public bool Play(Media media)
        {
            _console.WriteLine("Executing play on media player .. ");
            return _mediaPlayer.Play(media);
        }

        public int Volume
        {
            get => _mediaPlayer.Volume;
            set
            {
                if (value < 0)
                {
                    value = 0;
                }

                if (value > 100)
                {
                    value = 100;
                }

                _mediaPlayer.Volume = value;

                _console.WriteLine($"Media player volume set to {value}.");
            }
        }

        private void OnStopped(object? sender, EventArgs e)
        {
            _console.WriteLine("Media player stopped!");
            Stopped?.Invoke(this, e);
        }

        public void Pause()
        {
            _console.WriteLine("Executing pause on media player .. ");
            _mediaPlayer.Pause();
        }

        public void Dispose()
        {
            _console.WriteLine("Disposing media player ...");
            _mediaPlayer.Stopped -= OnStopped;
            _mediaPlayer.Dispose();
        }
    }
}