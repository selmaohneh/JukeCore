using System;
using System.Threading;
using LibVLCSharp.Shared;

namespace JukeCore
{
    public class MediaPlayerWrapper : IMediaPlayer, IDisposable
    {
        private readonly MediaPlayer _mediaPlayer;
        private readonly IConsole _console;
        private readonly Playlist _playlist;

        public MediaPlayerWrapper(MediaPlayer mediaPlayer, IConsole console, Playlist playlist)
        {
            _mediaPlayer = mediaPlayer;
            _console = console;
            _playlist = playlist;
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

        private void OnStopped(object sender, EventArgs e)
        {
            _console.WriteLine("Reached end of track.");
            if (_playlist.AnyNext())
            {
                var nextMedia = _playlist.Next();
                ThreadPool.QueueUserWorkItem(_ => _mediaPlayer.Play(nextMedia));
                return;
            }

            _console.WriteLine("Nothing left to play.");
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