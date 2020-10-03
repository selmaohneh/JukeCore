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
        private readonly JukeCoreDataModel _dataModel;

        public MediaPlayerWrapper(MediaPlayer mediaPlayer, IConsole console, Playlist playlist, JukeCoreDataModel dataModel)
        {
            _mediaPlayer = mediaPlayer;
            _console = console;
            _playlist = playlist;
            _dataModel = dataModel;
            SubscribeToMediaPlayerEvents();
            _mediaPlayer.Volume = 50;
        }

        private void UpdateVolumeFromPlayer()
        {
            _dataModel.VolumePercent = (byte)_mediaPlayer.Volume;
        }

        private void SubscribeToMediaPlayerEvents()
        {
            _mediaPlayer.EndReached += OnStopped;
            _mediaPlayer.Paused += OnPlaybackStateChanged;
            _mediaPlayer.Stopped += OnPlaybackStateChanged;
            _mediaPlayer.Playing += OnPlaybackStateChanged;
            _mediaPlayer.MediaChanged += OnMediaChanged;
            _mediaPlayer.LengthChanged += OnLengthChanged;
            _mediaPlayer.TimeChanged += OnPlaybackTimeChanged;
            _mediaPlayer.VolumeChanged += OnVolumeChanged;
        }

        private void OnVolumeChanged(object sender, MediaPlayerVolumeChangedEventArgs e)
        {
            // e.Volume is float and fives inaccuracies when converted to percent - hence use the already
            // converted volume from player here
           UpdateVolumeFromPlayer();
        }

        private void OnPlaybackTimeChanged(object sender, MediaPlayerTimeChangedEventArgs e)
        {
            // throttle change to 1 per sec
            _dataModel.MediaPositionMs = (e.Time / 1000) * 1000;
        }

        private void OnLengthChanged(object sender, MediaPlayerLengthChangedEventArgs e)
        {
            _dataModel.MediaDurationMs = e.Length;
        }

        private void OnMediaChanged(object sender, MediaPlayerMediaChangedEventArgs e)
        {
            _dataModel.MediaFilename = e.Media.Mrl;
        }

        private void OnPlaybackStateChanged(object sender, EventArgs e)
        {
            switch (_mediaPlayer.State)
            {
                case VLCState.Paused:
                    _dataModel.PlaybackState = EPlaybackState.Paused;
                    break;
                case VLCState.Playing:
                    _dataModel.PlaybackState = EPlaybackState.Playing;
                    break;
                default:
                    _dataModel.PlaybackState = EPlaybackState.Stopped;
                    _dataModel.MediaDurationMs = 0;
                    _dataModel.MediaPositionMs = 0;
                    _dataModel.MediaFilename = string.Empty;
                    break;
            }
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
            _mediaPlayer.EndReached -= OnStopped;
            _mediaPlayer.Stopped -= OnPlaybackStateChanged;
            _mediaPlayer.Playing -= OnPlaybackStateChanged;
            _mediaPlayer.Paused -= OnPlaybackStateChanged;
            _mediaPlayer.MediaChanged -= OnMediaChanged;
            _mediaPlayer.TimeChanged -= OnPlaybackTimeChanged;
            _mediaPlayer.LengthChanged -= OnLengthChanged;
            _mediaPlayer.VolumeChanged -= OnVolumeChanged;
            _mediaPlayer.Dispose();
        }
    }
}