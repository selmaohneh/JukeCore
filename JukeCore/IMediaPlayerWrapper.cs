using LibVLCSharp.Shared;

namespace JukeCore
{
    public interface IMediaPlayer
    {
        bool Play(Media media);
        void Pause();
        int Volume { get; set; }
    }
}