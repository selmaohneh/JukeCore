using System;
using LibVLCSharp.Shared;

namespace JukeCore
{
    public interface IMediaPlayer
    {
        bool Play(Media media);
        void Pause();
        event EventHandler<EventArgs> Stopped;
        int Volume { get; set; }
        bool IsPlaying { get; }
    }
}